using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;

#endif

[ExecuteInEditMode, SelectionBase, RequireComponent(typeof(ReflectionSorceryTransformWatcher))]
public class ReflectionSorcery : MonoBehaviour
{
    private const string _REFLECTION_SORCERY_SHADERNAME = "ReflectionSorceryShader";
    private const string _REFLECTION_SORCERY_RV_SHADERNAME = "ReflectionSorceryRVShader";
    private const string _CAPTURE_NAME = "captureName";
    private const string DEBUG_KEYWORD = "_DEBUGVIEW_ON";
    private const string DEBUG_KEYWORD_BOOL = "_DebugView";

    private static readonly int Fade = Shader.PropertyToID("_Fade");

    //

    [SerializeField] private string captureName = _CAPTURE_NAME;

    [SerializeField] private string folderLocation = "";

    [SerializeField] private Texture captureTexture;
    private Texture captureTextureLast;

    [SerializeField, HideInInspector] private Material renderMaterial;

    [SerializeField, HideInInspector] private ReflectionSorceryTransformWatcher roomVolume;

    [SerializeField, HideInInspector] public ReflectionProbe captureProbe;

    [Space(10)]
    [Range(0, 1)] public float fade = 1;

    [Space(10)]
    [SerializeField] private List<PassRotationMatrix> rsRenderers = new List<PassRotationMatrix>();

    [SerializeField, HideInInspector] private Matrix4x4 capturedMatrix;

    //[SerializeField] 
    private bool showGameObjects = false;
    //private bool showGameObjectsLast = true;

    private Matrix4x4 cubeMatrix = Matrix4x4.identity;

    public void SetFade(float valIn)
    {
        fade = valIn;
        UpdateMaterials();
    }

    private void UpdateMaterials()
    {
        foreach (var passRotationMatrix in rsRenderers)
        {
            passRotationMatrix.matBlock.SetFloat(Fade, fade);
        }
    }

#if UNITY_EDITOR
    [SerializeField] private bool drawGizmosAlways = false;
    private Mesh gizmoMesh;

    private void Awake()
    {
        gizmoMesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
    }

    private void OnDrawGizmos()
    {
        if (drawGizmosAlways) DrawGizmos();
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawGizmosAlways) DrawGizmos();
    }

    private void DrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.TransformPoint(capturePosition), 0.1f);
        Gizmos.matrix = cubeMatrix;
        //Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        Gizmos.DrawWireMesh(gizmoMesh, Vector3.zero);
    }

    public void SetCaptureName(string folderLocation, string captureName)
    {
        this.folderLocation = folderLocation;
        this.captureName = captureName;
    }

    public string FolderLocation
    {
        get { return folderLocation; }
    }

    public string CaptureName
    {
        get { return captureName; }
    }

    public Matrix4x4 CapturedMatrix
    {
        get { return capturedMatrix; }
    }
#endif

    //TODO draw when children selected?
    //TODO draw cris cross wireframe or grid
    //TODO toggle to set room floor exactly
    //TODO make Capture button red if capture needs redoing

    // Public members
    [Space(10)] [Header("Settings")] public int resolution = 512;
    public bool renderDynamicObjects = true;
    public LayerMask mask = ~0;

    [Space(10)]
    public Vector3 capturePosition = new Vector3(0, 1.5f, 0);

    public Vector3 roomVolumePosition = new Vector3(0, 3f, 6f);
    public Vector3 roomVolumeRotation;
    public Vector3 roomVolumeScale = new Vector3(6, 6, 12);


    //

    //TODO check reset works
    [ContextMenu("FactoryReset")]
    public void FactoryReset()
    {
        DestroyRenderers();
        DestroyGameObjects();
        renderMaterial = null;
        captureTexture = null;
        Setup();
    }

    private void Reset()
    {
        Setup();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!isActiveAndEnabled) return;
        if (Application.isEditor) gameObject.name = "ReflectionSorcery-" + captureName;
        capturedMatrix = transform.localToWorldMatrix;

        ValuesChanged();
        CopySettingsToGameObjects();
        UpdateMaterials();
    }

    public bool CleanCapturetexture()
    {
        bool changed = captureTexture != null;
        captureTexture = null;
        return changed;
    }
