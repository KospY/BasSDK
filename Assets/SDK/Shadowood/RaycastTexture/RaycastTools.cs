#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using ThunderRoad;
using ThunderRoad.Splines;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using Object = UnityEngine.Object;

namespace Shadowood.RaycastTexture
{
    public static class RaycastTextureUtils
    {
        public static Texture2D ToTexture2D(this Texture rTex)
        {
            if (rTex == null) return null;
            if (rTex is Texture2D texA)
            {
                return texA;
            }

            if (rTex is RenderTexture texB)
            {
                return texB.ToTexture2D();
            }

            return null;
        }

        public static Texture2D ToTexture2D(this RenderTexture rTex, TextureFormat format = TextureFormat.RGBAFloat, bool linear = true)
        {
            //Debug.Log("Convert RT to Tex2D: " + rTex.name);
            Texture2D tex = new Texture2D(rTex.width, rTex.height, format, false, linear);
            tex.hideFlags = HideFlags.DontSave;
            RenderTexture.active = rTex;
            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0, false);
            tex.Apply();
            tex.name = rTex.name + " - Tex2D";
            return tex;
        }

        public static Texture2D ToTexture2D(this RenderTexture rTex, ref Texture2D tex, TextureFormat format = TextureFormat.RGBAFloat, bool linear = true)
        {
            //Debug.Log("Convert RT to Tex2D: " + rTex.name);
            if (tex == null || tex.width != rTex.width || tex.height != rTex.height || tex.format != format)
            {
                tex = new Texture2D(rTex.width, rTex.height, format, false, linear);
            }

            tex.hideFlags = HideFlags.DontSave;
            RenderTexture.active = rTex;

            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0, false);
            tex.Apply();
            tex.name = rTex.name + " - Tex2D";
            return tex;
        }
    }


    // Shows a cancellable progress bar for the specified number of seconds.
    public class EditorUtilityDisplayCancelableProgressBar : EditorWindow
    {
    }


    [Serializable]
    public class RaycastTools
    {
        //Mesh mesh,
        //int texSize,
        //int spreadSamples,
        //int subIndex,
        //bool useFirstUVs,
        ////int blurIterations,
        //float perPixelOcclusionDistance,
        //float perPixelBias,
        //int perPixelRays,
        //    Transform meshTransform,
        //bool debug)


        public struct CollisionStruct
        {
            public bool hit;

            public int index;

            public RaycastHit hitInfo;

            public Collider collider;

            public Mesh mesh;

            public Texture2D albedotexture;

            public Texture2D lightmaptexture;

            public Texture2D shadowmasktexture;
            //public Vector2 lightmapUV;
        }

        public static Texture2D DuplicateTexture(Texture2D source)
        {
            var nu = new Texture2D(source.width, source.height, source.format, source.mipmapCount, true);
            nu.filterMode = FilterMode.Bilinear;
            nu.hideFlags = HideFlags.DontSave;
            Graphics.CopyTexture(source, nu);
            return nu;

            RenderTexture renderTex = RenderTexture.GetTemporary(
                source.width,
                source.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height, source.format, false, true);
            readableText.hideFlags = HideFlags.DontSave;
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }

        public List<Texture> debugTex;

        public static List<MeshCollider> Copies(ref List<GameObject> meshColliders, bool hideCopies)
        {
            //var componentHitAnything = new List<bool>();
            var copies = new List<MeshCollider>();
            for (int i = 0; i < meshColliders.Count; i++)
            {
                var item = meshColliders[i];
                if (item == null) continue;
                //var mcOrg = item.GetComponent<MeshCollider>();
                var mrOrg = item.GetComponent<MeshRenderer>();
                var mfOrg = item.GetComponent<MeshFilter>();

                if (mrOrg == null)
                {
                    Debug.Log("No MeshRenderer on: " + item.name, item);
                    continue;
                }

                var itemCopy = new GameObject(item.gameObject.name + "-temp-" + i, typeof(MeshCollider), typeof(MeshRenderer), typeof(MeshFilter));
                itemCopy.hideFlags = hideCopies ? HideFlags.HideAndDontSave : HideFlags.DontSave;

                var mcCopy = itemCopy.GetComponent<MeshCollider>();
                var mrCopy = itemCopy.GetComponent<MeshRenderer>();
                var mfCopy = itemCopy.GetComponent<MeshFilter>();

                itemCopy.layer = 31;

                if (mrCopy)
                {
                    mrCopy.enabled = false;
                    mrCopy.lightmapIndex = mrOrg.lightmapIndex;
                    mrCopy.lightmapScaleOffset = mrOrg.lightmapScaleOffset;
                    mrCopy.scaleInLightmap = mrOrg.scaleInLightmap;
                }

                if (mfCopy)
                {
                    mfCopy.sharedMesh = mfOrg.sharedMesh;
                }

                Mesh mesh = null;
                //if (mcOrg)
                //{
                //    mesh = mcOrg.sharedMesh;
                //}
                if (mrOrg)
                {
                    var mf = mrOrg.GetComponent<MeshFilter>();
                    if (mf) mesh = mf.sharedMesh;
                }

                if (mesh != null)
                {
                    mcCopy.transform.SetPositionAndRotation(item.transform.position, item.transform.rotation);
                    mcCopy.transform.localScale = item.transform.lossyScale;

                    mcCopy.convex = false;
                    mcCopy.sharedMesh = mesh;

                    if (mrOrg) mrCopy.sharedMaterials = mrOrg.sharedMaterials;

                    copies.Add(mcCopy);
                }
            }

            return copies;
        }

        /// <summary>
        /// RGB for color capture of the entire world under the ocean plane, A depth capture of shoreline for transparency blend with eg: a sandy beach
        /// </summary>
        public static Texture CreateColorDepthMap(
            //Mesh mesh, 
            int texSizeX,
            int texSizeY,
            int subIndex,
            bool useFirstUVs,
            MeshCollider meshCollider,
            float maxDistance,
            float depthOffset,
            bool debug,
            ref List<MeshCollider> meshColliders,
            bool useLightmaps,
            bool useNewCapture,
            //int captureTextureRes,
            int mipLevel,
            //bool bilinearSample,
            ThunderSpline thunderSpline)
        {
            var componentHitAnything = new List<bool>();
            var componentExtras = new List<MeshCollider>();

            foreach (var o in meshColliders)
            {
                componentHitAnything.Add(false);
                componentExtras.Add(o);
            }

            var meshTransform = meshCollider.transform;
            //var surfaceCollider = meshCollider; //meshTransform.GetComponent<MeshCollider>();
            var mesh = meshCollider.sharedMesh;


            //TODO remove/ignore colliders not below the water surface, ocean can use plane or box, river/waterfall can use box colliders?

            Color[] pix = new Color[texSizeX * texSizeY];
            CollisionStruct[] collisionStructs = new CollisionStruct[pix.Length];

            int skipCount = 0;

            Dictionary<int, Texture2D> textureCopies = new Dictionary<int, Texture2D>();
            Dictionary<Texture2D, Color[]> textureCopiesData = new Dictionary<Texture2D, Color[]>();
            Dictionary<Mesh, bool> uv2present = new Dictionary<Mesh, bool>();

            double lastUpdatedProgress = 0;

            Vector3[] vertices = mesh.vertices;
            Vector2[] vector2Array = !useFirstUVs ? mesh.uv2 : mesh.uv;
            int[] numArray2 = subIndex >= 0 ? mesh.GetTriangles(subIndex) : mesh.triangles;

            //var captureTexture = new CaptureTexture();
            var captureRay = new CaptureRay();

            //bool useNewCapture = true;

            for (int index = 0; index < numArray2.Length; index += 3)
            {
                Vector2 a1 = vector2Array[numArray2[index]];
                Vector2 b = vector2Array[numArray2[index + 1]];
                Vector2 c = vector2Array[numArray2[index + 2]];
                Vector3 vector3 = Vector3.Normalize(vertices[numArray2[index + 1]] - vertices[numArray2[index]]);
                Vector3 normal = Vector3.Normalize(Vector3.Cross(vector3, vertices[numArray2[index + 2]] - vertices[numArray2[index]]));

                int num1 = Mathf.FloorToInt(Mathf.Min(a1.x, b.x, c.x) * (float) texSizeX);
                int num2 = Mathf.CeilToInt(Mathf.Max(a1.x, b.x, c.x) * (float) texSizeX);

                int num3 = Mathf.FloorToInt(Mathf.Min(a1.y, b.y, c.y) * (float) texSizeY);
                int num4 = Mathf.CeilToInt(Mathf.Max(a1.y, b.y, c.y) * (float) texSizeY);

                int pixelCounter = 0;
                for (int a2 = num1; a2 < num2; ++a2)
                {
                    int numX = modulo(a2, texSizeX);
                    for (int a3 = num3; a3 < num4; ++a3)
                    {
                        pixelCounter++;

                        if (Time.realtimeSinceStartup - lastUpdatedProgress > 1.0 / 5.0f)
                        {
                            lastUpdatedProgress = Time.realtimeSinceStartup;
                            if (EditorUtility.DisplayCancelableProgressBar("RaycastTools", "CreateDepthMap..." + index / 3 + "/" + (numArray2.Length / 3), (float) (index / 3.0f) / ((numArray2.Length / 3.0f))))
                            {
                                EditorUtility.ClearProgressBar();
                                return null;
                            }
                        }

                        int numY = modulo(a3, texSizeY);
                        if ((double) pix[numY * texSizeX + numX].a == 0.0)
                        {
                            Color colResult = Color.black;
                            Vector2 barycentricCoo = GetBarycentricCoo(new Vector2((float) ((double) a2 * 2.0 + 1.0) / (float) (texSizeX * 2), (float) ((double) a3 * 2.0 + 1.0) / (float) (texSizeY * 2)), a1, b, c);
                            if (isPInsideABC(barycentricCoo, a1, b, c, true))
                            {
                                float depthResult = 100; //float.MaxValue; // Default to max value/infinite distance if nothing is hit  eg: its just skybox under the water ( edit: dont use infinite as a blur post process will screw up )

                                var v1 = meshTransform.TransformPoint(vertices[numArray2[index + 0]]);
                                var v2 = meshTransform.TransformPoint(vertices[numArray2[index + 1]]);
                                var v3 = meshTransform.TransformPoint(vertices[numArray2[index + 2]]);
                                var normal2 = meshTransform.TransformDirection(normal);
                                //var vector3b = meshTransform.TransformDirection(vector3);

                                var worldPos = BarryCentricTo3D(barycentricCoo, v1, v2, v3);


                                ///for (int i = 0; i < meshColliders.Count; i++)
                                //var collider = surfaceCollider;

                                ///var collider = componentExtras[i];
                                ///if (collider == null) continue;

                                /*
                                if (Vector3.Distance(worldPos,collider.ClosestPoint(worldPos)) > maxDistance)
                                {
                                    continue;
                                    skipCount++;
                                }
                                */

                                //if (!collider.bounds.Contains(worldPos))
                                //{
                                //   skipCount++;
                                //   continue;
                                //}


                                if (useNewCapture)
                                {
                                    Vector3 direction2 = -normal2;
                                    var pos = worldPos;
                                    pos += new Vector3(0, depthOffset, 0);
                                    var captureRes = captureRay.Capture(pos, direction2, maxDistance);


                                    colResult = new Color(captureRes.r, captureRes.g, captureRes.b, 1);
                                    //depthResult = captureRes.a;

                                    depthResult = -Depth(worldPos, normal2, maxDistance, out var collisionStruct, depthOffset, debug);
                                }
                                else
                                {
                                    //CollisionStruct collisionStruct = new CollisionStruct();
                                    //worldPos = new Vector3(worldPos.x, worldPos.y + 0.1f, worldPos.z);
                                    var depthRes = -Depth(worldPos, normal2, maxDistance, out var collisionStruct, depthOffset, debug);

                                    if (collisionStruct.hit)
                                    {
                                        //Debug.Log("Hit: " + a2 + "x" + a3 +" " +worldPos + ":" + collisionStruct.hitInfo.collider.name, collisionStruct.hitInfo.collider.gameObject);

                                        ///if (collisionStruct.hit) componentHitAnything[i] = true;

                                        // Set the collisionStruct for this pixel to the collision that is closest, this will then be the mesh it takes albedo texture from
                                        //if (depthRes > result)
                                        //{
                                        //TODO bad perf, ran per per pixel

                                        var mr = collisionStruct.collider.gameObject.GetComponent<MeshRenderer>();

                                        if (mr == null) continue;
                                        var mat = mr.sharedMaterial;
                                        if (mat == null) continue;

                                        depthResult = depthRes;


                                        //if (useCaptureTexture)
                                        //{
                                        //    collisionStructs[numY * texSizeX + numX] = collisionStruct;
                                        //}
                                        //else
                                        {
                                            //TODO add emission texture support? no: wait for real shader capture instead
                                            Texture mt = null;

                                            if (mat.HasTexture("_BaseMap")) mt = mat.GetTexture("_BaseMap");
                                            if (mt == null && mat.HasTexture("_MainTex")) mt = mat.GetTexture("_MainTex");
                                            //if (mt==null) mt = mat.mainTexture;

                                            if (mt)
                                            {
                                                collisionStruct.albedotexture = mt as Texture2D;
                                            }

                                            if (collisionStruct.albedotexture != null)
                                            {
                                                collisionStruct.albedotexture = null;

                                                ///collisionStruct.index = i;
                                                /// 

                                                collisionStructs[numY * texSizeX + numX] = collisionStruct;
                                            }
                                        }
                                    }
                                }

                                if (thunderSpline && thunderSpline.isActiveAndEnabled)
                                {
                                    var inside = ThunderSplineWithin.IsInsideSpline(thunderSpline.transform.InverseTransformPoint(worldPos), thunderSpline);
                                    if (inside) depthResult = 0;
                                    //if (!inside) result = 1;
                                }

                                pix[numY * texSizeX + numX] = new Color(colResult.r, colResult.g, colResult.b, Mathf.Abs(depthResult));
                            }
                        }
                    }
                }

                if (!useNewCapture)
                {
                    //foreach (var collisionStruct in collisionStructs)
                    //{
                    //    collisionStruct.collider
                    //}

                    // Loop thru every pixel in the grid and if a collision was present on that pixel go and sample the texture hit
                    // captures a RGB color texture of the whole grid under the water surface along with multiplying it by any lightmap found
                    // It creates material copies and texture copies if needed
                    //
                    Profiler.BeginSample("Loopy");
                    for (int i = 0; i < pix.Length; i++)
                    {
                        var collisionStruct = collisionStructs[i];
                        if (collisionStruct.collider == null) continue;

                        //Debug.Log(i + "Collider: " + cs.collider.name, cs.collider);

                        MeshRenderer mr = null;
                        Material mat = null;

                        if (collisionStruct.albedotexture == null)
                        {
                            mr = collisionStruct.collider.gameObject.GetComponent<MeshRenderer>();
                            if (mr == null) continue;

                            //if (!useCaptureTexture)
                            {
                                mat = mr.sharedMaterial;
                                if (mat == null) continue;
                                //mat = cs.collider.sharedMaterial;
                                if (mat.mainTexture)
                                {
                                    //else
                                    {
                                        collisionStruct.albedotexture = mat.mainTexture as Texture2D;
                                        //if (cs.albedotexture == null && mat.HasTexture("_BaseMap")) cs.albedotexture = mat.GetTexture("_BaseMap") as Texture2D;
                                        //if (cs.albedotexture == null && mat.HasTexture("_MainTex")) cs.albedotexture = mat.GetTexture("_MainTex") as Texture2D;

                                        if (mr.sharedMaterial.name.Contains(" - RCClone"))
                                        {
                                            mat = mr.sharedMaterial;
                                        }
                                        else
                                        {
                                            mat = mr.sharedMaterial = new Material(mat);
                                            mat.name += " - RCClone";
                                            mat.hideFlags = HideFlags.DontSave;
                                        }
                                    }
                                }
                            }
                        }

                        ///componentHitAnything[collisionStructs[i].index] = true;


                        //TODO perf bad, lots of dict compares
                        /*
                        if (useCaptureTexture)
                        {
                            int hash = mr.GetHashCode();

                            if (textureCopies.ContainsKey(hash))
                            {
                                collisionStruct.albedotexture = textureCopies[hash];
                            }
                            else
                            {
                                captureTexture.reuseTex2D = false;
                                collisionStruct.albedotexture = captureTexture.CaptureTex(mr, captureTextureRes);

                                if (debug) Debug.Log("Raycast: clone texture: " + collisionStruct.albedotexture.name + " - " + hash, collisionStruct.collider);

                                //collisionStruct.albedotexture = DuplicateTexture(collisionStruct.albedotexture);
                                textureCopies[hash] = collisionStruct.albedotexture; // Cache the captured texture per meshRenderer

                                //mat.mainTexture = collisionStruct.albedotexture;
                            }
                        }
                        else*/
                        {
                            if (collisionStruct.albedotexture == null) continue;

                            if (collisionStruct.albedotexture && !collisionStruct.albedotexture.isReadable)
                            {
                                if (textureCopies.ContainsKey(collisionStruct.albedotexture.GetHashCode()))
                                {
                                    //Debug.Log("Raycast: found cloned texture: " + collisionStruct.albedotexture.name, collisionStruct.collider);
                                    collisionStruct.albedotexture = textureCopies[collisionStruct.albedotexture.GetHashCode()];
                                }
                                else
                                {
                                    var hash = collisionStruct.albedotexture.GetHashCode();

                                    if (debug) Debug.Log("Raycast: clone texture: " + collisionStruct.albedotexture.name, collisionStruct.collider);

                                    collisionStruct.albedotexture = DuplicateTexture(collisionStruct.albedotexture);
                                    textureCopies[hash] = collisionStruct.albedotexture;

                                    mat.mainTexture = collisionStruct.albedotexture;
                                }
                            }
                        }

                        //var col = cs.albedotexture.GetPixel((int) (cs.albedotexture.width * uv.x), (int) (cs.albedotexture.height * uv.y));


                        var albedo = Color.white;
                        var lightmap = Color.white;
                        var shadowmaskRes = Color.black;

                        //bool bilinearSample = true;

                        if (collisionStruct.albedotexture)
                        {
                            var uv = collisionStruct.hitInfo.textureCoord;

                            //if (!useCaptureTexture)
                            {
                                uv *= mat.mainTextureScale;
                                uv += mat.mainTextureOffset;
                            } /*
                            else
                            {
                                // Use the lightmapuv when useCaptureTexture
                                var lightmapScaleOffset = mr.lightmapScaleOffset;

                                var sharedMesh = collisionStruct.mesh;

                                bool uv2s = false;
                                if (!uv2present.ContainsKey(sharedMesh))
                                {
                                    var uv2t = sharedMesh.uv2;
                                    uv2s = uv2present[sharedMesh] = uv2t != null && uv2t.Length > 0;
                                }
                                else
                                {
                                    uv2s = uv2present[sharedMesh];
                                }

                                if (uv2s)
                                {
                                    uv = collisionStruct.hitInfo.textureCoord2;
                                    //uv *= new Vector2(lightmapScaleOffset.x, lightmapScaleOffset.y);
                                    //uv += new Vector2(lightmapScaleOffset.z, lightmapScaleOffset.w);
                                }
                                else
                                {
                                    uv = collisionStruct.hitInfo.textureCoord;
                                    uv *= new Vector2(lightmapScaleOffset.x, lightmapScaleOffset.y);
                                    uv += new Vector2(lightmapScaleOffset.z, lightmapScaleOffset.w);
                                }
                                //collisionStruct.mesh.;

                                //var uv2 = collisionStruct.hitInfo.lightmapCoord;

                                //uv2 *= new Vector2(lightmapScaleOffset.x, lightmapScaleOffset.y);
                                //uv2 += new Vector2(lightmapScaleOffset.z, lightmapScaleOffset.w);

                                //uv = collisionStruct.hitInfo.lightmapCoord;
                                //Debug.Log("Coord: " + uv +":"+ collisionStruct.hitInfo.textureCoord +":"+uv2);
                            }*/

                            //if (bilinearSample)
                            {
                                //if (useCaptureTexture) mipLevel = 0;
                                albedo = collisionStruct.albedotexture.GetPixelBilinear(uv.x, uv.y, mipLevel).linear; // mipLevel
                            }
                            /*
                            else
                            {
                                // TODO have a max texture size instead of mip?
    
                                var w = collisionStruct.albedotexture.width / (int) Math.Pow(2, mipLevel);
                                var h = collisionStruct.albedotexture.height / (int) Math.Pow(2, mipLevel);
    
                                if (!textureCopiesData.ContainsKey(collisionStruct.albedotexture))
                                {
                                    //Debug.Log("RaycastTools: Get all the pixels: " + collisionStruct.albedotexture.name + ":" + collisionStruct.albedotexture.width + ":" + w);
                                    textureCopiesData[collisionStruct.albedotexture] = collisionStruct.albedotexture.GetPixels(mipLevel);
                                }
                                else
                                {
                                    int x = Mathf.Clamp((int) ((uv.x % 1.0) * w), 0, w);
                                    int y = Mathf.Clamp((int) ((uv.y % 1.0) * h), 0, h) * w;
                                    int ind = Mathf.Clamp(x + y, 0, w * h);
    
                                    //if (debug) Debug.Log("Read pixel: " + x + ":" + y + " l:" + textureCopiesData[collisionStruct.albedotexture].Length + " : " + (uv.x % 1.0) + "x" + (uv.y % 1.0));
                                    albedo = textureCopiesData[collisionStruct.albedotexture][ind];
                                }
                            }*/
                        }

                        Color col = Color.black;

                        //if (!useCaptureTexture)
                        {
                            if (useLightmaps)
                            {
                                var lightmapSettings = LightmapSettings.lightmaps;
                                if (collisionStruct.lightmaptexture == null)
                                {
                                    if (mr == null) mr = collisionStruct.collider.gameObject.GetComponent<MeshRenderer>();

                                    if (mr.lightmapIndex >= 0 && lightmapSettings != null && mr.lightmapIndex < lightmapSettings.Length)
                                    {
                                        var lm = lightmapSettings[mr.lightmapIndex].lightmapColor;
                                        collisionStruct.lightmaptexture = lm;

                                        var sm = lightmapSettings[mr.lightmapIndex].shadowMask;
                                        collisionStruct.shadowmasktexture = sm;

                                        if (collisionStruct.lightmaptexture != null && !collisionStruct.lightmaptexture.isReadable)
                                        {
                                            if (textureCopies.ContainsKey(collisionStruct.lightmaptexture.GetHashCode()))
                                            {
                                                collisionStruct.lightmaptexture = textureCopies[collisionStruct.lightmaptexture.GetHashCode()];
                                            }
                                            else
                                            {
                                                var hash = lm.GetHashCode();
                                                if (debug) Debug.Log("Raycast: clone lightmap texture: " + collisionStruct.lightmaptexture.name, collisionStruct.collider);
                                                collisionStruct.lightmaptexture = DuplicateTexture(collisionStruct.lightmaptexture);
                                                textureCopies[hash] = collisionStruct.lightmaptexture;
                                            }
                                        }

                                        if (collisionStruct.shadowmasktexture != null && !collisionStruct.shadowmasktexture.isReadable)
                                        {
                                            if (textureCopies.ContainsKey(collisionStruct.shadowmasktexture.GetHashCode()))
                                            {
                                                collisionStruct.shadowmasktexture = textureCopies[collisionStruct.shadowmasktexture.GetHashCode()];
                                            }
                                            else
                                            {
                                                var hash = sm.GetHashCode();
                                                if (debug) Debug.Log("Raycast: clone shadowmask texture: " + collisionStruct.shadowmasktexture.name, collisionStruct.collider);
                                                collisionStruct.shadowmasktexture = DuplicateTexture(collisionStruct.shadowmasktexture);
                                                textureCopies[hash] = collisionStruct.shadowmasktexture;
                                            }
                                        }
                                    }
                                }

                                if (collisionStruct.lightmaptexture)
                                {
                                    //Vector2 uv2 = collisionStruct.lightmapUV;
                                    //var sharedMesh = collisionStruct.collider.sharedMesh;
                                    //var mf = collisionStruct.collider.GetComponent<MeshFilter>();

                                    //var sharedMesh = collisionStruct.collider.GetComponent<MeshFilter>().sharedMesh;
                                    var sharedMesh = collisionStruct.mesh;
                                    /// 
                                    Vector2 uv2 = Vector2.zero;

                                    bool uv2s = false;
                                    if (!uv2present.ContainsKey(sharedMesh))
                                    {
                                        var uv2t = sharedMesh.uv2;
                                        uv2s = uv2present[sharedMesh] = uv2t != null && uv2t.Length > 0;
                                    }
                                    else
                                    {
                                        uv2s = uv2present[sharedMesh];
                                    }

                                    if (uv2s)
                                    {
                                        uv2 = collisionStruct.hitInfo.textureCoord2;
                                    }
                                    else
                                    {
                                        uv2 = collisionStruct.hitInfo.textureCoord;
                                    }

                                    var lightmapScaleOffset = mr.lightmapScaleOffset;
                                    uv2 *= new Vector2(lightmapScaleOffset.x, lightmapScaleOffset.y);
                                    uv2 += new Vector2(lightmapScaleOffset.z, lightmapScaleOffset.w);
                                    //collisionStruct.lightmapUV = uv2;
                                    //uv2 = collisionStruct.hitInfo.lightmapCoord;

                                    //if (bilinearSample)
                                    {
                                        lightmap = collisionStruct.lightmaptexture.GetPixelBilinear(uv2.x, uv2.y); // mipLevel

                                        if (collisionStruct.shadowmasktexture)
                                        {
                                            var shadowMask = collisionStruct.shadowmasktexture.GetPixelBilinear(uv2.x, uv2.y); // mipLevel
                                            shadowmaskRes = new Color(shadowMask.r, shadowMask.r, shadowMask.r, 0);
                                            if (RenderSettings.sun && RenderSettings.sun.enabled && RenderSettings.sun.gameObject.activeInHierarchy)
                                            {
                                                shadowmaskRes *= RenderSettings.sun.color.linear * RenderSettings.sun.intensity * 0.3f; // TODO cant get the brightness to match
                                            }
                                            else
                                            {
                                                shadowmaskRes *= 0;
                                            }
                                        }
                                    }
                                    /*else
                                    {
                                        var w = collisionStruct.lightmaptexture.width / (int) Math.Pow(2, mipLevel);
                                        var h = collisionStruct.lightmaptexture.height / (int) Math.Pow(2, mipLevel);
            
                                        if (!textureCopiesData.ContainsKey(collisionStruct.lightmaptexture))
                                        {
                                            //Debug.Log("RaycastTools: Get all the pixels: " + collisionStruct.albedotexture.name + ":" + collisionStruct.albedotexture.width + ":" + w);
                                            textureCopiesData[collisionStruct.lightmaptexture] = collisionStruct.lightmaptexture.GetPixels(mipLevel);
                                        }
                                        else
                                        {
                                            int x = Mathf.Clamp((int) ((uv2.x % 1.0) * w), 0, w);
                                            int y = Mathf.Clamp((int) ((uv2.y % 1.0) * h), 0, h) * w;
                                            int ind = Mathf.Clamp(x + y, 0, w * h);
            
                                            //if (debug) Debug.Log("Read pixel: " + x + ":" + y + " l:" + textureCopiesData[collisionStruct.albedotexture].Length + " : " + (uv.x % 1.0) + "x" + (uv.y % 1.0));
                                            lightmap = textureCopiesData[collisionStruct.lightmaptexture][ind];
                                        }
                                    }*/

                                    //lightmap = collisionStruct.lightmaptexture.GetPixelBilinear(uv2.x, uv2.y);
                                    //Debug.Log("LM: " + uv2 + " : " + collisionStruct.lightmaptexture.name);

                                    //var lightmapCol = new Color(lightmap.r, lightmap.g, lightmap.b);
                                    //lightmapCol *= (lightmap.a * 8.0f);  // Decode lightmap

                                    //lightmap = new Color(lightmap.r, lightmap.g, lightmap.b, 0);
                                }

                                col = (albedo * mat.color) * (lightmap + shadowmaskRes);
                            }
                            else
                            {
                                col = (albedo * mat.color);
                            }
                        }
                        //else
                        //{
                        //    col = albedo;
                        //}

                        //Debug.Log(i + " found tex: " + cs.albedotexture.name + " : " + uv.x + "x" + uv.y + " - " + cs.collider.name, cs.collider);

                        pix[i] = new Color(col.r, col.g, col.b, pix[i].a);
                    }

                    Profiler.EndSample();
                }
            }

            collisionStructs = null;

            if (useNewCapture) captureRay.CaptureFinished();

            textureCopies.Clear();
            textureCopiesData.Clear();
            uv2present.Clear();

            /*
            for (int i = componentHitAnything.Count - 1; i >= 0; i--)
            {
                if (componentHitAnything[i] == false) RemoveItem(ref meshColliders, i);
            }*/

            //Debug.Log("Raycast: Skipped: " + skipCount);

            //Color[] colors = Spread(WornEdges.blur(pix, texSize, blurIterations, false), texSize, spreadSamples);
            //Color[] colors = Spread(pix, texSize, spreadSamples);

            Texture2D texture2D = new Texture2D(texSizeX, texSizeY, TextureFormat.RGBAFloat, false);
            texture2D.hideFlags = HideFlags.DontSave;
            texture2D.SetPixels(pix);
            texture2D.Apply();
            return texture2D;
        }

        private static void RemoveItem(ref List<MeshCollider> meshColliders, int i)
        {
            var o = meshColliders[i];
            if (o != null)
            {
                if (Application.isPlaying)
                {
                    Object.Destroy(o.gameObject);
                }
                else
                {
                    Object.DestroyImmediate(o.gameObject);
                }
            }

            meshColliders.RemoveAt(i);
        }

        /// <summary>
        /// A single color coast edge line/intersection mask used to generate distance field from
        /// </summary>
        public static Texture2D CreateIntersectionMap(
            //Mesh mesh,
            int texSizeX, int texSizeY,
            //int spreadSamples,
            int subIndex,
            bool useFirstUVs,
            //int blurIterations,
            float rayDistance,
            float perPixelBias,
            int perPixelRays,
            MeshCollider surfaceCollider,
            bool debug,
            ref List<MeshCollider> meshColliders,
            ThunderSpline thunderSpline)
        {
            var componentHitAnything = new List<bool>();
            var componentExtras = new List<MeshCollider>();
            foreach (var meshCollider in meshColliders)
            {
                componentHitAnything.Add(false);
                componentExtras.Add(meshCollider);
            }

            var meshTransform = surfaceCollider.transform;

            //var surfaceCollider = meshTransform.GetComponent<MeshCollider>();

            //var meshColliders = FindMeshColliders(meshTransform, rayDistance);

            Color[] pix = new Color[texSizeX * texSizeY];
            /*
            MeshCollider component = new GameObject("temp", new System.Type[1]
            {
                typeof(MeshCollider)
            }).GetComponent<MeshCollider>();
            component.hideFlags = HideFlags.HideAndDontSave;
            component.convex = false;
            component.sharedMesh = (Mesh) null;
*/

            /*
            Mesh mesh1 = new Mesh();
            int[] numArray1 = new int[mesh.triangles.Length * 2];
            mesh.triangles.CopyTo((Array) numArray1, mesh.triangles.Length);
            for (int index = 0; index < mesh.triangles.Length; ++index)
            {
              numArray1[mesh.triangles.Length + index] = mesh.triangles[mesh.triangles.Length - 1 - index];
            }
        
            mesh1.vertices = mesh.vertices;
            mesh1.triangles = numArray1;
            mesh1.normals = mesh.normals;
            mesh1.RecalculateBounds();
            component.sharedMesh = mesh1;
            */
            //Mesh mesh1 = mesh;

            //

            //int skipCount = 0;

            //Bounds meshBounds = new Bounds();
            //if(meshTransform.GetComponent<MeshCollider>())  meshBounds = meshTransform.GetComponent<MeshCollider>().bounds;
            //if(meshTransform.GetComponent<BoxCollider>())  meshBounds = meshTransform.GetComponent<BoxCollider>().bounds;
            //if(meshBounds==new Bounds())Debug.LogWarning("meshTransform needs a collider component");
            //meshBounds.max += new Vector3(perPixelOcclusionDistance,perPixelOcclusionDistance,perPixelOcclusionDistance);
            //meshBounds.min -= new Vector3(perPixelOcclusionDistance,perPixelOcclusionDistance,perPixelOcclusionDistance); 

            //bool dupeMesh = false;

            /*
            var componentHitAnything = new List<Boolean>();
            var componentExtras = new List<MeshCollider>();
            for (int i = 0; i < meshColliders.Count; i++)
            {
                MeshCollider component2 = new GameObject("temp-" + i, new System.Type[1]
                {
                    typeof(MeshCollider)
                }).GetComponent<MeshCollider>();

                var mesh3 = meshColliders[i].GetComponent<MeshCollider>().sharedMesh;
                //FFS
                
                component2.transform.SetPositionAndRotation(meshColliders[i].transform.position, meshColliders[i].transform.rotation);
                component2.transform.localScale = meshColliders[i].transform.lossyScale;
                component2.hideFlags = HideFlags.HideAndDontSave;
                component2.convex = false;
                component2.sharedMesh = (Mesh) null;

                {
                    component2.sharedMesh = mesh3;
                }

                componentExtras.Add(component2);
                componentHitAnything.Add(false);
            }
            */

            //Mesh mesh1 = new Mesh();
            //int[] numArray1 = new int[mesh.triangles.Length * 2];
            //mesh.triangles.CopyTo((Array) numArray1, mesh.triangles.Length);
            //for (int index = 0; index < mesh.triangles.Length; ++index)
            //  numArray1[mesh.triangles.Length + index] = mesh.triangles[mesh.triangles.Length - 1 - index];
            //mesh1.vertices = mesh.vertices;
            //mesh1.triangles = numArray1;
            //mesh1.normals = mesh.normals;
            //mesh1.RecalculateBounds();
            //component.sharedMesh = mesh1;

            //

            var mesh = surfaceCollider.sharedMesh;
            Vector3[] vertices = mesh.vertices;
            Vector2[] vector2Array = !useFirstUVs ? mesh.uv2 : mesh.uv;
            int[] numArray2 = subIndex >= 0 ? mesh.GetTriangles(subIndex) : mesh.triangles;
            for (int index = 0; index < numArray2.Length; index += 3)
            {
                Vector2 a1 = vector2Array[numArray2[index]];
                Vector2 b = vector2Array[numArray2[index + 1]];
                Vector2 c = vector2Array[numArray2[index + 2]];
                Vector3 vector3 = Vector3.Normalize(vertices[numArray2[index + 1]] - vertices[numArray2[index]]);
                Vector3 normal = Vector3.Normalize(Vector3.Cross(vector3, vertices[numArray2[index + 2]] - vertices[numArray2[index]]));

                int num1 = Mathf.FloorToInt(Mathf.Min(a1.x, b.x, c.x) * (float) texSizeX);
                int num2 = Mathf.CeilToInt(Mathf.Max(a1.x, b.x, c.x) * (float) texSizeX);

                int num3 = Mathf.FloorToInt(Mathf.Min(a1.y, b.y, c.y) * (float) texSizeY);
                int num4 = Mathf.CeilToInt(Mathf.Max(a1.y, b.y, c.y) * (float) texSizeY);

                /*{
                  var v1a = meshTransform.TransformPoint(vertices[numArray2[index + 0]]);
                  var v2a = meshTransform.TransformPoint(vertices[numArray2[index + 1]]);
                  var v3a = meshTransform.TransformPoint(vertices[numArray2[index + 2]]);
                  var normal2 = meshTransform.TransformDirection(normal);
                  var vector3b = meshTransform.TransformDirection(vector3);
          
                  Debug.DrawLine(v1a, v2a, Color.red, 1);
                  Debug.DrawLine(v2a, v3a, Color.green, 1);
                  Debug.DrawLine(v3a, v1a, Color.blue, 1);
                }*/

                //if (EditorUtility.DisplayCancelableProgressBar("Creating Map", "Generating pixels", (float) index / (float) numArray2.Length))
                {
                    //EditorUtility.ClearProgressBar();
                    //return (Texture2D) null;
                }

                double lastUpdatedProgress = 0;

                for (int a2 = num1; a2 < num2; ++a2)
                {
                    int numX = modulo(a2, texSizeX);
                    for (int a3 = num3; a3 < num4; ++a3)
                    {
                        if (Time.realtimeSinceStartup - lastUpdatedProgress > 1.0 / 5.0f)
                        {
                            lastUpdatedProgress = Time.realtimeSinceStartup;

                            if (EditorUtility.DisplayCancelableProgressBar("RaycastTools", "CreateIntersectionMap..." + index / 3 + "/" + (numArray2.Length / 3), (float) (index / 3.0f) / ((numArray2.Length / 3.0f))))
                            {
                                EditorUtility.ClearProgressBar();
                                return null;
                            }
                        }

                        int numY = modulo(a3, texSizeY);

                        if ((double) pix[numY * texSizeX + numX].a == 0.0)
                        {
                            Vector2 barycentricCoo = GetBarycentricCoo(new Vector2((float) ((double) a2 * 2.0 + 1.0) / (float) (texSizeX * 2), (float) ((double) a3 * 2.0 + 1.0) / (float) (texSizeY * 2)), a1, b, c);
                            if (isPInsideABC(barycentricCoo, a1, b, c, true))
                            {
                                float num7 = 0;

                                var v1 = meshTransform.TransformPoint(vertices[numArray2[index + 0]]);
                                var v2 = meshTransform.TransformPoint(vertices[numArray2[index + 1]]);
                                var v3 = meshTransform.TransformPoint(vertices[numArray2[index + 2]]);
                                var normal2 = meshTransform.TransformDirection(normal);
                                var vector3b = meshTransform.TransformDirection(vector3);

                                var worldPos = BarryCentricTo3D(barycentricCoo, v1, v2, v3);

                                ///for (int i = 0; i < meshColliders.Count; i++)
                                {
                                    //var collider = meshTransform.GetComponent<MeshCollider>();

                                    ///var collider = componentExtras[i];
                                    if (surfaceCollider == null) continue;

                                    //if (!collider.bounds.Contains(worldPos))
                                    //{
                                    //    skipCount++;
                                    //    continue;
                                    //}

                                    //bool hitAnything = false;

                                    var aores = Intersection(worldPos, normal2, vector3b, surfaceCollider, meshColliders, rayDistance, out bool hitAnything, perPixelBias, perPixelRays, debug);

                                    if (thunderSpline && thunderSpline.isActiveAndEnabled)
                                    {
                                        var inside = ThunderSplineWithin.IsInsideSpline(thunderSpline.transform.InverseTransformPoint(worldPos), thunderSpline);
                                        if (inside) hitAnything = true;
                                    }

                                    //num7 += aores;
                                    if (hitAnything)
                                    {
                                        num7 = 1;

                                        ///componentHitAnything[i] = true;
                                        //break;
                                    }
                                }

                                pix[numY * texSizeX + numX] = new Color(num7, num7, num7, 1);
                            }
                        }
                    }
                }
            }

            ///for (int i = componentHitAnything.Count - 1; i >= 0; i--)
            ///{
            ///    if (componentHitAnything[i] == false) RemoveItem(ref meshColliders, i);
            ///}

            //Debug.Log("Raycast: Skipped: " + skipCount);
            //EditorUtility.ClearProgressBar();
            //UnityEngine.Object.DestroyImmediate((UnityEngine.Object) component.gameObject);
            //foreach (var componentExtra in componentExtras)
            //{
            //    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) componentExtra.gameObject);
            //}

            //Color[] colors = Spread(WornEdges.blur(pix, texSize, blurIterations, false), texSize, spreadSamples);
            //Color[] colors = Spread(pix, texSize, spreadSamples);

            Texture2D texture2D = new Texture2D(texSizeX, texSizeY);
            texture2D.hideFlags = HideFlags.DontSave;
            texture2D.SetPixels(pix);
            texture2D.Apply();

            EditorUtility.ClearProgressBar();

            return texture2D;
        }


        private static bool isPInsideABC(
            Vector2 p,
            Vector2 a,
            Vector2 b,
            Vector2 c,
            bool isBarycentric = false)
        {
            Vector2 vector2 = p;
            if (!isBarycentric)
                vector2 = GetBarycentricCoo(p, a, b, c);
            return (double) vector2.x >= 0.0 && (double) vector2.y >= 0.0 &&
                   (double) vector2.x + (double) vector2.y <= 1.0;
        }

        private static bool isPInsideABCIgnoreY(Vector3 p, Vector3 a, Vector3 b, Vector3 c) =>
            isPInsideABC(new Vector2(p.x, p.z), new Vector2(a.x, a.z), new Vector2(b.x, b.z), new Vector2(c.x, c.z));

        private static bool isPInsideABCIgnoreY(Vector2 p, Vector3 a, Vector3 b, Vector3 c) => isPInsideABC(p,
            new Vector2(a.x, a.z), new Vector2(b.x, b.z), new Vector2(c.x, c.z), true);

        private static Vector2 GetBarycentricCoo(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
        {
            Vector2 vector2_1 = c - a;
            Vector2 vector2_2 = b - a;
            Vector2 rhs = p - a;
            float num1 = Vector2.Dot(vector2_1, vector2_1);
            float num2 = Vector2.Dot(vector2_1, vector2_2);
            float num3 = Vector2.Dot(vector2_1, rhs);
            float num4 = Vector2.Dot(vector2_2, vector2_2);
            float num5 = Vector2.Dot(vector2_2, rhs);
            float num6 = (float) (1.0 / ((double) num1 * (double) num4 - (double) num2 * (double) num2));
            return new Vector2((float) ((double) num4 * (double) num3 - (double) num2 * (double) num5),
                (float) ((double) num1 * (double) num5 - (double) num2 * (double) num3)) * num6;
        }

        private static Vector2 GetBarycentricCoo(Vector3 p, Vector3 a, Vector3 b, Vector3 c) =>
            GetBarycentricCoo(new Vector2(p.x, p.z), new Vector2(a.x, a.z), new Vector2(b.x, b.z),
                new Vector2(c.x, c.z));

        private static Vector3 BarryCentricTo3D(Vector2 P, Vector3 A, Vector3 B, Vector3 C) =>
            A + (C - A) * P.x + (B - A) * P.y;

        private static Vector2 BarryCentricTo2D(Vector2 P, Vector2 A, Vector2 B, Vector2 C) =>
            A + (C - A) * P.x + (B - A) * P.y;

        public static int modulo(int a, int b) => (a + Mathf.Abs(a * b)) % b;

        public static List<MeshRenderer> FindMeshesWithoutCollider(Transform meshTransform, List<Bounds> boundsIn, float expandBounds)
        {
            var renderers = Object.FindObjectsOfType<MeshRenderer>(false).ToList();

            //if (bounds == null || bounds.Count == 0) Debug.LogError("RaycastTools: needs valid collider for bounds");


            //Bounds meshBounds = GetTargetCollider(meshTransform).bounds;
            //Bounds meshBounds = collider.bounds;

            //if (meshBounds == new Bounds()) Debug.LogWarning("RaycastTools: meshTransform needs a collider component");
            //meshBounds.Expand(new Vector3(expandBounds, expandBounds, expandBounds));

            for (int i = renderers.Count - 1; i >= 0; i--)
            {
                bool skip = false;

                // Skip if not static
                if (!renderers[i].gameObject.isStatic) skip = true;

                if (!renderers[i].enabled) skip = true;

                // Skip if not within the targetColliderBounds ( BoxCollider )
                //if (!meshBounds.Intersects(renderers[i].bounds)) skip = true;
                bool hitAny = false;
                foreach (var bounds in boundsIn)
                {
                    //if (bounds == null) continue;
                    if (bounds.Intersects(renderers[i].bounds))
                    {
                        hitAny = true;
                        break;
                    }
                }

                if (!hitAny) skip = true;

                //var col = renderers[i].GetComponent<MeshCollider>();
                //if (col) skip = true;

                // Skip invalid meshes
                var mf = renderers[i].GetComponent<MeshFilter>();
                if (mf == null || mf.sharedMesh == null || mf.sharedMesh.vertexCount <= 0) skip = true;

                if (skip) renderers.RemoveAt(i);
            }

            if (renderers.Count <= 0)
            {
                Debug.LogError("RaycastTools: FindMeshesWithoutCollider: None found, requires static Renderers");
            }
            else
            {
                Debug.Log("RaycastTools: A FindMeshesWithoutCollider: Collide with: " + renderers.Count);
            }

            return renderers;
        }
/*
        public static Collider GetTargetCollider(Transform targetTransform)
        {
            Collider collider = targetTransform.GetComponent<MeshCollider>();
            if (collider == null) collider = targetTransform.GetComponent<BoxCollider>();

            return collider;
        }
*/

/*
        public static List<MeshCollider> FindMeshColliders(Transform meshTransform, float expandBounds)
        {
            var meshColliders = Object.FindObjectsOfType<MeshCollider>(false).ToList();

            //var mr = meshTransform.GetComponent<MeshCollider>();

            Bounds meshBounds = GetTargetCollider(meshTransform).bounds;
            if (meshBounds == new Bounds()) Debug.LogWarning("RaycastTools: meshTransform needs a collider component");
            meshBounds.Expand(new Vector3(expandBounds, expandBounds, expandBounds));

            //meshBounds.max += new Vector3(perPixelOcclusionDistance,perPixelOcclusionDistance,perPixelOcclusionDistance);
            //meshBounds.min -= new Vector3(perPixelOcclusionDistance,perPixelOcclusionDistance,perPixelOcclusionDistance); 


            for (int i = meshColliders.Count - 1; i >= 0; i--)
            {
                bool skip = false;

                if (meshColliders[i] == null) skip = true;

                if (!meshColliders[i].gameObject.isStatic) skip = true;

                if (meshColliders[i].sharedMesh == null || meshColliders[i].sharedMesh.vertexCount <= 0) skip = true;

                var mr2 = meshColliders[i].GetComponent<MeshRenderer>();
                if (mr2 == null) skip = true;

                //if (!meshColliders[i].bounds.Intersects(meshBounds)) skip = true;
                if (!meshBounds.Intersects(meshColliders[i].bounds)) skip = true;

                //!meshColliders[i].gameObject.CompareTag("Intersection"));

                if (skip) meshColliders.RemoveAt(i);
            }

            if (meshColliders.Count <= 0)
            {
                Debug.LogError("RaycastTools: FindMeshColliders: None found, requires static Mesh Colliders");
            }
            else
            {
                Debug.Log("RaycastTools: A FindMeshColliders: Collide with: " + meshColliders.Count);
            }

            //Debug.Log("RaycastTools: FindMeshColliders: Collide with: " + meshColliders.Count);
            return meshColliders;
        }
*/
        /// <summary>
        /// https://gist.github.com/unitycoder/58f4b5d80f423d29e35c814a9556f9d9
        /// </summary>
        public static void DrawBounds(Bounds b, float delay = 0)
        {
            // bottom
            var p1 = new Vector3(b.min.x, b.min.y, b.min.z);
            var p2 = new Vector3(b.max.x, b.min.y, b.min.z);
            var p3 = new Vector3(b.max.x, b.min.y, b.max.z);
            var p4 = new Vector3(b.min.x, b.min.y, b.max.z);

            Debug.DrawLine(p1, p2, Color.blue, delay);
            Debug.DrawLine(p2, p3, Color.red, delay);
            Debug.DrawLine(p3, p4, Color.yellow, delay);
            Debug.DrawLine(p4, p1, Color.magenta, delay);

            // top
            var p5 = new Vector3(b.min.x, b.max.y, b.min.z);
            var p6 = new Vector3(b.max.x, b.max.y, b.min.z);
            var p7 = new Vector3(b.max.x, b.max.y, b.max.z);
            var p8 = new Vector3(b.min.x, b.max.y, b.max.z);

            Debug.DrawLine(p5, p6, Color.blue, delay);
            Debug.DrawLine(p6, p7, Color.red, delay);
            Debug.DrawLine(p7, p8, Color.yellow, delay);
            Debug.DrawLine(p8, p5, Color.magenta, delay);

            // sides
            Debug.DrawLine(p1, p5, Color.white, delay);
            Debug.DrawLine(p2, p6, Color.gray, delay);
            Debug.DrawLine(p3, p7, Color.green, delay);
            Debug.DrawLine(p4, p8, Color.cyan, delay);
        }

        public static void DrawBounds(Bounds b, Color colorIn, float delay = 0)
        {
            // bottom
            var p1 = new Vector3(b.min.x, b.min.y, b.min.z);
            var p2 = new Vector3(b.max.x, b.min.y, b.min.z);
            var p3 = new Vector3(b.max.x, b.min.y, b.max.z);
            var p4 = new Vector3(b.min.x, b.min.y, b.max.z);

            Debug.DrawLine(p1, p2, colorIn, delay);
            Debug.DrawLine(p2, p3, colorIn, delay);
            Debug.DrawLine(p3, p4, colorIn, delay);
            Debug.DrawLine(p4, p1, colorIn, delay);

            // top
            var p5 = new Vector3(b.min.x, b.max.y, b.min.z);
            var p6 = new Vector3(b.max.x, b.max.y, b.min.z);
            var p7 = new Vector3(b.max.x, b.max.y, b.max.z);
            var p8 = new Vector3(b.min.x, b.max.y, b.max.z);

            Debug.DrawLine(p5, p6, colorIn, delay);
            Debug.DrawLine(p6, p7, colorIn, delay);
            Debug.DrawLine(p7, p8, colorIn, delay);
            Debug.DrawLine(p8, p5, colorIn, delay);

            // sides
            Debug.DrawLine(p1, p5, colorIn, delay);
            Debug.DrawLine(p2, p6, colorIn, delay);
            Debug.DrawLine(p3, p7, colorIn, delay);
            Debug.DrawLine(p4, p8, colorIn, delay);
        }

//worldPos, normal2, vector3b, collider, rayDistance, out hitAnything, perPixelBias, perPixelRays, debug
        private static float Intersection(
            Vector3 pos,
            Vector3 normal,
            Vector3 binormal,
            MeshCollider collider,
            List<MeshCollider> colliders,
            float maxDistance,
            out bool hitAnything,
            float bias = 0.001f,
            int raysAmount = 6,
            //Bounds meshBounds = default,
            bool debug = false
        )
        {
            var backmem = Physics.queriesHitBackfaces;
            Physics.queriesHitBackfaces = true;

            /*
            List<Vector4> floatList = new List<Vector4>();
            //Vector3 direction1 = normal;
            //RaycastHit hitInfo = new RaycastHit();
            RaycastHit hitInfo1 = new RaycastHit();
            ;
            RaycastHit hitInfo2 = new RaycastHit();
            ;

            bool hit1, hit2;
            */

            //if (collider.Raycast(new Ray(pos + direction1 * bias, direction1), out hitInfo, occlusionDistance))
            //  floatList.Add(hitInfo.distance);
            //else 
            //if (collider.Raycast(new Ray(pos + direction1 * occlusionDistance, -direction1), out hitInfo, occlusionDistance))
            //  floatList.Add(occlusionDistance - hitInfo.distance);
            //else
            //  floatList.Add(occlusionDistance);

            /*
            float num1 = 89f / (float) raysAmount;
            
            for (int index1 = 1; index1 < raysAmount; ++index1)
            {
              Vector3 vector3 = Quaternion.AngleAxis(num1 * (float) index1, binormal) * normal;
              Vector3 direction2 = Quaternion.AngleAxis(360f * UnityEngine.Random.value, normal) * vector3;
              int num2 = index1 * 3 + 3;
              for (int index2 = 0; index2 < num2; ++index2)
              {
                float num3 = occlusionDistance;
                direction2 = Quaternion.AngleAxis((float) (360.0 * ((double) index2 / (double) num2)), normal) * direction2;
        
                hit1 = collider.Raycast(new Ray(pos + direction2 * bias, direction2), out hitInfo1, occlusionDistance);
                hit2 = collider.Raycast(new Ray(pos + direction2 * occlusionDistance, -direction2), out hitInfo2, occlusionDistance) && (double) occlusionDistance - (double) hitInfo2.distance < (double) num3;
                
                if (hit1) num3 = hitInfo1.distance;
                if (hit2 ) num3 = occlusionDistance - hitInfo2.distance;
        
                var occluded = 0;
        
                //if (hit1 && hit2) occluded = 1;
                
                //if(backface)
                
                floatList.Add(new Vector4(num3,occluded,num3,0));
              }
            }
            
            Vector4 num4 = Vector4.zero;
            foreach (var num2 in floatList) num4 += num2;
            //var res = num4 / (occlusionDistance * (float) floatList.Count);
            var ao = num4.x / (occlusionDistance * (float) floatList.Count);
            var bf = num4.y / (float) floatList.Count;
            
            Physics.queriesHitBackfaces = backmem;
        
            return bf;*/

            //if (!collider.bounds.Intersects(meshBounds)) return 0;

            float angleInc = 360f / (float) raysAmount;

            var randOffset = 360f * UnityEngine.Random.value;

            if (debug) Debug.DrawLine(pos, pos + normal * maxDistance, Color.blue, 2f + (raysAmount * 0.1f));

            RaycastHit hitInfo = new RaycastHit();

            float insideCollider = 0;

            hitAnything = false;

            for (int i = 0; i < raysAmount; ++i)
            {
                Vector3 direction2 = Quaternion.AngleAxis(randOffset + angleInc * (float) i, normal) * binormal;
                var hits = Physics.RaycastAll(new Ray(pos, direction2), maxDistance, LayerMask.GetMask("PlayerHandAndFoot"));

                //Debug.Log("Hits: " + pos + " : " + hits.Length);
                Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

                bool hit = false;
                foreach (var raycastHit in hits)
                {
                    if (raycastHit.collider == collider) continue;
                    //Debug.Log("Hit:" + raycastHit.collider.gameObject.layer + " - " + raycastHit.collider.name, raycastHit.collider.gameObject);
                    // if (raycastHit.collider.gameObject.layer != LayerMask.GetMask("PlayerHandAndFoot")) continue;
                    //if (!colliders.Contains(raycastHit.collider)) continue;
                    //if (!raycastHit.collider is MeshCollider) continue;
                    //var mf = raycastHit.collider.GetComponent<MeshFilter>();
                    //if (mf == null || mf.sharedMesh == null) continue;
                    //if (mf)
                    {
                        hit = true;
                        hitInfo = raycastHit;
                        //hitMesh = mf.sharedMesh;
                        break;
                    }
                }

                if (hit)
                {
                    hitAnything = true;
                    insideCollider = 1;
                    if (!debug) break;
                }

                var color = hit ? Color.cyan : Color.red * 1F;

                //if(insideCollider==1) color = Color.cyan;

                if (debug) Debug.DrawLine(pos, pos + direction2 * maxDistance, color, 3f + i * 0.1f);
            }

            if (hitAnything)
            {
                //Debug.Log("Hit: Valid");
            }
            else
            {
                // if (debug) Debug.DrawLine(pos, pos + Vector3.up * 0.1f, Color.red, 2f + i * 0.1f);
            }

            /*

            RaycastHit hitInfo3 = new RaycastHit();
            RaycastHit hitInfo4 = new RaycastHit();

            //normal = binormal; // Change to test binormal
            // Bias pushes the ray start away from the mesh surface slightly
            var hit3 = collider.Raycast(new Ray(pos, normal), out hitInfo3, float.MaxValue);
            var hit4 = collider.Raycast(new Ray(pos, -normal), out hitInfo4, float.MaxValue);
            //if(hit3)Debug.DrawLine(pos, pos + normal * hitInfo3.distance, Color.yellow, 2);
            //if(hit4)Debug.DrawLine(pos, pos + -normal * hitInfo4.distance, Color.blue, 2);
            
   

            //hit3 = false;

            var insideCollider = 0;

            hitAnything = false;


            if (hit3 && hit4 && !debug)
            {
                insideCollider = 1;
            }

            if (insideCollider == 0)
            {
                float angleInc = 360f / (float) raysAmount;

                var randOffset = 360f * UnityEngine.Random.value;

                if (debug) Debug.DrawLine(pos, pos + normal * maxDistance, Color.blue, 2f + (raysAmount * 0.1f));

                RaycastHit hitInfo = new RaycastHit();


                for (int i = 0; i < raysAmount; ++i)
                {
                    Vector3 direction2 = Quaternion.AngleAxis(randOffset + angleInc * (float) i, normal) * binormal;

                    bool hit = collider.Raycast(new Ray(pos, direction2), out hitInfo, maxDistance);


                    if (hit)
                    {
                        hitAnything = true;
                        insideCollider = 1;
                        if (!debug) continue;
                    }

                    var color = hit ? Color.cyan : Color.red * 1F;

                    //if(insideCollider==1) color = Color.cyan;

                    if (debug) Debug.DrawLine(pos, pos + direction2 * maxDistance, color, 2f + i * 0.1f);


                    //continue;
                   
                }
            }
*/
            Physics.queriesHitBackfaces = backmem;
            return insideCollider;
        }


        private static float Depth(
            Vector3 pos,
            Vector3 normal,
            //MeshCollider collider,
            float maxDistance,
            out CollisionStruct collisionStruct,
            float depthOffset = 0.2f,
            bool debug = false
        )
        {
            Vector3 direction2 = -normal;

            pos += new Vector3(0, depthOffset, 0);
/*
            bool rayCapture = true;

            if (rayCapture)
            {
                var captureRay = new CaptureRay();
                var col = captureRay.Capture(pos, direction2, maxDistance);
                    
                captureRay.CaptureFinished();
                return;
            }*/

            var distance = float.MaxValue; // Set infinitely far away if nothing is hit

            Mesh hitMesh = null;

            var hitInfo = new RaycastHit();

            var backmem = Physics.queriesHitBackfaces;
            Physics.queriesHitBackfaces = true;
            //bool hit = collider.Raycast(new Ray(pos, direction2), out hitInfo, maxDistance);
            //bool hit = Physics.Raycast(new Ray(pos, direction2), out hitInfo, maxDistance);
            var hits = Physics.RaycastAll(new Ray(pos, direction2), maxDistance, LayerMask.GetMask("PlayerHandAndFoot"));
            Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

            bool hit = false;
            foreach (var raycastHit in hits)
            {
                //if (!raycastHit.collider is MeshCollider) continue;
                var mf = raycastHit.collider.GetComponent<MeshFilter>();
                //if (raycastHit.collider.gameObject.layer != 31) continue;
                if (mf == null || mf.sharedMesh == null) continue;
                if (mf)
                {
                    hit = true;
                    hitInfo = raycastHit;
                    hitMesh = mf.sharedMesh;
                    break;
                }
            }

            if (hit)
            {
                distance = hitInfo.distance;
            }


            var color = hit ? Color.cyan : Color.red * 1f;

            if (debug)
            {
                if (hit)
                {
                    Debug.DrawLine(pos, pos + direction2 * (hitInfo.distance + 0.0f), color, 5f);
                }
                else
                {
                    Debug.DrawLine(pos, pos + direction2 * 0.1f, color, 5f);
                }
            }

            distance -= depthOffset;

            Physics.queriesHitBackfaces = backmem;

            collisionStruct = new CollisionStruct()
            {
                hit = hit,
                hitInfo = hitInfo,
                //collider = collider
                collider = hitInfo.collider,
                mesh = hitMesh
            };

            return distance;
        }


        private static float IntersectionOld(
            Vector3 pos,
            Vector3 normal,
            Vector3 binormal,
            MeshCollider collider,
            float occlusionDistance,
            float bias = 0.001f,
            int raysAmount = 6)
        {
            var backmem = Physics.queriesHitBackfaces;
            Physics.queriesHitBackfaces = true;

            /*
            List<Vector4> floatList = new List<Vector4>();
            //Vector3 direction1 = normal;
            //RaycastHit hitInfo = new RaycastHit();
            RaycastHit hitInfo1 = new RaycastHit();
            ;
            RaycastHit hitInfo2 = new RaycastHit();
            ;

            bool hit1, hit2;
            */

            //if (collider.Raycast(new Ray(pos + direction1 * bias, direction1), out hitInfo, occlusionDistance))
            //  floatList.Add(hitInfo.distance);
            //else 
            //if (collider.Raycast(new Ray(pos + direction1 * occlusionDistance, -direction1), out hitInfo, occlusionDistance))
            //  floatList.Add(occlusionDistance - hitInfo.distance);
            //else
            //  floatList.Add(occlusionDistance);

            /*
            float num1 = 89f / (float) raysAmount;
            
            for (int index1 = 1; index1 < raysAmount; ++index1)
            {
              Vector3 vector3 = Quaternion.AngleAxis(num1 * (float) index1, binormal) * normal;
              Vector3 direction2 = Quaternion.AngleAxis(360f * UnityEngine.Random.value, normal) * vector3;
              int num2 = index1 * 3 + 3;
              for (int index2 = 0; index2 < num2; ++index2)
              {
                float num3 = occlusionDistance;
                direction2 = Quaternion.AngleAxis((float) (360.0 * ((double) index2 / (double) num2)), normal) * direction2;
        
                hit1 = collider.Raycast(new Ray(pos + direction2 * bias, direction2), out hitInfo1, occlusionDistance);
                hit2 = collider.Raycast(new Ray(pos + direction2 * occlusionDistance, -direction2), out hitInfo2, occlusionDistance) && (double) occlusionDistance - (double) hitInfo2.distance < (double) num3;
                
                if (hit1) num3 = hitInfo1.distance;
                if (hit2 ) num3 = occlusionDistance - hitInfo2.distance;
        
                var occluded = 0;
        
                //if (hit1 && hit2) occluded = 1;
                
                //if(backface)
                
                floatList.Add(new Vector4(num3,occluded,num3,0));
              }
            }
            
            Vector4 num4 = Vector4.zero;
            foreach (var num2 in floatList) num4 += num2;
            //var res = num4 / (occlusionDistance * (float) floatList.Count);
            var ao = num4.x / (occlusionDistance * (float) floatList.Count);
            var bf = num4.y / (float) floatList.Count;
            
            Physics.queriesHitBackfaces = backmem;
        
            return bf;*/


            RaycastHit hitInfo3 = new RaycastHit();
            RaycastHit hitInfo4 = new RaycastHit();

            //normal = binormal; // Change to test binormal
            // Bias pushes the ray start away from the mesh surface slightly
            var hit3 = collider.Raycast(new Ray(pos + normal * bias, normal), out hitInfo3, occlusionDistance);
            var hit4 = collider.Raycast(new Ray(pos, -normal), out hitInfo4, occlusionDistance);
            //if(hit3)Debug.DrawLine(pos, pos + normal * hitInfo3.distance, Color.yellow, 2);
            //if(hit4)Debug.DrawLine(pos, pos + -normal * hitInfo4.distance, Color.blue, 2);


            var insideCollider = 0;
            if (hit3 && hit4)
            {
                insideCollider =
                    1; // If it hits a collider ray sent out from normal and negative normal then it must be inside the collider
            }
            else if (hit3 || hit4)
            {
                // If only hit one way, then check it hit a backface
                var dot = 0.0f;
                int hitCounter = 0;

                float num1 = 89f / (float) raysAmount;

                for (int index1 = 1; index1 < raysAmount; ++index1)
                {
                    Vector3 vector3 = Quaternion.AngleAxis(num1 * (float) index1, binormal) * normal;
                    Vector3 direction2 = Quaternion.AngleAxis(360f * UnityEngine.Random.value, normal) * vector3;
                    int num2 = index1 * 3 + 3;
                    for (int index2 = 0; index2 < num2; ++index2)
                    {
                        direction2 = Quaternion.AngleAxis((float) (360.0 * ((double) index2 / (double) num2)), normal) *
                                     direction2;

                        bool hit5 = false;
                        bool hit6 = false;
                        hit5 = collider.Raycast(new Ray(pos, direction2), out hitInfo3, occlusionDistance);
                        hit6 = collider.Raycast(new Ray(pos, -direction2), out hitInfo4, occlusionDistance);

                        if (hit5) hit3 = true;
                        if (hit6) hit4 = true;

                        if (hit3 && hit4)
                        {
                            insideCollider = 1;
                            break;
                        }

                        if (hit3)
                        {
                            dot += Vector3.Dot(hitInfo3.normal, direction2);
                            hitCounter++;
                        }

                        if (hit4)
                        {
                            dot += Vector3.Dot(hitInfo4.normal, -direction2);
                            hitCounter++;
                        }
                    }

                    dot /= hitCounter;

                    if (dot > 0.1)
                    {
                        insideCollider = 1;
                    }
                }
            }

            Physics.queriesHitBackfaces = backmem;
            return insideCollider;
        }

        private static Color[] Spread(
            Color[] pix,
            int texWidth,
            int texHeight,
            int count,
            bool modifyAlpha,
            float cutoff = 0.5f)
        {
            Color[] colorArray1 = new Color[texWidth * texHeight];
            if (!modifyAlpha)
                pix.CopyTo((Array) colorArray1, 0);
            for (int index1 = 0; index1 < count; ++index1)
            {
                Color[] colorArray2 = new Color[texWidth * texHeight];
                pix.CopyTo((Array) colorArray2, 0);
                //EditorUtility.DisplayProgressBar("Creating Map", "Extending pixels", (float) index1 / (float) count);
                for (int index2 = 0; index2 < texWidth; ++index2)
                {
                    for (int index3 = 0; index3 < texHeight; ++index3)
                    {
                        int num1 = texWidth - 1;
                        int num2 = texHeight - 1;
                        if ((double) pix[index3 * texWidth + index2].a <= (double) cutoff)
                        {
                            bool flag = false;
                            int num3 = 0;
                            Color black = Color.black;
                            if (index2 > 0)
                            {
                                if ((double) pix[index3 * texWidth + index2 - 1].a > (double) cutoff)
                                {
                                    ++num3;
                                    black += pix[index3 * texWidth + index2 - 1];
                                    flag = true;
                                }

                                if (index3 > 0 && (double) pix[(index3 - 1) * texWidth + index2 - 1].a >
                                    (double) cutoff)
                                {
                                    ++num3;
                                    black += pix[(index3 - 1) * texWidth + index2 - 1];
                                }

                                if (index3 < num2 && (double) pix[(index3 + 1) * texWidth + index2 - 1].a >
                                    (double) cutoff)
                                {
                                    ++num3;
                                    black += pix[(index3 + 1) * texWidth + index2 - 1];
                                }
                            }

                            if (index3 > 0)
                            {
                                if ((double) pix[(index3 - 1) * texWidth + index2].a > (double) cutoff)
                                {
                                    ++num3;
                                    black += pix[(index3 - 1) * texWidth + index2];
                                    flag = true;
                                }

                                if (index2 < num1 && (double) pix[(index3 - 1) * texWidth + index2 + 1].a >
                                    (double) cutoff)
                                {
                                    ++num3;
                                    black += pix[(index3 - 1) * texWidth + index2 + 1];
                                }
                            }

                            if (index2 < num1)
                            {
                                if ((double) pix[index3 * texWidth + index2 + 1].a > (double) cutoff)
                                {
                                    ++num3;
                                    black += pix[index3 * texWidth + index2 + 1];
                                    flag = true;
                                }

                                if (index3 < num2 && (double) pix[(index3 + 1) * texWidth + index2 + 1].a >
                                    (double) cutoff)
                                {
                                    ++num3;
                                    black += pix[(index3 + 1) * texWidth + index2 + 1];
                                }
                            }

                            if (index3 < num2 && (double) pix[(index3 + 1) * texWidth + index2].a > (double) cutoff)
                            {
                                ++num3;
                                black += pix[(index3 + 1) * texWidth + index2];
                                flag = true;
                            }

                            if (num3 > 0 && flag)
                            {
                                Color color = black / (float) num3;
                                color.a = 1f;
                                colorArray2[index3 * texWidth + index2] = color;
                            }
                        }
                    }
                }

                pix = colorArray2;
            }

            if (!modifyAlpha)
            {
                for (int index = 0; index < pix.Length; ++index)
                    pix[index].a = colorArray1[index].a;
            }

            //EditorUtility.ClearProgressBar();
            return pix;
        }

        private static Color[] Spread(Color[] pix, int texSize, int count = 10) =>
            Spread(pix, texSize, texSize, count, true);

/*
        public static Bounds GetTargetBounds(GameObject go)
        {
            Bounds bounds = new Bounds();
            if (go.GetComponent<MeshCollider>())
            {
                bounds = go.GetComponent<MeshCollider>().bounds;
            }
            else
            {
                bounds = go.GetComponent<MeshRenderer>().bounds;
            }

            return bounds;
        }
        */
    }
}
#endif
