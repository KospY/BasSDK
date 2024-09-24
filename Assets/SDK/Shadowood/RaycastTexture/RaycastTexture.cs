using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if FLUXY
using Fluxy;
#endif
#if MIRRORSANDREFLECTIONS
using Fragilem17.MirrorsAndPortals;
#endif
using Shadowood.RaycastTexture;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
#if UNITY_EDITOR
#if DYNAMICBLITPASS
using Shadowood.DynamicBlitPass;
#endif
#if ODIN_INSPECTOR && UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif
using UnityEditor;
#endif
using ThunderRoad.AssetSorcery;
using UnityEngine;
using UnityEngine.Rendering;
using ThunderRoad.Splines;

namespace Shadowood.RaycastTexture
{
    [ExecuteInEditMode]
    public class RaycastTexture : MonoBehaviour
    {
        internal const string FOLDER_PATH = "Assets/SDK/Shadowood";

#if WATERPLATFORMSETTINGS
        public WaterPlatformSettings platformSettings;
#endif

        public bool debug;
        public bool debugColliders;
        public bool debugCollidersDepth;

        [InlineButton("DebugVisualsToggle", "@debugVisuals ? \"DebugVisuals: On\" : \"DebugVisuals: Off\"")]
        public bool debugVisuals;

        private bool debugAboveAll;

        [EnumToggleButtons, HideLabel]
        [ShowIf("@debugVisuals && mode == eMode.Ocean")] //
        [GUIColor("@debugVisuals ? Color.cyan : Color.white")]
        //
        //[InlineButton("DebugVisualsToggle", "@debugVisuals ? \"DebugVisuals: On\" : \"DebugVisuals: Off\"")]
        //[InlineButton("DebugAboveAllToggle", "@debugAboveAll ? \"DebugAbove: On\" : \"DebugAbove: Off\"")]
        public eDebugModes debugMode = eDebugModes.Waves_Foam;

        [EnumToggleButtons, HideLabel]
        [ShowIf("@debugVisuals && mode == eMode.River")] //
        [GUIColor("@debugVisuals ? Color.cyan : Color.white")]
        //
        //[InlineButton("DebugVisualsToggle", "@debugVisuals ? \"DebugVisuals: On\" : \"DebugVisuals: Off\"")]
        //[InlineButton("DebugAboveAllToggle", "@debugAboveAll ? \"DebugAbove: On\" : \"DebugAbove: Off\"")]
        public eDebugModesRiver debugModeRiver = eDebugModesRiver.Flow_Mix;


        private Shader restoreShader;

        [Button]
        public void DebugAboveAllToggle()
        {
            debugAboveAll = !debugAboveAll;
            DebugVisualsType();
        }

        public bool TryGetWaterHeight(Vector3 point, out float waterHeight)
        {
            return oceanFogController.TryGetWaterHeight(point, out waterHeight);
        }

#if UNITY_EDITOR
        public static bool TryFindAsset<T>(string searchTerm, string folder, out T foundObject) where T : UnityEngine.Object
        {
            foundObject = null;
            var guidAssets = AssetDatabase.FindAssets(searchTerm, new string[] {folder});
            if (guidAssets.Length != 0)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guidAssets[0]);
                foundObject = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            }

            return foundObject != null;
        }
#endif
        void DebugVisualsToggle()
        {
#if UNITY_EDITOR
            if (!surfaceMaterial) return;

            debugVisuals = !debugVisuals;

            var debugShaderName = "";

            if (debugVisuals)
            {
                restoreShader = surfaceMaterial.shader;


                if (restoreShader.name.Contains("Shoreline"))
                {
                    debugShaderName = "ThunderRoad/Dev/Shoreline - Debug - ASshader";
                }

                if (restoreShader.name.Contains("River"))
                {
                    debugShaderName = "ThunderRoad/Dev/River - Debug - ASshader";
                }

                if (debugShaderName == "")
                {
                    Debug.LogError("Cant find a debug shader to pair with: " + restoreShader.name);
                    debugVisuals = false;
                    return;
                }
                else
                {
                    var debugShader = Shader.Find(debugShaderName);
                    if (!debugShader)
                    {
                        Debug.LogError("Cant find debug shader: " + debugShaderName);
                        debugVisuals = false;
                        return;
                    }

                    surfaceMaterial.shader = debugShader;

                    DebugVisualsType();
                }

                if (surfaceMaterial.shader.name.Contains("Debug")) restoreShader = null;
            }
            else
            {
                if (!restoreShader && mode == eMode.Ocean) restoreShader = Shader.Find("ThunderRoad/Shoreline - ASshader");
                if (!restoreShader && mode == eMode.River) restoreShader = Shader.Find("ThunderRoad/River - ASshader");
                if (restoreShader) surfaceMaterial.shader = restoreShader;
            }

            ApplyTexture();
#endif
        }

        void DebugVisualsType()
        {
            if (!surfaceMaterial) return;
            if (useMatProps)
            {
                if (mode == eMode.Ocean) materialPropertyBlock.SetInt("_DebugVisual", (int) debugMode);
                if (mode == eMode.River) materialPropertyBlock.SetInt("_DebugVisual", (int) debugModeRiver);
                materialPropertyBlock.SetInt("_ZTestMode", debugAboveAll ? 6 : 2);
                materialPropertyBlock.SetInt("_DebugVisuals", debugVisuals ? 1 : 0);
            }
            else
            {
                if (mode == eMode.Ocean) surfaceMaterial.SetInt("_DebugVisual", (int) debugMode);
                if (mode == eMode.River) surfaceMaterial.SetInt("_DebugVisual", (int) debugModeRiver);
                surfaceMaterial.SetInt("_ZTestMode", debugAboveAll ? 6 : 2);
                surfaceMaterial.SetInt("_DebugVisuals", debugVisuals ? 1 : 0);
            }
        }

        public enum eDebugModes
        {
            Color_ColorCache_RGB,
            Depth_ColorCache_A,
            Direction_DepthCache_RG,
            Distance_DepthCache_B,
            Depth_DepthCache_A,
            Waves_Raw,
            Waves_Foam,
            Reflection_Raw,
            DepthTint,
            Lightmap,
            ShadowMask
        }

        public enum eDebugModesRiver
        {
            Color_ColorCache_RGB = 0,
            Depth_ColorCache_A = 1,

            Direction_DepthCache_RG = 2,
            Distance_DepthCache_B = 3,
            Depth_DepthCache_A = 4,

            //Fluid_Color_RGB = 5,
            Flow_Rainbow = 6,
            Flow_Mix = 7,
            Fluid_Color_A = 8,
            //Fluid_Vel_A = 9,
        }


        [Space] //
        [EnumToggleButtons]
        public eMode mode = eMode.Ocean;

        public enum eMode
        {
            Ocean, // Generates a simple quad meshCollider for raycasting along with a box collider to contain it and everything down under the surface for restricting renderers to raycast against within that volume
            River
        }

        [NonSerialized]
        [Space]
        [GUIColor("@planarReflection ? Color.cyan : Color.white")] //
        [InlineButton("PlanarReflectionToggle", "@planarReflection ? \"Planar Reflection: Turn Off\" : \"Planar Reflection: Turn On\"")]
        public bool planarReflection = false;

        private bool planarReflectionLast;

        private Matrix4x4 lastMatrix;

        //[PropertySpace]
        //[GUIColor("@planarReflection ? Color.cyan : Color.white")] //
        //[Button("@planarReflection ? \"Planar Reflection: Turn Off\" : \"Planar Reflection: Turn On\"")]
        public void PlanarReflectionToggle()
        {
            if (!planarReflection)
            {
                SetPlanarReflectionOn();
            }
            else
            {
                SetPlanarReflectionOff();
            }
        }

        //[Space(10)]
        //[UnityEngine.Range(0, 360)]
        //public float rotateCubemaps = 0;

        [Space(10)]
        [Header("File Settings")] //
        //
        [GUIColor("@SurfaceMaterialValid() ? Color.white : Color.red")] //
        [InlineButton("CreateNew", "Create New")]
        public Material surfaceMaterial;

        private Matrix4x4 waveCenterMatrix;


#if UNITY_EDITOR

        public bool SurfaceMaterialValid()
        {
            if (!surfaceMaterial) return false;
            if (surfaceMaterial.name == "Ocean-Template") return false;
            if (surfaceMaterial.name == "River-Template") return false;
            return true;
        }


        public void CreateNew()
        {
            if (mode == eMode.Ocean)
            {
                //find the material called Ocean-Template
                if (TryFindAsset<Material>("t:Material Ocean-Template", FOLDER_PATH, out Material mat))
                {
                    //new material from the template
                    surfaceMaterial = new Material(mat);
                    var nom = "Ocean-" + gameObject.scene.name;
                    nom = nom.Replace("Ocean-Ocean-", "Ocean-");
                    surfaceMaterial.name = nom;
                    AutoSetFileName();
                }
                else
                {
                    Debug.LogError("Ocean-Template not found");
                }
            }

            if (mode == eMode.River)
            {
                //find the material called Ocean-Template
                if (TryFindAsset<Material>("t:Material River-Template", FOLDER_PATH, out Material mat))
                {
                    //new material from the template
                    surfaceMaterial = new Material(mat);
                    var nom = "River-" + gameObject.scene.name;
                    nom = nom.Replace("River-River-", "River-");
                    surfaceMaterial.name = nom;
                    AutoSetFileName();
                }
                else
                {
                    Debug.LogError("River-Template not found");
                }
            }

            if (PrefabUtility.IsPartOfAnyPrefab(gameObject))
            {
                Debug.Log("Is Prefab");
                path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
            }
            else
            {
                path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
            }


            Debug.Log("Path: " + path);

            path = Path.GetDirectoryName(path) + "/";
            Debug.Log("Path 2: " + path);

            //Debug.Log("path: " + gameObject.scene.path + " -1- " + Directory.GetParent(gameObject.scene.path) + " -2- ");
            var dataPath = Application.dataPath.Replace("Assets", "");
            path = path.Replace("\\", "/");
            path = path.Replace(dataPath, "");

            Debug.Log("path B: " + path);

            string matPath = path + surfaceMaterial.name + ".mat";


            Debug.Log("matPath: " + matPath);

            matPath = AssetDatabase.GenerateUniqueAssetPath(matPath);
            Debug.Log("path U: " + matPath);

            UnityEditor.AssetDatabase.CreateAsset(surfaceMaterial, matPath);
            surfaceMaterial = AssetDatabase.LoadAssetAtPath<Material>(matPath);

            SetupMirrorSurface();
            //SetPathToMaterialFolder();
            ApplyTexture();
        }
#endif

        //

        //[InfoBox("fileName doesnt match material name", InfoMessageType.Warning, "FileNameWarn")]
        //[GUIColor("@FileNameWarn() ? Color.yellow : Color.white")] //
        [InlineButton("AutoSetFileName", "AutoSet")]
        public string fileName;

        public void AutoSetFileName()
        {
            var currentPlatform = AssetSorceryPlatformRuntime.AssetSorceryGetBuildPlatform();
            fileName = surfaceMaterial.name + "-" + currentPlatform;
        }
        //private bool FileNameWarn() => surfaceMaterial && fileName != surfaceMaterial.name;


        [GUIColor("@PathValid() ? Color.white : Color.red")] //
        [InlineButton("SetPathToMaterialFolder", "AutoSet")]
        public string path = "";

#if UNITY_EDITOR
        public bool PathValid()
        {
            var dataPath = Application.dataPath.Replace("Assets", "") + "/";
            return System.IO.Directory.Exists(dataPath + path);
        }

        private void SetPathToMaterialFolder()
        {
            if (surfaceMaterial)
            {
                path = Directory.GetParent(UnityEditor.AssetDatabase.GetAssetPath(surfaceMaterial)).FullName + "/";

                var dataPath = Application.dataPath + "/";
                path = path.Replace("\\", "/");
                path = path.Replace(dataPath, "");
                path = "Assets/" + path;
            }

            //fileName = gameObject.scene.name;
        }
#endif
        //

        [Space(10)]
        [Header("Input Settings")]
        //
        //[GUIColor("@targetSurfaceTransform ? Color.white : Color.red")] //
        //[Tooltip("MeshCollider is fetched for raycasting against")]
        //public Transform targetSurfaceTransform;

        //[GUIColor("@targetSurfaceMesh ? Color.white : Color.red")] //
        //[Tooltip("Mesh found on targetSurfaceTransform, used by fluXY fluidsim")]
        //public Mesh targetSurfaceMesh;

        //[GUIColor("@targetColliderBounds ? Color.white : Color.red")] //
        //[Tooltip("Used to restrict search for FindAllRenderers to within the bounds to raycast against")]
        //public BoxCollider targetColliderBounds;
        [GUIColor("@(mode == eMode.Ocean) || targetColliders.Count > 0 ? Color.white : Color.red")] //
        [Tooltip("Used to restrict search for FindAllRenderers to within the bounds to raycast against")]
        [InlineButton("CreateBoxCollider", "Create Collider")]
        //[InlineButton("ToggleDebugTargetColliders", "Debug")]
        public List<BoxCollider> targetColliders = new List<BoxCollider>();

        public void CreateBoxCollider()
        {
            targetColliders.RemoveAll(o => o == null);
            if (targetMeshCollider && targetColliders.Count == 0)
            {
                //if (targetMeshCollider.gameObject.GetOrAddComponent<BoxCollider>())
                //{
                //    targetColliders.Add(targetMeshCollider.gameObject.GetComponent<BoxCollider>());
                //}
                //else

                targetColliders.Add(targetMeshCollider.gameObject.GetOrAddComponent<BoxCollider>());
            }
        }

        //public void ToggleDebugTargetColliders()
        //{
        //    debugTargetColliders = !debugTargetColliders;
        //}

        //public BoxCollider genCollider;

        //[GUIColor("@debugTargetColliders ? Color.cyan : Color.white")] //
        //[InlineButton("ToggleDebugTargetColliders", "Debug")]
        //public bool debugTargetColliders;

        //TODO show found colliders stuff here

        [GUIColor("@targetMeshCollider && targetMeshCollider.sharedMesh !=null ? Color.white : Color.red")] //
        [Tooltip("Used for water surface raycast, uses its uv's for the texture capture, and passed to fluXY fluid sim")]
        //
        public MeshCollider targetMeshCollider;

        [GUIColor("@(riverMesh != null) && (riverMesh.GetComponent<MeshFilter>() != null) && (riverMesh.GetComponent<MeshFilter>().sharedMesh != null) ? Color.white : Color.red")] //
        [ShowIf("@mode == eMode.River")]
        //
        public MeshRenderer riverMesh;

        //public GameObject targetSurfaceGO;
        //public Transform targetRenderTransform;
        //public MeshCollider targetSurfaceMeshCollider;

        //[FormerlySerializedAs("oceanMesh")] public MeshRenderer targetSurfaceRenderer;

        //[HideInInspector] [Tooltip("Renderers get material changed to 'surfaceMaterial' and mat shader properties or matprop blocks set")]
        //public List<MeshRenderer> targetSurfaceRenderers = new List<MeshRenderer>();


        [Space(10)] [Header("Object Inclusion Settings")] [Tooltip("Enable these objects only when capturing the depth/color texture for under the water surface")]
        public List<GameObject> underWaterMeshes = new List<GameObject>();

        public List<GameObject> excludeList = new List<GameObject>();
        public List<GameObject> includeList = new List<GameObject>();

        [NonSerialized] internal bool showMaterialInspector = false;

        //

        [Space(10)]
        [Header("Feature Links")] //

        //
