using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Object = UnityEngine.Object;

namespace Shadowood.RaycastTexture
{
    [Serializable]
    public class CaptureTexture
    {
        public Texture2D resultTex2D = null;
        public Texture2D lightmapColor = null;
        public Texture2D shadowMask = null;
        public Texture2D lightmapDir = null;
        public Vector4 lightmapScaleOffset;

        private MaterialPropertyBlock matProp;

        public bool showCamera = false;

        public Material mat;
        public Mesh mesh;
        public Matrix4x4 matrix;

        public bool reuseTex2D;

        public string name;

        //

        public const string camName = "tempCamera-CaptureTexture";

        /// <summary>
        /// Flattens the MeshRenderers mesh to lightmap UV1 space including the scale/offset required to make it match the baked lightmap
        /// This can be then placed infront of a capture camera and the resulting texture will match up perfectly with a lightmap for the same mesh renderer.
        /// </summary>
        public Mesh FlattenMeshToUV(MeshRenderer rendererIn)
        {
            var meshIn = rendererIn.GetComponent<MeshFilter>().sharedMesh;

            var scaleOffset = rendererIn.lightmapScaleOffset;
            var textureOffset = new Vector2(scaleOffset.z, scaleOffset.w);
            var textureScale = new Vector2(scaleOffset.x, scaleOffset.y);
            
            var uv2t = meshIn.uv2;
            var uv2s = uv2t != null && uv2t.Length > 0;
            
            return FlattenMeshToUV(meshIn,textureOffset,textureScale, uv2s);
        }

        public Mesh FlattenMeshToUV(Mesh meshIn, Vector2 textureOffset, Vector2 textureScale, bool uv2s)
        {
            float bound = 100000f;
            var tmesh = new Mesh();
            Copy(meshIn, tmesh);
            tmesh.bounds = new Bounds(Vector3.zero, new Vector3(bound, 1, bound));

            List<Vector3> verts = new List<Vector3>();
            tmesh.GetVertices(verts);
            
            
            for (int i = 0; i < verts.Count; i++)
            {
                if (uv2s)
                {
                    verts[i] = new Vector3(tmesh.uv2[i].x, tmesh.uv2[i].y, 0);
                }
                else
                {
                    verts[i] = new Vector3(tmesh.uv[i].x, tmesh.uv[i].y, 0);
                }

                verts[i] *= textureScale;
                verts[i] += new Vector3(textureOffset.x,textureOffset.magnitude,0);
            }

            tmesh.SetVertices(verts);

            return tmesh;
        }

        // TODO move to utility class
        public static void Copy(Mesh src, Mesh dst)
        {
            if (dst == null || src == null) return;

            if (!src.isReadable) Debug.LogError("Copy: Mesh needs to be set to Read/Write in import settings: " + src.name);

            dst.Clear();
            dst.vertices = src.vertices;

            List<Vector4> uvs = new List<Vector4>();

            src.GetUVs(0, uvs);
            dst.SetUVs(0, uvs);
            src.GetUVs(1, uvs);
            dst.SetUVs(1, uvs);
            //src.GetUVs(2, uvs); dst.SetUVs(2, uvs);
            //src.GetUVs(3, uvs); dst.SetUVs(3, uvs);

            dst.normals = src.normals;
            dst.tangents = src.tangents;
            //dst.boneWeights = src.boneWeights;
            dst.colors = src.colors;
            //dst.bindposes = src.bindposes;

            dst.subMeshCount = src.subMeshCount;
            dst.indexFormat = src.indexFormat;
            dst.bounds = src.bounds;

            for (var i = 0; i < src.subMeshCount; i++) dst.SetIndices(src.GetIndices(i), src.GetTopology(i), i);

            dst.name = src.name + "-Copy";
        }

        public void DrawNowAdd()
        {
            RenderPipelineManager.beginCameraRendering -= DrawNow;
            RenderPipelineManager.beginCameraRendering += DrawNow;
        }
        
        public void DrawNowRemove()
        {
            RenderPipelineManager.beginCameraRendering -= DrawNow;
        }

