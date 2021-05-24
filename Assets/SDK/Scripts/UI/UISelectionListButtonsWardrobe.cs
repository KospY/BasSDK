using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ThunderRoad
{
    public class UISelectionListButtonsWardrobe : UISelectionListButtons
    {
        public ItemModuleWardrobe.Category category = ItemModuleWardrobe.Category.Body;
        public string channel = "Head";
        public string layer = "Hair";

        public bool allowNothing = true;

    }
}