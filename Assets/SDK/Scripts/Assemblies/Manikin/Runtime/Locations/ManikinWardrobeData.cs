using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ThunderRoad.Manikin
{
    [CreateAssetMenu(menuName = "Manikin/Wardrobe Data")]
    public class ManikinWardrobeData : ScriptableObject, IManikinPartPreview
    {
#if UNITY_EDITOR
        public ManikinEditorLocationLabels editorLocationLabels; //make assetReference? this is only used in the editor to see a list of available names
#endif

        public AssetReferenceManikinPart assetPrefab; //will loading this scriptableObject automatically load this prefab too? we don't want that so that the part list updating isn't delayed.
        public string[] channels;
        public int[] layers;
        public int[] fullyOccludedLayers;
        public int[] partialOccludedLayers;
        public int[] partialOccludedMasks;

        [Tooltip("This is the ID used to match with the ID on a ManikinPart to know to apply the occlusion bitmask to.")]
        public string occlusionID;
        [Tooltip("Hashed version of the string OcclusionID")]
        public int occlusionIDHash;

        public string[] tags;

        public ManikinWardrobeData[] dependencies;

        void Awake()
        {
            if(occlusionIDHash == 0 && !string.IsNullOrEmpty(occlusionID))
            {
                occlusionIDHash = Animator.StringToHash(occlusionID);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            occlusionIDHash = Animator.StringToHash(occlusionID);
        }

        public Texture2D GetOrCreatePreview(string id, string path, int width, int height)
        {
            if(assetPrefab != null && assetPrefab.editorAsset != null && (assetPrefab.editorAsset as GameObject).TryGetComponent(out ManikinPart part))
            {
                return part.GetOrCreatePreview(assetPrefab.AssetGUID.ToString(), path, width, height);
            }
            return null;
        }

        public Texture2D CreatePreview(string id, string path, int width, int height)
        {
            //Let's never create previews for wardrobes, always use it's asset prefab.
            return null;
        }

        public Texture2D GetPreview(string id, string path)
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(path + "/" + id + ".asset");
        }
#endif
    }
}
