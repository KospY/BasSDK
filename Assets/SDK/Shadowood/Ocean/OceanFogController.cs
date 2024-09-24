using System;
using System.Collections.Generic;
using System.Linq;
using Shadowood;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
#if UNITY_EDITOR && ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
#endif

using UnityEngine;
using UnityEngine.Rendering;

namespace Shadowood
{
    public static class Extensions
    {
        public static Vector3 RGB(this Color colorIn)
        {
            return new Vector3(colorIn.r, colorIn.g, colorIn.b);
        }

        public static Vector4 RGBandA(this Color colorIn, float alphaIn)
        {
            return new Vector4(colorIn.r, colorIn.g, colorIn.b, alphaIn);
        }
        
        public static bool IsInside(this BoxCollider p_Box, Vector3 p_Point)
        {
            p_Point = p_Box.transform.InverseTransformPoint(p_Point) - p_Box.center;

            float l_HalfX = (p_Box.size.x * 0.5f);
            float l_HalfY = (p_Box.size.y * 0.5f);
            float l_HalfZ = (p_Box.size.z * 0.5f);

            return (p_Point.x < l_HalfX && p_Point.x > -l_HalfX &&
                    p_Point.y < l_HalfY && p_Point.y > -l_HalfY &&
                    p_Point.z < l_HalfZ && p_Point.z > -l_HalfZ);
        }
    }
    [ExecuteInEditMode]
    public class OceanFogController : MonoBehaviour
    {
        public bool debug;

        [Space] //
        [EnumToggleButtons]
        public eMode mode = eMode.Ocean;

        [ShowIf("@mode == eMode.River")] [GUIColor("@riverCollider && riverCollider.enabled ? Color.white : Color.red")]
        public Collider riverCollider;

        [GUIColor("@boundLimit && boundLimit.enabled ? Color.white : Color.red")]
        public BoxCollider boundLimit;

        public enum eMode
        {
            Ocean, // Generates a simple quad meshCollider for raycasting along with a box collider to contain it and everything down under the surface for restricting renderers to raycast against within that volume
            River
        }

        [Space]
        [Tooltip("Transform to follow for the surface height of the ocean")] //
        [GUIColor("@oceanSurface ? Color.white : Color.red")]
        //[InlineButton("FindOceanSurface")]
        public Transform oceanSurface;

        public void FindOceanSurface()
        {
            //TODO FindOceanSurface not implemented
        }

        /*
        [Header("Unity Fog")] public bool affectUnityFog = false;

        [ShowIf("affectUnityFog")] public Color oceanFogColor = Color.cyan;
        [ShowIf("affectUnityFog")] public Color skyFogColor = Color.white;

        [ShowIf("affectUnityFog")] public float oceanFogStart = -10, oceanFogEnd = 30;
        [ShowIf("affectUnityFog")] public float skyFogStart = 0, skyFogEnd = 100000;
        */

        [Header("Light Sync")] [GUIColor("@lightTarget ? Color.white : Color.red")] [InlineButton("FindSun")] [InlineButton("SyncWithLight", "Sync")]
        public Light lightTarget;

        public Color lightColorMult = Color.white;
        public bool copyLightIntensity = true;

        [GUIColor("@lightIntensity > 0.03 ? Color.white : Color.red")]
        public float lightIntensity = 1;

        [Tooltip("Multiplies the lightIntensity")] [GUIColor("@lightIntensity > 0.03 ? Color.white : Color.red")]
        public float lightIntensityExtra = 1;

        public const float minNearClip = 0.02f;

        public float OceanHeight
        {
            get => transform.position.y;
            set
            {
                float diff = value - transform.position.y;
                this.transform.position += Vector3.up * diff;
            }
        }

        public void FindSun()
        {
            lightTarget = RenderSettings.sun;
            SyncWithLight();
        }

        public void SyncWithLight()
        {
            if (!lightTarget) return;
            lightColorMult = lightTarget.color;
            if (copyLightIntensity) lightIntensity = lightTarget.intensity;
        }

        //[Header("Face Clip Fix")] [Tooltip("Copies the cameras nearclip when true")]
        //public bool autoSetToNearClip = true;

