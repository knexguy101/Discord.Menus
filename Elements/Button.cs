using System;
using System.Collections.Generic;
using System.Text;

namespace Discord.Menus.Elements
{
    public class Button : GUIElement
    {
        private string EmojiTag { get; set; }
        private string LabelText { get; set; }
        private Action Action { get; set; }
        private bool Inline { get; set; }

        public Button(string EmojiTag, string LabelText, Action Action, bool Inline = false)
        {
            this.Type = GUIElementType.Button;
            this.Action = Action;
            this.EmojiTag = EmojiTag;
            this.LabelText = LabelText;
            this.Inline = Inline;
        }

        public Action GetAction()
        {
            return Action;
        }

        public string GetEmoji()
        {
            return EmojiTag;
        }

        public string GetLabelText()
        {
            return LabelText;
        }

        public bool GetInline()
        {
            return Inline;
        }
    }
}
