using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else 
using EasyButtons;
#endif
using System.Collections.Generic;

namespace ThunderRoad
{
    [Serializable]
    public class HandPoseData : CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("HandPose")]
#endif
        public List<Pose> poses = new List<Pose>();

        [Serializable]
        public class Pose
        {
            public string creatureName;

            public Fingers leftFingers = new Fingers();
            public Fingers rightFingers = new Fingers();

            [Serializable]
            public class Fingers
            {
                public Finger thumb = new Finger();
                public Finger index = new Finger();
                public Finger middle = new Finger();
                public Finger ring = new Finger();
                public Finger little = new Finger();
                public Vector3 gripLocalPosition;
                public Quaternion gripLocalRotation;
                public Vector3 rootLocalPosition;
            }

            [Serializable]
            public class Finger
            {
                public Bone proximal = new Bone();
                public Bone intermediate = new Bone();
                public Bone distal = new Bone();
                public Vector3 tipLocalPosition;
                [Serializable]
                public class Bone
                {
                    public Vector3 localPosition;
                    public Quaternion localRotation;
                }
            }

            public Pose(string creatureName)
            {
                this.creatureName = creatureName;
            }

            public void Save(Side side)
            {
#if UNITY_EDITOR
                if (side == Side.Right)
                {
                    SaveFinger(leftFingers.thumb, rightFingers.thumb);
                    SaveFinger(leftFingers.index, rightFingers.index);
                    SaveFinger(leftFingers.middle, rightFingers.middle);
                    SaveFinger(leftFingers.ring, rightFingers.ring);
                    SaveFinger(leftFingers.little, rightFingers.little);
                    leftFingers.gripLocalPosition = new Vector3(rightFingers.gripLocalPosition.x, -rightFingers.gripLocalPosition.y, rightFingers.gripLocalPosition.z);
                    leftFingers.gripLocalRotation = ReflectRotation(rightFingers.gripLocalRotation, Vector3.up);
                    leftFingers.rootLocalPosition = new Vector3(rightFingers.rootLocalPosition.x, -rightFingers.rootLocalPosition.y, rightFingers.rootLocalPosition.z);
                }
                else if (side == Side.Left)
                {
                    SaveFinger(rightFingers.thumb, leftFingers.thumb);
                    SaveFinger(rightFingers.index, leftFingers.index);
                    SaveFinger(rightFingers.middle, leftFingers.middle);
                    SaveFinger(rightFingers.ring, leftFingers.ring);
                    SaveFinger(rightFingers.little, leftFingers.little);
                    rightFingers.gripLocalPosition = new Vector3(leftFingers.gripLocalPosition.x, -leftFingers.gripLocalPosition.y, leftFingers.gripLocalPosition.z);
                    rightFingers.gripLocalRotation = ReflectRotation(leftFingers.gripLocalRotation, Vector3.up);
                    rightFingers.rootLocalPosition = new Vector3(leftFingers.rootLocalPosition.x, -leftFingers.rootLocalPosition.y, leftFingers.rootLocalPosition.z);
                }
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

            public Fingers GetFingers(Side side)
            {
                return side == Side.Right ? rightFingers : leftFingers;
            }

        }

        public override int GetCurrentVersion()
        {
            return 2;
        }

        public Pose GetCreaturePose(Creature creature)
        {
            return GetCreaturePose(creature.data.name);
        }

        public Pose GetCreaturePose(string creatureName)
        {
            int posesCount = poses.Count;
            for (var i = 0; i < posesCount; i++)
            {
                Pose pose = poses[i];
                if (creatureName == pose.creatureName)
                {
                    return pose;
                }
            }
            return null;
        }

        public Pose AddCreaturePose(Creature creature)
        {
            return AddCreaturePose(creature.data.name);
        }

        public Pose AddCreaturePose(string creatureName)
        {
            Pose newCreaturePose = new Pose(creatureName);
            poses.Add(newCreaturePose);
            return newCreaturePose;
        }
    }
}