        //[Tooltip("Higher values will creates a large jump in height, keep the nearclip and this offset small as possible")] [GUIColor("@earlyTriggerOffset <= 0.05 ? Color.white : Color.red")]
        //public float earlyTriggerOffset = 0.0f;
        //public float earlyTriggerOffset2 = 0.0f;

        public bool offsetSurface = true;


        [Tooltip("When emerging/submerging toggles a global keyword '_USEUNDERWATER' - when off it acts permanently submerged with effects on so that caustics can be seen below the water when above also")]
        public bool causticsOnlyWhenSubmerged = true;

        public Dictionary<Camera, PerCamData> perCamData = new Dictionary<Camera, PerCamData>();

        [Serializable]
        public class PerCamData
        {
            public Camera camera; // Debug use only

            public int underWater = -1;
            public int underWaterCurrent;

            //[NonSerialized] public int underWaterLast = -2;
            public float earlyTriggerOffset;

            //public float camHeight;
            public float oceanShift;

#if UNITY_EDITOR
            [GUIColor("@nearClip > 0.01 ? Color.red : Color.white")]
            public float nearClip;
#endif
        }

        [ShowInInspector] [NonSerialized] private List<PerCamData> debugList = new List<PerCamData>();

        private static readonly int GlobalOceanUse = Shader.PropertyToID("GlobalOceanUse");
        private static readonly int GlobalOceanHeight = Shader.PropertyToID("GlobalOceanHeight");

        private static readonly int GlobalOceanUnder = Shader.PropertyToID("GlobalOceanUnder");

        //private static readonly int GlobalOceanEarlyTriggerOffset = Shader.PropertyToID("GlobalOceanEarlyTriggerOffset");
        private static readonly int GlobalOceanOffset = Shader.PropertyToID("GlobalOceanOffset");

        /*
            [GUIColor("@mirrorSurface ? Color.white : Color.red")] [InlineButton("FindMirror")]
            public MirrorSurface mirrorSurface;
    
            public void FindMirror()
            {
                if (!mirrorSurface) mirrorSurface = FindObjectOfType<MirrorSurface>(false);
                if (!mirrorSurface) mirrorSurface = FindObjectOfType<MirrorSurface>(true);
            }
        */
        public bool runOnUpdate = false;

        //[Range(0.00001f, 1)] public float fogDensityMult = 1;

        [GUIColor("@oceanFogAsset ? Color.white : Color.red")] //
        [InlineButton("FindOceanFogAsset")]
        [InlineEditor]
        public OceanFogAsset oceanFogAsset;

#if UNITY_EDITOR
        public void FindOceanFogAsset()
        {
            oceanFogAsset = OceanFogAsset.CreateAsset("");
        }
#endif

        //public Settings _settings = Settings.kDefaultSettings;
        //public Material _skyBoxMaterial;

        //[Header("Events")] //
        //public UnityEvent submergedEvent = new UnityEvent();
        //
        //public UnityEvent emergedEvent = new UnityEvent();

        //

