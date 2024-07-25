using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad.Manikin
{
    /// <summary>
    /// Base component that is placed on part prefabs to be used in the Manikin System. Specialized versions are built on top of the base class.  For example, ManikinMeshPart, ManikinSmrPart, and ManikinLODSmrPart.
    /// </summary>
    [DisallowMultipleComponent()]
    public class ManikinPart : MonoBehaviour, IManikinPartPreview
    {
        [Header("Mesh Options"),Tooltip("Add the rig of the creature so that the Manikin clothing can fit over the creature.\n\nFor male rig, the file is called \"HummanMaleRig\" and for female it is \"HumanFemaleRig\"")]
        public GameObject rigPrefab;
        [Tooltip("Rotation applied to the mesh to correct rotation when displayed in previews.")]
        public Vector3 rootRotation;

        /// <summary>
        /// Struct to hold blend shapes, and make them blend when specific wardrobes are already equipped.
        /// </summary>
        [Serializable]
        public class PartBlendShape
        {
            public ManikinWardrobeData[] wardrobes;
            public SkinnedMeshRenderer[] skinnedMeshRenderers;
            public int blendShapeIndex;
            public float blendShapeDefaultValue;
            public float blendShapeValue;

            /// <summary>
            /// Updates the "blendShapeIndex" blend shape, with the "blendShapeValue" on the skinned mesh renderers;
            /// </summary>
            public void Update()
            {
                for (int i = 0; i < skinnedMeshRenderers.Length; i++)
                {
                    if (skinnedMeshRenderers[i] == null) continue;
                    skinnedMeshRenderers[i].SetBlendShapeWeight(blendShapeIndex, blendShapeValue);
                }
            }

            /// <summary>
            /// Resets the "blendShapeIndex" blend shape, with the "blendShapeDefaultValue" on the skinned mesh renderers;
            /// </summary>
            public void Reset()
            {
                for (int i = 0; i < skinnedMeshRenderers.Length; i++)
                {
                    if (skinnedMeshRenderers[i] == null) continue;
                    skinnedMeshRenderers[i].SetBlendShapeWeight(blendShapeIndex, blendShapeDefaultValue);
                }
            }
        }
        
        [Tooltip("Blend shapes to make the part fit specific wardrobes.")]
        public PartBlendShape[] blendShapes;

        protected virtual void Awake() { }

#if UNITY_EDITOR
        /// <summary>
        /// This is called when creating the part prefab in the editor.
        /// </summary>
        public virtual void Initialize() {}

        public virtual void PrefabStageOpened() { }

        public virtual void PrefabStageClosing() { }

        public virtual Texture2D GetOrCreatePreview(string id, string path, int width, int height) 
        {
            var settings = ManikinSettingsProvider.GetSettings();
            //If we have cachePreviews on, then attempt to find the cached preview texture.
            if (settings != null && settings.cachePreviews)
            {
                Texture2D preview = GetPreview(id, path);
                if (preview != null) 
                {
                    return preview; 
                }
            }
            return CreatePreview(id, path, width, height);
        }

        public virtual Texture2D CreatePreview(string id, string path, int width, int height) { return null; }

        public virtual Texture2D GetPreview(string id, string path) 
        {
            id = "/" + id + ".asset";
            return UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(path + id); 
        }

        [ContextMenu("Create Or Update Preview")]
        public void CreateOrUpdatePreview()
        {
            if(UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this))
            {
                var settings = ManikinSettingsProvider.GetSettings();
                if (UnityEditor.AssetDatabase.TryGetGUIDAndLocalFileIdentifier(this, out string guid, out long localId))
                {
                    if (settings != null)
                    {
                        CreatePreview(guid, settings.previewsFolder, settings.previewDimensions, settings.previewDimensions);
                    }
                    else
                    {
                        CreatePreview(guid, "Assets/ManikinPreviews", 128, 128);
                    }
                }
            }
            else
            {
                Debug.LogWarning("Didn't update. This instance is not a prefab!", this);
            }
        }

        public void SavePreview(string id, string path, Texture2D preview)
        {
            if (preview != null)
            {
                id = "/" + id + ".asset";
                var settings = ManikinSettingsProvider.GetSettings();
                //If we have cachePreviews on, then save the newly created texture to disk.
                if (settings != null && settings.cachePreviews)
                {
                    if (UnityEditor.AssetDatabase.IsValidFolder(path))
                    {
                        string filepath = path + id;
                        UnityEditor.AssetDatabase.CreateAsset(preview, filepath);
                    }
                }
            }
        }

        public static Texture2D DrawPreview(Renderer[] renderers, Vector3 rootRotation, int width, int height)
        {
            if (renderers == null || renderers.Length <= 0)
                return null;
            
            //Defaults
             Vector3 meshRotation = new Vector3(-90, 180, 0);
             Color backgroundColor = new Color(0.345f, 0.345f, 0.345f);
             float cameraDistanceFactor = 3;
             float cameraRotationX = -45;
             float cameraRotationY = 45;
            
            var settings = ManikinSettingsProvider.GetSettings();
             if (settings != null)
             {
                 meshRotation = settings.meshRotation;
                 backgroundColor = settings.backgroundcolor;
                 cameraDistanceFactor = settings.cameraDistanceFactor;
                 cameraRotationX = settings.cameraRotationX;
                 cameraRotationY = settings.cameraRotationY;
             }

             UnityEditor.PreviewRenderUtility previewRenderUtility = new UnityEditor.PreviewRenderUtility();

             previewRenderUtility.BeginStaticPreview(new Rect(0, 0, width, height));

             Matrix4x4 mat = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(meshRotation + rootRotation), Vector3.one);

             List<Mesh> meshes = new List<Mesh>();
             foreach (Renderer renderer in renderers)
             {
                 if (renderer.GetType() == typeof(SkinnedMeshRenderer))
                 {
                    if ((renderer as SkinnedMeshRenderer).sharedMesh == null)
                    {
                        Debug.LogError(renderer.name + " has no mesh!");
                        return null;
                    }
                    meshes.Add((renderer as SkinnedMeshRenderer).sharedMesh);
                 }
                 if (renderer.GetType() == typeof(MeshRenderer))
                 {
                     if (renderer.TryGetComponent(out MeshFilter filter))
                     {
                        if (filter.sharedMesh == null)
                        {
                            Debug.LogError(renderer.name + " has no mesh!");
                            return null;
                        }
                        meshes.Add(filter.sharedMesh);
                     }
                 }
             }

             if (meshes.Count <= 0)
                 return null;

             Bounds bounds = meshes[0].bounds;
             for (int i = 1; i < meshes.Count; i++)
             {
                 bounds.Encapsulate(meshes[i].bounds);
             }
             Vector3 center = mat.MultiplyPoint3x4(bounds.center);

             previewRenderUtility.camera.farClipPlane = 20;
             previewRenderUtility.camera.nearClipPlane = 0.01f;
             previewRenderUtility.camera.transform.rotation = Quaternion.LookRotation(Vector3.forward);
             previewRenderUtility.camera.transform.position = center - previewRenderUtility.camera.transform.forward * bounds.size.magnitude * cameraDistanceFactor;
             previewRenderUtility.camera.transform.RotateAround(center, Vector3.left, cameraRotationX);
             previewRenderUtility.camera.transform.RotateAround(center, Vector3.up, cameraRotationY);

             previewRenderUtility.lights[0].transform.rotation = Quaternion.Euler(40f, 40f, 0f);
             previewRenderUtility.lights[0].intensity = 1.4f;
             previewRenderUtility.lights[1].transform.rotation = Quaternion.Euler(40f, 85f, 0f);
             previewRenderUtility.lights[1].intensity = 1.4f;

             previewRenderUtility.camera.backgroundColor = backgroundColor;

             foreach (Renderer renderer in renderers)
             {
                 if (renderer == null)
                     continue;

                 Material[] materials = renderer.sharedMaterials;
                 if (materials == null)
                     continue;

                 if (renderer.GetType() == typeof(SkinnedMeshRenderer))
                 {
                     SkinnedMeshRenderer smr = renderer as SkinnedMeshRenderer;
                     for (int i = 0; i < smr.sharedMesh.subMeshCount; i++)
                     {
                         if (i < materials.Length && materials[i] != null)
                         {
                             previewRenderUtility.DrawMesh(smr.sharedMesh, mat, materials[i], i);
                         }
                     }
                 }

                 if (renderer.GetType() == typeof(MeshRenderer))
                 {
                     if (renderer.TryGetComponent(out MeshFilter filter))
                     {
                         for (int i = 0; i < filter.sharedMesh.subMeshCount; i++)
                         {
                             if (i < materials.Length && materials[i] != null)
                             {
                                 previewRenderUtility.DrawMesh(filter.sharedMesh, mat, materials[i], i);
                             }
                         }
                     }
                 }

             }

             previewRenderUtility.Render(true);
             Texture2D texture = previewRenderUtility.EndStaticPreview();

             previewRenderUtility.Cleanup();

             return texture;
        }
