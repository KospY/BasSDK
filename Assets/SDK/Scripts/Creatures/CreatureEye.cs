using System.Collections.Generic;
using UnityEngine;
using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/CreatureEye")]
    [AddComponentMenu("ThunderRoad/Creatures/Creature Eye")]
    public class CreatureEye : MonoBehaviour
    {
        [System.Serializable]
        public class EyeMoveable
        {
            public string name;
            public Transform transform;
            public RotationCurves curves;
            public bool isFixed { get; private set; } = false;

            public void ParentingFix()
            {
                if (isFixed) return;
                isFixed = true;
                transform = transform.parent;
            }

            [System.Serializable]
            public class RotationCurves
            {
                public AnimationCurve closeCurveX;
                public AnimationCurve closeCurveY;
                public AnimationCurve closeCurveZ;
                public AnimationCurve closeCurveW;

                public Quaternion TimeToQuaternion(float t)
                {
                    return new Quaternion(closeCurveX.Evaluate(t), closeCurveY.Evaluate(t), closeCurveZ.Evaluate(t), closeCurveW.Evaluate(t));
                }

                public void SetKeyframeByRotation(Transform pull, float t)
                {
                    AddOrMoveKeyframe(closeCurveX, t, pull.localRotation.x);
                    AddOrMoveKeyframe(closeCurveY, t, pull.localRotation.y);
                    AddOrMoveKeyframe(closeCurveZ, t, pull.localRotation.z);
                    AddOrMoveKeyframe(closeCurveW, t, pull.localRotation.w);
                }

                public void AddOrMoveKeyframe(AnimationCurve curve, float t, float v)
                {
                    if (curve.AddKey(t, v) != -1) return;
                    Keyframe toChange = new Keyframe();
                    foreach (Keyframe frame in curve.keys)
                    {
                        if (frame.time == t)
                        {
                            toChange = frame;
                            break;
                        }
                    }
                    toChange.value = v;
                }

                [Button]
                public void ClearCurves()
                {
                    closeCurveX = new AnimationCurve();
                    closeCurveY = new AnimationCurve();
                    closeCurveZ = new AnimationCurve();
                    closeCurveW = new AnimationCurve();
                }
            }
        }

        [Range(0f, 1f)]
        public float closeAmount = 0f;
        [NonSerialized]
        public float lastUpdateTime = 0f;

        [Button]
        public void SetKeyframes()
        {
            foreach (EyeMoveable moveable in eyeParts)
            {
                moveable.curves.SetKeyframeByRotation(moveable.transform, closeAmount);
            }
        }

        [Button]
        public void SetClose()
        {
            foreach (EyeMoveable moveable in eyeParts)
            {
                if (Application.isPlaying && !moveable.isFixed) continue;
                if (moveable.transform) moveable.transform.localRotation = moveable.curves.TimeToQuaternion(closeAmount);
            }
        }

        public string eyeTag = "";
        public List<EyeMoveable> eyeParts = new List<EyeMoveable>();
    }
}
