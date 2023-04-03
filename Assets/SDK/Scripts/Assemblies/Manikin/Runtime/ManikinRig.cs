using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Profiling;

namespace ThunderRoad.Manikin
{
    /// <summary>
    /// This is a core required component to allow the system to work.  This component stores a hash map of bone names to bone transforms so that each part that is instantiated it can efficiently hook up to the bone transforms it needs for it's own renderer. 
    /// When the rootbone parameter is assigned the component will calculate bone name hashes for the entire skeleton and a dictionary of hashed names to transforms.
    /// </summary>
    [RequireComponent(typeof(Animator)), DisallowMultipleComponent]
    public class ManikinRig : MonoBehaviour, ISerializationCallbackReceiver
    {
        [System.Serializable]
        public struct BoneData
        {
            public Transform boneTransform;
            public int referenceCount;
        }

        public Dictionary<int, int> skeletonHashToBone = new Dictionary<int, int>(); //TODO, move this to manikin avatar.

        [SerializeField]
        private SkeletonBone[] currentSkeletonBones;

        [SerializeField]
        private string[] skeletonBoneNames;
        [SerializeField]
        private Vector3[] skeletonBonePositions;
        [SerializeField]
        private Quaternion[] skeletonBoneRotations;
        [SerializeField]
        private Vector3[] skeletonBoneScales;

        public ManikinAvatar defaultManikinAvatar;

        static readonly string avatarNameSuffix = "Avatar(Instance)";

        [Header("Optional")]
        public GameObject rigPrefab;

        public Transform rootBone;
        public Dictionary<int, BoneData> bones = new Dictionary<int, BoneData>();

        private ManikinProperties properties;

        [SerializeField, HideInInspector, FormerlySerializedAs("_keys")]
        private List<int> _boneKeys = new List<int>();
        [SerializeField, HideInInspector, FormerlySerializedAs("_values")]
        private List<BoneData> _boneValues = new List<BoneData>();

        [SerializeField, HideInInspector]
        private List<int> _skeletonHashKeys = new List<int>();
        [SerializeField, HideInInspector]
        private List<int> _skeletonHashValues = new List<int>();

        public Animator animator;

        public bool SkeletonDataDirty { get { return skeletonDataDirty; } }
        private bool skeletonDataDirty;
        private List<int> workingKeys = new List<int>(2); //blackboard list for iterating over keys. Reuse for no GC

        //Returns false if there was no change, true if there was a change.
        public bool SetCurrentSkeletonBoneByHash(int boneNameHash, SkeletonBone bone)
        {
            return SetCurrentSkeletonBoneByIndex(skeletonHashToBone[boneNameHash], bone);
        }

        //Returns false if there was no change, true if there was a change.
        public bool SetCurrentSkeletonBoneByIndex(int index, SkeletonBone bone)
        {
            if (currentSkeletonBones[index].Equals(bone))
                return false;

            currentSkeletonBones[index] = bone;
            return true;
        }

        public bool TryGetDefaultSkeletonBone(int boneNameHash, out int index, out SkeletonBone bone)
        {
            if (skeletonHashToBone.TryGetValue(boneNameHash, out int hashIndex))
            {
                bone = defaultManikinAvatar.humanDescription.skeleton[hashIndex];
                index = hashIndex;
                return true;
            }

            bone = new SkeletonBone();
            index = -1;
            return false;
        }

        public void DirtySkeletonData()
        {
            skeletonDataDirty = true;
        }


        public void OnBeforeSerialize()
        {
            _boneKeys.Clear();
            _boneValues.Clear();

            foreach (var kvp in bones)
            {
                _boneKeys.Add(kvp.Key);
                _boneValues.Add(kvp.Value);
            }

            _skeletonHashKeys.Clear();
            _skeletonHashValues.Clear();

            foreach (var kvp in skeletonHashToBone)
            {
                _skeletonHashKeys.Add(kvp.Key);
                _skeletonHashValues.Add(kvp.Value);
            }

            SkeletonBone[] skeleton = currentSkeletonBones;
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
            bones.Clear();
            for (int i = 0; i != Math.Min(_boneKeys.Count, _boneValues.Count); i++)
                bones.Add(_boneKeys[i], _boneValues[i]);

            skeletonHashToBone.Clear();
            for (int i = 0; i != Math.Min(_skeletonHashKeys.Count, _skeletonHashValues.Count); i++)
                skeletonHashToBone.Add(_skeletonHashKeys[i], _skeletonHashValues[i]);

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
                currentSkeletonBones = skeleton;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            properties = GetComponent<ManikinProperties>();
            animator = GetComponent<Animator>();

            if (!Application.isPlaying)
            {
                if (rootBone == null && bones.Count > 0)
                {
                    bones.Clear();
                }

                if (rootBone != null && bones.Count <= 0)
                {
                    InitializeBones();
                }
            }
        }

        public virtual void InitializeBones()
        {
            Debug.Log("Initializing Bones...");
            if (rootBone != null)
            {
                UnityEditor.Undo.RecordObject(this, "Initialize Bones");

                bones.Clear();

                Transform[] transforms = rootBone.GetComponentsInChildren<Transform>();
                List<string> names = new List<string>();
                for (int i = 0; i < transforms.Length; i++)
                {
                    bones.Add(Animator.StringToHash(transforms[i].name), new BoneData() { boneTransform = transforms[i], referenceCount = 1 });
                    names.Add(transforms[i].name);
                }

                if (defaultManikinAvatar == null)
                {
                    Debug.LogError("No defaultManikinAvatar asset assigned!");
                    return;
                }
                SkeletonBone[] defaultSkeleton = defaultManikinAvatar.humanDescription.skeleton;
                currentSkeletonBones = new SkeletonBone[defaultSkeleton.Length];
                defaultSkeleton.CopyTo(currentSkeletonBones, 0);
                skeletonHashToBone.Clear();

                //Do a special name check and update for the rootbone in the skeleton list, which is set at index 0.
                currentSkeletonBones[0].name = gameObject.name;
#if UNITY_EDITOR
                if (!gameObject.name.Contains(defaultSkeleton[0].name))
                {
                    Debug.LogWarning("Root's name is different than stored in avatar.");
                }
#endif

                for (int i = 0; i < currentSkeletonBones.Length; i++)
                {
                    skeletonHashToBone.Add(Animator.StringToHash(currentSkeletonBones[i].name), i);
                }

                if (UnityEditor.PrefabUtility.IsPartOfAnyPrefab(this))
                {
                    UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
                }
            }
            else
            {
                Debug.LogWarning("Select a rootbone first!");
            }
        }
#endif

        //TODO This could be optimized with a single bounds for the entire rig.
        public bool IsVisible()
        {
            /*
            if (partList != null)
            {
                foreach (KeyValuePair<System.Guid, GameObject> pair in partList.PartGameObjects)
                {
                    Renderer r = pair.Value.GetComponent<Renderer>();
                    if (r != null)
                    {
                        if(r.isVisible)
                        {
                            return true;
                        }
                    }
                }
            }*/
            return false;
        }

    }
}
