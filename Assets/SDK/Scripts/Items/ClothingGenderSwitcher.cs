using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Profiling;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Items/Clothing Gender Switcher")]
    [RequireComponent(typeof(Item))]
    public class ClothingGenderSwitcher : MonoBehaviour
    {
        public GameObject maleModel;
        public GameObject femaleModel;

    }
}
