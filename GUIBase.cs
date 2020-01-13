using Discord.Menus.Elements;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Discord.Menus
{
    public class GUIBase
    {
        public IUserMessage Message { get; set; }
        public List<Tab> TabList = new List<Tab>();
        public DiscordSocketClient Client { get; set; }

        public GUIBase(IUserMessage message, List<Tab> TabList, DiscordSocketClient Client)
        {
            this.Message = message;
            this.TabList = TabList;
            this.Client = Client;
        }

        public string[] GetIndexEmoteList()
        {
            return new List<string>()
            {
                ":one:",
                ":two:",
                ":three:",
                ":four:",
                ":five:",
                ":six:",
                ":seven:",
                ":eight:",
                ":nine:"
            }.ToArray();
        }

        public Embed GetLoadingEmbed()
        {
            var eb = new EmbedBuilder();
            eb.AddField("Loading...", "This is the window that initializes the GUI");
            return eb.Build();
        }

        public IUserMessage GetMessage()
        {
            return Message;
        }
        
        public IEmote GetCustomEmote(string Name)
        {
            return ((SocketGuildChannel)Message.Channel).Guild.Emotes.First(a => a.Name == Name || a.Name.Replace(':', ' ').Trim() == Name);
        }

        public async void AddEmote(IEmote Emote)
        {
            Console.WriteLine("add emote");
            await Message.AddReactionAsync(Emote);
        }

        public async void AddEmotes(List<IEmote> Emotes)
        {
            await Message.AddReactionsAsync(Emotes.ToArray());
        }

        public async void RemoveEmote(IEmote Emote, IUser User = null)
        {
            User = User == null ? Message.Author : User;
            Console.WriteLine("remove emote");
            await Message.RemoveReactionAsync(Emote, User);
        }

        public async Task DeleteMessage(int Amount)
        {
            Console.WriteLine("delete messages");
            var messages = await Message.Channel.GetMessagesAsync(Amount).FlattenAsync();
            await (Message.Channel as SocketTextChannel).DeleteMessagesAsync(messages);
        }

        public GUIElement[] GetElementList(int Index)
        {
            return TabList[Index].GetElementList();
        }

        public IEmote WaitOnReaction(IUserMessage Message, CancellationToken Token, IUser User)
        {
            Console.WriteLine("waiting on reaction");
            IEmote emote = null;
            this.Client.ReactionAdded += async (arg1, arg2, arg3) => 
            {
                if (arg3.MessageId == Message.Id && arg3.User.GetValueOrDefault().Id == User.Id)
                {
                    emote = arg3.Emote;
                }
            };
            while (emote == null && !Token.IsCancellationRequested)
            {
                Thread.Sleep(500);
            }
            return emote;
        }

        public string WaitOnString(IUserMessage Message, CancellationToken Token, IUser User)
        {
            Console.WriteLine("waiting on string");
            string Text = null;
            this.Client.MessageReceived += async (arg) =>
            {
                if(arg.Channel == Message.Channel && arg.Author == User)
                {
                    Text = arg.Content;
                }
            };
            while(Text == null && !Token.IsCancellationRequested)
            {
                Thread.Sleep(500);
            }
            return Text;
        }

        public string GetIndexEmoji(int Index)
        {
            Index = Index + 1;
            switch (Index)
            {
                case 1:
                    return ":one:";
                case 2:
                    return ":two:";
                case 3:
                    return ":three:";
                case 4:
                    return ":four:";
                case 5:
                    return ":five:";
                case 6:
                    return ":six:";
                case 7:
                    return ":seven:";
                case 8:
                    return ":eight:";
                case 9:
                    return ":nine:";
            }
            return ":zero:";
        }

        //REPLACE
        public int GetEmojiIndex(string Name)
        {
            var temp = 0;
            switch (Name)
            {
                case ":one:":
                    temp = 1;
                    break;
                case ":two:":
                    temp = 2;
                    break;
                case ":three:":
                    temp = 3;
                    break;
                case ":four:":
                    temp = 4;
                    break;
                case ":five:":
                    temp = 5;
                    break;
                case ":six:":
                    temp = 6;
                    break;
                case ":seven:":
                    temp = 7;
                    break;
                case ":eight:":
                    temp = 8;
                    break;
                case ":nine:":
                    temp = 9;
                    break;
            }
            return temp - 1;
        }

        public int GetIndex(IEmote Emote)
        {
            var temp = 0;
            switch (Emote.Name)
            {
                case "1️⃣":
                    temp = 1;
                    break;
                case "2️⃣":
                    temp = 2;
                    break;
                case "3️⃣":
                    temp = 3;
                    break;
                case "4️⃣":
                    temp = 4;
                    break;
                case "5️⃣":
                    temp = 5;
                    break;
                case "6️⃣":
                    temp = 6;
                    break;
                case "7️⃣":
                    temp = 7;
                    break;
                case ":8️⃣":
                    temp = 8;
                    break;
                case "9️⃣":
                    temp = 9;
                    break;
            }
            return temp - 1;
        }

        public Emoji GetEmoji(int Index)
        {
            Index = Index + 1;
            switch (Index)
            {
                case 1:
                    return new Emoji("1️⃣");
                case 2:
                    return new Emoji("2️⃣");
                case 3:
                    return new Emoji("3️⃣");
                case 4:
                    return new Emoji("4️⃣");
                case 5:
                    return new Emoji("5️⃣");
                case 6:
                    return new Emoji("6️⃣");
                case 7:
                    return new Emoji("7️⃣");
                case 8:
                    return new Emoji("8️⃣");
                case 9:
                    return new Emoji("9️⃣");
            }
            return null;
        }

        public void LoadTab(int Index)
        {
            Console.WriteLine("loaded tab");
            try
            {
                if (TabList[Index].GetElementList().Length > 9)
                {
                    throw new Exception("Maximum number of elements in tab exceeded. Max is 9");
                }

                Message.RemoveAllReactionsAsync().GetAwaiter().GetResult();
                var eb = new EmbedBuilder();
                List<IEmote> EmojiList = new List<IEmote>()
                {
                    new Emoji("⬅️")
                };
                for (int x = 0; x < TabList[Index].GetElementList().Length; x++)
                {
                    switch (TabList[Index].GetElementList()[x].Type)
                    {
                        case GUIElementType.Button:
                            var button = (Button)TabList[Index].GetElementList()[x];
                            EmojiList.Add(this.GetCustomEmote(button.GetEmoji()));
                            eb.AddField(button.GetLabelText(), this.GetCustomEmote(button.GetEmoji()), button.GetInline());
                            break;
                        case GUIElementType.Textbox:
                            var textbox = (Textbox)TabList[Index].GetElementList()[x];
                            EmojiList.Add(this.GetEmoji(x));
                            eb.AddField(textbox.GetLabelText(), $"{GetIndexEmoji(x)} -> {textbox.GetText()}", textbox.GetInline());
                            break;
                        case GUIElementType.Field:
                            var field = (Field)TabList[Index].GetElementList()[x];
                            eb.AddField(field.GetLabelText(), field.GetText(), field.GetInline());
                            break;
                        case GUIElementType.Checkbox:
                            var checkbox = (Checkbox)TabList[Index].GetElementList()[x];
                            EmojiList.Add(this.GetEmoji(x));
                            eb.AddField(checkbox.GetLabelText(), $"{GetIndexEmoji(x)} -> {checkbox.GetChecked()}", checkbox.GetInline());
                            break;
                        case GUIElementType.Dropdown:
                            var dropdown = (Dropdown)TabList[Index].GetElementList()[x];
                            EmojiList.Add(this.GetEmoji(x));
                            eb.AddField(dropdown.GetLabelText(), $"{GetIndexEmoji(x)} -> {dropdown.GetComboBox()}", dropdown.GetInline());
                            break;
                    }
                }
                Message.ModifyAsync(msg => msg.Embed = eb.Build());
                EmojiList.Add(new Emoji("❌"));
                EmojiList.Add(new Emoji("➡️"));
                this.AddEmotes(EmojiList);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message} {ex.StackTrace}");
            }
        }
    }
}