#endif

    private void OnEnable()
    {
#if UNITY_EDITOR
        SetDebugState(false);
#endif
        showGameObjects = false;
        HandleGameObjects();
    }

    private void OnDisable()
    {
        showGameObjects = false;
        DestroyGameObjects();
    }

    private void OnDestroy()
    {
        foreach (var rs in rsRenderers)
        {
            if (Application.isPlaying)
            {
                if (rs != null) Destroy(rs.gameObject);
            }
            else
            {
                if (rs != null) DestroyImmediate(rs.gameObject);
            }
        }
    }

    public void HandleGameObjects()
    {
        if (Application.isPlaying) return;
        if (!showGameObjects)
        {
            DestroyGameObjects();
        }
        else
        {
            CopySettingsToGameObjects();
            //CreateCaptureProbe();
            //CreateGameObjects();
        }
    }

    [ContextMenu("CopySettingsToGameObjects")]
    void CopySettingsToGameObjects()
    {
        if (showGameObjects)
        {
            CreateCaptureProbe();
            CreateRoomVolume();
        }

        //SetupRoomSurfaces();
    }

    //public void CreateGameObjects()
    //{
    //    
    //    CreateCaptureProbe();
    //}

    public void DestroyRenderers()
    {
        foreach (var rs in rsRenderers)
        {
            if (Application.isPlaying)
            {
                if (rs != null) Destroy(rs.gameObject);
            }
            else
            {
                if (rs != null) DestroyImmediate(rs.gameObject);
            }
        }

        rsRenderers.Clear();
    }

    public void DestroyGameObjects()
    {
        if (Application.isPlaying)
        {
            if (captureProbe != null) Destroy(captureProbe.gameObject);
            if (roomVolume != null) Destroy(roomVolume.gameObject);
        }
        else
        {
            if (captureProbe != null) DestroyImmediate(captureProbe.gameObject);
            if (roomVolume != null) DestroyImmediate(roomVolume.gameObject);
        }

        //UpdateRenderers();
        foreach (var rs in rsRenderers)
        {
            //rs.hideFlags = HideFlags.HideInHierarchy;
        }

        captureProbe = null;
        roomVolume = null;
    }

    void CopyGameObjectsToSettings()
    {
        if (showGameObjects && captureProbe != null)
        {
            capturePosition = captureProbe.transform.localPosition;
            //
            roomVolumePosition = roomVolume.transform.localPosition;
            roomVolumeRotation = roomVolume.transform.localEulerAngles;
            roomVolumeScale = roomVolume.transform.localScale;
        }
    }


    [ContextMenu("CreateRoomVolume")]
    public void CreateRoomVolume()
    {
        if (roomVolume == null) // ass
        {
            var results = transform.GetComponentsInChildren<ReflectionSorceryTransformWatcher>();
            foreach (var o in results)
            {
                if (o.name.StartsWith("RoomVolume"))
                {
                    var co = o.GetComponent<ReflectionSorceryTransformWatcher>();
                    if (co != null)
                    {
                        roomVolume = co;
                        break;
                    }
                }
            }

            if (roomVolume != null)
            {
                Debug.Log("Found existing RoomVolume and added it: " + roomVolume);
                CopyGameObjectsToSettings();
                UpdateSurfaceRenderers();
            }

            if (roomVolume == null) roomVolume = new GameObject("RoomVolume", typeof(ReflectionSorceryTransformWatcher), typeof(MeshFilter)).GetComponent<ReflectionSorceryTransformWatcher>();
            roomVolume.transform.parent = transform;
        }

        roomVolume.transform.localScale = roomVolumeScale;
        roomVolume.transform.localPosition = roomVolumePosition;
        roomVolume.transform.localEulerAngles = roomVolumeRotation;

        var mr = roomVolume.GetComponent<MeshRenderer>();
        if (mr == null) mr = roomVolume.gameObject.AddComponent<MeshRenderer>();
        if (mr.sharedMaterial == null || mr.sharedMaterial.name != "RSRVMat")
        {
            var rsrvshader = Shader.Find(_REFLECTION_SORCERY_RV_SHADERNAME);
            mr.sharedMaterial = new Material(rsrvshader) {name = "RSRVMat"};
        }
        else
        {
            if (mr.sharedMaterial.shader.name != _REFLECTION_SORCERY_RV_SHADERNAME) mr.sharedMaterial.shader = Shader.Find(_REFLECTION_SORCERY_RV_SHADERNAME);
        }

        mr.hideFlags = HideFlags.None;
        mr.gameObject.hideFlags = HideFlags.None;

        var mf = roomVolume.GetComponent<MeshFilter>(); // Added to enable Unity rect/resizing tool to operate
        if (mf == null) mf = roomVolume.gameObject.AddComponent<MeshFilter>();
        if (mf.sharedMesh == null) mf.sharedMesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
    }

    [ContextMenu("Setup")]
    public void Setup()
    {
        rsRenderers.RemoveAll(item => item == null);
        var items = transform.GetComponentsInChildren<PassRotationMatrix>();
        foreach (var item in items)
        {
            bool added = false;
            if (!rsRenderers.Contains(item))
            {
                rsRenderers.Add(item);
                added = true;
            }

            if (added && rsRenderers.Count == 1) CopyGameObjectsToSettings();
        }

        if (rsRenderers == null || rsRenderers.Count == 0) CreateNewRenderSurface();
        UpdateSurfaceRenderers();
    }

    public void CreateCaptureProbe()
    {
        if (captureProbe == null)
        {
            captureProbe = new GameObject("CaptureProbe", typeof(ReflectionProbe), typeof(ReflectionSorceryTransformWatcher)).GetComponent<ReflectionProbe>();
            captureProbe.transform.parent = transform;
        }

        captureProbe.transform.localScale = Vector3.one;
        captureProbe.transform.localPosition = capturePosition;
        captureProbe.transform.localRotation = Quaternion.identity;
        captureProbe.renderDynamicObjects = renderDynamicObjects;
        captureProbe.mode = ReflectionProbeMode.Custom;
        captureProbe.enabled = false;
        captureProbe.resolution = resolution;
        captureProbe.hideFlags = HideFlags.DontSave;
        captureProbe.cullingMask = mask;
    }

    [ContextMenu("CreateNewRenderSurface")]
    public void CreateNewRenderSurface()
    {
        var renderVolume = new GameObject("RenderSurface", typeof(ReflectionSorceryTransformWatcher), typeof(PassRotationMatrix), typeof(MeshRenderer), typeof(MeshFilter));

        renderVolume.transform.parent = transform;
        renderVolume.transform.localScale = new Vector3(3, 3, 0);
        renderVolume.transform.localPosition = new Vector3(0, 1.5f, 0);
        renderVolume.transform.localRotation = Quaternion.identity;

        if (rsRenderers == null) rsRenderers = new List<PassRotationMatrix>();
        var rs = renderVolume.GetComponent<PassRotationMatrix>();
        rs.setPosition = true;
        rs.setInversion = true;
        rs.setSize = true;
        rs.setCubePosition = true;
        rs.type = PassRotationMatrix.eType.portal;
        rs.autoRun = true;
        rsRenderers.Add(rs);

        var mf = renderVolume.GetComponent<MeshFilter>();
        mf.sharedMesh = Resources.GetBuiltinResource<Mesh>("Quad.fbx");

        UpdateSurfaceRenderers();
    }


    public bool DebugState()
    {
        return renderMaterial.IsKeywordEnabled(DEBUG_KEYWORD);
    }