        /// <summary>
        /// Flattens the mesh with 'FlattenMeshToUV' and create a camera to capture the mesh, also applies the usual lightmap to the mesh copy so it gets the same baked lighting
        /// </summary>
        public Texture2D CaptureTex(MeshRenderer rendererIn, int res = 0)
        {
            var mr = rendererIn;

            mat = new Material(rendererIn.sharedMaterial) {name = rendererIn.sharedMaterial.name + " - Copy"};

            if (res <= 0) res = mat.mainTexture.width; // TODO match against lightmap width insteadd?

            float bound = 100000f;
            mesh = FlattenMeshToUV(rendererIn);

            // TODO copy lightmap

            matProp = new MaterialPropertyBlock();

            Camera tempCamera = GameObject.Find(camName)?.GetComponent<Camera>();
            if (tempCamera == null) tempCamera = new GameObject(camName, typeof(Camera)).GetComponent<Camera>();
            tempCamera.gameObject.hideFlags = HideFlags.DontSave;

            RenderTexture rt = RenderTexture.GetTemporary(res, res, 0);
            RenderTexture.active = rt;

            tempCamera.clearFlags = CameraClearFlags.SolidColor;
            tempCamera.backgroundColor = Color.red;
            tempCamera.targetTexture = RenderTexture.active;
            tempCamera.orthographic = true;
            tempCamera.enabled = showCamera;
            tempCamera.gameObject.transform.SetPositionAndRotation(new Vector3(0, 10000, 0), Quaternion.identity);
            tempCamera.nearClipPlane = 0;
            tempCamera.farClipPlane = 2;
            tempCamera.orthographicSize = 0.5f;
            tempCamera.useOcclusionCulling = false;
            if (tempCamera.GetComponent<UniversalAdditionalCameraData>() == null)
            {
                tempCamera.gameObject.AddComponent<UniversalAdditionalCameraData>();
            }
            tempCamera.GetComponent<UniversalAdditionalCameraData>().renderShadows = false;
            

            DrawNowAdd();

            matrix = Matrix4x4.TRS(new Vector3(-0.5f, 10000 - 0.5f, 1), Quaternion.identity, Vector3.one);



            

            bool applyLightmap = true;

            if (applyLightmap)
            {
                lightmapColor = null;
                shadowMask = null;
                lightmapDir = null;
                
                var lightmapSettings = LightmapSettings.lightmaps;
                
                if (mr.lightmapIndex >= 0 && lightmapSettings != null && mr.lightmapIndex < lightmapSettings.Length)
                {
                    lightmapColor = lightmapSettings[mr.lightmapIndex].lightmapColor;
                    shadowMask = lightmapSettings[mr.lightmapIndex].shadowMask;
                    lightmapDir = lightmapSettings[mr.lightmapIndex].lightmapDir;
                }

                lightmapScaleOffset = mr.lightmapScaleOffset;
                //var oceanBakeMatrix = mr.transform.worldToLocalMatrix;

                if (lightmapColor)
                {
                    mat.EnableKeyword("LIGHTMAP_ON");
                    mat.SetTexture("unity_Lightmap", lightmapColor);
                    if (lightmapDir) mat.SetTexture("unity_LightmapInd", lightmapDir);
                    if (shadowMask) mat.SetTexture("unity_ShadowMask", shadowMask);
                    //mat.SetTexture("unity_SpecCube0", );
                    mat.SetVector("unity_LightmapST", lightmapScaleOffset);


                    matProp.SetTexture("unity_Lightmap", lightmapColor);
                    if (lightmapDir) matProp.SetTexture("unity_LightmapInd", lightmapDir);
                    if (shadowMask) matProp.SetTexture("unity_ShadowMask", shadowMask);
                    //mat.SetTexture("unity_SpecCube0", );
                    matProp.SetVector("unity_LightmapST", lightmapScaleOffset);

                    //mat.SetTexture("_MainLightShadowmapTexture", );
                    //if (oceanbakeLM) matIn.SetMatrix(BakeMatrix, oceanBakeMatrix);
                }
                else
                {
                    mat.DisableKeyword("LIGHTMAP_ON");
                }
            }

            name = rendererIn.name + "-" + mat.name + "-" + mat.mainTexture.name;

            tempCamera.Render();

            if (!showCamera) DrawNowRemove();
#if UNITY_EDITOR //fix build compile errors
            if (reuseTex2D)
            {
                resultTex2D = rt.ToTexture2D(ref resultTex2D, TextureFormat.RGBAFloat, true);
            }
            else
            {
                resultTex2D = rt.ToTexture2D(TextureFormat.RGBAFloat, true);
            }

            resultTex2D.name = name;
#endif

            if (!showCamera) RenderTexture.ReleaseTemporary(rt);

            if (!showCamera) Object.DestroyImmediate(tempCamera.gameObject);

            return resultTex2D;
        }


        private void DrawNow(ScriptableRenderContext src, Camera camera)
        {
            if (camera.orthographic == false || camera.name != camName) return;

            //Debug.Log("DrawNow: " + name);

            //MaterialPropertyBlock matProp = new MaterialPropertyBlock();

            Graphics.DrawMesh(mesh, matrix, mat, 0, camera, 0, matProp, ShadowCastingMode.Off, false, null, LightProbeUsage.Off);

            UniversalRenderPipeline.RenderSingleCamera(src, camera);
        }
    }
}
