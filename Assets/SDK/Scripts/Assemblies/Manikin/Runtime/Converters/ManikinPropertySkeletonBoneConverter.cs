using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad.Manikin
{
    [CreateAssetMenu(menuName = "Manikin/Converters/SkeletonBone Converter")]
    public class ManikinPropertySkeletonBoneConverter : ManikinPropertyConverterBase
    {
        public string boneName;
        public Vector3 negativeOffset;
        public Vector3 positiveOffset;
        public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [SerializeField, HideInInspector]
        private int boneNameHash = -1;

        private void OnValidate()
        {
            boneNameHash = Animator.StringToHash(boneName);
        }

        public override void ApplyProperty(GameObject obj, float[] values, bool useSRPBatcher = false, Renderer renderer = null, int materialIndex = 0, object payload = null)
        {
            if(obj.TryGetComponent(out ManikinRig rig))
            {
                if(rig.TryGetDefaultSkeletonBone(boneNameHash, out int index, out SkeletonBone bone))
                {
                    float value = curve.Evaluate(Mathf.Abs(values[0]));

                    bone.position += (values[0] >= 0) ? value * positiveOffset : value * negativeOffset;
                    if (rig.SetCurrentSkeletonBoneByIndex(index, bone))
                    {
                        rig.DirtySkeletonData();
                    }
                }
            }
        }
    }
}
