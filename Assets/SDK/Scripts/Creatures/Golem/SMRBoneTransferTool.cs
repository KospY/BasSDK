using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class SMRBoneTransferTool : MonoBehaviour
    {
        public GameObject sourceHierarchy;
        //public Animator sourceAnimator;

        public GameObject targetHierarchy;
        //public Animator targetAnimator;

        [Button]
        public void ReBone()
        {
            Dictionary<string, Transform> boneMap = new Dictionary<string, Transform>();
            foreach (SkinnedMeshRenderer smr in sourceHierarchy.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                foreach (Transform bone in smr.bones)
                {
                    boneMap[bone.name] = bone;
                }
            }

            foreach (SkinnedMeshRenderer smr in targetHierarchy.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                if (boneMap.TryGetValue(smr.rootBone.name, out Transform sourceRootBone)) smr.rootBone = sourceRootBone;
                else Debug.LogError("failed to get bone: " + smr.rootBone.name);
                Transform[] boneArray = smr.bones;
                for (int idx = 0; idx < boneArray.Length; ++idx)
                {
                    string boneName = boneArray[idx].name;
                    if (!boneMap.TryGetValue(boneName, out boneArray[idx])) Debug.LogError("failed to get bone: " + boneName);
                }
                smr.bones = boneArray;
            }
        }
    }
}
