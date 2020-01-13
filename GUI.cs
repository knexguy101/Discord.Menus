using Discord.Menus.Elements;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Discord.Menus
{
    public class GUI
    {
        private GUIBase _GUIBase { get; set; }
        private IUser User { get; set; }

        public GUI(IUserMessage Message, List<Tab> TabList, IUser User, DiscordSocketClient Client)
        {
            this.User = User;
            _GUIBase = new GUIBase(Message, TabList, Client);
        }

        public async Task RunGUI()
        {
            try
            {

                //vars
                CancellationTokenSource token = new CancellationTokenSource();
                int CurrentMessageIndex = 0;
                
                //init
                _GUIBase.LoadTab(CurrentMessageIndex);

                while (true)
                {
                    var emote = _GUIBase.WaitOnReaction(_GUIBase.GetMessage(), token.Token, this.User);
                    var FixedName = _GUIBase.GetIndexEmoji(_GUIBase.GetIndex(emote));
                    _GUIBase.RemoveEmote(emote, User);
                    if (emote.Name == "⬅️" || emote.Name == "➡️") //navigation
                    {
                        int index = 0;
                        HandleNavigate(CurrentMessageIndex, emote.Name == "➡️", out index);
                        CurrentMessageIndex = index;
                    }
                    else if (_GUIBase.GetIndexEmoteList().Contains(FixedName)) //preset
                    {
                        HandleIndexButton(CurrentMessageIndex, emote, token.Token);
                    }
                    else if (_GUIBase.GetElementList(CurrentMessageIndex).Where(a => a.Type == GUIElementType.Button).Cast<Button>().Any(b => b.GetEmoji() == emote.Name)) //custom
                    {
                        HandleCustomButton(CurrentMessageIndex, emote);
                    }
                    else
                    {
                        await _GUIBase.Message.DeleteAsync();
                        break;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message} {ex.StackTrace}");
            }
        }

        private void HandleNavigate(int CurrentMessageIndex, bool forward, out int NewIndex)
        {
            if((forward && CurrentMessageIndex + 2 <= _GUIBase.TabList.Count) || (!forward && CurrentMessageIndex - 1 >= 0)) //right || left
            {
                CurrentMessageIndex = forward ? CurrentMessageIndex + 1 : CurrentMessageIndex - 1;
                _GUIBase.LoadTab(CurrentMessageIndex);
            }
            NewIndex = CurrentMessageIndex;
        }

        private void HandleCustomButton(int CurrentMessageIndex, IEmote Emote)
        {
            var temp = _GUIBase.GetElementList(CurrentMessageIndex).Where(a => a.Type == GUIElementType.Button).Cast<Button>().First(b => b.GetEmoji() == Emote.Name);
            Task.Factory.StartNew(() =>
            {
                temp.GetAction().Invoke();
            }); //we dont await it since it holds up main memory on main thread
        }

        private async void HandleIndexButton(int CurrentMessageIndex, IEmote Emote, CancellationToken Token)
        {
            var currentElement = _GUIBase.GetElementList(CurrentMessageIndex)[_GUIBase.GetEmojiIndex(_GUIBase.GetIndexEmoji(_GUIBase.GetIndex(Emote)))];
            switch (currentElement.Type) //only ones that require extra stuff
            {
                case GUIElementType.Checkbox:
                    if (await DoCheckbox((Checkbox)currentElement, Token))
                        _GUIBase.LoadTab(CurrentMessageIndex);
                    break;
                case GUIElementType.Dropdown:
                    if (await DoDropdown((Dropdown)currentElement, Token))                   
                        _GUIBase.LoadTab(CurrentMessageIndex);                
                    break;
                case GUIElementType.Textbox:
                    if (await DoTextbox((Textbox)currentElement, Token))
                        _GUIBase.LoadTab(CurrentMessageIndex);
                    break;
            }
        }

        private async Task<bool> DoCheckbox(Checkbox Checkbox, CancellationToken Token)
        {
            var eb = new EmbedBuilder();
            eb.AddField(Checkbox.GetLabelText(), Checkbox.GetChecked());
            var tempMessage = await _GUIBase.Message.Channel.SendMessageAsync("", false, eb.Build());
            await tempMessage.AddReactionsAsync(new List<Emoji>()
            {
                new Emoji("✅"),
                new Emoji("❌")
            }.ToArray());

            var emote = _GUIBase.WaitOnReaction(tempMessage, Token, User);
            if (emote.Name == "❌")
            {
                Checkbox.SetCheck("off");
                await tempMessage.DeleteAsync();
                return true;
            }
            else if(emote.Name == "✅")
            {
                Checkbox.SetCheck("on");
                await tempMessage.DeleteAsync();
                return true;
            }
            await tempMessage.DeleteAsync();
            return false;
        }

        private async Task<bool> DoDropdown(Dropdown Dropdown, CancellationToken Token)
        {
            var eb = new EmbedBuilder();
            for (int x = 0; x < Dropdown.GetItemList().Count(); x++)
            {
                eb.AddField(Dropdown.GetItemList()[x], _GUIBase.GetIndexEmoji(x));
            }
            var tempMessage = await _GUIBase.Message.Channel.SendMessageAsync("", false, eb.Build());
            for (int x = 0; x < Dropdown.GetItemList().Count(); x++)
            {
                await tempMessage.AddReactionAsync(_GUIBase.GetEmoji(x));
            }
            await tempMessage.AddReactionAsync(new Emoji("❌"));

            var emote = _GUIBase.WaitOnReaction(tempMessage, Token, User);
            if (_GUIBase.GetIndexEmoteList().Contains(_GUIBase.GetIndexEmoji(_GUIBase.GetIndex(emote))))
            {
                //dropdown item
                Dropdown.SetIndex(_GUIBase.GetEmojiIndex(_GUIBase.GetIndexEmoji(_GUIBase.GetIndex(emote))));
                await tempMessage.DeleteAsync();
                return true;
            }
            else
            {
                await tempMessage.DeleteAsync();
                return false;
            }
        }

        private async Task<bool> DoTextbox(Textbox Textbox, CancellationToken Token)
        {
            var eb = new EmbedBuilder();
            eb.AddField(Textbox.GetLabelText(), $"✅ -> {Textbox.GetText()}");
            var tempMessage = await _GUIBase.Message.Channel.SendMessageAsync("", false, eb.Build());
            await tempMessage.AddReactionsAsync(new List<Emoji>()
            {
                new Emoji("✅"),
                new Emoji("❌")
            }.ToArray());

            var emote = _GUIBase.WaitOnReaction(tempMessage, Token, User);
            if (emote.Name == "❌")
            {
                await tempMessage.DeleteAsync();
                return false;
            }
            else if (emote.Name == "✅")
            {
                var textMessage = _GUIBase.WaitOnString(tempMessage, Token, User);
                Textbox.SetText(textMessage);
                await _GUIBase.DeleteMessage(1);
                await tempMessage.DeleteAsync();
                return true;
            }
            await tempMessage.DeleteAsync();
            return false;
        }
    }
}