#if ODIN_INSPECTOR
        //tri inspector doesnt support this
        [InfoBox("@OceanBakeInvalidReason()", "OceanBakeQuadValid")]
#endif
        [GUIColor("@OceanBakeQuadValid() ? Color.red : Color.white")] //
#if UNITY_EDITOR
        [InlineButton(nameof(FindOceanBakeQuad))]
#endif
        [ShowIf("@mode == eMode.Ocean")]
        public MeshRenderer oceanBakeQuad;

        public bool OceanBakeQuadValid()
        {
            return OceanBakeInvalidReason() != "";
        }

        public string OceanBakeInvalidReason()
        {
            if (!oceanBakeQuad) return "oceanBakeQuad null";
            if (oceanBakeQuad.lightmapIndex < 0) return "No Lightmap (might need baking)";
            var lightmapSettings = LightmapSettings.lightmaps;
            if (oceanBakeQuad.lightmapIndex > lightmapSettings.Length) return "lightmap index invalid: " + oceanBakeQuad.lightmapIndex + "/" + lightmapSettings.Length;
            if (lightmapSettings[oceanBakeQuad.lightmapIndex] == null) return "lightmap null";
            return "";
        }

        [GUIColor("@oceanbakeLM ? Color.white : Color.red")] //
        [ShowIf("@oceanBakeQuad")]
        public Texture oceanbakeLM;

        [ShowIf("@oceanBakeQuad")] public Texture oceanbakeLMSM;
        [ShowIf("@oceanBakeQuad")] public Vector4 oceanBakeST;
        [HideInInspector] public Matrix4x4 oceanBakeMatrix;

        //

        [PropertySpace]
        [GUIColor("@mainLight ? Color.white : Color.red")] //
#if UNITY_EDITOR
        [InlineButton(nameof(FindMainLight))]
#endif
        public Light mainLight;

#if UNITY_EDITOR

        public void FindOceanBakeQuad()
        {
            if (oceanBakeQuad != null) return;

            oceanBakeQuad = transform.Find("OceanBakeQuad")?.GetComponent<MeshRenderer>();
            if (oceanBakeQuad != null) return;

            oceanBakeQuad = new GameObject("OceanBakeQuad", typeof(MeshRenderer), typeof(MeshFilter)).GetComponent<MeshRenderer>();
            oceanBakeQuad.transform.parent = transform.parent;
            oceanBakeQuad.transform.SetLocalPositionAndRotation(transform.localPosition, transform.localRotation);
            oceanBakeQuad.transform.localScale = transform.localScale;
            oceanBakeQuad.scaleInLightmap = 0.5f;
            if (TryFindAsset<Material>("t:material OceanBakeQuad", FOLDER_PATH, out Material mat))
            {
                oceanBakeQuad.sharedMaterial = mat;
            }
            else
            {
                Debug.LogError("OceanBakeQuad material not found");
            }

            oceanBakeQuad.gameObject.isStatic = true;
            oceanBakeQuad.tag = "EditorOnly";
            var path = UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("OceanPlane_2Tri").First());
            var lowMesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>(path);
            oceanBakeQuad.GetComponent<MeshFilter>().sharedMesh = lowMesh;
        }

        public void FindMainLight()
        {
            mainLight = RenderSettings.sun;
            if (mainLight == null)
            {
                var results = FindObjectsOfType<Light>();
                foreach (var result in results)
                {
                    if (result.isActiveAndEnabled && result.type == LightType.Directional)
                    {
                        mainLight = result;
                        RenderSettings.sun = mainLight;
                        return;
                    }
                }
            }

            //if(caustics)caustics.dirlight = mainLight;
            //if(oceanFogController)oceanFogController.lightTarget = mainLight;
        }
#endif
        //

#if OCEANHEIGHTSAMPLER
        [GUIColor("@oceanHeightSampleManager ? Color.white : Color.red")] //
        [InlineButton(nameof(FindOceanHeightSampleManager))] //
        [ShowIf("@mode == eMode.Ocean")]
        public OceanHeightSampleManager oceanHeightSampleManager;

#if UNITY_EDITOR
        public void FindOceanHeightSampleManager()
        {
            oceanHeightSampleManager = FindObjectOfType<OceanHeightSampleManager>();
            if (oceanHeightSampleManager == null)
            {
                oceanHeightSampleManager = new GameObject("OceanHeightSampleManager", typeof(OceanHeightSampleManager)).GetComponent<OceanHeightSampleManager>();
                oceanHeightSampleManager.transform.parent = transform.parent;
                oceanHeightSampleManager.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                oceanHeightSampleManager.raycastTexture = this;
            }
        }
#endif
#endif

#if MIRRORSANDREFLECTIONS
        [Tooltip("Link to PlanarReflection")]
        [GUIColor("@mirrorRenderer ? Color.white : Color.red")] //
#if UNITY_EDITOR        
        [InlineButton(nameof(FindPlanarReflection))]
#endif
#if ODIN_INSPECTOR
        [InfoBox("Reflecting EVERYTHING", InfoMessageType.Warning, "PlanarWarning")]
#endif
        //
        public MirrorRenderer mirrorRenderer;

        private bool PlanarWarning() => mirrorRenderer && mirrorRenderer.PlanarWarning();


#if UNITY_EDITOR
        public void FindPlanarReflection()
        {
            var obj = GameObject.Find("PlanarReflection");
            if (obj) mirrorRenderer = obj.GetComponentInChildren<MirrorRenderer>();

            if (!mirrorRenderer)
            {
                if (TryFindAsset<GameObject>("t:prefab PlanarReflectionPrefab", "Assets/SDK/Shadowood", out GameObject prefab))
                {
                    //planarReflectionObj = Instantiate(planarReflectionPrefab);
                    obj = UnityEditor.PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                    obj.transform.parent = transform.parent;
                    obj.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                    obj.name = "PlanarReflection";
                    mirrorRenderer = obj.GetComponentInChildren<MirrorRenderer>();

                    FindMirrorSurface();
                }
                else
                {
                    Debug.LogError("PlanarReflectionPrefab not found");
                }

                //mirrorSurface.bounds = new Bounds(Vector3.zero, new Vector3(float.MaxValue,0.1f, float.MaxValue));
            }
#if WATERPLATFORMSETTINGS
            UpdateWaterPlatformSettings();
#endif
            SetupMirrorSurface();
        }
#endif

        [Space]
        [Tooltip("Mirror for planar reflections")]
        [GUIColor("@mirrorSurface ? Color.white : Color.red")] //
#if UNITY_EDITOR        
        [InlineButton(nameof(FindMirrorSurface))]
#endif
        public MirrorSurface mirrorSurface;

        void FindMirrorSurface()
        {
            if (mirrorSurface == null && mirrorRenderer) mirrorSurface = mirrorRenderer.transform.parent.GetComponentInChildren<MirrorSurface>();
            if (mirrorSurface == null) mirrorSurface = FindObjectOfType<MirrorSurface>(true);
            if (mirrorSurface == null) Debug.LogError("Cannot find MirrorSurface", this);
#if WATERPLATFORMSETTINGS
            UpdateWaterPlatformSettings();
#endif
            SetupMirrorSurface();
        }
#endif
        //


        //


        //
#if WATERPLATFORMSETTINGS
        void UpdateWaterPlatformSettings()
        {

            if (platformSettings == null)
            {
                //wp = gameObject.AddComponent<WaterPlatformManager>();
                Debug.LogWarning("WaterPlatformManager missing");
            }
            else
            {
                platformSettings.Refresh();
            }
        }
#endif
        //

        [Tooltip("Only needed if you have any materials (like FogSpriteSkybox) using SpriteFoggedSkybox shader")]
        [GUIColor("@globalSkyCube ? Color.white : Color.red")] // Todo only go red if items need it
#if UNITY_EDITOR        
        [InlineButton(nameof(FindGlobalSkyCube))]
#endif        
        public GlobalSkyCube globalSkyCube;


