using UnityEngine;
using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class LiquidData : CatalogData
    {
        [Multiline]
        public string designation;
        public Color color;

        public class Content
        {
#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllLiquidID))]
#endif
            public string liquidId;
            [NonSerialized]
            public LiquidData liquidData;
            public float level;

            public Content Clone()
            {
                Content clonedContent = MemberwiseClone() as Content;
                if (liquidData != null) clonedContent.liquidData = liquidData.Clone() as LiquidData;
                return clonedContent;
            }

            public Content()
            {
                this.liquidId = "";
                this.liquidData = null;
                this.level = 0;
            }

            public Content(LiquidData liquidData, float level)
            {
                this.liquidId = liquidData.id;
                this.liquidData = liquidData.Clone() as LiquidData;
                this.level = level;
            }

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllLiquidID()
            {
                return Catalog.GetDropdownAllID(Category.Liquid);
            }
#endif
        }

    }
}