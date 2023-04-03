using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEngine.Assertions;
#endif

namespace ThunderRoad.Manikin
{
    [/*RequireComponent(typeof(SkinnedMeshRenderer)),*/DisallowMultipleComponent()]
    public class ManikinSmrPart : ManikinPart
    {
        [SerializeField, ShowOnlyInspector]
        private SkinnedMeshRenderer smr = default;
        [SerializeField, ShowOnlyInspector]
        private int rootBoneHash = default;
        [SerializeField, ShowOnlyInspector]
        private int[] boneNameHashes = default;
        [SerializeField, ShowOnlyInspector]
        private int[] weightedBoneNameHashes = default;

        protected override void Awake()
        {
            base.Awake();

            if (smr == null) { smr = GetComponent<SkinnedMeshRenderer>(); }
        }

        public SkinnedMeshRenderer GetSkinnedMeshRenderer()
        {
            return smr;
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if (smr == null) { smr = GetComponent<SkinnedMeshRenderer>(); }

            if(boneNameHashes != null && boneNameHashes.Length > 0)
            {
                if (weightedBoneNameHashes == null || weightedBoneNameHashes.Length == 0)
                {
                    if (smr.sharedMesh != null)
                    {
                        BoneWeight[] boneWeights = smr.sharedMesh.boneWeights;
                        HashSet<int> weightedBoneHashes = new HashSet<int>();
                        for (int index = 0; index < boneWeights.Length; index++)
                        {
                            if (boneWeights[index].weight0 > 0) { weightedBoneHashes.Add(boneNameHashes[boneWeights[index].boneIndex0]); }
                            if (boneWeights[index].weight1 > 0) { weightedBoneHashes.Add(boneNameHashes[boneWeights[index].boneIndex1]); }
                            if (boneWeights[index].weight2 > 0) { weightedBoneHashes.Add(boneNameHashes[boneWeights[index].boneIndex2]); }
                            if (boneWeights[index].weight3 > 0) { weightedBoneHashes.Add(boneNameHashes[boneWeights[index].boneIndex3]); }
                        }
                        weightedBoneNameHashes = new int[weightedBoneHashes.Count];
                        weightedBoneHashes.CopyTo(weightedBoneNameHashes);
                    }
                }
            }
        }

        /// <summary>
        /// This is called when creating the part prefab in the editor.
        /// </summary>
        public override void Initialize()
        {
            if(smr == null)
            {
                Debug.LogError("No SkinnedMeshRenderer found!");
                return;
            }
            if(smr.bones == null)
            {
                Debug.LogError("No bones found on this SkinnedMeshRenderer!");
                return;
            }
            if (smr.rootBone == null)
            {
                Debug.LogError("No rootbone found on this SkinnedMeshRenderer!");
                return;
            }

            rootBoneHash = Animator.StringToHash(smr.rootBone.name);

            boneNameHashes = new int[smr.bones.Length];
            for (int i = 0; i < smr.bones.Length; i++)
            {
                boneNameHashes[i] = Animator.StringToHash(smr.bones[i].name);
            }

            if (boneNameHashes != null && boneNameHashes.Length > 0)
            {
                BoneWeight[] boneWeights = smr.sharedMesh.boneWeights;
                HashSet<int> weightedBoneHashes = new HashSet<int>();
                for (int index = 0; index < boneWeights.Length; index++)
                {
                    if (boneWeights[index].weight0 > 0) { weightedBoneHashes.Add(boneNameHashes[boneWeights[index].boneIndex0]); }
                    if (boneWeights[index].weight1 > 0) { weightedBoneHashes.Add(boneNameHashes[boneWeights[index].boneIndex1]); }
                    if (boneWeights[index].weight2 > 0) { weightedBoneHashes.Add(boneNameHashes[boneWeights[index].boneIndex2]); }
                    if (boneWeights[index].weight3 > 0) { weightedBoneHashes.Add(boneNameHashes[boneWeights[index].boneIndex3]); }
                }
                weightedBoneNameHashes = new int[weightedBoneHashes.Count];
                weightedBoneHashes.CopyTo(weightedBoneNameHashes);
            }
        }