        /// <summary>
        /// Ran every frame for every camera
        /// </summary>
        public void SetSettings()
        {
            Settings settings;


            //if (boundLimit) boundLimit.enabled = false; // leave enabled and use layer masks for perf

            if (oceanFogAsset == null)
            {
                Debug.LogError("OceanFogAsset is null", this);
                settings = Settings.kDefaultSettings;
#if UNITY_EDITOR
                oceanFogAssethash = settings.Hash();
#endif
            }
            else
            {
#if UNITY_EDITOR
                oceanFogAssethash = oceanFogAsset.hash;
#endif
                settings = oceanFogAsset.settings;
            }

#if UNITY_EDITOR
            if (debug) Debug.Log("SetSettings: " + settings._intensity, this);
#endif
            //Shader.SetGlobalVector("pg", settings._bottomColor.linear * lightIntensity * lightColorMult.linear * lightIntensityExtra);
            Shader.SetGlobalVector("OceanFogBottom_RGB_Intensity", (settings._bottomColor.linear * lightIntensity * lightColorMult.linear * lightIntensityExtra).RGBandA(settings._intensity));
            Shader.SetGlobalVector("OceanFogTop_RGB_Exponent", (settings._topColor.linear * lightIntensity * lightColorMult.linear * lightIntensityExtra).RGBandA(settings._exponent));

            Shader.SetGlobalVector("OceanWaterTint_RGB", settings._underWaterTint.linear.RGBandA(0));

            //Shader.SetGlobalFloat("_skyGradientIntensity", settings._intensity);
            //Shader.SetGlobalFloat("_skyGradientExponent", settings._exponent);
            //Shader.SetGlobalFloat("_skyGradientHorizon", settings._horizon);
            //Shader.SetGlobalFloat("_gradientFogDensity", settings._fogDensity * fogDensityMult);
            //Shader.SetGlobalFloat("_gradientFogDensity2", settings._fogDensity2);
            //Shader.SetGlobalVector("DepthFadeDOLO", settings.DepthFadeDOLO);
            Shader.SetGlobalVector("OceanFogDensities", new Vector4(0, settings._fogDensity2, settings._oceanFogDensity, settings._fogOffset)); // settings._fogDensity * fogDensityMult
        }


        [ContextMenu("Run")]
        public void Run()
        {
            SetSettings();
        }

        public void OnValidate()
        {
            if (!isActiveAndEnabled) return;
            SetSettings();
        }

        /*
        [ContextMenu("SetGlobals")]
        public void SetGlobals()
        {
            if (!oceanSurface) return;
            //Shader.SetGlobalFloat(GlobalOceanHeight, 0);
            Shader.SetGlobalFloat(GlobalOceanHeight, oceanSurface.transform.position.y); // TODO perf, doesnt change often/ever, maybe run on Enable
            Shader.SetGlobalFloat(GlobalOceanUse, 0);
            Shader.SetGlobalFloat(GlobalOceanUse, 1.0f);
        }
    */

        private int oceanFogAssethash;

        void Update()
        {
            if (!oceanSurface) return;

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                FixSceneClipSceneCameras(); // Auto fix scene cameras only

                if (oceanFogAsset && oceanFogAsset.hash != oceanFogAssethash) SetSettings();

                //Shader.SetGlobalFloat(GlobalOceanHeight, 0);

                var badKeys = perCamData.Where(pair => pair.Value.camera == null).Select(pair => pair.Key).ToList();
                foreach (var badKey in badKeys)
                {
                    perCamData.Remove(badKey);
                }

                debugList.Clear();
                foreach (var o in perCamData)
                {
                    debugList.Add(o.Value);
                }
            }
#endif

            if (runOnUpdate) SetSettings();
            /*
            
    
            targetCamera = Camera.main;
            if (targetCamera == null || !targetCamera.isActiveAndEnabled)
            {
                targetCamera = Camera.current;
            }
    
    #if UNITY_EDITOR
            if (targetCamera == null) targetCamera = SceneView.lastActiveSceneView.camera;
    
    #endif
    */
        }

        /*
          public Material skyMat;
          
        
      private static readonly int _OceanFog = Shader.PropertyToID("_OceanFog");
      private static readonly int OCEANFOG_ON = Shader.PropertyToID("_OCEANFOG_ON");
      
      
      public void SetSkyKeyword(bool enabled)
      {
          if (!skyMat) return;
          if (enabled)
          {
              skyMat.SetFloat(_OceanFog, 1);
              skyMat.EnableKeyword("_OCEANFOG_ON");
          }
          else
          {
              skyMat.SetFloat(_OceanFog, 0);
              skyMat.DisableKeyword("_OCEANFOG_ON");
          }
      }
    
        
        [ContextMenu("SkyKeywordEnable")]
        void SkyKeywordEnable()
        {
            SetSkyKeyword(true);
        }
    
        [ContextMenu("SkyKeywordDisable")]
        void SkyKeywordDisable()
        {
            SetSkyKeyword(false);
        }
      */

        //public float earlyTriggerOffset = 0; // TODO Remove

