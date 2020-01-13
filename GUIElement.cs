using System;
using System.Collections.Generic;
using System.Text;

namespace Discord.Menus
{
    public class GUIElement
    {
        public GUIElementType Type { get; set; }
    }

    public enum GUIElementType
    {
        Button, Textbox, Checkbox, Field, Tab, Dropdown
    }
}
