using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad.Manikin
{
    [RequireComponent(typeof(MeshRenderer)), DisallowMultipleComponent()]
    public class ManikinMeshPart : ManikinPart
    {
        public bool useAttachment;
        public string boneAttachment;

        [SerializeField, ShowOnlyInspector]
        private MeshRenderer meshRenderer;

        [SerializeField, HideInInspector]
        private int boneHash;

        protected override void Awake()
        {
            boneHash = Animator.StringToHash(boneAttachment);

            if (meshRenderer == null) { meshRenderer = GetComponent<MeshRenderer>(); }
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if (meshRenderer == null) { meshRenderer = GetComponent<MeshRenderer>(); }
        }

        /// <summary>
        /// This is called when creating the part prefab in the editor.
        /// </summary>
        //public virtual void Initialize() { }

        public override Texture2D CreatePreview(string id, string path, int width, int height)
        {
            Renderer[] renderers = GetRenderers();
            if (renderers != null)
            {
                Texture2D preview = ManikinPart.DrawPreview(renderers, rootRotation, width, height);
                preview.name = id;

                SavePreview(id, path, preview);
                return preview;
            }

            Debug.LogWarning(name + " has no renderers for preview!");
            return null;
        }
#endif

        public override ManikinPart Instantiate(GameObject parent, ManikinRig rig = null)
        {
            if (useAttachment && rig != null)
            {
                if (string.IsNullOrEmpty(boneAttachment))
                    return null;

                if (rig.bones.TryGetValue(boneHash, out ManikinRig.BoneData boneData))
                {
                    return base.Instantiate(boneData.boneTransform.gameObject);
                }
            }

            return base.Instantiate(parent);
        }

        public override bool PartOfBone(int hash)
        {
            if (useAttachment)
            {
                return hash == boneHash;
            }
            return false;
        }

        public override Renderer[] GetRenderers() 
        {
            return new Renderer[1] { meshRenderer };
        }
    }
}
