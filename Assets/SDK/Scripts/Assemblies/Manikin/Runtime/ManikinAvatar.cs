using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad.Manikin
{
    public class ManikinAvatar : ScriptableObject, ISerializationCallbackReceiver
    {
        public Avatar avatar;
        public HumanDescription humanDescription;

        //HumanDescription
        [SerializeField]
        private bool hasTranslationDoF;
        [SerializeField]
        private float armStretch;
        [SerializeField]
        private float feetSpacing;
        [SerializeField]
        private float legStretch;
        [SerializeField]
        private float lowerArmTwist;
        [SerializeField]
        private float lowerLegTwist;
        [SerializeField]
        private float upperArmTwist;
        [SerializeField]
        private float upperLegTwist;

        //Human
        [SerializeField]
        private string[] humanBones_BoneName;
        [SerializeField]
        private string[] humanBones_HumanName;
        [SerializeField]
        private float[] humanLimits_axisLength;
        [SerializeField]
        private Vector3[] humanLimits_center;
        [SerializeField]
        private Vector3[] humanLimits_max;
        [SerializeField]
        private Vector3[] humanLimits_min;
        [SerializeField]
        private bool[] humanLimits_useDefaultValues;
        
        //Skeleton
        [SerializeField]
        private string[] skeletonBoneNames;
        [SerializeField]
        private Vector3[] skeletonBonePositions;
        [SerializeField]
        private Quaternion[] skeletonBoneRotations;
        [SerializeField]
        private Vector3[] skeletonBoneScales;


        public void OnBeforeSerialize()
        {
            hasTranslationDoF = humanDescription.hasTranslationDoF;
            armStretch = humanDescription.armStretch;
            feetSpacing = humanDescription.feetSpacing;
            legStretch = humanDescription.legStretch;
            lowerArmTwist = humanDescription.lowerArmTwist;
            lowerLegTwist = humanDescription.lowerLegTwist;
            upperArmTwist = humanDescription.upperArmTwist;
            upperLegTwist = humanDescription.upperLegTwist;

            HumanBone[] human = humanDescription.human;
            if(human != null)
            {
                humanBones_BoneName = new string[human.Length];
                humanBones_HumanName = new string[human.Length];
                humanLimits_axisLength = new float[human.Length];
                humanLimits_center = new Vector3[human.Length];
                humanLimits_max = new Vector3[human.Length];
                humanLimits_min = new Vector3[human.Length];
                humanLimits_useDefaultValues = new bool[human.Length];
                for (int i = 0; i < human.Length; i++)
                {
                    humanBones_BoneName[i] = human[i].boneName;
                    humanBones_HumanName[i] = human[i].humanName;
                    humanLimits_axisLength[i] = human[i].limit.axisLength;
                    humanLimits_center[i] = human[i].limit.center;
                    humanLimits_max[i] = human[i].limit.max;
                    humanLimits_min[i] = human[i].limit.min;
                    humanLimits_useDefaultValues[i] = human[i].limit.useDefaultValues;
                }
            }

            SkeletonBone[] skeleton = humanDescription.skeleton;
            if (skeleton != null)
            {
                skeletonBoneNames = new string[skeleton.Length];
                skeletonBonePositions = new Vector3[skeleton.Length];
                skeletonBoneRotations = new Quaternion[skeleton.Length];
                skeletonBoneScales = new Vector3[skeleton.Length];
                for (int i = 0; i < skeleton.Length; i++)
                {
                    skeletonBoneNames[i] = skeleton[i].name;
                    skeletonBonePositions[i] = skeleton[i].position;
                    skeletonBoneRotations[i] = skeleton[i].rotation;
                    skeletonBoneScales[i] = skeleton[i].scale;
                }
            }
        }

        public void OnAfterDeserialize()
        {
            humanDescription.hasTranslationDoF = hasTranslationDoF;
            humanDescription.armStretch = armStretch;
            humanDescription.feetSpacing = feetSpacing;
            humanDescription.legStretch = legStretch;
            humanDescription.lowerArmTwist = lowerArmTwist;
            humanDescription.lowerLegTwist = lowerLegTwist;
            humanDescription.upperArmTwist = upperArmTwist;
            humanDescription.upperLegTwist = upperLegTwist;
            
            if(humanBones_BoneName != null)
            {
                HumanBone[] human = new HumanBone[humanBones_BoneName.Length];
                for(int i = 0; i < human.Length; i++)
                {
                    human[i].boneName = humanBones_BoneName[i];
                    human[i].humanName = humanBones_HumanName[i];
                    human[i].limit = new HumanLimit
                    {
                        axisLength = humanLimits_axisLength[i],
                        center = humanLimits_center[i],
                        max = humanLimits_max[i],
                        min = humanLimits_min[i],
                        useDefaultValues = humanLimits_useDefaultValues[i]
                    };
                }
                humanDescription.human = human;
            }            

            if (skeletonBoneNames != null)
            {
                SkeletonBone[] skeleton = new SkeletonBone[skeletonBoneNames.Length];
                for (int i = 0; i < skeletonBoneNames.Length; i++)
                {
                    skeleton[i].name = skeletonBoneNames[i];
                    skeleton[i].position = skeletonBonePositions[i];
                    skeleton[i].rotation = skeletonBoneRotations[i];
                    skeleton[i].scale = skeletonBoneScales[i];
                }
                humanDescription.skeleton = skeleton;
            }
        }
    }
}
