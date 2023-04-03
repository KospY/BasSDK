using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ThunderRoad.Manikin
{
    [System.Serializable]
    public class AssetReferenceManikinWardrobe : AssetReference
    {
        public AssetReferenceManikinWardrobe(string guid) : base(guid) { }

        public override bool ValidateAsset(Object obj)
        {
            var wardrobe = obj as ManikinWardrobeData;
            return wardrobe != null;
        }

        public override bool ValidateAsset(string path)
        {
#if UNITY_EDITOR
            //this load can be expensive...
            var wardrobe = AssetDatabase.LoadAssetAtPath<ManikinWardrobeData>(path);
            return wardrobe != null;
#else
            return false;
#endif
        }

        public void ReleaseInstance(AsyncOperationHandle<ManikinWardrobeData> op)
        {
            // Release the handle
            Addressables.Release(op);
        }
    }
}
