using UnityEngine;
using UnityEngine.Rendering;
using System;

namespace ThunderRoad
{
    [RequireComponent(typeof(LightProbeVolume))]
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Areas/LightProbeVolume.html")]
    public class LightProbeVolumeGenerator : MonoBehaviour
    {
#if UNITY_EDITOR
        [Tooltip("The resolution of the Light Probe Volume")]
        public Vector3Int textureResolution = new Vector3Int(2, 2, 2);
        [Tooltip("Calculates the Texture Resolution by calculating the height by the width.")]
        public Vector2 textureResInterval = new Vector2(0.25f, 0.25f);
        public Vector3Int lightprobeResolution = new Vector3Int(2, 2, 2);
        [Tooltip("No other format is currently supported.")]
        public TextureFormat textureFormat = TextureFormat.RGBAHalf;
        [Tooltip("Does the 3D Lightprobe volume use Mipmaps?")]
        public bool useMipmaps = true;

        [Tooltip("When enabled, will create a grid of light probes in the radius of the lightprobe volume box.")]
        public bool autoUpdateLightProbes = false;
        [Tooltip("Will automatically adjust the box collider size to match the Light Probe Volume size. This box is naturally a little smaller than the light probe volume box.")]
        public bool autoUpdateBoxCollider = true;
        [Tooltip("Adds an offset to the box collider used for the Light Probe Volume.")]
        public Vector3 boxColliderSizeOffset;
        [Tooltip("Automatically sets the layer. It is recommended that Light Probe Volumes stay on the \"LightProbevolume\" layer")]
        public bool autoSetLayer = true;
        public int layerToSet = 8;
        [Tooltip("Visualize the intervals which show the resolution of the 3D volume")]
        public bool visualizeIntervals = false;
        [Tooltip("Visualizes the Light Probe Volumes which show colour of their area.")]
        public bool visualizeDataPlane = false;
        [Tooltip("Adjust the height of the Data Plane.")]
        [Range(-1f, 1f)]
        public float dataPlaneAxisPosition = 0f;

        [HideInInspector]
        public LightProbeVolume lightProbeVolume;

        private Mesh dataPlane;
        private Material dataMaterial;

        public static event Action OnGenerationStarted;
        public static event Action OnGenerationEnded;

        private void OnValidate()
        {
            if (!this.InPrefabScene()) return;
            if (this.gameObject.scene.name == null) return;

            textureResolution.x = Mathf.Max(2, textureResolution.x);
            textureResolution.y = Mathf.Max(2, textureResolution.y);
            textureResolution.z = Mathf.Max(2, textureResolution.z);

            textureResolution.x = Mathf.ClosestPowerOfTwo(textureResolution.x);
            textureResolution.y = Mathf.ClosestPowerOfTwo(textureResolution.y);
            textureResolution.z = Mathf.ClosestPowerOfTwo(textureResolution.z);

            lightprobeResolution.x = Mathf.Max(2, lightprobeResolution.x);
            lightprobeResolution.y = Mathf.Max(2, lightprobeResolution.y);
            lightprobeResolution.z = Mathf.Max(2, lightprobeResolution.z);

            lightProbeVolume = GetComponent<LightProbeVolume>();

            if (autoUpdateLightProbes)
            {
                UpdateLightProbes();
            }
            if (autoUpdateBoxCollider)
            {
                UpdateBoxCollider();
            }
            if (autoSetLayer)
            {
                UnityEditor.EditorApplication.delayCall += OnValidateDelayed;
            }
        }

        private void OnValidateDelayed()
        {
            // Avoid "SendMessage cannot be called during Awake, CheckConsistency, or OnValidate"
            UnityEditor.EditorApplication.delayCall -= OnValidateDelayed;
            if (this == null) return;
            gameObject.layer = layerToSet;
        }

        public void UpdateLightProbes()
        {
            LightProbeGroup group = GetComponent<LightProbeGroup>();
            if (group == null)
            {
                group = gameObject.AddComponent<LightProbeGroup>();
            }

            Vector3 halfSize = lightProbeVolume.size * 0.5f;
            Vector3 interval = new Vector3(lightProbeVolume.size.x / (lightprobeResolution.x - 1), lightProbeVolume.size.y / (lightprobeResolution.y - 1), lightProbeVolume.size.z / (lightprobeResolution.z - 1));
            Vector3[] positions = new Vector3[lightprobeResolution.x * lightprobeResolution.y * lightprobeResolution.z];
            for (int x = 0; x < lightprobeResolution.x; x++)
            {
                for (int y = 0; y < lightprobeResolution.y; y++)
                {
                    for (int z = 0; z < lightprobeResolution.z; z++)
                    {
                        positions[x + lightprobeResolution.x * (y + lightprobeResolution.y * z)] = new Vector3(x * interval.x - halfSize.x, y * interval.y - halfSize.y, z * interval.z - halfSize.z);
                    }
                }
            }
            group.probePositions = positions;
        }