#if UNITY_EDITOR

    public void ToggleDebugShader()
    {
        SetDebugState(!DebugState());
    }


    public void SetDebugState(bool valIn)
    {
        if (renderMaterial == null)
        {
            Shader renderShader = Shader.Find(_REFLECTION_SORCERY_SHADERNAME);
            renderMaterial = new Material(renderShader) { name = _REFLECTION_SORCERY_SHADERNAME + "-Material" };
        }


        if (valIn)
        {
            renderMaterial.SetKeyword(new LocalKeyword(renderMaterial.shader, DEBUG_KEYWORD), true);
            renderMaterial.SetFloat(DEBUG_KEYWORD_BOOL, 1);
        }
        else
        {
            renderMaterial.DisableKeyword(new LocalKeyword(renderMaterial.shader, DEBUG_KEYWORD));
            renderMaterial.SetFloat(DEBUG_KEYWORD_BOOL, 0);
        }

        //Debug.Log("SetDebugState: " + valIn);
        SceneView.RepaintAll();
    }

    public void ToggleEditMode()
    {
        showGameObjects = !showGameObjects;
        HandleGameObjects();
        CopySettingsToGameObjects();
        UpdateSurfaceRenderers();
    }


    [ContextMenu("SetCubeSettings")]
    public void SetCubeSettings(string path)
    {
        TextureImporter textureImporter = (TextureImporter) AssetImporter.GetAtPath(path);
        TextureImporterSettings settings = new TextureImporterSettings();
        textureImporter.ReadTextureSettings(settings);
        settings.cubemapConvolution = TextureImporterCubemapConvolution.None;
        textureImporter.SetTextureSettings(settings);
        AssetDatabase.ImportAsset(path);
    }

    public string GetPath(bool prompt = false)
    {
        var suffix = "exr";

        var path = Path.Combine(folderLocation, captureName + "." + suffix);
        if (prompt) path = EditorUtility.SaveFilePanelInProject("Save reflection probe's cubemap.", captureName, suffix, "", folderLocation);

        return string.IsNullOrEmpty(path) ? null : path;
    }

    private string savePath;

    [ContextMenu("Capture")]
    public void Capture(bool prompt = false)
    {
        Setup();

        RSRenderersSetEnabled(false);
        var showGameObjectsMemory = showGameObjects;
        showGameObjects = false;
        HandleGameObjects();

        CreateCaptureProbe();

        savePath = GetPath(prompt);

        if (captureName == _CAPTURE_NAME)
        {
            // ReflectionProbe.RenderProbe() does not function under SRP, no obvious way to capture to an in memory texture or renderTexture.
            Debug.LogWarning("ReflectionSorcery: Set the 'captureName' to be able to save the captured cubemap." + captureName + ":" + _CAPTURE_NAME, this);
        }
        else
        {
            var success = false;
            try
            {
                success = Lightmapping.BakeReflectionProbe(captureProbe, savePath);
                if (!success) throw new Exception("Lightmapping.BakeReflectionProbe returned false");
                if (!System.IO.File.Exists(savePath)) throw new Exception("BakeReflectionProbe: File does not exist: " + savePath);
            }
            catch (Exception e)
            {
                Debug.LogError("BakeReflectionProbe: Failed: " + savePath + " error: " + e.Message);
                return;
            }


            Debug.Log("Testing: " + success);

            captureTexture = AssetDatabase.LoadAssetAtPath<Texture>(savePath);
            SetCubeSettings(savePath);
            renderMaterial.SetTexture("_Cubemap", captureTexture);
        }

        RSRenderersSetEnabled(true);

        capturedMatrix = transform.localToWorldMatrix;

        showGameObjects = showGameObjectsMemory;

        UpdateSurfaceRenderers();
        HandleGameObjects();
    }