        //private List<Camera> sceneCameras = new List<Camera>();

#if UNITY_EDITOR
        public void FixSceneClip()
        {
            foreach (var o in debugList)
            {
                if (o.camera == null) continue;
                //o.camera.nearClipPlane = minNearClip;
            }

            //foreach (var o in UnityEditor.SceneView.GetAllSceneCameras()) o.nearClipPlane = minNearClip;

            FixSceneClipSceneCameras();
        }

        public void FixSceneClipSceneCameras()
        {
            foreach (var sceneView in UnityEditor.SceneView.sceneViews)
            {
                //if (sceneView is UnityEditor.SceneView view) view.cameraSettings.nearClip = minNearClip;
            }

            //UnityEditor.SceneView.lastActiveSceneView.cameraSettings.nearClip = minNearClip;
            //UnityEditor.SceneView.lastActiveSceneView.Repaint();
        }


        public bool FixSceneClipNeeded()
        {
            foreach (var o in debugList)
            {
                if (o.camera == null) continue;
                if (o.camera.nearClipPlane > minNearClip) return true;
            }

            return false;
        }
#endif

        private RaycastHit raycastHit = new RaycastHit();

        void Run(Camera targetCamera)
        {
            if (!oceanSurface) return;
            if (!targetCamera) return;

            // only allow this camera to control fog
            if (!targetCamera.CompareTag("MainCamera") && !targetCamera.CompareTag("SpectatorCamera") && targetCamera.cameraType != CameraType.SceneView)
            {
                Shader.SetGlobalFloat(GlobalOceanUse, 0);
                return;
            }

            if (!perCamData.ContainsKey(targetCamera)) perCamData.Add(targetCamera, new PerCamData());
            var camData = perCamData[targetCamera];
            camData.camera = targetCamera;
            camData.earlyTriggerOffset = targetCamera.nearClipPlane + targetCamera.stereoSeparation; // + earlyTriggerOffset;
            if (!offsetSurface) camData.earlyTriggerOffset = 0;
#if UNITY_EDITOR
            camData.nearClip = targetCamera.nearClipPlane;
#endif

            if (TryGetWaterHeight(camData.camera.transform.position, out float waterHeight))
            {
                if (targetCamera.transform.position.y < (waterHeight + (camData.earlyTriggerOffset)))
                {
                    camData.underWater = 1;
                    camData.oceanShift = camData.earlyTriggerOffset; // * 2;
                }
                else
                {
                    camData.oceanShift = 0;
                    camData.underWater = 0;
                }
            }
            else
            {
                camData.oceanShift = 0;
                camData.underWater = 0;
            }

            Shader.SetGlobalFloat(GlobalOceanHeight, oceanSurface.transform.position.y);
            //Shader.SetGlobalFloat(GlobalOceanEarlyTriggerOffset, camData.ffs);
            Shader.SetGlobalFloat(GlobalOceanOffset, camData.oceanShift);

            if (camData.underWater == 1)
            {
                //if (camData.underWaterCurrent != 1)
                {
                    /*if (affectUnityFog)
                    {
                        skyFogColor = RenderSettings.fogColor;
                        skyFogStart = RenderSettings.fogStartDistance;
                        skyFogEnd = RenderSettings.fogEndDistance;
                    }*/

                    Submerge(camData);
                }
            }
            else //if (camData.underWaterCurrent != 0)
            {
                Emerge(camData);
            }
        }

