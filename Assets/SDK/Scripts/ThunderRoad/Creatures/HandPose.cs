using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [CreateAssetMenu(menuName = "ThunderRoad/Creatures/Hand pose")]
    public class HandPose : ScriptableObject
    {
        public Fingers left;
        public Fingers right;

        [Serializable]
        public class Fingers
        {
            public Finger thumb;
            public Finger index;
            public Finger middle;
            public Finger ring;
            public Finger little;
            public Vector3 gripLocalPosition;
            public Quaternion gripLocalRotation;
        }

        [Serializable]
        public class Finger
        {
            public Bone proximal;
            public Bone intermediate;
            public Bone distal;
            [Serializable]
            public class Bone
            {
                public Vector3 localPosition;
                public Quaternion localRotation;
            }
        }

        public Fingers GetFingers(Side side)
        {
            return side == Side.Right ? right : left;
        }

        public void Save(Side side)
        {
#if UNITY_EDITOR
            if (side == Side.Right)
            {
                SaveFinger(left.thumb, right.thumb);
                SaveFinger(left.index, right.index);
                SaveFinger(left.middle, right.middle);
                SaveFinger(left.ring, right.ring);
                SaveFinger(left.little, right.little);
                left.gripLocalPosition = new Vector3(right.gripLocalPosition.x, -right.gripLocalPosition.y, right.gripLocalPosition.z);
                left.gripLocalRotation = ReflectRotation(right.gripLocalRotation, Vector3.up);
            }
            else if (side == Side.Left)
            {
                SaveFinger(right.thumb, left.thumb);
                SaveFinger(right.index, left.index);
                SaveFinger(right.middle, left.middle);
                SaveFinger(right.ring, left.ring);
                SaveFinger(right.little, left.little);
                right.gripLocalPosition = new Vector3(left.gripLocalPosition.x, -left.gripLocalPosition.y, left.gripLocalPosition.z);
                right.gripLocalRotation = ReflectRotation(left.gripLocalRotation, Vector3.up);
            }
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        private Quaternion ReflectRotation(Quaternion source, Vector3 normal)
        {
            return Quaternion.LookRotation(Vector3.Reflect(source * Vector3.forward, normal), Vector3.Reflect(source * Vector3.up, normal));
        }

        protected void SaveFinger(Finger finger1, Finger finger2)
        {
            finger1.proximal.localPosition = new Vector3(finger2.proximal.localPosition.x, -finger2.proximal.localPosition.y, finger2.proximal.localPosition.z);
            finger1.proximal.localRotation = new Quaternion(finger2.proximal.localRotation.x * -1.0f, finger2.proximal.localRotation.y, finger2.proximal.localRotation.z * -1.0f, finger2.proximal.localRotation.w);

            finger1.intermediate.localPosition = finger2.intermediate.localPosition;
            finger1.intermediate.localRotation = finger2.intermediate.localRotation;

            finger1.distal.localPosition = finger2.distal.localPosition;
            finger1.distal.localRotation = finger2.distal.localRotation;
        }
    }
}
