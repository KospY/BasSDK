using UnityEngine;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class BoneColliderToRagdollParts : MonoBehaviour
    {
        public Transform rig;
        public bool addParts = true;
        public List<Transform> partsWithSubBones = new List<Transform>();

        [Button]
        public void Transfer()
        {
            Dictionary<Transform, List<Collider>> colliderParents = new Dictionary<Transform, List<Collider>>();
            List<Collider> allColliders = new List<Collider>();
            allColliders.AddRange(rig.GetComponentsInChildren<Collider>());
            foreach (Transform bone in partsWithSubBones)
            {
                var subBoneColliders = bone.GetComponentsInChildren<Collider>();
                colliderParents.Add(bone, new List<Collider>());
                foreach (Collider collider in subBoneColliders)
                {
                    colliderParents[bone].Add(collider);
                    allColliders.Remove(collider);
                }
            }
            foreach (Collider collider in allColliders)
            {
                Transform parent = collider.transform.parent;
                if (!colliderParents.TryGetValue(parent, out List<Collider> childs))
                {
                    childs = new List<Collider>();
                    colliderParents.Add(parent, childs);
                }
                childs.Add(collider);
            }
            Dictionary<Transform, RagdollPart> bonedParts = new Dictionary<Transform, RagdollPart>();
            foreach (KeyValuePair<Transform, List<Collider>> parentChilds in colliderParents)
            {
                Transform newPart = transform.FindOrAddTransform(parentChilds.Key.name, parentChilds.Key.position, parentChilds.Key.rotation, parentChilds.Key.lossyScale);
                foreach (Collider collider in parentChilds.Value)
                {
                    collider.transform.parent = newPart;
                }
                if (addParts)
                {
                    RagdollPart newRagdollPart = newPart.gameObject.AddComponent<RagdollPart>();
                    newRagdollPart.meshBone = parentChilds.Key;
                    bonedParts[parentChilds.Key] = newRagdollPart;
                }
            }
            foreach (KeyValuePair<Transform, RagdollPart> bonedPart in bonedParts)
            {
                RagdollPart part = bonedPart.Value;
                Transform boneParent = bonedPart.Key.parent;
                while (boneParent != null)
                {
                    if (bonedParts.TryGetValue(boneParent, out RagdollPart parentPart))
                    {
                        part.parentPart = parentPart;
                        break;
                    }
                    boneParent = boneParent.parent;
                }
            }
        }
    }
}
