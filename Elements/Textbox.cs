using System;
using System.Collections.Generic;
using System.Text;

namespace Discord.Menus.Elements
{
    public class Textbox : GUIElement
    {
        private string Text { get; set; }
        private string LabelText { get; set; }
        private bool Inline { get; set; }

        public Textbox(string Text, string LabelText, bool Inline = false)
        {
            this.Type = GUIElementType.Textbox;
            this.Text = Text;
            this.LabelText = LabelText;
            this.Inline = Inline;
        }

        public void SetText(string Text)
        {
            this.Text = Text;
        }

        public string GetText()
        {
            return Text;
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
