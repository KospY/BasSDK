using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    public class ObjectSpawner : MonoBehaviour
    {
        public string address;
        public bool spawnOnStart = true;
        public bool disableOnMobile;

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
            if (disableOnMobile && Application.isMobilePlatform) return;
            Addressables.InstantiateAsync(address, this.transform.position, this.transform.rotation, this.transform);
        }
    }
}