        public bool TryGetWaterHeight(Vector3 point, out float waterHeight)
        {
            if (!enabled || (boundLimit && !boundLimit.IsInside(point)))
            {
                waterHeight = 0;
                return false;
            }

            if (mode == eMode.Ocean)
            {
                waterHeight = OceanHeight;
                return true;
            }
            else if (mode == eMode.River)
            {
                if (!riverCollider || !riverCollider.enabled)
                {
                    Debug.LogError("OceanFogController: Water detection issue, River Collider is null or disabled : " + transform.parent.name, this);
                    waterHeight = 0;
                    return false;
                }

                var backmem = Physics.queriesHitBackfaces;
                Physics.queriesHitBackfaces = true;
                var triggermem = Physics.queriesHitTriggers;
                Physics.queriesHitTriggers = false;
                //riverCollider.enabled = true; // Use layer instead for perf

                /*
                var closest = riverCollider.ClosestPoint(pos); // convex mesh or simple collider shapes only
                var dir = (pos - closest).normalized;
    
                riverCollider.Raycast(new Ray {origin = pos, direction = dir}, out raycastHit, maxDistance);
    
                if (raycastHit.distance > 0)
                {
                    if (debug)
                    {
                        Debug.DrawLine(closest, closest + Vector3.up * 0.1f, Color.green, 0.1f);
    
                        Debug.DrawRay(pos, dir, Color.red, 0.1f);
                        var dot = Vector3.Dot(raycastHit.normal, dir);
                        Debug.Log("Hit: " + camData.camera.name + ":" + raycastHit.distance + ":" + raycastHit.normal + ":" + dot);
                    }
                }*/


                float maxDistance = 10000;

                Ray ray = new Ray(point, Vector3.up);
                bool hasHit = riverCollider.Raycast(ray, out raycastHit, maxDistance);


                if (hasHit)
                {
#if UNITY_EDITOR                    
                    if (debug) Debug.DrawRay(ray.origin, ray.direction, Color.red, 0.1f);
#endif
                }
                else
                {
                    ray.direction = Vector3.down;
                    hasHit = riverCollider.Raycast(ray, out raycastHit, maxDistance);

                    if (hasHit)
                    {
#if UNITY_EDITOR                        
                        if (debug) Debug.DrawRay(ray.origin, ray.direction, Color.green, 0.1f);
#endif                        
                    }
                }

                //riverCollider.enabled = false;

                Physics.queriesHitBackfaces = backmem;
                Physics.queriesHitTriggers = triggermem;

                if (hasHit)
                {
#if UNITY_EDITOR                    
                    if (debug)
                    {
                        var dot = Vector3.Dot(raycastHit.normal.normalized, ray.direction);
                        Debug.DrawLine(raycastHit.point, raycastHit.point + Vector3.up * 0.1f, Color.yellow, 0.1f);
                        //Debug.Log("Hit: Dir:" + dir + ":" + raycastHit.distance + ":" + raycastHit.normal.normalized + ":" + dot);
                    }
#endif
                    waterHeight = raycastHit.point.y;
                    return true;
                }
            }

            waterHeight = 0;
            return false;
        }

        /*
            private void Start()
            {
                underWaterLast = -2;
                RenderPipelineManager.beginCameraRendering -= BeginCameraRendering;
                RenderPipelineManager.beginCameraRendering += BeginCameraRendering;
            }
        */
        
        
        private void OnEnable()
        {
            perCamData.Clear();

            if (riverCollider) riverCollider.gameObject.layer = 4; // Set water layer

            if (lightTarget == null) FindSun();

            /*
            SetSkyKeyword(true);
            skyMat = RenderSettings.skybox;
            if (skyMat != null)
            {
                if (!skyMat.shader.name.Contains("SkySorcery"))
                {
                    Debug.LogError("OceanFog requires Sky to use SkySorcery shader");
                    skyMat = null;
                }
            }*/

            //WillRun();

            //underWaterLast = -2;
            //Update();
            RenderPipelineManager.beginCameraRendering -= BeginCameraRendering;
            RenderPipelineManager.beginCameraRendering += BeginCameraRendering;

            SetSettings();

            FindAllOceanVisibilityControllers();
        }

        [NonSerialized] [ShowInInspector] private List<OceanVisibilityController> oceanVisibilityControllers = new List<OceanVisibilityController>();

        public void FindAllOceanVisibilityControllers()
        {
            oceanVisibilityControllers = FindObjectsOfType<OceanVisibilityController>(true).ToList();
            //UpdateOceanVisibilityControllers();
        }

        /// <summary>
        /// Runs every frame for every camera, TODO PERF remove, super slow
        /// </summary>
        public void UpdateOceanVisibilityControllers(bool underWater)
        {
            foreach (var o in oceanVisibilityControllers)
            {
                if (o == null) continue;
                if (underWater && o.hideUnderwater)
                {
                    o.gameObject.SetActive(false);
                }
                else
                {
                    o.gameObject.SetActive(true);
                }
            }
        }

