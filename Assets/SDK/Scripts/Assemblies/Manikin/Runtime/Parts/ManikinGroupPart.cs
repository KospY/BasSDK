using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad.Manikin
{
    [DisallowMultipleComponent()]
    public class ManikinGroupPart : ManikinPart
    {
        [System.Serializable]
        public struct PartLOD
        {
            public List<Renderer> renderers;
        }
        public List<PartLOD> partLODs = new List<PartLOD>();

        public bool copyLastLodToAnySuperiorLOD = true;

        public ManikinPart[] ChildParts { get { return parts; } }
        public int ChildPartCount 
        {
            get { return (parts == null) ? 0 : parts.Length; } 
        }

#pragma warning disable 0649
        [SerializeField, ShowOnlyInspector]
        private ManikinPart[] parts;
#pragma warning restore
#if UNITY_EDITOR
        private void OnValidate()
        {
            parts = GetChildManikinParts();
        }

        private ManikinPart[] GetChildManikinParts()
        {
            List<ManikinPart> childParts = new List<ManikinPart>();
            GetComponentsInChildren<ManikinPart>(true, childParts);
            childParts.Remove(this);

            return childParts.ToArray();
        }

        /// <summary>
        /// This is called when creating the part prefab in the editor.
        /// </summary>
        [ContextMenu("Initialize")]
        public override void Initialize()
        {
            parts = GetChildManikinParts();

            if (parts == null)
            {
                Debug.LogError("No Parts found!");
                return;
            }

            for (int i = 0; i < parts.Length; i++)
            {
                parts[i].Initialize();
            }

            InitializeLODs();
        }

        public override void PrefabStageOpened()
        {
            if (rigPrefab != null)
            {
                GameObject rigPrefab = GameObject.Instantiate(base.rigPrefab, transform);
                rigPrefab.name = base.rigPrefab.name;
                rigPrefab.hideFlags = HideFlags.HideAndDontSave;
                
                ManikinRig rig = rigPrefab.AddComponent<ManikinRig>();
                rig.rootBone = rigPrefab.transform;
                rig.rootBone.rotation = Quaternion.Euler(rootRotation);
                rig.InitializeBones();

                for (int i = 0; i < parts.Length; i++)
                {
                    parts[i].RuntimeInitialize(parts[i].gameObject, rig);
                }
            }
        }

        public void InitializeLODs()
        {
            if (parts == null)
            {
                Debug.LogError("No Parts found!");
                return;
            }
            partLODs.Clear();

            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].name.Contains("_LOD"))
                {
                    int startIndex = parts[i].name.IndexOf("_LOD");
                    int lodLevel = int.Parse(parts[i].name.Substring(startIndex + 4));
                    lodLevel = Mathf.Clamp(lodLevel, 0, 5); //sanity check

                    while (partLODs.Count <= lodLevel)
                    {
                        partLODs.Add(new PartLOD { renderers = new List<Renderer>() });
                    }

                    Renderer[] renderers = parts[i].GetRenderers();
                    if (renderers != null)
                    {
                        partLODs[lodLevel].renderers.AddRange(renderers);
                    }
                }
            }
        }

        /// <summary>
        /// Editor only function to remove LODs from a prefab.  Intended to be used from build scripts.
        /// </summary>
        /// <param name="lodLevel"></param>
        public void RemovePartsOfLOD(int lodLevel)
        {
            string lod = "_LOD" + lodLevel;
            List<ManikinPart> removal = new List<ManikinPart>();
            List<ManikinPart> keep = new List<ManikinPart>();

            for (int i = 0; i < parts.Length; i++)
            {
                if(parts[i].name.Contains(lod))
                {
                    removal.Add(parts[i]);
                }
                else
                {
                    keep.Add(parts[i]);
                }
            }

            parts = keep.ToArray();
            InitializeLODs();

            foreach(ManikinPart part in removal)
            {
                DestroyImmediate(part.gameObject, true);
            }
        }

        public override Texture2D CreatePreview(string id, string path, int width, int height)
        {
            Texture2D preview = ManikinPart.DrawPreview(GetRenderersLOD(0), rootRotation, width, height);
            if (preview != null)
            {
                preview.name = id;

                SavePreview(id, path, preview);
            }
            return preview;
        }
#endif

        public override ManikinPart Instantiate(GameObject parent, ManikinRig rig = null)
        {
            //Base instantiates and gets the ManikinPartList
            ManikinGroupPart obj = base.Instantiate(parent, rig) as ManikinGroupPart;

            if (obj.parts != null)
            {
                for (int i = 0; i < obj.parts.Length; i++)
                {
                    obj.parts[i].RuntimeInitialize(obj.parts[i].gameObject, rig);
                }
            }

            return obj;
        }

        public override bool PartOfBone(int hash)
        {
            if (parts != null)
            {
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i].PartOfBone(hash))
                        return true;
                }
            }
            return false;
        }

        public override bool PartOfBones(int[] hashes)
        {
            if (parts != null)
            {
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i].PartOfBones(hashes))
                        return true;
                }
            }
            return false;
        }

        public override Renderer[] GetRenderers()
        {
            List<Renderer> renderers = new List<Renderer>();
            for (int i = 0; i < parts.Length; i++)
            {
                renderers.AddRange(parts[i].GetRenderers());
            }
            return renderers.ToArray();
        }

        public Renderer[] GetRenderersLOD(int lod = 0)
        {
            if(lod < partLODs.Count)
            {
                return partLODs[lod].renderers.ToArray(); ;
            }
            return null;
        }
    }
}