        public void UpdateBoxCollider()
        {
            if (TryGetComponent(out BoxCollider collider))
            {
                collider.size = lightProbeVolume.size + boxColliderSizeOffset;
            }
        }

        static void CalculateSphericalHarmonicsAtPositions(Matrix4x4 localToWorld, Vector3 size, Vector3Int resolution, out SphericalHarmonicsL2[] lightProbesSH, out Vector4[] occlusionProbes)
        {
            int count = resolution.x * resolution.y * resolution.z;
            Vector3[] positions = new Vector3[count];
            Vector3 halfSize = size * 0.5f;
            Vector3 interval = new Vector3(size.x / (resolution.x - 1), size.y / (resolution.y - 1), size.z / (resolution.z - 1));
            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    for (int z = 0; z < resolution.z; z++)
                    {
                        positions[x + resolution.x * (y + resolution.y * z)] = localToWorld.MultiplyPoint3x4(new Vector3(x * interval.x - halfSize.x, y * interval.y - halfSize.y, z * interval.z - halfSize.z));
                    }
                }
            }

            lightProbesSH = new SphericalHarmonicsL2[positions.Length];
            occlusionProbes = new Vector4[positions.Length];
            LightProbes.CalculateInterpolatedLightAndOcclusionProbes(positions, lightProbesSH, occlusionProbes);
        }

        public bool Generate3dTextures()
        {
            OnGenerationStarted?.Invoke();
            Debug.LogFormat(this, "Generating 3DTexture on " + this.name);
            int width = Mathf.ClosestPowerOfTwo(textureResolution.x);
            int height = Mathf.ClosestPowerOfTwo(textureResolution.y);
            int depth = Mathf.ClosestPowerOfTwo(textureResolution.z);

            lightProbeVolume.SHAr = new Texture3D(width, height, depth, textureFormat, useMipmaps);
            lightProbeVolume.SHAr.wrapMode = TextureWrapMode.Clamp;
            lightProbeVolume.SHAr.filterMode = FilterMode.Bilinear;

            lightProbeVolume.SHAg = new Texture3D(width, height, depth, textureFormat, useMipmaps);
            lightProbeVolume.SHAg.wrapMode = TextureWrapMode.Clamp;
            lightProbeVolume.SHAg.filterMode = FilterMode.Bilinear;

            lightProbeVolume.SHAb = new Texture3D(width, height, depth, textureFormat, useMipmaps);
            lightProbeVolume.SHAb.wrapMode = TextureWrapMode.Clamp;
            lightProbeVolume.SHAb.filterMode = FilterMode.Bilinear;

            lightProbeVolume.occ = new Texture3D(width, height, depth, TextureFormat.RGBA32, useMipmaps);
            lightProbeVolume.occ.wrapMode = TextureWrapMode.Clamp;
            lightProbeVolume.occ.filterMode = FilterMode.Bilinear;

            CalculateSphericalHarmonicsAtPositions(transform.localToWorldMatrix, lightProbeVolume.size, textureResolution, out SphericalHarmonicsL2[] lightProbesSH, out Vector4[] occlusionProbes);

            for (int z = 0; z < textureResolution.z; z++)
            {
                for (int x = 0; x < textureResolution.x; x++)
                {
                    for (int y = 0; y < textureResolution.y; y++)
                    {
                        int index = x + textureResolution.x * (y + textureResolution.y * z);
                        Vector4[] coeff = LightProbeVolume.GetShaderSHL1CoeffsFromNormalizedSH(lightProbesSH[index]);

                        lightProbeVolume.SHAr.SetPixel(x, y, z, coeff[0]);
                        lightProbeVolume.SHAg.SetPixel(x, y, z, coeff[1]);
                        lightProbeVolume.SHAb.SetPixel(x, y, z, coeff[2]);
                        lightProbeVolume.occ.SetPixel(x, y, z, occlusionProbes[index]);
                    }
                }
            }

            lightProbeVolume.SHAr.Apply(true);
            lightProbeVolume.SHAg.Apply(true);
            lightProbeVolume.SHAb.Apply(true);
            lightProbeVolume.occ.Apply(true);

            OnGenerationEnded?.Invoke();
            return true;
        }

        private void OnDrawGizmos()
        {
            if (visualizeIntervals && lightProbeVolume != null)
            {
                Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
                Gizmos.matrix = transform.localToWorldMatrix;
                Vector3 interval = new Vector3(lightProbeVolume.size.x / textureResolution.x, lightProbeVolume.size.y / textureResolution.y, lightProbeVolume.size.z / textureResolution.z);
                Vector3 halfSize = lightProbeVolume.size * 0.5f;
                Vector3 halfInterval = interval * 0.5f;

                for (int x = 0; x < textureResolution.x; x += textureResolution.x - 1)
                {
                    for (int y = 0; y < textureResolution.y; y++)
                    {
                        for (int z = 0; z < textureResolution.z; z++)
                        {
                            Gizmos.DrawWireCube(halfSize - new Vector3(interval.x * x, interval.y * y, interval.z * z) - halfInterval, interval);
                        }
                    }
                }

                for (int x = 0; x < textureResolution.x; x++)
                {
                    for (int y = 0; y < textureResolution.y; y += textureResolution.y - 1)
                    {
                        for (int z = 0; z < textureResolution.z; z++)
                        {
                            Gizmos.DrawWireCube(halfSize - new Vector3(interval.x * x, interval.y * y, interval.z * z) - halfInterval, interval);
                        }
                    }
                }


                for (int x = 0; x < textureResolution.x; x++)
                {
                    for (int y = 0; y < textureResolution.y; y++)
                    {
                        for (int z = 0; z < textureResolution.z; z += textureResolution.z - 1)
                        {
                            Gizmos.DrawWireCube(halfSize - new Vector3(interval.x * x, interval.y * y, interval.z * z) - halfInterval, interval);
                        }
                    }
                }

                Gizmos.matrix = Matrix4x4.identity;
            }

            if (visualizeDataPlane && lightProbeVolume != null && lightProbeVolume.SHAr != null && lightProbeVolume.SHAg != null && lightProbeVolume.SHAb != null)
            {
                if (dataPlane == null)
                {
                    Vector3 halfSize = lightProbeVolume.size * 0.5f;
                    dataPlane = new Mesh();
                    dataPlane.vertices = new Vector3[]
                    {
                        new Vector3(-halfSize.x, 0, -halfSize.z),
                        new Vector3(halfSize.x, 0, -halfSize.z),
                        new Vector3(halfSize.x, 0, halfSize.z),
                        new Vector3(-halfSize.x, 0, halfSize.z) }
                    ;
                    dataPlane.normals = new Vector3[]
                    {
                        new Vector3(0, 1, 0),
                        new Vector3(0, 1, 0),
                        new Vector3(0, 1, 0),
                        new Vector3(0, 1, 0)
                    };
                    dataPlane.uv = new Vector2[]
                    {
                        new Vector2(0,0),
                        new Vector2(1,0),
                        new Vector2(1,1),
                        new Vector2(0,1)
                    };
                    dataPlane.SetTriangles(new int[] { 2, 1, 0, 0, 3, 2 }, 0);
                }

                if (dataMaterial == null)
                {
                    dataMaterial = new Material(Shader.Find("Hidden/RainyReignGames/ProbeVolumeVisualization"));
                }

                dataMaterial.SetFloat("_CullMode", 0);
                lightProbeVolume.UpdateMaterialProperties(dataMaterial);
                dataMaterial.SetPass(0);

                Vector3 offset;
                if (dataPlaneAxisPosition >= 0)
                {
                    offset = transform.transform.TransformPoint(new Vector3(0, lightProbeVolume.size.y * 0.5f, 0));
                    offset = Vector3.Lerp(transform.position, offset, dataPlaneAxisPosition);
                }
                else
                {
                    offset = transform.transform.TransformPoint(new Vector3(0, -lightProbeVolume.size.y * 0.5f, 0));
                    offset = Vector3.Lerp(offset, transform.position, dataPlaneAxisPosition + 1f);
                }

                Graphics.DrawMeshNow(dataPlane, offset, transform.rotation);
            }
        }
#endif
    }
}