#if UNITY_EDITOR
        void FindGlobalSkyCube()
        {
            globalSkyCube = FindObjectOfType<GlobalSkyCube>();

            if (globalSkyCube == null)
            {
                globalSkyCube = new GameObject("GlobalSkyCube", typeof(GlobalSkyCube)).GetComponent<GlobalSkyCube>();
                globalSkyCube.transform.SetParent(transform.parent);

                //globalSkyCube.Reset();
            }

            globalSkyCube.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
#endif

        //
        [GUIColor("@toneMapping ? Color.white : Color.red")] //
#if UNITY_EDITOR
        [InlineButton(nameof(FindToneMappinInlineButtong))]
#endif        
        public Tonemapping toneMapping;

#if UNITY_EDITOR
        void FindToneMappinInlineButtong()
        {
            FindToneMapping(false);
        }

        void FindToneMapping(bool findOnly)
        {
#if WATERPLATFORMSETTINGS
            toneMapping = FindObjectOfType<Tonemapping>();
            if (!toneMapping && !findOnly)
            {
                if (TryFindAsset<GameObject>("t:prefab TonemappingPrefab", FOLDER_PATH, out GameObject prefab))
                {
                    toneMapping = (UnityEditor.PrefabUtility.InstantiatePrefab(prefab) as GameObject).GetComponent<Tonemapping>();
                    //toneMapping.transform.parent = transform.parent;
                    toneMapping.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                    toneMapping.name = "Tonemapping";
                }
                else
                {
                    Debug.LogError("TonemappingPrefab not found");
                }
            }

            if (toneMapping)
            {
                UpdateWaterPlatformSettings();
            }
#endif
        }
#endif
        //

        [PropertySpace]
        [Tooltip("Link to OceanRenderer")]
        [GUIColor("@oceanRenderer ? Color.white : Color.red")] //
        [ShowIf("@mode == eMode.Ocean")] //
#if UNITY_EDITOR        
        [InlineButton(nameof(FindOceanRenderer))]
#endif        
        //
        public OceanRenderer oceanRenderer;


#if UNITY_EDITOR
        void FindOceanRenderer()
        {
            if (oceanRenderer == null) oceanRenderer = transform.GetComponentInChildren<OceanRenderer>();
            if (oceanRenderer == null && transform.parent) oceanRenderer = transform.parent.GetComponentInChildren<OceanRenderer>();
            if (oceanRenderer == null)
            {
                oceanRenderer = new GameObject("OceanRenderer", typeof(OceanRenderer)).GetComponent<OceanRenderer>();
                oceanRenderer.transform.SetParent(transform.parent);

                //ApplyTexture(); // Sets the Mat
                oceanRenderer.Reset();
            }

            oceanRenderer.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            oceanRenderer.thunderSplineWithin = oceanSplineMask;
            var targetSurfaceTransform = targetMeshCollider.transform;
            oceanRenderer.targetTransform = targetSurfaceTransform;

            ApplyTexture();
            oceanRenderer.RunWithChosenPlatform();
#if WATERPLATFORMSETTINGS
            UpdateWaterPlatformSettings();
#endif
        }
#endif
        //

        [Tooltip("Link to OceanFogController")] //
#if UNITY_EDITOR        
        [InlineButton(nameof(FindOceanFogController))] //
#endif        
        [GUIColor("@oceanFogController ? Color.white : Color.red")]
        public OceanFogController oceanFogController;

#if UNITY_EDITOR
        void FindOceanFogController()
        {
            if (oceanFogController == null) oceanFogController = transform.GetComponentInChildren<OceanFogController>();
            if (oceanFogController == null) oceanFogController = transform.parent.GetComponentInChildren<OceanFogController>();
            if (oceanFogController == null)
            {
                //var nom = "OceanFogController";
                //if (mode == eMode.River) nom = "RiverFogController";
                oceanFogController = new GameObject("", typeof(OceanFogController)).GetComponent<OceanFogController>();
                oceanFogController.transform.SetParent(transform.parent);
                oceanFogController.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                oceanFogController.transform.localScale = Vector3.one;
                //ApplyTexture(); // Sets the Mat

                var targetSurfaceTransform = targetMeshCollider.transform;
                oceanFogController.oceanSurface = targetSurfaceTransform;

                switch (mode)
                {
                    case eMode.Ocean:
                        oceanFogController.mode = OceanFogController.eMode.Ocean;
                        break;
                    case eMode.River:
                        oceanFogController.mode = OceanFogController.eMode.River;
                        oceanFogController.riverCollider = targetMeshCollider;
                        break;
                }

                oceanFogController.Reset();

                //oceanFogController.mirrorSurface = mirrorSurface;
            }
        }
#endif
        //


        [Tooltip("Mask spline to hide water surface")] //
        [GUIColor("@oceanSplineMask ? Color.white : Color.red")] //
        [ShowIf("@mode == eMode.Ocean")] //
#if UNITY_EDITOR        
        [InlineButton(nameof(FindOceanSplineMask))]
#endif        
        public ThunderSplineWithin oceanSplineMask;

#if UNITY_EDITOR
        void FindOceanSplineMask()
        {
            if (!oceanSplineMask)
            {
                var oceanSplineMaskObj = GameObject.Find("OceanSplineMask");
                if (oceanSplineMaskObj && oceanSplineMaskObj.GetComponent<ThunderSpline>()) oceanSplineMask = oceanSplineMaskObj.GetComponentInChildren<ThunderSplineWithin>();
                if (!oceanSplineMask)
                {
                    if (TryFindAsset<GameObject>("t:prefab OceanSplineMaskPrefab", FOLDER_PATH, out GameObject prefab))
                    {
                        oceanSplineMaskObj = (UnityEditor.PrefabUtility.InstantiatePrefab(prefab) as GameObject);
                        oceanSplineMaskObj.transform.parent = transform.parent;
                        oceanSplineMaskObj.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                        oceanSplineMaskObj.name = "OceanSplineMask";
                        oceanSplineMask = oceanSplineMaskObj.GetComponentInChildren<ThunderSplineWithin>();
                    }
                    else
                    {
                        Debug.LogError("OceanSplineMaskPrefab not found");
                    }
                }

                // if (thunderSpline == null) thunderSpline = FindObjectOfType<ThunderSplineWithin>(false);
                if (oceanSplineMask)
                {
                    oceanSplineMask.raycastTexture = this;
                    //oceanSplineMask.transformTarget = oceanSplineMaskObj.transform;
                    oceanSplineMask.Run();
                }

                if (oceanRenderer) oceanRenderer.thunderSplineWithin = oceanSplineMask;
            }
        }
#endif

        //

        [PropertySpace]
        [Tooltip("Sets the XZ center for where ocean waves emanate")] //
        [GUIColor("@waveCenter ? Color.white : Color.red")] //
        [ShowIf("@mode == eMode.Ocean")] //
#if UNITY_EDITOR        
        [InlineButton(nameof(FindWaveCenter))]
#endif        
        public Transform waveCenter;

#if UNITY_EDITOR
        public void FindWaveCenter()
        {
            var found = GameObject.Find("WaveCenter");
            if (found) waveCenter = found.transform;
            if (!waveCenter)
            {
                var waveCenterH = new GameObject("WaveCenterH").transform;
                waveCenterH.SetParent(transform.parent);
                waveCenterH.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

                waveCenter = new GameObject("WaveCenter").transform;
                waveCenter.SetParent(waveCenterH);
                waveCenter.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
                waveCenter.transform.localScale = new Vector3(1f, -0.5f, 1f);

                if (caustics)
                {
                    caustics.panDirFromTransform = waveCenter;
                    caustics.dirMethod = Caustics.eDirMethod.tranWaveCenter;
                }
            }
        }
#endif

        [Tooltip("Water caustics")] //
        [GUIColor("@caustics ? Color.white : Color.red")] //
#if UNITY_EDITOR        
        [InlineButton(nameof(FindCaustics))]
#endif        
        public Caustics caustics;

#if UNITY_EDITOR

        public void FindCaustics()
        {
            var found = transform.parent.GetComponentInChildren<Caustics>();
            if (found) caustics = found;
            if (!caustics)
            {
                caustics = new GameObject("Caustics", typeof(Caustics)).GetComponent<Caustics>();
                caustics.transform.SetParent(transform.parent);
                caustics.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                caustics.transform.localScale = Vector3.one;
                if (waveCenter)
                {
                    caustics.panDirFromTransform = waveCenter;
                    caustics.dirMethod = Caustics.eDirMethod.tranWaveCenter;
                    caustics.Reset();
                }
            }
        }
#endif
        //

        [PropertySpace]
        [ShowIf("@mode == eMode.Ocean")]
        [Tooltip("Reflection probe to capture cubemap for ocean reflections fallback when no planar reflections")] //
        [GUIColor("@reflectionCapture ? Color.white : Color.red")] //
        //[InlineButton("ReflectionCapture", "Capture")] //
        [InlineButton("FindReflectionCapture", "Find Ref Probe")]
        public ReflectionProbe reflectionCapture;

        [ShowIf("@mode == eMode.Ocean")]
        [GUIColor("@reflectionCubemap ? Color.white : Color.red")] //
        [InlineButton("ReflectionCapture", "Capture")]
        //
        public Texture reflectionCubemap;

#if UNITY_EDITOR
        public void FindReflectionCapture()
        {
            var found = transform.parent.GetComponentInChildren<ReflectionProbe>();
            if (found) reflectionCapture = found;
            if (!reflectionCapture)
            {
                var nom = "ReflectionProbe-" + mode.ToString();
                reflectionCapture = new GameObject(nom, typeof(ReflectionProbe)).GetComponent<ReflectionProbe>();
                reflectionCapture.transform.SetParent(transform.parent);
                reflectionCapture.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                //
                //reflectionCapture.cullingMask = LayerMask.NameToLayer("SkyDome");
                reflectionCapture.cullingMask = LayerMask.GetMask("SkyDome");
                reflectionCapture.resolution = 256;
                reflectionCapture.clearFlags = ReflectionProbeClearFlags.Skybox;
                reflectionCapture.farClipPlane = 100000;

                if (mode == eMode.Ocean) reflectionCapture.enabled = false;
            }

            reflectionCapture.mode = ReflectionProbeMode.Custom;
            reflectionCapture.renderDynamicObjects = true;
            reflectionCapture.size = Vector3.positiveInfinity;
            reflectionCapture.hdr = true;

            SetCubemapOnMaterial();
        }

        private static readonly int GlobalSkyBlur = Shader.PropertyToID("GlobslSkyBlur");

        //private static readonly int TonemappingMasterBlend = Shader.PropertyToID("_TonemappingMasterBlend");
        private static readonly int PlanarReflectionRendering = Shader.PropertyToID("PlanarReflectionRendering");

        public void ReflectionCapture()
        {
            if (!reflectionCapture) FindReflectionCapture();

            // https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/Inspector/ReflectionProbeEditor.cs

            reflectionCapture.enabled = true;

            oceanFogController.Emerge(null);

            string targetPath = null;

            switch (reflectionCapture.mode)
            {
                case ReflectionProbeMode.Baked:
                    targetPath = AssetDatabase.GetAssetPath(reflectionCapture.bakedTexture);
                    break;
                case ReflectionProbeMode.Custom:
                    targetPath = AssetDatabase.GetAssetPath(reflectionCapture.customBakedTexture);
                    break;
            }

            if (string.IsNullOrEmpty(targetPath))
            {
                //targetPath = SceneManager.GetActiveScene().path.Replace(".unity", "");
                targetPath = path;

                Debug.Log("targetPath: " + targetPath);

                if (string.IsNullOrEmpty(targetPath))
                {
                    targetPath = "Assets";
                }
                else if (Directory.Exists(targetPath) == false)
                {
                    Directory.CreateDirectory(targetPath);
                }

                //string targetExtension = reflectionCapture.hdr ? "exr" : "png";
                //string fname = reflectionCapture.name + (reflectionCapture.hdr ? "-reflectionHDR" : "-reflection") + "." + targetExtension;

                string fname = fileName + "-reflectionHDR.exr";
                targetPath += "/" + fname;
            }

            Debug.Log("targetPath: " + targetPath);

            Shadowood.Tonemapping.TonemappingBrieflyDisable();

            switch (reflectionCapture.mode)
            {
                case ReflectionProbeMode.Baked:
                    //Lightmapping.BakeReflectionProbeSnapshot(reflectionCapture); // Internal -_-
                    //MethodInfo dynMethod1 = typeof(UnityEditor.Lightmapping).GetMethod("BakeReflectionProbeSnapshot", BindingFlags.Static | BindingFlags.NonPublic);
                    //dynMethod1.Invoke(reflectionCapture, new object[] {reflectionCapture});
                    Lightmapping.BakeReflectionProbe(reflectionCapture, targetPath);
                    break;
                case ReflectionProbeMode.Realtime:
                    reflectionCapture.RenderProbe();
                    break;
                case ReflectionProbeMode.Custom:
                    //ReflectionProbeEditor.BakeCustomReflectionProbe(reflectionProbeTarget, true); // Internal Private -_-
                    //MethodInfo dynMethod = typeof(ReflectionProbeEditor).GetMethod("BakeCustomReflectionProbe", BindingFlags.NonPublic | BindingFlags.Instance);
                    //dynMethod.Invoke(reflectionCapture, new object[]{reflectionCapture, true});
                    Lightmapping.BakeReflectionProbe(reflectionCapture, targetPath);
                    break;
            }

            //oceanFogController.WillRun();

            Shadowood.Tonemapping.TonemappingBrieflyReEnable();

            //Shader.SetGlobalFloat(GlobalSkyBlur, skyBlurStore);
            Shader.SetGlobalFloat(PlanarReflectionRendering, 0.0f);

            var tpath = AssetDatabase.GetAssetPath(reflectionCubemap);
            var textureImporter = (TextureImporter) AssetImporter.GetAtPath(tpath);
            var androidOverrides = textureImporter.GetPlatformTextureSettings("Android");
            androidOverrides.overridden = true;
            androidOverrides.format = TextureImporterFormat.RGBAHalf;
            textureImporter.SetPlatformTextureSettings(androidOverrides);
            //AssetDatabase.ImportAsset(tpath, ImportAssetOptions.ForceSynchronousImport);
            textureImporter.SaveAndReimport();
            reflectionCapture.enabled = false;

            SetCubemapOnMaterial();
        }
#endif

        // Adapted from: http://answers.unity.com/answers/975894/view.html


        [ContextMenu("SetCubemapOnMaterial")]
        public void SetCubemapOnMaterial()
        {
            if (reflectionCapture)
            {
                //TODO better support realtime / baked modes
                switch (reflectionCapture.mode)
                {
                    case ReflectionProbeMode.Baked:
                        if (reflectionCapture.bakedTexture) reflectionCubemap = reflectionCapture.bakedTexture;
                        break;
                    case ReflectionProbeMode.Realtime:
                        if (reflectionCapture.realtimeTexture) reflectionCubemap = reflectionCapture.realtimeTexture;
                        break;
                    case ReflectionProbeMode.Custom:
                        if (reflectionCapture.customBakedTexture) reflectionCubemap = reflectionCapture.customBakedTexture;
                        break;
                }
            }

            if (!reflectionCubemap) return;

            if (globalSkyCube) globalSkyCube.SetCubemap(reflectionCubemap);

            if (useMatProps)
            {
                if (materialPropertyBlock == null) materialPropertyBlock = new MaterialPropertyBlock();
                materialPropertyBlock.SetTexture("_Cubemap", reflectionCubemap);
            }
            else
            {
                if (surfaceMaterial) surfaceMaterial.SetTexture("_Cubemap", reflectionCubemap);
            }
        }

#if FLUXY
        //
        [Header("Fluidsim (FluXY)")] //
        //
        [Tooltip("Link to Fluxy container")] //
        [GUIColor("@fluxyContainer ? Color.white : Color.red")] //
        [ShowIf("@mode==eMode.River")] //
#if UNITY_EDITOR
        [InlineButton(nameof(FindFluxyContainer))]
#endif
        public FluxyContainer fluxyContainer;

        void FindFluxyContainer()
        {
            if (fluxyContainer == null) fluxyContainer = FindObjectOfType<FluxyContainer>();
            if (fluxyContainer == null) fluxyContainer = transform.GetComponentInChildren<FluxyContainer>();
            if (fluxyContainer == null) fluxyContainer = transform.parent.GetComponentInChildren<FluxyContainer>();
        }

        //

        [Tooltip("Link to Fluxy Target ( for collision with depth mask/shore )")] //
        [GUIColor("@fluxyTarget ? Color.white : Color.red")] //
        [ShowIf("@mode==eMode.River")] //
#if UNITY_EDITOR        
        [InlineButton(nameof(FindFluxyTarget))]
#endif        
        public FluxyTarget fluxyTarget;

        void FindFluxyTarget()
        {
            if (fluxyTarget == null) fluxyTarget = transform.GetComponentInChildren<FluxyTarget>();
            if (fluxyTarget == null) fluxyTarget = transform.parent.GetComponentInChildren<FluxyTarget>();
            if (fluxyTarget == null) fluxyTarget = FindObjectOfType<FluxyTarget>(true);
        }

        [ShowIf("@fluxyTarget && fluxyContainer")]
#if UNITY_EDITOR        
        [InlineButton(nameof(UpdateFluid))]
#endif
        public FluidSimSettings fluidSimSettings;
#endif


        //


        /*
        [Space]
        public Color bakeTransparencyColor = Color.white;
        private Texture2D bakeTransparencyTex;

        [ContextMenu("SetTransparencyColor"), Button]
        void SetTransparencyColor()
        {
            if (!Application.isEditor) return;
            if (bakeTransparencyTex == null) bakeTransparencyTex = new Texture2D(8, 8);
            var colors = new Color[bakeTransparencyTex.width * bakeTransparencyTex.height];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = bakeTransparencyColor;
            }

            bakeTransparencyTex.SetPixels(colors);
            bakeTransparencyTex.Apply();

            // Baking with colored texture wont work with material property blocks
            //if (useMatProps)
            //{
            //    materialPropertyBlock.SetTexture("_TransparencyLM", bakeTransparencyTex);
            //}
            //else
            {
                surfaceMaterial.SetTexture("_TransparencyLM", bakeTransparencyTex);
            }
        }
*/

        private static int[] TextureSizes = new int[] {8, 32, 64, 128, 256, 512, 1024, 2048, 4096};

        [Space]
        [Header("Intersection Settings")] //
        [ValueDropdown("TextureSizes")]
        //
        public int resolution = 512;

        [ValueDropdown("TextureSizes")] //
        public int resolutionColor = 1024;

        [ShowIf("@fluxyContainer != null")] [ValueDropdown("TextureSizes")]
        public int fluidRes = 512;

        [Space] //
        [PropertySpace]
        //[Header("Advanced Settings")]
        //
        [Tooltip("Use material property block, wont alter original material")]
        [FoldoutGroup("Advanced Settings")]
        public bool useMatProps = true;

        [FoldoutGroup("Advanced Settings")] public float depthOffset = 0.0f;

        [FoldoutGroup("Advanced Settings")] public bool useLightmaps = true;

        //[FoldoutGroup("Advanced Settings")] public bool useCaptureTexture = true;
        [FoldoutGroup("Advanced Settings")] public bool useNewCapture = true;

        [FoldoutGroup("Advanced Settings")] public bool captureFakeTransparency = true;

        //[ValueDropdown("TextureSizes")] [FoldoutGroup("Advanced Settings")]
        //public int captureTextureRes = 512;


        [FoldoutGroup("Advanced Settings")] public int rays = 4;
        [FoldoutGroup("Advanced Settings")] public float rayDistance = 0.1f;

        [FoldoutGroup("Advanced Settings")] public float bias = 1e-05f;
        //[Tooltip("Baking extend")] public int spreadSamples = 15;

        //[FoldoutGroup("Advanced Settings")] public bool bilinearSample = true;

        [FoldoutGroup("Advanced Settings")] [UnityEngine.Range(1, 8)]
        public int mipLevel = 1;

        [Space] //
        //[Header("Distance Field Settings")]
        [ShowIf("@mode == eMode.River")] //
        [FoldoutGroup("Advanced Settings")]
        public JumpFlood jumpFlood;

        [Space] //
        [ShowIf("@mode == eMode.River")] //
        [FoldoutGroup("Advanced Settings")]
        //
        public string fluidColorTexName = "_FluidColor";

        [FoldoutGroup("Advanced Settings")] //
        [ShowIf("@mode == eMode.River")]
        //
        public string fluidVelocityTexName = "_FluidVelocity";

        [Space] //
        [Header("Texture Results")] //
        [FoldoutGroup("Advanced Settings")]
        public Texture2D depthCacheTex;

        [FoldoutGroup("Advanced Settings")] //
        public Texture2D colorCacheTex;

        [Space] //
        [ShowIf("@mode == eMode.River")] //
        [FoldoutGroup("Advanced Settings")]
        public Texture2D fluidVelCacheTex;

        [ShowIf("@mode == eMode.River")] //
        [FoldoutGroup("Advanced Settings")]
        //
        public Texture2D fluidColCacheTex;

        [Space] //
        [ShowIf("@mode == eMode.River")] //
        [FoldoutGroup("Advanced Settings")]
        public RenderTexture dilationResult;

        [Space] //
        [FoldoutGroup("Advanced Settings")]
        public Texture intersectionTexture;

        [Space] //
        [FoldoutGroup("Advanced Settings")]
        public Texture colorDepthTexture;

        [Space] //
        [Header("Blur")] //
        [FoldoutGroup("Advanced Settings")]
        public Shader blurShader;

        [FoldoutGroup("Advanced Settings")] public Shader blurMixShader;
        [FoldoutGroup("Advanced Settings")] public RenderTexture blurredColorRT;
        [FoldoutGroup("Advanced Settings")] public RenderTexture blurredDepthRT;

        //public Shader dilationShader;
        //public Material dilationMat;
        //public Texture depthTexture;
        //public Texture distanceTexture;
        //public Texture textureResult;
        //public Texture colorTexture;
        //[FormerlySerializedAs("depthTexture")] [FormerlySerializedAs("distanceTexture")] [FormerlySerializedAs("textureResult")]
        //[FormerlySerializedAs("colorTexture")] [FormerlySerializedAs("depthTexture")]
        //public List<Material> targetMats;

        [HideInInspector] public float lastRan, lastRanStore = -1;

        //

        private void OnValidate()
        {
            if (!isActiveAndEnabled) return;

#if WATERPLATFORMSETTINGS
            if (!platformSettings) platformSettings = this.GetComponent<WaterPlatformSettings>();
#endif
            Setup(true);
            //SetTransparencyColor();

            DebugVisualsType();

            ApplyTexture();
        }

        private void OnEnable()
        {
            if (debugVisuals) DebugVisualsToggle();
            debugVisuals = false;
            planarReflectionLast = !planarReflection;
            //AutoSetFileName();
            Setup();
#if UNITY_EDITOR
            //LoadTex(); // Weird bug causes the cache textures to wipe when reloading scene, this forces load -_-
#endif
            ApplyTexture();
        }


#if UNITY_EDITOR
        private void Reset()
        {
            //targetSurfaceTransform = transform;
            if (gameObject.name.StartsWith("GameObject")) gameObject.name = "RaycastTexture";
        }

        [Button]
        public void CreateDefaultOceanQuad()
        {
            if (targetMeshCollider == null || targetMeshCollider.gameObject != this) targetMeshCollider = gameObject.GetOrAddComponent<MeshCollider>();

            if (targetMeshCollider.sharedMesh == null || targetMeshCollider.sharedMesh.name != "OceanPlane-2Tri")
            {
                var path2 = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("OceanPlane-2Tri").First());
                targetMeshCollider.sharedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(path2);
            }

            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.localScale = new Vector3(100, 0, 100);
            targetMeshCollider.enabled = false;

            //if (targetMeshCollider.sharedMesh == null || targetMeshCollider.sharedMesh.name != "Quad") targetMeshCollider.sharedMesh = Resources.GetBuiltinResource<Mesh>("Quad.fbx");
            //transform.rotation = Quaternion.Euler(90,0,0);
            //transform.localScale = new Vector3(100,100,0);

            UpdateBoundsFromColliders();
            //AutoFitBoxColliderToMeshCollider();
        }

        public void RemoveDefaultQuad()
        {
            DestroyImmediate(targetMeshCollider);
            targetMeshCollider = null;
            UpdateBoundsFromColliders();
        }
