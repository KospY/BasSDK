using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace ThunderRoad
{
    [CreateAssetMenu(menuName = "ThunderRoad/Area/Biome Addressable Config")]
    public class BiomeAddressableConfig : ScriptableObject
    {
        
        public AddressableAssetGroup CommonPrefabGroup;
        public AddressableAssetGroup HDPrefabGroup;
        public AddressableAssetGroup AndroidPrefabGroup;

        public AddressableAssetGroup CommonResourcesGroup;
        public AddressableAssetGroup HDResourcesGroup;
        public AddressableAssetGroup AndroidResourcesGroup;

    }
}
