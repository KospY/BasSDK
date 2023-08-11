using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine;
public class TRLitCleaner : EditorWindow
{
    private Shader targetShader;
    private Vector2 scrollPosition;
    private List<Material> foundMaterials;
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");
    private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");


    [MenuItem("ThunderRoad (SDK)/TRLitCleaner")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TRLitCleaner));
    }

    private void OnGUI()
    {
        GUILayout.Label("Clean TRLit Materials", EditorStyles.boldLabel);
        targetShader = Shader.Find("ThunderRoad/Lit");
        if (GUILayout.Button("Search Materials"))
        {
            SearchMaterials();
        }
        if (GUILayout.Button("Clean Found Materials Of Old References"))
        {
            CleanMaterials();
        }
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Found Materials:");

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        if (foundMaterials != null)
        {
            foreach (Material material in foundMaterials)
            {
                // Display the material name as a clickable label
                if (GUILayout.Button(material.name, EditorStyles.toolbarButton))
                {
                    Selection.activeObject = material;
                    EditorGUIUtility.PingObject(material);
                }
                // Display the mainTex preview
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("_MainTex");
                if (material.HasTexture(MainTex))
                {
                    Texture texture = material.GetTexture(MainTex);
                    if (GUILayout.Button(EditorGUIUtility.ObjectContent(texture, typeof(Texture2D)).image, new []{GUILayout.Width(50), GUILayout.Height(50)}))
                    {
                        Selection.activeObject = texture;
                        EditorGUIUtility.PingObject(texture);
                    }
                }
                else
                {
                    GUILayout.Box("", new []{GUILayout.Width(50), GUILayout.Height(50)});
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("_BaseMap");
                if (material.HasTexture(BaseMap))
                {
                    Texture texture = material.GetTexture(BaseMap);
                    if (GUILayout.Button(EditorGUIUtility.ObjectContent(texture, typeof(Texture2D)).image, new []{GUILayout.Width(50), GUILayout.Height(50)}))
                    {
                        Selection.activeObject = texture;
                        EditorGUIUtility.PingObject(texture);
                    }
                }
                else
                {
                    GUILayout.Box("", new []{GUILayout.Width(50), GUILayout.Height(50)});
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.EndScrollView();
    }

    private void SearchMaterials()
    {
        if (targetShader == null)
        {
            Debug.LogError("Could not find ThunderRoad/Lit");
            return;
        }
        foundMaterials ??= new List<Material>();
        foundMaterials.Clear();
        string[] allMaterials = AssetDatabase.FindAssets("t:Material");

        foreach (string materialGUID in allMaterials)
        {
            string materialPath = AssetDatabase.GUIDToAssetPath(materialGUID);
            Material material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);

            if (material.shader == targetShader)
            {
                foundMaterials.Add(material);
            }
        }
    }
    
    private void CleanMaterials()
    {
        float progress = 0f;
        EditorUtility.DisplayProgressBar("Processing TRLit Materials", "Processing...", progress);
        var count = foundMaterials.Count;
        float i = 0;
        foreach (var material in foundMaterials)
        {
            var path = AssetDatabase.GetAssetPath(material);
            if (material.HasTexture(MainTex))
            {
                Debug.Log($"Removing mainTex reference from {path}");
                EditorUtility.DisplayProgressBar($"Removing mainTex references..", $"{material.name}", i / count);
                material.SetTexture(MainTex, null);
                EditorUtility.SetDirty(material);
                AssetDatabase.ImportAsset(path);
            }
            i++;
        }
        EditorUtility.ClearProgressBar();
        SearchMaterials();
    }
}
