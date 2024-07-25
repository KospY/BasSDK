using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad.Manikin
{
    /// <summary>
    /// BoneScaleConverters should usually only be used in one place in the hierarchy, likely the root.
    /// </summary>
    [CreateAssetMenu(menuName = "Manikin/Converters/BoneScale Converter")]
    public class ManikinPropertyBoneScaleConverter : ManikinPropertyConverterBase
    {
        public string boneName;
        public float minScale = 0.1f;
        public float maxScale = 2.0f;
        public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [SerializeField, HideInInspector]
        private int boneNameHash = -1;

        private void OnValidate()
        {
            boneNameHash = Animator.StringToHash(boneName);
        }

        public override void ApplyProperty(GameObject obj, float[] values, bool useSRPBatcher = false, Renderer renderer = null, int materialIndex = 0, object payload = null)
        {
            if(boneNameHash != -1 && obj.TryGetComponent(out ManikinRig rig))
            {
                if (rig.bones.TryGetValue(boneNameHash, out ManikinRig.BoneData boneData))
                {
                    float scale = Mathf.Lerp(minScale, maxScale, curve.Evaluate(values[0]));
                    boneData.boneTransform.localScale = new Vector3(scale, scale, scale);
                }
            }
        }
    }
}
