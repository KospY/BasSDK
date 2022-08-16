using System.Collections.Generic;
using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;

namespace ThunderRoad
{
    [Serializable]
    public class CreatureData : CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public string name;

    }
}