#endif

    private void RSRenderersSetEnabled(bool enabledIn)
    {
        foreach (var rsRenderer in rsRenderers)
        {
            if (rsRenderer == null) continue;
            var mr = rsRenderer.GetComponent<MeshRenderer>();
            if (mr) mr.enabled = enabledIn;
        }
    }

    [ContextMenu("UpdateSurfaceRenderers")]
    public void UpdateSurfaceRenderers()
    {
        if (renderMaterial == null)
        {
            Shader renderShader = Shader.Find(_REFLECTION_SORCERY_SHADERNAME);
            renderMaterial = new Material(renderShader) {name = _REFLECTION_SORCERY_SHADERNAME + "-Material"};
        }

        renderMaterial.SetTexture("_Cubemap", captureTexture);

        int i = 0;
        foreach (var rs in rsRenderers)
        {
            i++;
            //captureProbe.transform.localScale = Vector3.one;

            if (rs == null) continue;
            //if (captureTexture == null)

            rs.name = "RenderSurface";

            if (rsRenderers.Count > 1)
            {
                rs.name += "-" + i;
            }

            rs.enabled = true;
            rs.gameObject.hideFlags = HideFlags.None;

            rs.Setup();

            var mr = rs.GetComponent<MeshRenderer>();
            mr.sharedMaterial = renderMaterial;
            mr.shadowCastingMode = ShadowCastingMode.Off;
            mr.receiveShadows = false;
            mr.lightProbeUsage = LightProbeUsage.Off;
            mr.reflectionProbeUsage = ReflectionProbeUsage.Off;
            mr.enabled = true;

            if (captureTexture != null) rs.matBlock.SetTexture("_Cubemap", captureTexture);
            rs.scale = roomVolumeScale;
            rs.position = (transform.localToWorldMatrix * Matrix4x4.TRS(roomVolumePosition, Quaternion.Euler(roomVolumeRotation), roomVolumeScale)).MultiplyPoint(Vector3.zero); // TODO cache
            rs.cubeposition = capturePosition - roomVolumePosition;
            rs.capturedMatrix = (transform.worldToLocalMatrix * capturedMatrix).inverse;
            rs.parentMatrix = transform.localToWorldMatrix;
            rs.cubeRotMatrix = transform.localToWorldMatrix * (Matrix4x4.TRS(roomVolumePosition, Quaternion.Euler(roomVolumeRotation), Vector3.one)); // TODO cache
            rs.enabled = true;

            rs.Run();
        }
    }

    public void SetCaptureTexture(Texture captureTexture, Matrix4x4 capturedMatrix)
    {
        this.captureTexture = captureTexture;
        this.capturedMatrix = capturedMatrix;
        captureTextureLast = captureTexture;
        ValuesChanged();
        CopyGameObjectsToSettings();
    }

    void Update()
    {
        bool changed = false;

        if (captureTexture != captureTextureLast)
        {
            captureTextureLast = captureTexture;
            changed = true;
        }

        if (transform.hasChanged)
        {
            transform.hasChanged = false;
            changed = true;
        }

        if (roomVolume != null && captureProbe != null)
        {
            if (roomVolume.transform.hasChanged || captureProbe.transform.hasChanged)
            {
                roomVolume.transform.hasChanged = false;
                captureProbe.transform.hasChanged = false;
                changed = true;
            }
        }

        if (changed)
        {
            ValuesChanged();
            CopyGameObjectsToSettings();
        }

        HandleGameObjects();
    }

    [ContextMenu("ValuesChanged")]
    private void ValuesChanged()
    {
        cubeMatrix = transform.localToWorldMatrix * Matrix4x4.TRS(roomVolumePosition, Quaternion.Euler(roomVolumeRotation), roomVolumeScale);
        UpdateSurfaceRenderers();
    }
}

#if UNITY_EDITOR
namespace UnityEditor
{
    [CustomEditor(typeof(ReflectionSorcery))]
    public class ReflectionSorceryEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(10);

            var obj = target as ReflectionSorcery;

            if (GUILayout.Button("Capture"))
            {
                obj.Capture(false);
            }

            if (GUILayout.Button("Capture - Dialog"))
            {
                obj.Capture(true);
            }

            if (GUILayout.Button("Setup"))
            {
                obj.Setup();
            }

            if (GUILayout.Button("ToggleDebugShader: " + (obj.DebugState() ? "Off" : "On")))
            {
                obj.ToggleDebugShader();
            }

            if (GUILayout.Button("ToggleEditMode"))
            {
                obj.ToggleEditMode();
            }
        }
    }
}
#endif