        public override void PrefabStageOpened()
        {
            if (rigPrefab != null)
            {
                GameObject rigObj = GameObject.Instantiate(rigPrefab, transform);
                rigObj.name = rigPrefab.name;
                rigObj.hideFlags = HideFlags.HideAndDontSave;

                ManikinRig rig = rigObj.AddComponent<ManikinRig>();
                rig.rootBone = rigObj.transform;
                rig.rootBone.rotation = Quaternion.Euler(rootRotation);
                rig.InitializeBones();

                RuntimeInitialize(gameObject, rig);
            }
        }

        public override Texture2D CreatePreview(string id, string path, int width, int height) 
        {
            Renderer[] renderers = GetRenderers();
            if (renderers != null)
            {
                Texture2D preview = ManikinPart.DrawPreview(renderers, rootRotation, width, height);
                preview.name = id;

                SavePreview(id, path, preview);
                return preview;
            }

            Debug.LogWarning(name + " has no renderers for preview!");
            return null;
        }
#endif

        public override ManikinPart Instantiate(GameObject parent, ManikinRig rig = null)
        {
            //Base instantiates and gets the ManikinPartList
            ManikinPart obj = base.Instantiate(parent);

            if(rig == null)
            {
                if(!parent.TryGetComponent(out rig))
                {
                    Debug.LogError("No ManikinRig found on parent!");
                    return obj;
                }
            }
            RuntimeInitialize(obj.gameObject, rig);

            return obj;
        }

        public override bool RuntimeInitialize(GameObject obj, ManikinRig rig)
        {
            if (obj.TryGetComponent(out SkinnedMeshRenderer objSmr))
            {
                bool error = false;
#if UNITY_EDITOR
                Assert.IsNotNull(boneNameHashes, "BoneNameHashes must contains some bones.");
                Assert.IsTrue(boneNameHashes.Length > 0, "BoneNameHashes must contain at least one bone.");
                StringBuilder errorMsg = new StringBuilder("Couldn't find bone hashes for part " + obj.name + "!\n", 1000);
#endif
                //First let's find our rootbone and set it on our skinnedMeshRenderer.
                if (rig.bones.TryGetValue(rootBoneHash, out ManikinRig.BoneData rootBoneData))
                {
                    objSmr.rootBone = rootBoneData.boneTransform;
                }

                //Next, let's find all the bones that this part needs to reference.
                Transform[] bones = new Transform[boneNameHashes.Length];
                for (int i = 0; i < boneNameHashes.Length; i++)
                {
                    if (rig.bones.TryGetValue(boneNameHashes[i], out ManikinRig.BoneData boneData))
                    {
                        bones[i] = boneData.boneTransform;
                    }
                    else
                    {
                        error = true;
#if UNITY_EDITOR
                        errorMsg.Append(boneNameHashes[i]);
                        errorMsg.Append("\n");
#endif
                    }
                }

                if (error)
                {
#if UNITY_EDITOR
                    Debug.LogError(errorMsg, obj.transform.parent);
#endif
                    return false;
                }

                objSmr.bones = bones;
                return true;
            }
            else
            {
                Debug.LogError("No SkinnedMeshRenderer found on this part!");
                return false;
            }
        }

        public override bool PartOfBone(int hash)
        {
            for(int i = 0; i < weightedBoneNameHashes.Length; i++)
            {
                if (weightedBoneNameHashes[i] == hash)
                    return true;
            }
            return false;
        }

        public override bool PartOfBones(int[] hashes)
        {
            for (int i = 0; i < weightedBoneNameHashes.Length; i++)
            {
                for (int hashIndex = 0; hashIndex < hashes.Length; hashIndex++)
                {
                    if (weightedBoneNameHashes[i] == hashes[hashIndex])
                        return true;
                }
            }
            return false;
        }

        public override Renderer[] GetRenderers() 
        {
            return new Renderer[1] { smr };
        }
    }
}