#endif

        [ContextMenu("Setup"), Button]
        public void Setup()
        {
            Setup(true);
        }

        /// <summary>
        /// Setup ran on Validate and OnEnable and on demand
        /// </summary>
        public void Setup(bool silent = true)
        {
            if (!silent) Debug.Log("RaycastTexture: Setup");

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (mainLight == null) FindMainLight();

                if (toneMapping == null) FindToneMapping(true);
            }
            switch (mode)
            {
                case eMode.Ocean:
                    //AutoFitBoxColliderToMeshCollider();
                    UpdateBoundsFromColliders();
                    break;
                case eMode.River:
                    if (targetMeshCollider && targetMeshCollider.gameObject == this) RemoveDefaultQuad();
                    break;
            }
#endif
            //if (genCollider != null && !targetColliders.Contains(genCollider)) targetColliders.Add(genCollider);

            if (materialPropertyBlock == null) materialPropertyBlock = new MaterialPropertyBlock();

#if FLUXY
            if (fluxyContainer && targetMeshCollider)
            {
                fluxyContainer.customMesh = targetMeshCollider.sharedMesh;
#if UNITY_EDITOR

                fluxyContainer.meshCollider = targetMeshCollider; //RaycastTools.GetTargetCollider(targetSurfaceTransform) as MeshCollider;
#endif
            }

            if (fluxyTarget && fluxyContainer)
            {
                fluxyTarget.densityTexture = depthCacheTex;
                fluxyTarget.velocityTexture = depthCacheTex;
                if (fluidSimSettings == null || !fluidSimSettings.setup) fluidSimSettings = new FluidSimSettings(fluxyContainer, fluxyTarget);
                fluidSimSettings.setup = true;
                fluidSimSettings.ApplySettings(fluxyContainer, fluxyTarget);
            }
#endif

            if (!isActiveAndEnabled) return;

            if (planarReflectionLast != planarReflection)
            {
                planarReflectionLast = planarReflection;

                if (planarReflection)
                {
                    SetPlanarReflectionOn();
                }
                else
                {
                    SetPlanarReflectionOff();
                }
            }

            if (oceanFogController)
            {
                switch (mode)
                {
                    case eMode.Ocean:
                        oceanFogController.mode = OceanFogController.eMode.Ocean;
                        break;
                    case eMode.River:
                        oceanFogController.mode = OceanFogController.eMode.River;
                        break;
                }
            }

            //foreach (var surfaceRenderer in targetSurfaceRenderers)
            //{
            //    if (surfaceRenderer == null) continue;
            //    if (surfaceMaterial) surfaceRenderer.sharedMaterial = surfaceMaterial;
            //}

            SetupMirrorSurface();
        }


        void SetupMirrorSurface()
        {
#if MIRRORSANDREFLECTIONS
            if (mirrorSurface != null) mirrorSurface.materials = new List<Material> {surfaceMaterial};
#endif
        }

        //

        [ContextMenu("PlanarReflection On")]
        public void SetPlanarReflectionOn()
        {
#if MIRRORSANDREFLECTIONS
            Debug.Log("RaycastTexture: SetPlanarReflectionOn: mir:" + mirrorRenderer);
            planarReflection = true;

            if (mirrorRenderer) mirrorRenderer.transform.parent.gameObject.SetActive(true);

            Shader.EnableKeyword("GlobalPlanarReflection");
            Shader.SetKeyword(new GlobalKeyword("GlobalPlanarReflection"), true);


            //foreach (var material in materials)
            {
                //if (oceanMaterial == null) continue;

                surfaceMaterial?.EnableKeyword("_PLANARREFLECTION_ON");

                if (useMatProps)
                {
                    if (materialPropertyBlock == null) materialPropertyBlock = new MaterialPropertyBlock();
                    materialPropertyBlock.SetFloat("_PlanarReflection", 1);
                }
                else
                {
                    surfaceMaterial?.SetFloat("_PlanarReflection", 1);
                }
            }

            ApplyTexture();
#endif
        }

        private MaterialPropertyBlock materialPropertyBlock;

        [ContextMenu("PlanarReflection Off")]
        public void SetPlanarReflectionOff()
        {
#if MIRRORSANDREFLECTIONS
#if UNITY_EDITOR
            Debug.Log("Shorelines: SetPlanarReflectionOff: mir:" + mirrorRenderer);
#endif
            planarReflection = false;
            if (mirrorRenderer) mirrorRenderer.transform.parent.gameObject.SetActive(false);

            Shader.EnableKeyword("GlobalPlanarReflection");
            Shader.SetKeyword(new GlobalKeyword("GlobalPlanarReflection"), false);

            //foreach (var material in materials)
            {
                //if (material == null) continue;

                surfaceMaterial?.DisableKeyword("_PLANARREFLECTION_ON");

                if (useMatProps)
                {
                    if (materialPropertyBlock == null) materialPropertyBlock = new MaterialPropertyBlock();
                    materialPropertyBlock.SetFloat("_PlanarReflection", 0);
                }
                else
                {
                    surfaceMaterial?.SetFloat("_PlanarReflection", 0);
                }
            }

            ApplyTexture();
#endif
        }


        public void SetReflectionCube(Texture textureIn)
        {
            Debug.Log("Shorelines: SetReflectionCube");

            //foreach (var mat in materials)
            {
                //if (mat == null) continue;
                if (useMatProps)
                {
                    materialPropertyBlock.SetTexture("_Cubemap", textureIn);
                }
                else
                {
                    surfaceMaterial?.SetTexture("_Cubemap", textureIn);
                }
            }
        }

        //

        public void UnderwaterMeshes(bool visible)
        {
            foreach (var underWaterMesh in underWaterMeshes)
            {
                if (underWaterMesh == null) continue;
                underWaterMesh.SetActive(visible);
            }
        }

        //

        [HideInInspector] public bool hideCopies = true;

