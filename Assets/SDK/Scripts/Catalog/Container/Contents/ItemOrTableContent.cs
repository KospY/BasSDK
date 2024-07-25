using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{[System.Serializable]
    public abstract class ItemOrTableContent<J, K> : ContainerContent<J, K> where J : CatalogData, IContainerLoadable<J> where K : ItemOrTableContent<J, K>
    {
#if ODIN_INSPECTOR
        [HorizontalGroup("Fields", Width = 75), BoxGroup("Fields/Quantity"), HideLabel, PropertyOrder(10)]
#endif
        public int quantity = 1;

#if ODIN_INSPECTOR
        [HorizontalGroup("Fields"), BoxGroup("Fields/State"), HideLabel, PropertyOrder(11)]
#endif
        public ContentState state;

#if ODIN_INSPECTOR
        [HorizontalGroup("Fields"), BoxGroup("Fields/Custom data"), HideLabel, PropertyOrder(12)]
#endif
        public List<ContentCustomData> customDataList;

    }
}
