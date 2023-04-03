using System;

namespace ThunderRoad
{
	public class MenuModule
    {
        [NonSerialized]
        public MenuData menuData;

        [NonSerialized]
        public bool shown;

        public virtual void Init(MenuData menuData, UIMenu menu)
        {
            this.menuData = menuData;
        }

        public virtual void OnShow(bool show)
        {
            this.shown = show;
        }
    }
}