#if UNITY_EDITOR
        /// <summary>
        /// Apply Blurs if iteration levels above 0
        /// </summary>
        public void PostProcess()
        {
            if (debug) Debug.Log("PostProcess");

            BlurSorcery(dilationResult); // blurs to dilationTex amd blurredDepthRT


            if (blurDepthIteration > 0 && blurredDepthRT != null)
            {
                depthCacheTex = blurredDepthRT.ToTexture2D();
                //oceanMaterial.SetTexture("_DepthCache", blurredDepthRT);
            }
            else
            {
                if (dilationResult) depthCacheTex = dilationResult.ToTexture2D();
                //oceanMaterial.SetTexture("_DepthCache", dilationResult);
            }

            if (captureFakeTransparency)
            {
                BlurSorceryColor(dilationResult, colorDepthTexture); // blurs to blurredColorRT


                if (blurColorIteration > 0)
                {
                    colorCacheTex = blurredColorRT.ToTexture2D();
                    //oceanMaterial.SetTexture("_ColorCache", blurredColorRT);
                }
                else
                {
                    colorCacheTex = colorDepthTexture.ToTexture2D();
                    //oceanMaterial.SetTexture("_ColorCache", depthTexture);
                }
            }

            lastRan = Time.timeSinceLevelLoad;
        }

        /// <summary>
        /// A single color coast edge line/intersection mask used to generate distance field from
        /// </summary>
        public Texture RenderIntersectionTexture()
        {
            if (meshCollidersCopies.Count <= 0)
            {
                Debug.LogError("meshCollidersCopies is empty, 'Find Colliders' first");
                FindRenderers();
            }

            if (meshCollidersCopies.Count <= 0)
            {
                Debug.LogError("meshCollidersCopies is empty");
                return null;
            }

            int resolutionX = 0;
            int resolutionY = 0;
            var targetSurfaceTransform = targetMeshCollider.transform;
            if (targetSurfaceTransform.lossyScale.x > targetSurfaceTransform.lossyScale.z)
            {
                resolutionX = resolution;
                resolutionY = (int) (resolution * (targetSurfaceTransform.lossyScale.z / targetSurfaceTransform.lossyScale.x));
            }
            else
            {
                resolutionY = resolution;
                resolutionX = (int) (resolution * (targetSurfaceTransform.lossyScale.x / targetSurfaceTransform.lossyScale.z));
            }

            resolutionX = Math.Abs(resolutionX);
            resolutionY = Math.Abs(resolutionY);

            Debug.Log("CreateIntersectionMap: " + targetSurfaceTransform.lossyScale + " : " + resolutionX + "x" + resolutionY);

            foreach (var o in meshCollidersCopies)
            {
                o.gameObject.SetActive(true);
            }

            foreach (var o in meshCollidersDepthCopies)
            {
                o.gameObject.SetActive(false);
            }


            targetMeshCollider.enabled = true;
            var tex = RaycastTools.CreateIntersectionMap(resolutionX, resolutionY, 0, true, rayDistance, bias, rays, targetMeshCollider, debug, ref meshCollidersCopies, oceanSplineMask?.thunderSpline);
            targetMeshCollider.enabled = false;
            return tex;
        }

        /// <summary>
        /// RGB for color capture of the entire world under the ocean plane, A depth capture of shoreline for transparency blend with eg: a sandy beach
        /// </summary>
        public Texture RenderColorDepthTexture()
        {
            //UnderwaterMeshes(true);

            if (meshCollidersDepthCopies.Count <= 0)
            {
                Debug.LogError("meshCollidersDepthCopies is empty, 'Find Colliders' first");
                FindRenderers();
            }

            if (meshCollidersDepthCopies.Count <= 0)
            {
                Debug.LogError("meshCollidersDepthCopies is empty");
                return null;
            }

            int resolutionX = 0;
            int resolutionY = 0;
            var targetSurfaceTransform = targetMeshCollider.transform;
            if (targetSurfaceTransform.lossyScale.x > targetSurfaceTransform.lossyScale.z)
            {
                resolutionX = resolutionColor;
                resolutionY = (int) (resolutionColor * (targetSurfaceTransform.lossyScale.z / targetSurfaceTransform.lossyScale.x));
            }
            else
            {
                resolutionY = resolutionColor;
                resolutionX = (int) (resolutionColor * (targetSurfaceTransform.lossyScale.x / targetSurfaceTransform.lossyScale.z));
            }

            resolutionX = Math.Abs(resolutionX);
            resolutionY = Math.Abs(resolutionY);

            Debug.Log("RenderColorDepthTexture: " + targetSurfaceTransform.lossyScale + " : " + resolutionX + "x" + resolutionY);

            foreach (var o in meshCollidersCopies)
            {
                o.gameObject.SetActive(false);
            }

            foreach (var o in meshCollidersDepthCopies)
            {
                o.gameObject.SetActive(true);
            }

            if(oceanRenderer)oceanRenderer.enabled = false;
            if (riverMesh) riverMesh.enabled = false;
            targetMeshCollider.enabled = true;
            var tex = RaycastTools.CreateColorDepthMap(resolutionX, resolutionY, 0, true, targetMeshCollider, 1000, depthOffset, debug, ref meshCollidersDepthCopies, useLightmaps, useNewCapture, mipLevel, oceanSplineMask?.thunderSpline);
            targetMeshCollider.enabled = false;
            if(oceanRenderer)oceanRenderer.enabled = true;
            if (riverMesh) riverMesh.enabled = true;
            //UnderwaterMeshes(false);

            return tex;

            //PostProcess();
            //ApplyTexture();
        }
