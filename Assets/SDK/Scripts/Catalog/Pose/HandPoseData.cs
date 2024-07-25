using System;
using UnityEngine;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
	[Serializable]
    public class HandPoseData : CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("HandPose")] 
#endif
        public List<Pose> poses = new List<Pose>();

        public enum FingerType
        {
            Thumb,
            Index,
            Middle,
            Ring,
            Little
        }

        [Button]
        public void MirrorRightTipToLeft()
        {
            foreach (Pose pose in poses)
            {
                foreach (FingerType fingerType in Enum.GetValues(typeof(FingerType)))
                {
                    Pose.Finger rightFinger = pose.rightFingers.GetFinger(fingerType);
                    Pose.Finger leftFinger = pose.leftFingers.GetFinger(fingerType);
                    leftFinger.tipLocalPosition = new Vector3(rightFinger.tipLocalPosition.x, -rightFinger.tipLocalPosition.y, rightFinger.tipLocalPosition.z);
                }
            }
        }

        [Button]
        public void MirrorAllPoseTipLocals()
        {
            foreach (HandPoseData handPose in Catalog.GetDataList<HandPoseData>())
            {
                handPose.MirrorRightTipToLeft();
            }
        }

        [Serializable]
        public class Pose
        {
#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllCreatureName))] 
#endif
            [JsonMergeKey]
            public string creatureName;

            public Fingers leftFingers = new Fingers();
            public Fingers rightFingers = new Fingers();

            MirrorParams mirrorParams = new MirrorParams();

            [Serializable]
            public class MirrorParams
            {
                public bool invertQuaternionX = true;
                public bool invertQuaternionY;
                public bool invertQuaternionZ = true;
                public bool invertQuaternionW;
                public bool invertPositionX;
                public bool invertPositionY = true;
                public bool invertPositionZ;
                public bool gripReflectX;
                public bool gripReflectY;
                public bool gripReflectZ;
            }

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

                public Finger GetFinger(FingerType type)
                {
                    return type switch
                    {
                        FingerType.Thumb => thumb,
                        FingerType.Index => index,
                        FingerType.Middle => middle,
                        FingerType.Ring => ring,
                        FingerType.Little => little,
                        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                    };
                }
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

            public void Save(Side side, MirrorParams mirrorParams)
            {
#if UNITY_EDITOR
                this.mirrorParams = mirrorParams;
                if (side == Side.Right)
                {
                    SaveFinger(leftFingers.thumb, rightFingers.thumb);
                    SaveFinger(leftFingers.index, rightFingers.index);
                    SaveFinger(leftFingers.middle, rightFingers.middle);
                    SaveFinger(leftFingers.ring, rightFingers.ring);
                    SaveFinger(leftFingers.little, rightFingers.little);
                    leftFingers.gripLocalPosition = MirrorPosition(rightFingers.gripLocalPosition);
                    leftFingers.gripLocalRotation = MirrorRotation(rightFingers.gripLocalRotation);
                    leftFingers.rootLocalPosition = MirrorPosition(rightFingers.rootLocalPosition);
                }
                else if (side == Side.Left)
                {
                    SaveFinger(rightFingers.thumb, leftFingers.thumb);
                    SaveFinger(rightFingers.index, leftFingers.index);
                    SaveFinger(rightFingers.middle, leftFingers.middle);
                    SaveFinger(rightFingers.ring, leftFingers.ring);
                    SaveFinger(rightFingers.little, leftFingers.little);
                    rightFingers.gripLocalPosition = MirrorPosition(leftFingers.gripLocalPosition);
                    rightFingers.gripLocalRotation = MirrorRotation(leftFingers.gripLocalRotation);
                    rightFingers.rootLocalPosition = MirrorPosition(leftFingers.rootLocalPosition);
                }
#endif
            }

            protected void SaveFinger(Finger finger1, Finger finger2)
            {
                finger1.proximal.localPosition = MirrorPosition(finger2.proximal.localPosition);
                finger1.proximal.localRotation = InverseRotation(finger2.proximal.localRotation);
                finger1.intermediate.localPosition = MirrorPosition(finger2.intermediate.localPosition);
                finger1.intermediate.localRotation = InverseRotation(finger2.intermediate.localRotation);
                finger1.distal.localPosition = MirrorPosition(finger2.distal.localPosition);
                finger1.distal.localRotation = InverseRotation(finger2.distal.localRotation);
                finger1.tipLocalPosition = MirrorPosition(finger2.tipLocalPosition);
            }

            public Fingers GetFingers(Side side)
            {
                return side == Side.Right ? rightFingers : leftFingers;
            }

            private Quaternion ReflectRotation(Quaternion source, Vector3 normal)
            {
                return Quaternion.LookRotation(Vector3.Reflect(source * Vector3.forward, normal), Vector3.Reflect(source * Vector3.up, normal));
            }

            protected Vector3 MirrorPosition(Vector3 position)
            {
                if (mirrorParams.invertPositionX) position.x *= -1;
                if (mirrorParams.invertPositionY) position.y *= -1;
                if (mirrorParams.invertPositionZ) position.z *= -1;
                return position;
            }

            protected Quaternion MirrorRotation(Quaternion rotation)
            {
                if (mirrorParams.gripReflectX) rotation = ReflectRotation(rotation, Vector3.right);
                if (mirrorParams.gripReflectY) rotation = ReflectRotation(rotation, Vector3.up);
                if (mirrorParams.gripReflectZ) rotation = ReflectRotation(rotation, Vector3.forward);
                return rotation;
            }

            protected Quaternion InverseRotation(Quaternion rotation)
            {
                if (mirrorParams.invertQuaternionX) rotation.x *= -1;
                if (mirrorParams.invertQuaternionY) rotation.y *= -1;
                if (mirrorParams.invertQuaternionZ) rotation.z *= -1;
                if (mirrorParams.invertQuaternionW) rotation.w *= -1;
                return rotation;
            }

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllCreatureName()
            {
                List<ValueDropdownItem<string>> dropdownList = new List<ValueDropdownItem<string>>();
                foreach (CreatureData creatureData in Catalog.GetDataList(Category.Creature))
                {
                    if (dropdownList.Exists(d => d.Value == creatureData.name))
                    {
                        continue;
                    }
                    dropdownList.Add(new ValueDropdownItem<string>(creatureData.name, creatureData.name));
                }
                return dropdownList;
            } 
#endif
        }

        public override int GetCurrentVersion()
        {
            return 2;
        }

        public Pose GetCreaturePose(Creature creature)
        {
            Pose creaturePose = GetCreaturePose(creature.data.name);
            return creaturePose;
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
            #if UNITY_EDITOR
            Debug.LogWarning("No pose found for creature " + creatureName + " in hand pose data " + id);
            #endif
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
