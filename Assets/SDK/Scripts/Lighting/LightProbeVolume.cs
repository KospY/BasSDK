using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.IO;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [ExecuteInEditMode]
    public class LightProbeVolume : MonoBehaviour
    {
        public static List<LightProbeVolume> list = new List<LightProbeVolume>();

        public Vector3 size = Vector3.one;

        public bool useOcclusion = true;

        public Vector3 ProbeVolumeMin { get { return probeVolumeMin; } }
        [SerializeField, HideInInspector]
        private Vector3 probeVolumeMin;

        public Vector3 ProbeVolumeSizeInverse { get { return probeVolumeSizeInverse; } }
        [SerializeField, HideInInspector]
        private Vector3 probeVolumeSizeInverse;

        public Texture3D SHAr;
        public Texture3D SHAg;
        public Texture3D SHAb;
        public Texture3D occ;
        private static readonly int UseProbeVolume = Shader.PropertyToID("_UseProbeVolume");
        private static readonly int ProbeWorldToTexture = Shader.PropertyToID("_ProbeWorldToTexture");
        private static readonly int VolumeMin = Shader.PropertyToID("_ProbeVolumeMin");
        private static readonly int ProbeVolumeSizeInv = Shader.PropertyToID("_ProbeVolumeSizeInv");
        private static readonly int ProbeVolumeShR = Shader.PropertyToID("_ProbeVolumeShR");
        private static readonly int ProbeVolumeShG = Shader.PropertyToID("_ProbeVolumeShG");
        private static readonly int ProbeVolumeShB = Shader.PropertyToID("_ProbeVolumeShB");
        private static readonly int ProbeVolumeOcc = Shader.PropertyToID("_ProbeVolumeOcc");
        private static readonly int UnitySHAr = Shader.PropertyToID("unity_SHAr");
        private static readonly int UnitySHAg = Shader.PropertyToID("unity_SHAg");
        private static readonly int UnitySHAb = Shader.PropertyToID("unity_SHAb");
        private static readonly int UnityProbesOcclusion = Shader.PropertyToID("unity_ProbesOcclusion");

        /// <summary>
        /// Used from the custom editor to change the gizmos.
        /// </summary>
        [NonSerialized] public bool editingSizeThroughEditor;
        
        private void OnEnable()
        {
            list.Add(this);
#if UNITY_EDITOR
            LightmapBakeHelper.onBakeCompleted += OnBakeCompleted;
#endif
        }

        private void OnDisable()
        {
            list.Remove(this);
#if UNITY_EDITOR
            LightmapBakeHelper.onBakeCompleted -= OnBakeCompleted;
#endif
        }

        public void UpdateMaterialProperties(Renderer renderer)
        {
            Material[] materials = null;
            if (Application.isPlaying)
            {
                //renderer.GetMaterials(gc_Materials);
                materials = renderer.materialInstances();
            }
            
            foreach (Material material in materials)
            {
                UpdateMaterialProperties(material);
            }
        }

        public void UpdateMaterialProperties(Material material)
        {
            if (material != null)
            {
                material.SetFloat(UseProbeVolume, 1); //this is to see it in the inspector
                material.EnableKeyword("_PROBEVOLUME_ON");
                material.SetMatrix(ProbeWorldToTexture, transform.worldToLocalMatrix);
                material.SetVector(VolumeMin, probeVolumeMin);
                material.SetVector(ProbeVolumeSizeInv, probeVolumeSizeInverse);

                material.SetTexture(ProbeVolumeShR, SHAr);
                material.SetTexture(ProbeVolumeShG, SHAg);
                material.SetTexture(ProbeVolumeShB, SHAb);

                if (useOcclusion)
                {
                    material.SetTexture(ProbeVolumeOcc, occ);
                }
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
                if (SHAr)
                {
                    Texture3D existingSHAr = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture3D>(Path.Combine(targetFolderPath, name + "_SHAr.asset"));
                    if (existingSHAr != null)
                    {
                        SHAr.name = name + "_SHAr";
                        UnityEditor.EditorUtility.CopySerialized(SHAr, existingSHAr);
                        SHAr = existingSHAr;
                    }
                    else
                    {
                        UnityEditor.AssetDatabase.CreateAsset(SHAr, Path.Combine(targetFolderPath, name + "_SHAr.asset"));
                        SHAr = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture3D>(Path.Combine(targetFolderPath, name + "_SHAr.asset"));
                    }
                }

                if (SHAg)
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

                if (SHAb)
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

                if (useOcclusion && occ)
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

            if(editingSizeThroughEditor)
            {
                Gizmos.color = new Color(1f, .1f, .2f, .2f);
                Gizmos.DrawCube(Vector3.right / 2f * size.x, new Vector3(0f, size.y, size.z));
                Gizmos.DrawCube(Vector3.left / 2f * size.x, new Vector3(0f, size.y, size.z));
                Gizmos.DrawCube(Vector3.forward / 2f * size.z, new Vector3(size.x, size.y, 0f));
                Gizmos.DrawCube(Vector3.back / 2f * size.z, new Vector3(size.x, size.y, 0f));
                Gizmos.DrawCube(Vector3.up / 2f * size.y, new Vector3(size.x, 0f, size.z));
                Gizmos.DrawCube(Vector3.down / 2f * size.y, new Vector3(size.x, 0f, size.z));
            }
            else {
                Gizmos.color = new Color(1f, .1f, .2f);
                Gizmos.DrawWireCube(Vector3.zero, size);
            }
        }
#endif
    }
}