#endif

        public virtual ManikinPart Instantiate(GameObject parent, ManikinRig rig = null)
        {
            //This function should usually be pulling data off the instantiated obj instance instead of 
            //the class calling this function, because that is the prefab.

            ManikinPart obj = null;
            if (Application.isPlaying)
            {
                obj = Instantiate<ManikinPart>(this, parent.transform);
            }
#if UNITY_EDITOR
            else
            {
                //We want this part to be linked to it's prefab asset when created in the editor so that any update to the prefab asset updates prefab instances.
                obj = UnityEditor.PrefabUtility.InstantiatePrefab(this, parent.transform) as ManikinPart;
            }
#endif

            return obj;
        }

        public virtual bool RuntimeInitialize(GameObject obj, ManikinRig rig)  { return true; }

        public virtual bool PartOfBone(int hash) { return false; }

        public virtual bool PartOfBones(int[] hashes) { return false; }

        public virtual Renderer[] GetRenderers() { return null; }

        /// <summary>
        /// Updates the part blends shapes values.
        /// </summary>
        /// <param name="equippedWardrobes">Current creature wardrobes</param>
        public virtual void UpdateBlendShapes(ManikinWardrobeData[] equippedWardrobes)
        {
            for (int i = 0; i < blendShapes.Length; i++)
                blendShapes[i].Reset();
            
            for (int i = 0; i < equippedWardrobes.Length; i++)
            {
                var (found, blendShapeToUpdate) = IsBlendShapesContainingWardrobe(equippedWardrobes[i]);
                if (found)
                    blendShapeToUpdate.Update();
            }
        }

        /// <summary>
        /// Checks if the part blend shape struct is containing the given wardrobe as a dependency
        /// </summary>
        /// <param name="wardrobe">Wardroe to check for</param>
        /// <returns>True and the concerned part blend shape if found. False and null otherwise</returns>
        private (bool, PartBlendShape) IsBlendShapesContainingWardrobe(ManikinWardrobeData wardrobe)
        {
            for (int i = 0; i < blendShapes.Length; i++)
            for (int j = 0; j < blendShapes[i].wardrobes.Length; j++)
                    if(blendShapes[i].wardrobes[j] == wardrobe) return (true, blendShapes[i]);

            return (false, null);
        }
    }
}