#endif


        private Material matBlur;
        private Material matBlurMix;


        //[Header("Blur Rain")] [Range(0, 20)] public int blurIteration = 0;
        //public RenderTexture blurredRT;

        [Header("Blur Color")] [UnityEngine.Range(0, 200)]
        public int blurColorIteration = 0;

        [UnityEngine.Range(0, 1)] public float blurBlendMix = 1;


        [Header("Blur Depth")] [UnityEngine.Range(0, 20)]
        public int blurDepthIteration = 0;


        //public bool dilationSorcery;

        //public RenderTextureFormat textureFormat = RenderTextureFormat.ARGBHalf;

        //public RenderTexture dilationResult;

        //public bool dilationSwizzle = true;
        //public Shader dilationShader;
        //public Material dilationMat;

        public void SetupBlur()
        {
            if (matBlur == null)
            {
                if (blurShader == null) blurShader = Shader.Find("Hidden/Gaussian Blur Filter");
                if (blurShader == null) Debug.LogError("Blur Shader Missing");
                matBlur = new Material(blurShader);
            }

            if (matBlurMix == null)
            {
                if (blurMixShader == null) blurMixShader = Shader.Find("Hidden/Blur Mix");
                if (blurMixShader == null) Debug.LogError("Blur Mix Shader Missing");
                matBlurMix = new Material(blurMixShader);
            }
        }

        /// <summary>
        /// dilationTex : RG floodfill/distance field direction, B field distance, A coast edge line/intersection mask used to generate the field from
        /// colorResult : RGB albedo color for under water, A coast edge line/intersection mask
        /// depthTex : 
        /// </summary>
        private void BlurSorcery(Texture dilationTex) //, Texture depthTex)
        {
            if (dilationTex == null) return;

            if (debug) Debug.Log("BlurSorcery");

            SetupBlur();

            if (blurDepthIteration > 0)
            {
                if (blurredDepthRT == null || blurredDepthRT.width != dilationTex.width)
                {
                    Debug.Log("New RT: blurredDepthRT");
                    blurredDepthRT = new RenderTexture(dilationTex.width, dilationTex.height, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
                    blurredDepthRT.name = "blurredDepthRT-" + blurredDepthRT.GetHashCode();
                    blurredDepthRT.hideFlags = HideFlags.DontSave;
                }

                //var rt1 = RenderTexture.GetTemporary(ct.width, ct.height);
                var rt2 = RenderTexture.GetTemporary(dilationTex.width, dilationTex.height, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);


                Graphics.Blit(dilationTex, blurredDepthRT);

                for (var i = 0; i < blurDepthIteration; i++)
                {
                    Graphics.Blit(blurredDepthRT, rt2, matBlur, 1);
                    Graphics.Blit(rt2, blurredDepthRT, matBlur, 2);
                }

                //Shader.SetGlobalTexture("_DepthCache", blurredRT);
                //rt2.Release();
                RenderTexture.ReleaseTemporary(rt2);

                dilationTex = blurredDepthRT;
            }
            else
            {
                blurredDepthRT = null;
                // Graphics.Blit(ct, blurredRT);
                //if (ct) Shader.SetGlobalTexture("_DepthCache", ct);
            }
        }

        /// <summary>
        /// dilationTex : RG floodfill/distance field direction, B field distance, A coast edge line/intersection mask used to generate the field from
        /// colorResult : RGB albedo color for under water, A coast edge line/intersection mask
        /// depthTex : 
        /// </summary>
        private void BlurSorceryColor(Texture dilationTex, Texture colorTex) //, Texture depthTex)
        {
            if (colorTex == null) return;

            SetupBlur();

            if (blurColorIteration > 0)
            {
                if (blurredColorRT == null || blurredColorRT.width != colorTex.width)
                {
                    Debug.Log("New RT: blurredColorRT");
                    blurredColorRT = new RenderTexture(colorTex.width, colorTex.height, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
                    blurredColorRT.name = "blurredColorRT-" + blurredColorRT.GetHashCode();
                    blurredColorRT.hideFlags = HideFlags.DontSave;
                }

                //var rt1 = RenderTexture.GetTemporary(ct.width, ct.height);
                var rt2 = RenderTexture.GetTemporary(colorTex.width, colorTex.height, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);


                Graphics.Blit(colorTex, blurredColorRT);

                for (var i = 0; i < blurColorIteration; i++)
                {
                    Graphics.Blit(blurredColorRT, rt2, matBlur, 1);
                    Graphics.Blit(rt2, blurredColorRT, matBlur, 2);
                }

                Graphics.Blit(blurredColorRT, rt2);
                matBlurMix.SetFloat("_BlendMix", blurBlendMix);
                matBlurMix.SetTexture("_A", rt2);
                matBlurMix.SetTexture("_B", colorTex);
                if (dilationTex) matBlurMix.SetTexture("_BlendMask", colorTex); // depthTex
                //matBlurMix.SetFloat("_DepthOffset", transform.position.y);
                Graphics.Blit(null, blurredColorRT, matBlurMix);
                //Graphics.Blit(null, blurredColorRT);

                //Shader.SetGlobalTexture("_DepthCache", blurredColorRT);
                //rt2.Release();
                RenderTexture.ReleaseTemporary(rt2);
            }
            else
            {
                blurredColorRT = null;
            }


            //TODO what is using the global texture _ColorCache

            if (blurColorIteration > 0 && blurredColorRT != null)
            {
                Shader.SetGlobalTexture("_ColorCache", blurredColorRT);
            }
            else
            {
                if (colorTex) Shader.SetGlobalTexture("_ColorCache", colorTex);
            }
        }

        [HideIf("hideCopies")] public List<GameObject> meshCollidersDebug = new List<GameObject>();

        private List<GameObject> meshColliders = new List<GameObject>();
        private List<GameObject> meshCollidersDepth = new List<GameObject>();
        private List<MeshCollider> meshCollidersCopies = new List<MeshCollider>();
        private List<MeshCollider> meshCollidersDepthCopies = new List<MeshCollider>();

        //public List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
        //public List<MeshRenderer> meshRenderersDepth = new List<MeshRenderer>();


        //TODO might as well combine FindMeshColliders & FindMeshesWithoutCollider and only find renderers now
        //TODO keep the colliders cached inside an editoronly parent?

#if UNITY_EDITOR

        [PropertySpace]
        [ContextMenu("Find Colliders"), Button("@FindCollidersName()")]
        public void FindCollidersButton()
        {
            meshColliders.Clear();
            meshCollidersDepth.Clear();
            //FindColliders();
            FindRenderers();
            MakeCopies();
        }

        public string FindCollidersName()
        {
            var st = "Find Colliders";
            if (meshColliders.Count > 0) st += " (" + meshColliders.Count + ")";
            return st;
        }

        /*
        [ContextMenu("Find Colliders")] //[Button]
        public void FindColliders()
        {
            UnderwaterMeshes(true);

            var tmeshColliders = RaycastTools.FindMeshColliders(targetSurfaceTransform, rayDistance);

            foreach (var o in tmeshColliders)
            {
                if (!meshColliders.Contains(o.gameObject)) meshColliders.Add(o.gameObject);
            }

            meshCollidersDepth = new List<GameObject>(meshColliders);
            UnderwaterMeshes(false);

            findCollidersLastRan = Time.realtimeSinceStartup;
        }*/

        private List<Bounds> boundsList = new List<Bounds>();

        public void UpdateBoundsFromColliders()
        {
            if (boundsList == null) boundsList = new List<Bounds>();
            boundsList.Clear();
            foreach (var targetCollider in targetColliders)
            {
                if (targetCollider == null) continue;
                boundsList.Add(targetCollider.bounds);
            }

            if (mode == eMode.Ocean && targetMeshCollider != null)
            {
                var bounds = targetMeshCollider.bounds;
                //bounds = bounds.Transform(targetMeshCollider.transform.localToWorldMatrix);
                bounds.Encapsulate(new Vector3(bounds.center.x, bounds.center.y + 0.1f, bounds.center.z));
                bounds.Encapsulate(new Vector3(bounds.center.x, bounds.center.y - 10f, bounds.center.z)); // Extend bounds below ocean surface
                boundsList.Add(bounds);
            }
        }


        //[ContextMenu("Find Renderers")][Button]
        public void FindRenderers()
        {
            targetMeshCollider.enabled = true;
            UnderwaterMeshes(true);
            UpdateBoundsFromColliders();

            var targetSurfaceTransform = targetMeshCollider.transform;
            var tmeshRenderers = RaycastTools.FindMeshesWithoutCollider(targetSurfaceTransform, boundsList, rayDistance);

            foreach (var o in tmeshRenderers)
            {
                if (!meshColliders.Contains(o.gameObject)) meshColliders.Add(o.gameObject);
            }

            meshCollidersDepth = new List<GameObject>(meshColliders);
            UnderwaterMeshes(false);

            findCollidersLastRan = Time.realtimeSinceStartup;
            targetMeshCollider.enabled = false;
        }
#endif

        private double findCollidersLastRan, findCollidersLastRanStore;
        private double renderNowLastRan, renderNowLastRanStore;
        private static readonly int DepthCache = Shader.PropertyToID("_DepthCache");
        private static readonly int ColorCache = Shader.PropertyToID("_ColorCache");

        private static readonly int DepthMatrix = Shader.PropertyToID("_DepthMatrix");

        private static readonly int BakeMatrix = Shader.PropertyToID("_BakeMatrix");
        private static readonly int DepthOffset = Shader.PropertyToID("_DepthOffset");

        private static readonly int DepthPosition = Shader.PropertyToID("_DepthPosition");
        private static readonly int WaveDirectionSettings = Shader.PropertyToID("_WaveDirectionSettings");

        private static readonly int CubemapRotation = Shader.PropertyToID("_CubemapRotation");


#if UNITY_EDITOR
        //[ContextMenu("MakeCopies")][Button, GUIColor("@findCollidersLastRan == findCollidersLastRanStore ? Color.white : Color.red")]
        public void MakeCopies()
        {
            //if (!excludeList.Contains(targetSurfaceGO)) excludeList.Add(targetSurfaceGO);
            //if (!excludeList.Contains(targetSurfaceTransform.gameObject)) excludeList.Add(targetSurfaceTransform.gameObject);
            var targetSurfaceTransform = targetMeshCollider.transform;
            for (int j = 0; j < excludeList.Count; j++)
            {
                if (excludeList[j] == null) continue;
                for (int i = meshColliders.Count - 1; i >= 0; i--)
                {
                    if (meshColliders[i] == null) continue;
                    if (meshColliders[i].gameObject == excludeList[j]) meshColliders.RemoveAt(i);
                }

                for (int i = meshCollidersDepth.Count - 1; i >= 0; i--)
                {
                    if (meshCollidersDepth[i] == null) continue;
                    if (meshCollidersDepth[i].gameObject == excludeList[j]) meshCollidersDepth.RemoveAt(i);
                }
            }

            for (int i = meshColliders.Count - 1; i >= 0; i--)
            {
                if (meshColliders[i].gameObject == targetSurfaceTransform.gameObject) meshColliders.RemoveAt(i);
                if (oceanBakeQuad && meshColliders[i].gameObject == oceanBakeQuad.gameObject) meshColliders.RemoveAt(i);
            }

            for (int i = meshCollidersDepth.Count - 1; i >= 0; i--)
            {
                if (meshCollidersDepth[i].gameObject == targetSurfaceTransform.gameObject) meshCollidersDepth.RemoveAt(i);
                if (oceanBakeQuad && meshCollidersDepth[i].gameObject == oceanBakeQuad.gameObject) meshCollidersDepth.RemoveAt(i);
            }

            for (int i = 0; i < includeList.Count; i++)
            {
                if (includeList[i] == null) continue;
                if (!meshColliders.Contains(includeList[i])) meshColliders.Add(includeList[i]);
                if (!meshCollidersDepth.Contains(includeList[i])) meshCollidersDepth.Add(includeList[i]);
            }

            DestroyCopies();
            meshCollidersCopies = RaycastTools.Copies(ref meshColliders, hideCopies);

            UnderwaterMeshes(true);
            meshCollidersDepthCopies = RaycastTools.Copies(ref meshCollidersDepth, hideCopies);
            UnderwaterMeshes(false);

            findCollidersLastRanStore = findCollidersLastRan;

            renderNowLastRan = Time.realtimeSinceStartup;
        }
#endif

        [ContextMenu("DestroyCopies")] //[Button]
        public void DestroyCopies()
        {
            foreach (var o in meshCollidersCopies)
            {
                if (o == null) continue;
                if (Application.isPlaying)
                {
                    Destroy(o.gameObject);
                }
                else
                {
                    DestroyImmediate(o.gameObject);
                }
            }

            meshCollidersCopies.Clear();

            foreach (var o in meshCollidersDepthCopies)
            {
                if (o == null) continue;
                if (Application.isPlaying)
                {
                    Destroy(o.gameObject);
                }
                else
                {
                    DestroyImmediate(o.gameObject);
                }
            }

            meshCollidersDepthCopies.Clear();
        }

        public string RenderNowName()
        {
            var nom = "Render Now";
#if WATERPLATFORMSETTINGS
            if (platformSettings)
            {
                nom += " (" + platformSettings.currentPlatform + ")";
            }
#endif

            return nom;
        }


        [PropertySpace]
        [Button("Render Now (desktop only)")]
        [GUIColor("@SurfaceMaterialValid() ? Color.white : Color.red")]
        public void RenderNowDesktop()
        {
            captureFakeTransparency = false;
            RenderNow();
        }

        [Button("Render Now (old fast)")]
        [GUIColor("@SurfaceMaterialValid() ? Color.white : Color.red")]
        public void RenderNowOld()
        {
            captureFakeTransparency = true;
            useNewCapture = false;
            RenderNow();
        }

        [Button("@RenderNowName()")]
        [GUIColor("@SurfaceMaterialValid() ? Color.white : Color.red")]
        public void RenderNowNew()
        {
            captureFakeTransparency = true;
            useNewCapture = true;
            RenderNow();
        }

        public void RenderNow()
        {
#if UNITY_EDITOR
            Debug.Log("RaycastTexture: RenderNow");

            if (!PathValid())
            {
                Debug.LogError("Path invalid: " + path);
                return;
            }

            var storeTextureLimit = QualitySettings.masterTextureLimit;
            QualitySettings.masterTextureLimit = 0;

            Setup();
            //FindColliders();
            meshColliders.Clear();
            meshCollidersDepth.Clear();

            FindRenderers();
            MakeCopies();
            //bool disableLater = false;
            //if (!gameObject.activeSelf)
            //{
            //    disableLater = true;
            //    gameObject.SetActive(true);
            //}

            //UnderwaterMeshes(true);

            if (oceanRenderer) oceanRenderer.thunderSplineWithin = oceanSplineMask;

            if (targetMeshCollider.sharedMesh == null)
            {
                Debug.LogError("targetMeshCollider is null", this);
                return;
            }

            intersectionTexture = RenderIntersectionTexture();
            if (intersectionTexture == null) return;

            if (captureFakeTransparency)
            {
                colorDepthTexture = RenderColorDepthTexture();
                if (colorDepthTexture == null) return;
            }
            else
            {
                colorDepthTexture = null;
            }

            lastRan = Time.timeSinceLevelLoad;

            DilationDistanceField(); // Take single color intersection texture and floodfill create a distance field, output to: dilationResult

            //

            PostProcess();

            UpdateFluid();

            StoreTextures();


            //UnderwaterMeshes(false);
            //if (disableLater) gameObject.SetActive(false);

            ApplyTexture();


            renderNowLastRanStore = renderNowLastRan;

            QualitySettings.masterTextureLimit = storeTextureLimit;
#else
              ApplyTexture();
#endif
        }

#if UNITY_EDITOR

#if FLUXY
        [PropertySpace]
        [ContextMenu("UpdateFluid")]
        [Button, ShowIf("@fluxyContainer != null")]
        private void UpdateFluid()
        {
            if (fluxyContainer)
            {
                fluxyContainer.solver.desiredResolution = fluidRes;
                fluxyContainer.solver.DisposeOfFramebuffer();
                fluxyContainer.solver.Prewarm();
                fluxyContainer.solver.UpdateFramebuffer();
                fluxyContainer.solver.UpdateSolver10();
            }

            StoreTexturesFluid();
        }
#else
        private void UpdateFluid()
        {
        }
#endif

        const string suffix = ".exr";

        /// <summary>
        /// Store the textures to color arrays for restoration later
        /// </summary>
        [ContextMenu("StoreTextures")] //[Button, GUIColor("@lastRan == lastRanStore ? Color.white : Color.red")]
        public void StoreTextures()
        {
            Debug.Log("RaycastTexture: StoreTextures", this);

            StoreTexturesColorDepth();
            StoreTexturesFluid();
            //LoadTex();
            ApplyTexture();


            EditorUtility.SetDirty(this);

            lastRanStore = lastRan;

            //if (depthCacheTex is RenderTexture) depthCacheTex = ToTexture2D(depthCacheTex as RenderTexture);
            //if (colorCacheTex is RenderTexture) colorCacheTex = ToTexture2D(colorCacheTex as RenderTexture);

            //textureCache.depthCachePixels = (depthCacheTex as Texture2D).GetPixels();
            //textureCache.colorCachePixels = (colorCacheTex as Texture2D).GetPixels();
        }

        private void StoreTexturesColorDepth()
        {
            if (surfaceMaterial)
            {
                if (path == "") SetPathToMaterialFolder();
                if (fileName == "") AutoSetFileName();
            }

            var dataPath = Application.dataPath.Replace("Assets", "") + "/";
            path = path.Replace("\\", "/");


            var settingsDefault = new TextureImporterPlatformSettings
            {
                textureCompression = TextureImporterCompression.Uncompressed,
                format = TextureImporterFormat.Automatic
            };


            if (depthCacheTex)
            {
                var pathCombined = path + fileName + "-Depth" + suffix;
                Debug.Log("RaycastTexture: StoreTextures: Depth:" + path + " dp: " + dataPath + " combined: " + pathCombined);
                var bytes = depthCacheTex.EncodeToEXR(Texture2D.EXRFlags.OutputAsFloat);
                depthCacheTex = null;
                File.WriteAllBytes(dataPath + pathCombined, bytes);
                AssetDatabase.ImportAsset(pathCombined, ImportAssetOptions.ForceSynchronousImport);
                //AssetDatabase.Refresh();

                var textureImporter2 = (TextureImporter) AssetImporter.GetAtPath(pathCombined);

                textureImporter2.SetPlatformTextureSettings(settingsDefault);
                var androidOverrides = textureImporter2.GetPlatformTextureSettings("Android");
                androidOverrides.overridden = true;
                androidOverrides.format = TextureImporterFormat.RGBAHalf;
                textureImporter2.SetPlatformTextureSettings(androidOverrides);
                //var standaloneOverrides = textureImporter2.GetPlatformTextureSettings("Standalone");
                //standaloneOverrides.overridden = true;
                //standaloneOverrides.format = TextureImporterFormat.Automatic;
                //standaloneOverrides.maxTextureSize = 512;
                //textureImporter2.SetPlatformTextureSettings(standaloneOverrides);

                textureImporter2.npotScale = TextureImporterNPOTScale.None;
                //textureImporter2.npotScale = TextureImporterNPOTScale.ToLarger;
                textureImporter2.isReadable = true;
                textureImporter2.SaveAndReimport();

                //depthCacheTex = AssetDatabase.LoadAssetAtPath<Texture2D>(pathCombined);
            }

            if (colorCacheTex && captureFakeTransparency)
            {
                var pathCombined2 = path + fileName + "-Color" + suffix;
                Debug.Log("RaycastTexture: StoreTextures: Color:" + path + " dp: " + dataPath + " combined: " + pathCombined2);
                var bytes2 = colorCacheTex.EncodeToEXR(Texture2D.EXRFlags.OutputAsFloat);
                colorCacheTex = null;
                File.WriteAllBytes(dataPath + pathCombined2, bytes2);
                AssetDatabase.ImportAsset(pathCombined2, ImportAssetOptions.ForceSynchronousImport);
                //AssetDatabase.Refresh();

                var textureImporter2 = (TextureImporter) AssetImporter.GetAtPath(pathCombined2);

                textureImporter2.SetPlatformTextureSettings(settingsDefault);
                var androidOverrides = textureImporter2.GetPlatformTextureSettings("Android");
                androidOverrides.overridden = true;
                androidOverrides.format = TextureImporterFormat.RGBAHalf;
                textureImporter2.SetPlatformTextureSettings(androidOverrides);
                //var standaloneOverrides = textureImporter2.GetPlatformTextureSettings("Standalone");
                //standaloneOverrides.overridden = true;
                //standaloneOverrides.format = TextureImporterFormat.Automatic;
                //standaloneOverrides.maxTextureSize = 512;
                //textureImporter2.SetPlatformTextureSettings(standaloneOverrides);

                textureImporter2.npotScale = TextureImporterNPOTScale.None;
                textureImporter2.isReadable = true;
                textureImporter2.SaveAndReimport();

                //colorCacheTex = AssetDatabase.LoadAssetAtPath<Texture2D>(pathCombined2);
            }

            LoadTexColorDepth();

#if WATERPLATFORMSETTINGS
            if (platformSettings)
            {
                var settings = platformSettings.GetCurrentSettings();
                settings.colorTexture = colorCacheTex;
                settings.depthTexture = depthCacheTex;
            }
#endif
        }

        private void StoreTexturesFluid()
        {
            if (surfaceMaterial)
            {
                if (path == "") SetPathToMaterialFolder();
                if (fileName == "") AutoSetFileName();
            }

            var dataPath = Application.dataPath.Replace("Assets", "") + "/";
            path = path.Replace("\\", "/");


            var settingsDefault = new TextureImporterPlatformSettings
            {
                textureCompression = TextureImporterCompression.Uncompressed,
                format = TextureImporterFormat.Automatic
            };

#if FLUXY
            if (fluxyContainer && fluxyContainer.solver && fluxyContainer.solver.framebuffer != null)
            {
                if (fluxyContainer.solver.framebuffer.stateA)
                {
                    var pathCombined2 = path + fileName + "-FluidCol" + suffix;
                    var bytes2 = fluxyContainer.solver.framebuffer.stateA.ToTexture2D().EncodeToEXR(Texture2D.EXRFlags.OutputAsFloat);
                    File.WriteAllBytes(dataPath + pathCombined2, bytes2);
                    AssetDatabase.ImportAsset(pathCombined2, ImportAssetOptions.ForceSynchronousImport);
                    //AssetDatabase.Refresh();

                    var textureImporter2 = (TextureImporter) AssetImporter.GetAtPath(pathCombined2);

                    textureImporter2.SetPlatformTextureSettings(settingsDefault);
                    var androidOverrides = textureImporter2.GetPlatformTextureSettings("Android");
                    androidOverrides.overridden = true;
                    androidOverrides.format = TextureImporterFormat.RGBAHalf;
                    textureImporter2.SetPlatformTextureSettings(androidOverrides);
                    //var standaloneOverrides = textureImporter2.GetPlatformTextureSettings("Standalone");
                    //standaloneOverrides.overridden = true;
                    //standaloneOverrides.format = TextureImporterFormat.Automatic;
                    //standaloneOverrides.maxTextureSize = 512;
                    //textureImporter2.SetPlatformTextureSettings(standaloneOverrides);

                    //textureImporter2.npotScale = TextureImporterNPOTScale.None;
                    textureImporter2.isReadable = true;
                    textureImporter2.SaveAndReimport();

                    //fluidColCacheTex = AssetDatabase.LoadAssetAtPath<Texture2D>(pathCombined2);
                }

                if (fluxyContainer.solver.framebuffer.velocityA)
                {
                    var pathCombined2 = path + fileName + "-FluidVel" + suffix;
                    var bytes2 = fluxyContainer.solver.framebuffer.velocityA.ToTexture2D().EncodeToEXR(Texture2D.EXRFlags.OutputAsFloat);
                    File.WriteAllBytes(dataPath + pathCombined2, bytes2);
                    AssetDatabase.ImportAsset(pathCombined2, ImportAssetOptions.ForceSynchronousImport);
                    //AssetDatabase.Refresh();

                    var textureImporter2 = (TextureImporter) AssetImporter.GetAtPath(pathCombined2);

                    textureImporter2.SetPlatformTextureSettings(settingsDefault);
                    var androidOverrides = textureImporter2.GetPlatformTextureSettings("Android");
                    androidOverrides.overridden = true;
                    androidOverrides.format = TextureImporterFormat.RGBAHalf;
                    textureImporter2.SetPlatformTextureSettings(androidOverrides);
                    //var standaloneOverrides = textureImporter2.GetPlatformTextureSettings("Standalone");
                    //standaloneOverrides.overridden = true;
                    //standaloneOverrides.format = TextureImporterFormat.Automatic;
                    //standaloneOverrides.maxTextureSize = 512;
                    //textureImporter2.SetPlatformTextureSettings(standaloneOverrides);

                    //textureImporter2.npotScale = TextureImporterNPOTScale.None;
                    textureImporter2.isReadable = true;
                    textureImporter2.SaveAndReimport();

                    //fluidVelCacheTex = AssetDatabase.LoadAssetAtPath<Texture2D>(pathCombined2);
                }

                LoadTexFluid();

#if WATERPLATFORMSETTINGS
                if (platformSettings)
                {
                    var settings = platformSettings.GetCurrentSettings();
                    settings.fluidColorTexture = fluidColCacheTex;
                    settings.fluidVelocityTexture = fluidVelCacheTex;
                }
#endif
            }
#endif
        }

        [PropertySpace]
        [Button]
        public void LoadTex()
        {
            //Debug.Log("LoadTex");
            LoadTexColorDepth();
            if (mode == eMode.River) LoadTexFluid();
            ApplyTexture();
        }

        public void LoadTexColorDepth()
        {
            var pathCombined = path + fileName + "-Depth" + suffix;
            Debug.Log("LoadTexColorDepth: Depth:" + pathCombined);
            var depthCacheTex2 = AssetDatabase.LoadAssetAtPath<Texture2D>(pathCombined);
            if (depthCacheTex2 == null) Debug.LogError("Failed to load: " + pathCombined);
            if (depthCacheTex2 != null) depthCacheTex = depthCacheTex2;

            if (captureFakeTransparency)
            {
                var pathCombined2 = path + fileName + "-Color" + suffix;
                Debug.Log("LoadTexColorDepth: Color:" + pathCombined2);
                var colorCacheTex2 = AssetDatabase.LoadAssetAtPath<Texture2D>(pathCombined2);
                if (colorCacheTex2 == null) Debug.LogError("Failed to load: " + pathCombined2);
                if (colorCacheTex2 != null) colorCacheTex = colorCacheTex2;
            }
            else
            {
                colorCacheTex = null;
            }
        }

        public void LoadTexFluid()
        {
            var pathCombined3 = path + fileName + "-FluidCol" + suffix;
            Debug.Log("LoadTexFluid: FluidCol:" + pathCombined3);
            var fluidColCacheTex2 = AssetDatabase.LoadAssetAtPath<Texture2D>(pathCombined3);
            if (fluidColCacheTex2 == null) Debug.LogError("Failed to load: " + pathCombined3);
            if (fluidColCacheTex2 != null) fluidColCacheTex = fluidColCacheTex2;

            var pathCombined4 = path + fileName + "-FluidVel" + suffix;
            Debug.Log("LoadTexFluid: FluidVel:" + pathCombined4);
            var fluidVelCacheTex2 = AssetDatabase.LoadAssetAtPath<Texture2D>(pathCombined4);
            if (fluidVelCacheTex2 == null) Debug.LogError("Failed to load: " + pathCombined4);
            if (fluidVelCacheTex2 != null) fluidVelCacheTex = fluidVelCacheTex2;
        }


        [PropertySpace]
        [ContextMenu("Blur Sorcery"), Button("Blur Sorcery")]
        public void BlurSorceryButton()
        {
            BlurSorcery(dilationResult);
            BlurSorceryColor(dilationResult, colorDepthTexture);
            PostProcess();
            ApplyTexture();
            UpdateFluid();
        }
#endif

        private void OnDisable()
        {
            if (debugVisuals) DebugVisualsToggle();
            DestroyCopies();
            meshCollidersCopies.Clear();
            meshCollidersDepthCopies.Clear();

#if UNITY_EDITOR
            //LoadTex(); // Weird bug causes the cache textures to wipe when reloading scene, this forces load -_-
#endif
        }

#if UNITY_EDITOR


        [ContextMenu("Render Color/Depth Texture Only"), Button("Render Color/Depth Texture Only")]
        public void RenderColorDepthTextureButton()
        {
            FindRenderers();
            MakeCopies();

            colorDepthTexture = RenderColorDepthTexture();
            if (colorDepthTexture == null) return;

            PostProcess();
            Setup();
            StoreTextures();
            ApplyTexture();
            UpdateFluid();
        }


        [ContextMenu("Render Intersection Texture Only"), Button("Render Intersection Texture Only")]
        public void RenderIntersectionTextureButton()
        {
            FindRenderers();
            MakeCopies();

            intersectionTexture = RenderIntersectionTexture();
            if (intersectionTexture == null) return;

            DilationDistanceField();
            PostProcess();
            Setup();
            StoreTextures();
            ApplyTexture();
        }


        [ContextMenu("ToggleColliderCopies"), Button("@hideCopies ? \"Show Collider Copies\" : \"Hide Collider Copies\"")]
        public void ToggleColliderCopies()
        {
            hideCopies = !hideCopies;

            if (!hideCopies)
            {
                meshCollidersDebug = new List<GameObject>(meshColliders);
            }
            else
            {
                meshCollidersDebug.Clear();
            }

            foreach (var o in meshCollidersCopies)
            {
                if (o == null) continue;
                o.gameObject.hideFlags = hideCopies ? HideFlags.HideAndDontSave : HideFlags.DontSave;
            }

            foreach (var o in meshCollidersDepthCopies)
            {
                if (o == null) continue;
                o.gameObject.hideFlags = hideCopies ? HideFlags.HideAndDontSave : HideFlags.DontSave;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (oceanRenderer) oceanRenderer.OnDrawGizmosSelected();
            /*
            if (targetSurfaceTransform)
            {
                Gizmos.color = Color.cyan;
                Gizmos.matrix = targetSurfaceTransform.localToWorldMatrix;
                Gizmos.DrawWireCube(Vector3.zero, new Vector3(1, 0.01f, 1));
            }*/
            //}
            //private void OnDrawGizmos()
            //{
            Color col = Gizmos.color;


            if (targetMeshCollider)
            {
                Gizmos.color = Color.red;
                Gizmos.matrix = targetMeshCollider.transform.localToWorldMatrix;
                Gizmos.DrawWireCube(Vector3.zero, new Vector3(1, 0, 1));

                Gizmos.matrix = Matrix4x4.identity;
                RaycastTools.DrawBounds(targetMeshCollider.bounds, Color.yellow);

                //var bounds2 = targetMeshCollider.bounds.Transform(targetMeshCollider.transform.localToWorldMatrix);
                //RaycastTools.DrawBounds(bounds2,Color.magenta);
            }


            //if (debugTargetColliders)
            {
                UpdateBoundsFromColliders();
                //var mc = RaycastTools.GetTargetCollider(targetSurfaceTransform);
                //foreach (var targetCollider in targetColliders)
                foreach (var bounds in boundsList)
                {
                    //if (targetCollider == null) continue;
                    //var bounds = targetCollider.bounds;
                    RaycastTools.DrawBounds(bounds, Color.red);
                }
            }

            if (debugColliders)
            {
                int i = 0;
                foreach (var mc in meshCollidersCopies)
                {
                    if (mc == null) continue;
                    i++;
                    UnityEngine.Random.InitState(i);
                    Gizmos.color = UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1);
                    Bounds bounds = mc.bounds;
                    //Gizmos.DrawCube(bounds.center, bounds.extents * 2);
                    Gizmos.DrawWireCube(bounds.center, bounds.extents * 2);
                }
            }

            if (debugCollidersDepth)
            {
                int i = 0;
                foreach (var mc in meshCollidersDepthCopies)
                {
                    if (mc == null) continue;
                    i++;
                    UnityEngine.Random.InitState(i);
                    Gizmos.color = UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1);
                    Bounds bounds = mc.bounds;
                    //Gizmos.DrawCube(bounds.center, bounds.extents * 2);
                    Gizmos.DrawWireCube(bounds.center, bounds.extents * 2);
                }
            }

            Gizmos.color = col;
        }
#endif


        /// <summary>
        /// Distancefield: RG for XY direction towards shoreline, B for distance, A unused set to 1.0, created from single color intersection texture input
        /// </summary>
        //[ContextMenu("Dilation DistanceField"), Button]
        public void DilationDistanceField()
        {
            if (jumpFlood == null) jumpFlood = new JumpFlood();
            /*
                        if (dilationShader == null) dilationShader = Shader.Find("Hidden/ShorelineSwizzle");
                        if (dilationShader == null)
                        {
                            Debug.LogError("ShorelineSwizzle missing", this);
                            return;
                        }

                        if (dilationMat == null) dilationMat = new Material(dilationShader);
            */
            RenderTexture textureRT = RenderTexture.GetTemporary(intersectionTexture.width, intersectionTexture.height, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            textureRT.hideFlags = HideFlags.DontSave;
            textureRT.name = "DilationDistanceField temp";

            if (dilationResult == null || dilationResult.width != intersectionTexture.width)
            {
                dilationResult = new RenderTexture(intersectionTexture.width, intersectionTexture.height, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
                dilationResult.name = "dilationResult - " + dilationResult.GetHashCode();
                dilationResult.hideFlags = HideFlags.DontSave;
            }

            Graphics.Blit(intersectionTexture, textureRT);

            jumpFlood.shader = Shader.Find("Hidden/ScreenSpaceDistanceField-MeshIntersection");
            jumpFlood.InitMaterial(true);
            var result = jumpFlood.BuildDistanceField(textureRT);

            Graphics.Blit(result, dilationResult);
            result = null;

            RenderTexture.ReleaseTemporary(textureRT);
            //textureRT.Release();

            ApplyTexture();
        }

        //public Material debugMat;
        /*
                [ContextMenu("ApplyDebugMat"), Button]
                public void ApplyDebugMat()
                {
                    var renderer = targetTransform.GetComponent<Renderer>();
                    if (!renderer) return;

                    var sname = "DistanceFieldDebugSimple";
                    var debugShader = Shader.Find(sname);
                    if (debugShader == null)
                    {
                        Debug.LogError("Cant find shader: " + sname);
                        return;
                    }

                   //debugMat = new Material(debugShader);
                   //debugMat.name = "DebugMat - " + debugMat.GetHashCode();
                   //renderer.sharedMaterial = debugMat;

                    ApplyTexture();
                }
        */

        private void Update()
        {
            if (!isActiveAndEnabled) return;


#if MIRRORSANDREFLECTIONS
            if (mirrorRenderer)
            {
                if (planarReflection && !mirrorRenderer.transform.parent.gameObject.activeSelf) SetPlanarReflectionOff();
                if (!planarReflection && mirrorRenderer.transform.parent.gameObject.activeSelf) SetPlanarReflectionOn();
            }
            else
            {
                if (planarReflection) SetPlanarReflectionOff();
            }
#endif
            bool willApplyTex = false;

            if (waveCenter && waveCenter.transform.worldToLocalMatrix != waveCenterMatrix)
            {
                waveCenterMatrix = waveCenter.transform.worldToLocalMatrix; // If waveCenter changed then update things
                willApplyTex = true;
            }

            if (applyEveryFrame || lastMatrix != transform.localToWorldMatrix)
            {
                lastMatrix = transform.localToWorldMatrix; // If rotated make sure we update things
                willApplyTex = true;
            }

            if (willApplyTex) ApplyTexture();
        }

        public bool applyEveryFrame = false; // TODO remove

        //public bool inverseMatrix = false;

        public void ApplyTex(Material matIn)
        {
            if (mode == eMode.River)
            {
#if FLUXY
                if (fluxyContainer && fluxyContainer.isActiveAndEnabled && fluxyContainer.solver && fluxyContainer.solver.isActiveAndEnabled && fluxyContainer.solver.framebuffer != null && fluxyContainer.solver.framebuffer.stateA)
                {
                    matIn.SetTexture(fluidColorTexName, fluxyContainer.solver.framebuffer.stateA);
                    matIn.SetTexture(fluidVelocityTexName, fluxyContainer.solver.framebuffer.velocityA);
                }
                else
#endif

                {
                    // Fluidsim might be disabled or stored baked to a texture
                    if (fluidColCacheTex) matIn.SetTexture(fluidColorTexName, fluidColCacheTex);
                    if (fluidVelCacheTex) matIn.SetTexture(fluidVelocityTexName, fluidVelCacheTex);
                }
            }

            if (depthCacheTex) matIn.SetTexture(DepthCache, depthCacheTex);
            if (colorCacheTex) matIn.SetTexture(ColorCache, colorCacheTex);

            matIn.SetMatrix(DepthMatrix, targetMeshCollider.transform.worldToLocalMatrix);
            matIn.SetFloat(DepthOffset, 0); // transform.position.y
            matIn.SetVector(DepthPosition, targetMeshCollider.transform.position);

            if (waveCenter) matIn.SetVector(WaveDirectionSettings, new Vector4(waveCenter.transform.position.x, waveCenter.transform.position.z, waveCenter.transform.localScale.y, 0));

            //matIn.SetFloat(CubemapRotation, rotateCubemaps);

            if (mode == eMode.Ocean)
            {
#if UNITY_EDITOR
                if (oceanBakeQuad)
                {
                    var mr = oceanBakeQuad;
                    var lightmapSettings = LightmapSettings.lightmaps;
                    if (mr.lightmapIndex >= 0 && lightmapSettings != null && mr.lightmapIndex < lightmapSettings.Length)
                    {
                        oceanbakeLM = lightmapSettings[mr.lightmapIndex].lightmapColor;
                        oceanbakeLMSM = lightmapSettings[mr.lightmapIndex].shadowMask;
                    }
                    else
                    {
                        oceanbakeLM = null;
                        oceanbakeLMSM = null;
                    }

                    oceanBakeST = mr.lightmapScaleOffset;
                    oceanBakeMatrix = mr.transform.worldToLocalMatrix;
                }
#endif

                if (oceanbakeLM) matIn.SetTexture("_bakeLightmap", oceanbakeLM);
                if (oceanbakeLMSM) matIn.SetTexture("_bakeLightmapSM", oceanbakeLMSM);
                if (oceanbakeLM) matIn.SetVector("_bakeLightmapST", oceanBakeST);
                if (oceanbakeLM) matIn.SetMatrix(BakeMatrix, oceanBakeMatrix);

                /*
                if (oceanbakeLM) matIn.SetTexture("unity_Lightmap", oceanbakeLM);
                if (oceanbakeLMSM) matIn.SetTexture("unity_ShadowMask", oceanbakeLMSM);
                if (oceanbakeLM) matIn.SetVector("unity_LightmapST", oceanBakeST);
                if (oceanbakeLM)
                {
                    matIn.EnableKeyword("LIGHTMAP_ON");
                }
                else
                {
                    matIn.DisableKeyword("LIGHTMAP_ON");
                }*/
            }
        }

        /*
        /// <summary>
        ///  Values is wrapped to stay within 0-360
        /// Sets rotation of the cubemap for reflection on surface and the cubemap you see faking a grabpass from under surface.
        /// </summary>
        public void SetCubemapRotation(float valIn)
        {
            rotateCubemaps = valIn;
            ApplyTexture();
        }
        */

        public void ApplyTex(MaterialPropertyBlock matIn)
        {
            if (materialPropertyBlock == null) materialPropertyBlock = new MaterialPropertyBlock();
            //
#if FLUXY
            if (fluxyContainer && fluxyContainer.isActiveAndEnabled && fluxyContainer.solver && fluxyContainer.solver.isActiveAndEnabled && fluxyContainer.solver.framebuffer != null && fluxyContainer.solver.framebuffer.stateA)
            {
                matIn.SetTexture(fluidColorTexName, fluxyContainer.solver.framebuffer.stateA);
                matIn.SetTexture(fluidVelocityTexName, fluxyContainer.solver.framebuffer.velocityA);
            }
            else
#endif
            {
                // Fluidsim might be disabled or stored baked to a texture
                if (fluidColCacheTex) matIn.SetTexture(fluidColorTexName, fluidColCacheTex);
                if (fluidVelCacheTex) matIn.SetTexture(fluidVelocityTexName, fluidVelCacheTex);
            }

            if (depthCacheTex) matIn.SetTexture(DepthCache, depthCacheTex);
            if (colorCacheTex) matIn.SetTexture(ColorCache, colorCacheTex);
            matIn.SetMatrix(DepthMatrix, targetMeshCollider.transform.worldToLocalMatrix);
            matIn.SetFloat(DepthOffset, 0); // transform.position.y
            matIn.SetVector(DepthPosition, targetMeshCollider.transform.position);

            if (waveCenter) matIn.SetVector(WaveDirectionSettings, new Vector4(waveCenter.transform.position.x, waveCenter.transform.position.z, waveCenter.transform.localScale.y, 0));

            //rotateCubemaps = rotateCubemaps.Wrap(360); // Wrap around value
            //if(globalSkyCube)globalSkyCube.SetCubemapRotation(rotateCubemaps);
            //matIn.SetFloat(CubemapRotation, rotateCubemaps);

            if (mode == eMode.Ocean)
            {
#if UNITY_EDITOR
                if (oceanBakeQuad)
                {
                    var mr = oceanBakeQuad;
                    var lightmapSettings = LightmapSettings.lightmaps;
                    if (mr.lightmapIndex >= 0 && lightmapSettings != null && mr.lightmapIndex < lightmapSettings.Length)
                    {
                        oceanbakeLM = lightmapSettings[mr.lightmapIndex].lightmapColor;
                        oceanbakeLMSM = lightmapSettings[mr.lightmapIndex].shadowMask;
                    }
                    else
                    {
                        oceanbakeLM = null;
                        oceanbakeLMSM = null;
                    }

                    oceanBakeST = mr.lightmapScaleOffset;
                    oceanBakeMatrix = mr.transform.worldToLocalMatrix;
                }
#endif

                //
                if (oceanbakeLM) matIn.SetTexture("_bakeLightmap", oceanbakeLM);
                if (oceanbakeLMSM) matIn.SetTexture("_bakeLightmapSM", oceanbakeLMSM);
                if (oceanbakeLM) matIn.SetVector("_bakeLightmapST", oceanBakeST);
                if (oceanbakeLM) matIn.SetMatrix(BakeMatrix, oceanBakeMatrix);

                /*
                if (oceanbakeLM) matIn.SetTexture("unity_Lightmap", oceanbakeLM);
                if (oceanbakeLMSM) matIn.SetTexture("unity_ShadowMask", oceanbakeLMSM);
                if (oceanbakeLM) matIn.SetVector("unity_LightmapST", oceanBakeST);
   
                if (oceanbakeLM)
                {
                    surfaceMaterial.EnableKeyword("LIGHTMAP_ON");
                }
                else
                {
                    surfaceMaterial.DisableKeyword("LIGHTMAP_ON");
                }*/
            }
        }

        // Ran on Update
        [ContextMenu("ApplyTexture")]
        [Button]
        public void ApplyTexture()
        {
            if (!targetMeshCollider) return;
            if (!surfaceMaterial) return;

            switch (mode)
            {
                case eMode.Ocean:
                    transform.SetLocalScaleY(1);
                    break;
                case eMode.River:
                    break;
            }

            SetCubemapOnMaterial();

            if (oceanRenderer) oceanRenderer.SetMaterial(surfaceMaterial);
            if (riverMesh) riverMesh.sharedMaterial = surfaceMaterial;

            if (useMatProps)
            {
                ApplyTex(materialPropertyBlock);
                if (oceanRenderer) oceanRenderer.SetMatProp(materialPropertyBlock);
                if (riverMesh) riverMesh.SetPropertyBlock(materialPropertyBlock);
            }
            else
            {
                ApplyTex(surfaceMaterial);
                if (oceanRenderer) oceanRenderer.SetMatProp(null);
                if (riverMesh) riverMesh.SetPropertyBlock(null);
            }
        }
    }
}


#if UNITY_EDITOR

namespace UnityEditor
{
    [CustomEditor(typeof(RaycastTexture)), CanEditMultipleObjects]
    public class RaycastTextureEditor : OdinEditor
    {
        private MaterialEditor materialEditor;

        private bool checkMaterials;
        private bool checkSprites;

#if CREST
        private bool checkMasks;
        public List<ClipSurface> masksToFix = new List<ClipSurface>();
#endif
        private List<Renderer> renderers = new List<Renderer>();
        //string[] guids;

        private List<ValueTuple<string, int, string>> matToFix = new List<(string, int, string)>();

        private List<SpriteRenderer> spritesToFIx = new List<SpriteRenderer>();
        //private int count;
        /*
                private int Check(bool dryrun, IEnumerable<Renderer> rend)
                {
                    int count = 0;
                    var guids = AssetDatabase.FindAssets("t:shader " + AssetSorceryShader.ASSETSORCERY_FILE_EXTENSION);
                    foreach (var guid in guids)
                    {
                        var path = AssetDatabase.GUIDToAssetPath(guid);
                        //Debug.Log("Path: " + path);
                        //var asset = AssetDatabase.LoadAssetAtPath<AssetSorceryShaderAsset>(path);
                        //var ass = AssetSorceryShader.GetAtPath(path);

                        count += AssetSorceryShaderEditor.ReplaceShaderScene2(path, dryrun, rend);
                    }



                    return count;
                }

                private int CheckSprites()
                {
                    int count = 0;
                    var sprites = FindObjectsOfType<SpriteRenderer>();
                    foreach (var sprite in sprites)
                    {
                        if (sprite.material.name.Contains("Default with Fog"))
                        {
                            //spritesNeedUpdating = true;
                            count++;
                        }
                    }

                    return count;
                }*/

        //[PropertySpace]
        //[GUIColor("@FixWaterShadowProxyPresent() ? Color.red : Color.white")] //
        //[Button]
        void FixWaterShadowProxy()
        {
            var go = GameObject.Find("WaterShadowProxy_Day");
            if (!go || !go.activeInHierarchy) return;
            go.SetActive(false);
        }

        bool FixWaterShadowProxyPresent()
        {
            var go = GameObject.Find("WaterShadowProxy_Day");
            if (!go || !go.activeInHierarchy) return false;
            return true;
        }


        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var targ = target as RaycastTexture;

            if (!targ.surfaceMaterial) return;

#if !ODIN_INSPECTOR
            GUI.color = targ.SurfaceMaterialValid() ? Color.white : Color.red;
            if (GUILayout.Button(targ.RenderNowName()))
            {
                targ.RenderNow();
            }
            GUI.color = Color.white;
#endif

            GUILayout.Space(10);

            var items = Enum.GetValues(typeof(AssetSorceryPlatformRuntime.ePlatformAS));
            foreach (var item in items)
            {
                if (GUILayout.Button("Set Platform Ocean: " + item.ToString()))
                {
                    //var asset = AssetSorceryShader.ReadFromPath(path);
                    var path = AssetDatabase.GetAssetPath(targ.surfaceMaterial.shader);
                    AssetSorceryPlatform.AssetSorceryShaderSetPlatformLocal((AssetSorceryPlatformRuntime.ePlatformAS) item, path);
                }
            }

            GUILayout.Space(10);

            foreach (var item in items)
            {
                if (GUILayout.Button("Set Platform ALL: " + item.ToString()))
                {
                    AssetSorceryPlatform.AssetSorceryShaderSetPlatform((AssetSorceryPlatformRuntime.ePlatformAS) item);
                }
            }

            GUILayout.Space(10);

#if DYNAMICBLITPASS
            if (!DynamicBlitPass.CheckPass())
            {
                GUI.color = Color.red;
                if (GUILayout.Button("Add DynamicBlitPass")) DynamicBlitPass.AddPass();
                GUI.color = Color.white;
            }
            else
            {
                if (GUILayout.Button("Remove DynamicBlitPass")) DynamicBlitPass.RemovePass();
            }
#endif

            GUILayout.Space(10);

            if (FixWaterShadowProxyPresent()) GUI.color = Color.red;
            if (GUILayout.Button("Fix Water Shadow Proxy")) FixWaterShadowProxy();
            GUI.color = Color.white;
            GUILayout.Space(10);

#if CREST
            if (!checkMasks || GUILayout.Button("Check Masks"))
            {
                masksToFix.Clear();
                var found = FindObjectsOfType<ClipSurface>();
                foreach (var o in found)
                {
                    if (!o.isActiveAndEnabled) continue;
                    masksToFix.Add(o);
                }

                checkMasks = true;
            }

            if (masksToFix.Count > 0)
            {
                foreach (var o in masksToFix)
                {
                    GUILayout.BeginHorizontal();
                    GUI.color = Color.red;
                    if (GUILayout.Button("Fix: " + o.name))
                    {
                        var waterDepthMaskPath = UnityEditor.AssetDatabase.FindAssets("t:material WaterDepthMask").Select(UnityEditor.AssetDatabase.GUIDToAssetPath).First();
                        var waterDepthMask = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>(waterDepthMaskPath);
                        if (!waterDepthMask)
                        {
                            Debug.LogError("Cant find waterDepthMask material");
                            return;
                        }

                        o.enabled = false;
                        var rend = o.GetComponent<MeshRenderer>();
                        if (!rend) continue;
                        rend.enabled = true;
                        if (rend.sharedMaterial.name == "ClipSurfaceConvexHull")
                        {
                            rend.sharedMaterial = waterDepthMask;
                        }

                        checkMasks = false;
                    }

                    GUI.color = Color.white;
                    if (GUILayout.Button("Show"))
                    {
                        Selection.activeObject = o;
                        SceneView.lastActiveSceneView.FrameSelected();
                    }

                    GUILayout.EndHorizontal();
                }
            }


            GUILayout.Space(10);
#endif

            if (!checkSprites || GUILayout.Button("Check Sprites"))
            {
                spritesToFIx.Clear();
                var sprites = FindObjectsOfType<SpriteRenderer>();
                foreach (var sprite in sprites)
                {
                    //Debug.Log("Srpite:"+ sprite.name +":"+sprite.material.name, sprite);
                    if (sprite.sharedMaterial && sprite.sharedMaterial.shader.name.Contains("Default with Fog"))
                    {
                        if (!spritesToFIx.Contains(sprite)) spritesToFIx.Add(sprite);
                    }
                }

                checkSprites = true;
            }

            foreach (var o in spritesToFIx)
            {
                GUI.color = Color.red;
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Fix: " + o.name))
                {
                    if (RaycastTexture.TryFindAsset<Material>("t:material FogSpriteSkybox", RaycastTexture.FOLDER_PATH, out Material mat))
                    {
                        o.sharedMaterial = mat;
                        checkSprites = false;
                    }
                    else Debug.LogError("Cant find FogSpriteSkybox material");
                }

                GUI.color = Color.white;
                if (GUILayout.Button("Show"))
                {
                    //SceneView.lastActiveSceneView.Frame(o.bounds, false);
                    Selection.activeObject = o;
                    SceneView.lastActiveSceneView.FrameSelected();
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.Space(10);

            if (!checkMaterials || GUILayout.Button("Check Materials"))
            {
                renderers = FindObjectsOfType<Renderer>().ToList();
                //count = Check(true, renderers);
                var guids = AssetDatabase.FindAssets("t:shader " + AssetSorceryShader.ASSETSORCERY_FILE_EXTENSION);
                matToFix.Clear();
                foreach (var guid in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var itemData = AssetSorceryShader.ReadFromPath(path);
                    var count = AssetSorceryShaderEditor.ReplaceShaderScene2(path, true, renderers);
                    if (count > 0) matToFix.Add((itemData.shaderSettings.customShaderName, count, path));
                }

                checkMaterials = true;
            }

            foreach (var o in matToFix)
            {
                GUI.color = Color.red;
                //GUILayout.BeginHorizontal();
                if (GUILayout.Button("Fix: " + o.Item1 + " (" + o.Item2 + ")"))
                {
                    AssetSorceryShaderEditor.ReplaceShaderScene2(o.Item3, false, renderers);
                    checkMaterials = false;
                }

                //GUILayout.EndHorizontal();
                GUI.color = Color.white;
            }


            //if (count > 0)
            {
                //if (GUILayout.Button("Fix Materials ( " + count + ")"))
                //{
                //    Check(false, renderers);
                //    count = Check(true, renderers);
                //}
            }

            GUILayout.Space(10);

            if (GUILayout.Button(targ.showMaterialInspector ? "Hide Material Inspector" : "Show Material Inspector"))
            {
                targ.showMaterialInspector = !targ.showMaterialInspector;
            }

            if (targ.showMaterialInspector) ExtensionsMisc.DrawMaterialEditor(targ.surfaceMaterial, serializedObject, ref materialEditor);
        }

        protected override void OnDisable()
        {
            if (materialEditor != null) DestroyImmediate(materialEditor); // Free the memory used by default MaterialEditor.
        }
    }
}


#endif
