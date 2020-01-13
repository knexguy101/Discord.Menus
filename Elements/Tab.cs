using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Discord.Menus.Elements
{
    public class Tab : GUIElement
    {
        private List<GUIElement> ElementList = new List<GUIElement>();

        public Tab()
        {
            this.Type = GUIElementType.Tab;
        }

        public void AddElement(GUIElement Element)
        {
            this.ElementList.Add(Element);
        }

        public void RemoveElement(GUIElement Element)
        {
            this.ElementList.Remove(Element);
        }

        public GUIElement[] GetElementList()
        {
            return ElementList.ToArray();
        }
    }
}
