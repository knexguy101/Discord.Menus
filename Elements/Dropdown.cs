using System;
using System.Collections.Generic;
using System.Text;

namespace Discord.Menus.Elements
{
    public class Dropdown : GUIElement
    {
        private string[] ItemList { get; set; }
        private int CurrentIndex { get; set; }
        private string LabelText { get; set; }
        private bool Inline { get; set; }

        public Dropdown(string[] ItemList, string LabelText, int CurrentIndex = 0, bool Inline = false)
        {
            this.Type = GUIElementType.Dropdown;
            this.CurrentIndex = CurrentIndex;
            this.ItemList = ItemList;
            this.LabelText = LabelText;
            this.Inline = Inline;

            if(this.ItemList.Length > 9)
            {
                throw new Exception("Item List too large, maximum is 9 items");
            }
        } 

        public string[] GetItemList()
        {
            return ItemList;
        }
        
        public void SetIndex(int Index)
        {
            this.CurrentIndex = Index;
        }

        public string GetComboBox()
        {
            return $"{ItemList[this.CurrentIndex]}    **V**";
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
