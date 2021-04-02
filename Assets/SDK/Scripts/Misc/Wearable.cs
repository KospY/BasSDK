using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.AddressableAssets;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Wearable")]
    public class Wearable : Interactable
    {
        public string wardrobeChannel;
        public string[] wardrobeLayers;

    }
}
