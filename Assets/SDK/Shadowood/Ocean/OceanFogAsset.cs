using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

[InlineEditor]
#endif
[CreateAssetMenu(fileName = "OceanFogAsset", menuName = "ThunderRoad/Ocean/OceanFogAsset")]
public class OceanFogAsset : ScriptableObject
{
    public Settings settings = new Settings();

#if UNITY_EDITOR
    internal int hash;
    private int hashLast;

    //[Button]
    private void ForceSave()
    {
        hashLast = hash = settings.Hash();
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }

    public void OnValidate()
    {
        hash = settings.Hash();
        if (hashLast != hash)
        {
            hashLast = hash;
            EditorUtility.SetDirty(this);
        }
    }


    public static OceanFogAsset CreateAsset(string path)
    {
        OceanFogAsset asset = ScriptableObject.CreateInstance<OceanFogAsset>();
        asset.settings = Settings.kDefaultSettings;

        if (string.IsNullOrEmpty(path))
        {
            path = "Assets/OceanFogAsset.asset";
            path = AssetDatabase.GenerateUniqueAssetPath(path);
        }

        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;

        asset.ForceSave();

        return asset;
    }

    public static OceanFogAsset CreateAsset(Settings settingsIn, string path)
    {
        var asset = CreateAsset(path);
        asset.settings = settingsIn;
        asset.ForceSave();
        return asset;
    }
#endif
}

[Serializable]
public struct Settings
{
    public Color _topColor;
    public Color _bottomColor;
    public Color _underWaterTint;

    public float _exponent;

    //[Range(-1.0f, 1.0f)] public float _horizon;
    [Range(0.0f, 1.0f)] public float _intensity;

    //[Range(0.00001f, 1.0f)] public float _fogDensity;
    
    
    [Tooltip("As camera travels deeper in Y under the water it can darken, this controls that density")]
    [Range(0.00001f, 2.0f)]
    public float _fogDensity2;


    //public float _fogDepthDensity;

    [Range(0.00001f, 2.0f)]
    public float _oceanFogDensity;

    [Tooltip("Offset the start of the fog, minus will pull it forwards making it murkier, plus values will push it out clearing a fog free area around the camera")]
    public float _fogOffset;
    //public Vector4 DepthFadeDOLO;

    public static readonly Settings kDefaultSettings = new Settings()
    {
        _topColor = new Color(0.1195265f, 0.2801172f, 0.3207547f),
        _bottomColor = new Color(0, 0.1423433f, 0.2078431f),
        _exponent = 8.0f,
        //_horizon = -0.827f,
        _intensity = 0.7f,
        //_fogDensity = 0.2f,
        _fogDensity2 = 0.1f,
        //DepthFadeDOLO = new Vector4(0.01f, 1f, 222f, 0.01f),
        _underWaterTint = new Color(0.3843137f, 0.5380536f, 0.5960785f, 1),
        _oceanFogDensity = 0.1f,
        //_fogDepthDensity = 0.1f,
        _fogOffset = 0.0f
    };

    public int Hash()
    {
        return (((float) _topColor.GetHashCode() + (float) _bottomColor.GetHashCode() + _exponent + _intensity + _fogDensity2 + (float) _underWaterTint.GetHashCode() + _oceanFogDensity + _oceanFogDensity + _fogOffset) * 12345.6).GetHashCode();
    }
}

/*
#if UNITY_EDITOR

namespace UnityEditor
{
    [CustomEditor(typeof(OceanFogAsset)), CanEditMultipleObjects]
    public class OceanFogAssetEditor : OdinEditor
    {
        SerializedProperty prop;

        void OnEnable()
        {
            prop = serializedObject.FindProperty("settings");
        }

        public override void OnInspectorGUI()    
        {
            //DrawDefaultInspector();

            var targ = target as OceanFogAsset;


            EditorGUILayout.PropertyField(prop, new GUIContent("Settings Field"), true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}


#endif
*/
