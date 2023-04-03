using System;
using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceLocations;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class MenuData : CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Menu")] 
#endif
        public float order;

#if ODIN_INSPECTOR
        [BoxGroup("Menu")] 
#endif
        public bool isDefault;

#if ODIN_INSPECTOR
        [BoxGroup("Menu")]
#endif
        public string nameId;

#if ODIN_INSPECTOR
        [BoxGroup("Menu")]
#endif
        public string iconAddress;
        [NonSerialized]
        public IResourceLocation iconLocation;

#if ODIN_INSPECTOR
        [BoxGroup("Menu")]
#endif
        public string iconRollhoverAddress;
        [NonSerialized]
        public IResourceLocation iconRollhoverLocation;

#if ODIN_INSPECTOR
        [BoxGroup("Menu")]
#endif
        public string prefabAddress;
        [NonSerialized]
        public IResourceLocation prefabLocation;

#if ODIN_INSPECTOR
        [BoxGroup("Menu"), ShowInInspector] 
#endif
        public MenuModule module;

#if ODIN_INSPECTOR
        [BoxGroup("Instance"), ShowInInspector, ReadOnly]
#endif
        [NonSerialized]
        public Transform navigationPane;
#if ODIN_INSPECTOR
        [BoxGroup("Instance"), ShowInInspector, ReadOnly]
#endif
        [NonSerialized]
        public Transform contentArea;

        public override int GetCurrentVersion()
        {
            return 1;
        }
    }
}