        private void OnDisable()
        {
            //SetSkyKeyword(false);

            RenderPipelineManager.beginCameraRendering -= BeginCameraRendering;
            Emerge(null);
            SetDefaults();
            //GlobalEmerge();
        }

        public void Reset()
        {
            name = "OceanFogController";
            if (mode == eMode.River) name = "RiverFogController";
            FindSun();
            //FindMirror();
        }


        private void BeginCameraRendering(ScriptableRenderContext arg1, Camera arg2)
        {
            if (!isActiveAndEnabled) return;
            Run(arg2);
        }


        /// <summary>
        /// Runs every frame for every camera, if no camData passed in will loop thru all cameras
        /// </summary>
        public void Submerge(PerCamData camData)
        {
            //if (camData.underWaterCurrent == 1) return;
            //if (debug) Debug.Log("OceanFogController: Submerge", this);

            if (camData == null)
            {
                foreach (var o in perCamData)
                {
                    o.Value.underWaterCurrent = o.Value.underWater = 1;
                }
            }
            else
            {
#if UNITY_EDITOR                
                if (debug) Debug.LogFormat(camData.camera, "Submerge " + camData.camera.name);
#endif                
                //if (camData.underWaterCurrent == 1) return;
                camData.underWaterCurrent = camData.underWater = 1;
            }

            SetSettings();

            //SetSkyKeyword(true);
            UpdateOceanVisibilityControllers(true);
            //ffs

            /*if (affectUnityFog)
            {
                RenderSettings.fogColor = oceanFogColor;
                RenderSettings.fogStartDistance = oceanFogStart;
                RenderSettings.fogEndDistance = oceanFogEnd;
            }*/

            Shader.SetGlobalFloat(GlobalOceanUnder, 1);
            //Shader.SetGlobalFloat(GlobalOceanEarlyTriggerOffset, camData.oceanShift);

            //if (submergedEvent != null) submergedEvent.Invoke();

            if (causticsOnlyWhenSubmerged)
            {
                AffectKeywords(false);
            }
            else
            {
                AffectKeywords(false);
            }

            /*
            if (mirrorSurface)
            {
                mirrorSurface.SetNormalFlip(true);
                //mirrorSurface.transform.SetLocalY(earlyTriggerOffset);
                mirrorSurface.transform.localPosition = new Vector3(mirrorSurface.transform.localPosition.x, camData.earlyTriggerOffset, mirrorSurface.transform.localPosition.z);
            }*/
        }


        [Button]
        public void AffectKeywordsEmerge()
        {
            AffectKeywords(true);
        }

        [Button]
        public void AffectKeywordsSubmerge()
        {
            AffectKeywords(false);
        }


        [Button]
        public void GlobalSubmerge()
        {
            Shader.EnableKeyword("_USEUNDERWATER");
            Shader.SetKeyword(new GlobalKeyword("_USEUNDERWATER"), true);
        }

        [Button]
        public void GlobalEmerge()
        {
            Shader.EnableKeyword("_USEUNDERWATER");
            Shader.SetKeyword(new GlobalKeyword("_USEUNDERWATER"), false);
        }

        [Button]
        public void GlobalUnderSubmerge()
        {
            Shader.SetGlobalFloat(GlobalOceanUnder, 1);
        }

        [Button]
        public void GlobalUnderEmerge()
        {
            Shader.SetGlobalFloat(GlobalOceanUnder, 0);
        }


        public static void AffectKeywords(bool emerging)
        {
            Shader.EnableKeyword("_USEUNDERWATER");
            Shader.SetKeyword(new GlobalKeyword("_USEUNDERWATER"), !emerging);
        }

