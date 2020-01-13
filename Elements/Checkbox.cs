using System;
using System.Collections.Generic;
using System.Text;

namespace Discord.Menus.Elements
{
    public class Checkbox : GUIElement
    {
        private bool Checked { get; set; }
        private string LabelText { get; set; }
        private bool Inline { get; set; }

        public Checkbox(bool Checked, string LabelText, bool Inline = false)
        {
            this.Type = GUIElementType.Checkbox;
            this.Checked = Checked;
            this.LabelText = LabelText;
            this.Inline = Inline;
        }

        public void SetCheck(string Text)
        {
            this.Checked = Text.ToLower() == "on" ? true : false;
        }

        public string GetChecked()
        {
            return Checked ? ":white_check_mark:" : ":x:";
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
