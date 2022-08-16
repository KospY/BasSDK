using UnityEngine;
using UnityEngine.AddressableAssets;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/PrefabSpawner")]
    [AddComponentMenu("ThunderRoad/Levels/Spawners/Prefab Spawner")]
    public class PrefabSpawner : MonoBehaviour
    {
        public string address;
        public bool spawnOnStart = true;
        public Platform platform = Platform.Android | Platform.Windows;

        [Flags]
        public enum Platform
        {
            Windows = 1,
            Android = 2,
        }

        protected void Start()
        {
            if (spawnOnStart)
            {
                Spawn();
            }
        }

        [Button]
        public void Spawn()
        {
            if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && platform.HasFlag(Platform.Windows))
            {
                Addressables.InstantiateAsync(address, this.transform.position, this.transform.rotation, this.transform);
            }
            else if (Application.platform == RuntimePlatform.Android && platform.HasFlag(Platform.Android))
            {
                Addressables.InstantiateAsync(address, this.transform.position, this.transform.rotation, this.transform);
            }
        }
    }
}
