using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.IO;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Areas/LightProbeVolume.html")]
    [ExecuteInEditMode]
    public class LightProbeVolume : MonoBehaviour
    {
        public static List<LightProbeVolume> list = new List<LightProbeVolume>();
        public static Dictionary<Area, List<LightProbeVolume>> areaToVolume = new Dictionary<Area, List<LightProbeVolume>>();

        /// <summary>
        /// Is there at least one LightProbeVolume in the scene?
        /// </summary>
        public static bool Exists = false;
        public static void Register(LightProbeVolume lightProbeVolume)
        {
            list.Add(lightProbeVolume);
            //Get the area of the light probe volume
            if (lightProbeVolume.area)
            {
                if (!areaToVolume.TryGetValue(lightProbeVolume.area, out List<LightProbeVolume> volumes))
                {
                    volumes = new List<LightProbeVolume>();
                    areaToVolume.Add(lightProbeVolume.area, volumes);
                }
                volumes.Add(lightProbeVolume);
            }
            else
            {
                Debug.LogError($"Light Probe Volume: {lightProbeVolume.gameObject.GetPathFromRoot()} is not in an Area! LVRs will not work properly.");               
            }
            Exists = true;
        }
        public static void Unregister(LightProbeVolume lightProbeVolume)
        {
            list.Remove(lightProbeVolume);
            if (list.Count == 0)
            {
                Exists = false;
            }
            if (lightProbeVolume.area)
            {
                if (areaToVolume.TryGetValue(lightProbeVolume.area, out List<LightProbeVolume> volumes))
                {
                    volumes.Remove(lightProbeVolume);
                    if (volumes.Count == 0)
                    {
                        areaToVolume.Remove(lightProbeVolume.area);
                    }
                }
            }
        }

        [Tooltip("The lower the priority, the more likely the volume will be used by items. If an item is inside two volumes, it will select the one with the lowest priority.")]
        public int priority = 1;

        [Tooltip("Size of the Light Volume box.")]
        public Vector3 size = Vector3.one;

        [Tooltip("Enables Light Probe occlusion (used for light occlusion e.g. directional light in an interior space.)")]
        public bool useOcclusion = true;

        public Vector3 ProbeVolumeMin => probeVolumeMin;

        [SerializeField, HideInInspector]
        private Vector3 probeVolumeMin;

        public Vector3 ProbeVolumeSizeInverse => probeVolumeSizeInverse;

        [SerializeField, HideInInspector] public Vector3 probeVolumeSizeInverse;

        [Tooltip("Ambient 3D Texture of the Light Probe Volume")]
        public Texture3D Ambient;
        [Tooltip("The Red Channel 3D Texture of the Light Probe Volume")]
        public Texture3D SHAr;
        [Tooltip("The Green Channel 3D Texture of the Light Probe Volume")]
        public Texture3D SHAg;
        [Tooltip("The Blue Channel 3D Texture of the Light Probe Volume")]
        public Texture3D SHAb;
        [Tooltip("The Occlusion Channel 3D Texture of the Light Probe Volume")]
        public Texture3D occ;
        private static readonly int UseProbeVolume = Shader.PropertyToID("_UseProbeVolume");
        private static readonly int UseProbeVolumeLux = Shader.PropertyToID("_PROBEVOLUME");
        private static readonly int ProbeWorldToTexture = Shader.PropertyToID("_ProbeWorldToTexture");
        private static readonly int VolumeMin = Shader.PropertyToID("_ProbeVolumeMin");
        private static readonly int ProbeVolumeSizeInv = Shader.PropertyToID("_ProbeVolumeSizeInv");
        private static readonly int ProbeVolumeAmbient = Shader.PropertyToID("_ProbeVolumeAmbient");
        private static readonly int ProbeVolumeShR = Shader.PropertyToID("_ProbeVolumeShR");
        private static readonly int ProbeVolumeShG = Shader.PropertyToID("_ProbeVolumeShG");
        private static readonly int ProbeVolumeShB = Shader.PropertyToID("_ProbeVolumeShB");
        private static readonly int ProbeVolumeOcc = Shader.PropertyToID("_ProbeVolumeOcc");
        private static readonly int UnitySHAr = Shader.PropertyToID("unity_SHAr");
        private static readonly int UnitySHAg = Shader.PropertyToID("unity_SHAg");
        private static readonly int UnitySHAb = Shader.PropertyToID("unity_SHAb");
        private static readonly int UnityProbesOcclusion = Shader.PropertyToID("unity_ProbesOcclusion");
        
#if UNITY_EDITOR
        [ContextMenu("ProcessTextures")]
        public void ProcessTextures()
        {
            var format = SHAr.graphicsFormat;
            var tex = new Texture3D(SHAr.width,SHAr.height,SHAr.depth, format, UnityEngine.Experimental.Rendering.TextureCreationFlags.None);

            var r = SHAr.GetPixels();
            var g = SHAg.GetPixels();
            var b = SHAb.GetPixels();
            Color[] col = new Color[r.Length];

            for (int i = 0; i < r.Length; i++)
            {
                col[i] = new Color(Vector4.Dot(r[i],new Vector4(0,1,0,1)),Vector4.Dot(g[i],new Vector4(0,1,0,1)),Vector4.Dot(b[i],new Vector4(0,1,0,1)),1);
            }
            tex.SetPixels(col);
            tex.Apply();

            var path = UnityEditor.AssetDatabase.GetAssetPath(SHAr).Replace("SHAr", "Ambient");
            UnityEditor.AssetDatabase.CreateAsset(tex, path);
            UnityEditor.AssetDatabase.ImportAsset(path);

            Ambient = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture3D>(path);

        }
#endif

        private BoxCollider _boxCollider;
        private Bounds _bounds;
        private bool hasBoxCollider;

        public BoxCollider BoxCollider
        {
            get => _boxCollider;
            set
            {
                if (value == null) return;
                _boxCollider = value;
                hasBoxCollider = true;
                _bounds = _boxCollider.bounds;
            }
        }

        /// <summary>
        /// The area this LPV is in
        /// </summary>
        [NonSerialized]
        [ShowInInspector]
        public Area area;

        public bool IsInVolume(Bounds bounds, Vector3 position)
        {
            if (!hasBoxCollider)
            {
                //try to get the box collider
                BoxCollider = GetComponent<BoxCollider>();
            }
            //check the position first as that may be more accurate, otherwise check the bounds
            return _bounds.Contains(position) || _bounds.Intersects(bounds);
        }
        
        public bool IsPositionInVolume(Vector3 position)
        {
            if (!hasBoxCollider)
            {
                //try to get the box collider
                BoxCollider = GetComponent<BoxCollider>();
            }
            return _bounds.Contains(position);
        }
        /// <summary>
        /// Used from the custom editor to change the gizmos.
        /// </summary>
        [NonSerialized]
        public bool editingSizeThroughEditor;

        private Dictionary<MonoBehaviour, List<Material>> _registeredMaterials;

        public void SetTexture(Texture3D SHAr,
            Texture3D SHAg,
            Texture3D SHAb,
            Texture3D occ)
        {
            this.SHAr = SHAr;
            this.SHAg = SHAg;
            this.SHAb = SHAb;
            this.occ = occ;

            if (_registeredMaterials.IsNullOrEmpty()) return;
            foreach (KeyValuePair<MonoBehaviour, List<Material>> item in _registeredMaterials)
            {
                int count = item.Value.Count;
                for (int i = count - 1; i >= 0; i--)
                {
                    Material mat = item.Value[i];
                    if (mat != null)
                    {
                        UpdateMaterialProperties(mat);
                    }
                    else
                    {
                        item.Value.RemoveAt(i);
                        Debug.LogError("Material not alive or null in LightProbeVolume : " + gameObject.name + " for mono register type " + item.Key.GetType().Name + " in game object " + item.Key.gameObject.name);
                    }
                }
            }
        }

        private void Awake()
        {
            area = GetComponentInParent<Area>();
            BoxCollider = GetComponent<BoxCollider>();
        }
        private void OnEnable()
        {
            Register(this);
#if UNITY_EDITOR
            LightmapBakeHelper.onBakeCompleted += OnBakeCompleted;
#endif
        }

        private void OnDisable()
        {
            Unregister(this);
#if UNITY_EDITOR
            LightmapBakeHelper.onBakeCompleted -= OnBakeCompleted;
#endif
        }

        private void OnDestroy()
        {
            Unregister(this);
            SetTexture(null, null, null, null);
            ClearAllRegisteredMaterials();
        }
        
        private void ClearAllRegisteredMaterials()
        {
            if (_registeredMaterials.IsNullOrEmpty()) return;
            //loop through all the registered materials, set the textures to null and remove the weak references
            foreach (var item in _registeredMaterials)
            {
                int count = item.Value.Count;
                for (int i = count - 1; i >= 0; i--)
                {
                    Material mat = item.Value[i];
                    if (mat != null)
                    {
                        mat.SetFloat(UseProbeVolume, 0); // This is to see checkbox ticked in the inspector
                        mat.SetFloat(UseProbeVolumeLux, 0); // Lux URP  uses this instead of the keyword it seems
                        mat.DisableKeyword("_PROBEVOLUME_ON");
                        mat.SetTexture(ProbeVolumeShR, null);
                        mat.SetTexture(ProbeVolumeShG, null);
                        mat.SetTexture(ProbeVolumeShB, null);
                        mat.SetTexture(ProbeVolumeOcc, null);
                    }
                    item.Value.RemoveAt(i);
                }
            }
            _registeredMaterials.Clear();
            
        }


        public void RegisterMaterials(MonoBehaviour receiver, Material[] materials)
        {
            if (receiver == null) return;
            if (materials.IsNullOrEmpty()) return;
            if (_registeredMaterials == null) _registeredMaterials = new Dictionary<MonoBehaviour, List<Material>>();

            List<Material> previousMaterials;
            if (!_registeredMaterials.TryGetValue(receiver, out previousMaterials))
            {
                previousMaterials = new List<Material>();
                _registeredMaterials.Add(receiver, previousMaterials);
            }

            for (int i = 0; i < materials.Length; i++)
            {
                previousMaterials.Add(materials[i]);
            }

            for (int i = 0; i < materials.Length; i++)
            {
                UpdateMaterialProperties(materials[i]);
            }
        }
        public void UnregisterMaterials(MonoBehaviour receiver)
        {
            if (_registeredMaterials.IsNullOrEmpty()) return;
            if (receiver == null) return;

            if (_registeredMaterials.Remove(receiver, out List<Material> materials))
            {
                int count = materials.Count;
                for (int i = count - 1; i >= 0 ; i--)
                {
                    Material mat = materials[i];
                    if(mat != null)
                    {
                        // release texture
                        mat.SetTexture(ProbeVolumeAmbient, null);
                        mat.SetTexture(ProbeVolumeShR, null);
                        mat.SetTexture(ProbeVolumeShG, null);
                        mat.SetTexture(ProbeVolumeShB, null);
                        mat.SetTexture(ProbeVolumeOcc, null);
                    }
                    else
                    {
                        materials.RemoveAt(i);
                        Debug.LogError("Material not alive or null in LightProbeVolume : " + gameObject.name + " for mono register type " + receiver.GetType().Name + " in game object " + receiver.gameObject.name);
                    }
                }
            }
        }

        public void UpdateMaterialProperties(Material material)
        {
            if (material != null)
            {
                material.SetFloat(UseProbeVolume, 1); // This is to see checkbox ticked in the inspector
                material.SetFloat(UseProbeVolumeLux, 1); // Lux URP  uses this instead of the keyword it seems
                material.EnableKeyword("_PROBEVOLUME_ON");

                material.SetMatrix(ProbeWorldToTexture, transform.worldToLocalMatrix);
                material.SetVector(VolumeMin, probeVolumeMin);
                material.SetVector(ProbeVolumeSizeInv, probeVolumeSizeInverse);

                material.SetTexture(ProbeVolumeAmbient, Ambient);
                material.SetTexture(ProbeVolumeShR, SHAr);
                material.SetTexture(ProbeVolumeShG, SHAg);
                material.SetTexture(ProbeVolumeShB, SHAb);

                if (useOcclusion)
                {
                    material.SetTexture(ProbeVolumeOcc, occ);
                }
            }
            else
            {
                Debug.LogError("Material null");
            }
        }

        public void UpdateMaterialPropertiesMatrixOnly(Material material)
        {
            if (material != null)
            {
                //material.SetFloat(UseProbeVolume, 1); //this is to see it in the inspector
                //material.EnableKeyword("_PROBEVOLUME_ON");
                material.SetMatrix(ProbeWorldToTexture, transform.worldToLocalMatrix);
                //material.SetVector(VolumeMin, probeVolumeMin);
                //material.SetVector(ProbeVolumeSizeInv, probeVolumeSizeInverse);
                //
                //material.SetTexture(ProbeVolumeShR, SHAr);
                //material.SetTexture(ProbeVolumeShG, SHAg);
                //material.SetTexture(ProbeVolumeShB, SHAb);
                //
                //if (useOcclusion)
                //{
                //    material.SetTexture(ProbeVolumeOcc, occ);
                //}
            }
        }

        public void UpdateMaterialPropertyBlock(MaterialPropertyBlock block, Vector3 worldPosition)
        {
            if (SHAr != null && SHAg != null && SHAb != null)
            {
                Vector3 position = transform.worldToLocalMatrix.MultiplyPoint(worldPosition);
                Vector3 inverse = probeVolumeSizeInverse;
                Vector3 texCoord = position - probeVolumeMin;
                texCoord.x *= inverse.x;
                texCoord.y *= inverse.y;
                texCoord.z *= inverse.z;

                block.SetVector(UnitySHAr, SHAr.GetPixelBilinear(texCoord.x, texCoord.y, texCoord.z));
                block.SetVector(UnitySHAg, SHAg.GetPixelBilinear(texCoord.x, texCoord.y, texCoord.z));
                block.SetVector(UnitySHAb, SHAb.GetPixelBilinear(texCoord.x, texCoord.y, texCoord.z));

                if (useOcclusion && occ != null)
                {
                    block.SetVector(UnityProbesOcclusion, occ.GetPixelBilinear(texCoord.x, texCoord.y, texCoord.z));
                }
            }
        }

        public static Vector4[] GetShaderSHL1CoeffsFromNormalizedSH(SphericalHarmonicsL2 probe)
        {
            Vector4[] coeff = new Vector4[3];
            for (int i = 0; i < 3; i++)
            {
                coeff[i].x = probe[i, 3];
                coeff[i].y = probe[i, 1];
                coeff[i].z = probe[i, 2];
                coeff[i].w = probe[i, 0] - probe[i, 6];
            }

            return coeff;
        }

        public static Texture3D DiscardTopMipmap(Texture3D texture)
        {
            if (texture == null)
            {
                Debug.LogError("Texture is null!");
                return null;
            }

            if (texture.mipmapCount <= 1)
            {
                Debug.LogError("Texture has no mip maps!");
                return null;
            }

            Vector3Int dimension = new Vector3Int(texture.width / 2, texture.height / 2, texture.depth / 2);
            Texture3D resizedTexture = new Texture3D(dimension.x, dimension.y, dimension.z, texture.format, texture.mipmapCount > 1);
            resizedTexture.filterMode = texture.filterMode;
            resizedTexture.wrapMode = texture.wrapMode;

            for (int x = 0; x < dimension.x; x++)
            {
                for (int y = 0; y < dimension.y; y++)
                {
                    for (int z = 0; z < dimension.z; z++)
                    {
                        Vector3 uvw = new Vector3((float)x / (float)dimension.x, (float)y / (float)dimension.y, (float)z / (float)dimension.z);

                        Color pixel = texture.GetPixelBilinear(uvw.x, uvw.y, uvw.z, 1);

                        resizedTexture.SetPixel(x, y, z, pixel, 0);
                    }
                }
            }

            resizedTexture.Apply();

            return resizedTexture;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (this.InPrefabScene()) return;
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            probeVolumeMin = new Vector4(size.x * -0.5f, size.y * -0.5f, size.z * -0.5f, 0);
            probeVolumeSizeInverse = new Vector4(1f / size.x, 1f / size.y, 1f / size.z, 0);

            if (TryGetComponent(out LightProbeVolumeGenerator generator))
            {
                if (generator.autoUpdateLightProbes)
                {
                    generator.UpdateLightProbes();
                }
                if (generator.autoUpdateBoxCollider)
                {
                    generator.UpdateBoxCollider();
                }
            }
        }

        protected void OnBakeCompleted(LightmapBakeHelper.BakeResult bakeResult)
        {
            if (bakeResult == LightmapBakeHelper.BakeResult.Successfull && TryGetComponent(out LightProbeVolumeGenerator generator))
            {
                generator.Generate3dTextures();
                if (!this.GetComponentInParent<LightingGroup>())
                {
                    GameObject sourcePrefab = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(this.gameObject);
                    string sourcePrefabPath = sourcePrefab ? UnityEditor.AssetDatabase.GetAssetPath(sourcePrefab) : this.gameObject.scene.path;
                    Export3DTextures(Path.GetDirectoryName(sourcePrefabPath));
                }
            }
        }

        public static void ReduceAndSave3DTextures(List<LightingGroup.LightProbeVolumeReference> references)
        {
            foreach (LightingGroup.LightProbeVolumeReference reference in references)
            {
                LightProbeVolume.ReduceAndSave3DTexture(reference.lightProbeVolume.SHAr);
                LightProbeVolume.ReduceAndSave3DTexture(reference.lightProbeVolume.SHAg);
                LightProbeVolume.ReduceAndSave3DTexture(reference.lightProbeVolume.SHAb);
                LightProbeVolume.ReduceAndSave3DTexture(reference.lightProbeVolume.occ);
            }
        }

        public static void ReduceAndSave3DTexture(Texture3D texture3D)
        {
            string texture3DPath = UnityEditor.AssetDatabase.GetAssetPath(texture3D);
            Debug.Log($"Reduce and save 3D Texture: {texture3DPath}");
            Texture3D newTexture3D = DiscardTopMipmap(texture3D);
            Common.EditorCreateOrReplaceAsset(newTexture3D, texture3DPath);
        }

        public static bool TryReduceAndSave3DTexture(Texture3D texture3D, string newTexture3DPath)
        {
            Texture3D newTexture3D = DiscardTopMipmap(texture3D);
            if (newTexture3D)
            {
                Debug.Log($"Reduce and save 3D Texture: {newTexture3DPath}");
                Common.EditorCreateOrReplaceAsset(newTexture3D, newTexture3DPath);
                return true;
            }
            return false;
        }

        public void Export3DTextures(string targetFolderPath)
        {
            if (!string.IsNullOrEmpty(targetFolderPath))
            {
                if (SHAr && string.IsNullOrEmpty(UnityEditor.AssetDatabase.GetAssetPath(SHAr)))
                {
                    string assetPath = Path.Combine(targetFolderPath, name + "_SHAr.asset");
                    Texture3D existingSHAr = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture3D>(assetPath);
                    if (existingSHAr != null)
                    {
                        SHAr.name = name + "_SHAr";
                        UnityEditor.EditorUtility.CopySerialized(SHAr, existingSHAr);
                        SHAr = existingSHAr;
                    }
                    else
                    {
                        UnityEditor.AssetDatabase.CreateAsset(SHAr, assetPath);
                        SHAr = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture3D>(assetPath);
                    }
                }

                if (SHAg && string.IsNullOrEmpty(UnityEditor.AssetDatabase.GetAssetPath(SHAg)))
                {
                    Texture3D existingSHAg = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture3D>(Path.Combine(targetFolderPath, name + "_SHAg.asset"));
                    if (existingSHAg != null)
                    {
                        SHAg.name = name + "_SHAg";
                        UnityEditor.EditorUtility.CopySerialized(SHAg, existingSHAg);
                        SHAg = existingSHAg;
                    }
                    else
                    {
                        UnityEditor.AssetDatabase.CreateAsset(SHAg, Path.Combine(targetFolderPath, name + "_SHAg.asset"));
                        SHAg = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture3D>(Path.Combine(targetFolderPath, name + "_SHAg.asset"));
                    }
                }

                if (SHAb && string.IsNullOrEmpty(UnityEditor.AssetDatabase.GetAssetPath(SHAb)))
                {
                    Texture3D existingSHAb = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture3D>(Path.Combine(targetFolderPath, name + "_SHAb.asset"));
                    if (existingSHAb != null)
                    {
                        SHAb.name = name + "_SHAb";
                        UnityEditor.EditorUtility.CopySerialized(SHAb, existingSHAb);
                        SHAb = existingSHAb;
                    }
                    else
                    {
                        UnityEditor.AssetDatabase.CreateAsset(SHAb, Path.Combine(targetFolderPath, name + "_SHAb.asset"));
                        SHAb = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture3D>(Path.Combine(targetFolderPath, name + "_SHAb.asset"));
                    }
                }

                if (useOcclusion && occ && string.IsNullOrEmpty(UnityEditor.AssetDatabase.GetAssetPath(occ)))
                {
                    Texture3D existingOcc = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture3D>(Path.Combine(targetFolderPath, name + "_Occ.asset"));
                    if (existingOcc != null)
                    {
                        occ.name = name + "_Occ";
                        UnityEditor.EditorUtility.CopySerialized(occ, existingOcc);
                        occ = existingOcc;
                    }
                    else
                    {
                        UnityEditor.AssetDatabase.CreateAsset(occ, Path.Combine(targetFolderPath, name + "_Occ.asset"));
                        occ = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture3D>(Path.Combine(targetFolderPath, name + "_Occ.asset"));
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.matrix = transform.localToWorldMatrix;

            if (editingSizeThroughEditor)
            {
                Gizmos.color = new Color(1f, .1f, .2f, .2f);
                Gizmos.DrawCube(Vector3.right / 2f * size.x, new Vector3(0f, size.y, size.z));
                Gizmos.DrawCube(Vector3.left / 2f * size.x, new Vector3(0f, size.y, size.z));
                Gizmos.DrawCube(Vector3.forward / 2f * size.z, new Vector3(size.x, size.y, 0f));
                Gizmos.DrawCube(Vector3.back / 2f * size.z, new Vector3(size.x, size.y, 0f));
                Gizmos.DrawCube(Vector3.up / 2f * size.y, new Vector3(size.x, 0f, size.z));
                Gizmos.DrawCube(Vector3.down / 2f * size.y, new Vector3(size.x, 0f, size.z));
            }
            else
            {
                Gizmos.color = new Color(1f, .1f, .2f);
                Gizmos.DrawWireCube(Vector3.zero, size);
            }
        }
#endif
    }
}