        /// <summary>
        /// Runs every frame for every camera, if no camData passed in will loop thru all cameras
        /// </summary>
        public void Emerge(PerCamData camData)
        {
            if (camData == null)
            {
                foreach (var o in perCamData)
                {
                    o.Value.underWaterCurrent = o.Value.underWater = 0;
                }
            }
            else
            {
#if UNITY_EDITOR                
                if (debug) Debug.LogFormat(camData.camera, "Emerge " + camData.camera.name);
#endif                
                //if (camData.underWaterCurrent == 0) return;
                camData.underWaterCurrent = camData.underWater = 0;
            }

            //if (debug) Debug.Log("OceanFogController: Emerge", this);


            //SetSkyKeyword(true);

            UpdateOceanVisibilityControllers(false);


            /*if (affectUnityFog)
            {
                RenderSettings.fogColor = skyFogColor;
                RenderSettings.fogStartDistance = skyFogStart;
                RenderSettings.fogEndDistance = skyFogEnd;
            }*/


            if (causticsOnlyWhenSubmerged)
            {
                AffectKeywords(true);
            }
            else
            {
                AffectKeywords(false);
            }

            Shader.SetGlobalFloat(GlobalOceanUnder, 0);
            //Shader.SetGlobalFloat(GlobalOceanEarlyTriggerOffset, camData.oceanShift);

            //emergedEvent.Invoke();

            /*if (mirrorSurface)
            {
                mirrorSurface.SetNormalFlip(false);
                //mirrorSurface.transform.SetLocalY(0);
                mirrorSurface.transform.localPosition = new Vector3(mirrorSurface.transform.localPosition.x, 0, mirrorSurface.transform.localPosition.z);
            }*/
        }
        
#if UNITY_EDITOR
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            SetDefaults();
        }
#endif
        
        /// <summary>
        /// Sets sensible defaults for shader globals with the effects turned off
        /// </summary>
        public static void SetDefaults()
        {
#if UNITY_EDITOR
            Debug.Log("OceanFogContoller: SetDefaults (off / above water)");
#endif
            Shader.SetGlobalFloat(GlobalOceanUnder, 0);
            Shader.SetGlobalFloat(GlobalOceanHeight, -10000000);
            Shader.SetGlobalFloat(GlobalOceanOffset, 0);
            AffectKeywords(true);
        }

        public bool SkyShaderValid()
        {
            if (!RenderSettings.skybox) return false;
            if (!RenderSettings.skybox.shader.name.Contains("SkySorcery - ASshader")) return false;
            return true;
        }
    }
}

#if UNITY_EDITOR

namespace UnityEditor
{
    [CustomEditor(typeof(OceanFogController)), CanEditMultipleObjects]
    public class OceanFogControllerEditor : OdinEditor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var targ = target as OceanFogController;

            //var myScripts = targets.Cast<ToggleObjects>().ToList();

            if (RenderSettings.skybox != null)
            {
                if (!RenderSettings.skybox.shader.name.Contains("SkySorcery - ASshader"))
                {
                    GUI.color = targ.SkyShaderValid() ? Color.white : Color.red;
                    if (GUILayout.Button("Set Sky to SkySorcery Shader"))
                    {
                        var shaderName = "Skybox/SkySorcery - ASshader";
                        var found = Shader.Find(shaderName);
                        if (!found)
                        {
                            Debug.LogError("Cant find shader: " + shaderName);
                        }
                        else
                        {
                            RenderSettings.skybox.shader = found;
                            //targ.SetSkyKeyword(true);
                        }
                    }

                    GUI.color = Color.white;
                }
                else
                {
                    /*
                    GUI.color = targ.SkyShaderValid() && RenderSettings.skybox.GetFloat("_OceanFog") >= 1 ? Color.white : Color.red;

                    if (GUILayout.Button("Set Sky Keyword"))
                    {
                        targ.SetSkyKeyword(true);
                    }

                    GUI.color = Color.white;
                    if (GUILayout.Button("Remove Sky Keyword"))
                    {
                        targ.SetSkyKeyword(false);
                    }*/
                }
            }

            if (targ.FixSceneClipNeeded())
            {
                GUI.color = Color.red;
                if (GUILayout.Button("Fix Cameras Near Clip"))
                {
                    targ.FixSceneClip();
                }

                GUI.color = Color.white;
            }

            if (GUILayout.Button("Emerge to Surface"))
            {
                targ.Emerge(null);
            }

            if (GUILayout.Button("Submerge below Surface"))
            {
                targ.Submerge(null);
            }
        }
    }
}


#endif
