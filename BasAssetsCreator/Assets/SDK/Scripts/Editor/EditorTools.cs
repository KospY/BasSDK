using UnityEditor;
using System.IO;
using System;
using UnityEngine;
using System.Collections.Generic;
public class EditorTools : EditorWindow
{
    public Dictionary<string, bool> exportBundle = new Dictionary<string, bool>();
    List<string> bundleNames;
    List<AssetBundleBuild> assetBundleBuilds;
    [MenuItem("Blade and Sorcery/Asset Builder")] 
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<EditorTools>("B&S Asset Builder");
    }
    
    private void OnFocus()  
    {
        bundleNames = new List<string>(AssetDatabase.GetAllAssetBundleNames());
        foreach (string bundle in bundleNames)
        {
            try {
                exportBundle[bundle] = EditorPrefs.GetBool("ExportBundle" + bundle);
                } catch (Exception) 
            {
                exportBundle[bundle] = true;
                EditorPrefs.SetBool("ExportBundle" + bundle, true);
            }
        }
    }
    Vector3 scroll0;
    private void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label(new GUIContent("  Select the following assets to export"), new GUIStyle("BoldLabel"));
        GUILayout.Space(10);

        scroll0 = GUILayout.BeginScrollView(scroll0);
        foreach (string bundle in bundleNames)
        {
            exportBundle[bundle] = GUILayout.Toggle(exportBundle[bundle], bundle);
        }
        GUILayout.EndScrollView();

        GUILayout.Space(10);

        Handles.BeginGUI();
        Handles.color = Color.black;
        Handles.DrawLine(new Vector3(0, 36), new Vector3(10000, 36));
        Handles.EndGUI();

        if (GUILayout.Button("Build Asset Bundles"))
        {
            BuildAllAssetBundles();
        }

    }

    private void OnDisable()
    {
        foreach (string bundle in bundleNames)
        {
            try { 
            EditorPrefs.SetBool("ExportBundle" + bundle, exportBundle[bundle]);
            } catch (Exception)
            {
                EditorPrefs.SetBool("ExportBundle" + bundle, true);
            }
        }
    }

    private void BuildAllAssetBundles()
    {
        assetBundleBuilds = new List<AssetBundleBuild>();
        string assetBundleDirectory = "Assets/AssetBundles";
        DirectoryInfo dir = new DirectoryInfo(assetBundleDirectory);
        if (!Directory.Exists(assetBundleDirectory)) Directory.CreateDirectory(assetBundleDirectory);
        
        foreach (string bundle in bundleNames)
        {
            foreach (FileInfo file in dir.GetFiles("*.*"))
            {
                if (file.Extension != ".bundle" || exportBundle[bundle])
                {
                    file.Delete();
                }
            }
            if (exportBundle[bundle]) { 
            AssetBundleBuild build = new AssetBundleBuild
            {
                assetBundleName = bundle,
                assetBundleVariant = AssetImporter.GetAtPath(assetBundleDirectory).assetBundleVariant,
                assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(bundle)
            };
                assetBundleBuilds.Add(build);
            }
            try
            {
                EditorPrefs.SetBool("ExportBundle" + bundle, exportBundle[bundle]);
            }
            catch (Exception)
            {
                EditorPrefs.SetBool("ExportBundle" + bundle, true);
            }
        }

        BuildPipeline.BuildAssetBundles(assetBundleDirectory, assetBundleBuilds.ToArray(), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

        bundleNames = new List<string>(AssetDatabase.GetAllAssetBundleNames());

        foreach (FileInfo file in dir.GetFiles("*.*"))
        {
            if (bundleNames.Contains(file.Name))
            {
                file.MoveTo(file.FullName + ".bundle");
            }
            else if (file.Extension != ".bundle")
            {
                file.Delete();
            }
        }

        AssetDatabase.Refresh();
        if (assetBundleBuilds.ToArray().Length > 0) {
            string bundleNames = "";
            string s = "";
            foreach (AssetBundleBuild build in assetBundleBuilds)
            {
                if (assetBundleBuilds[0].assetBundleName == build.assetBundleName)
                {
                    bundleNames = " "+build.assetBundleName;
                } else if (assetBundleBuilds[assetBundleBuilds.Count -1].assetBundleName == build.assetBundleName) {
                    s = "s";
                    bundleNames = bundleNames + " and " + build.assetBundleName;
                }
                else {
                    s = "s";
                    bundleNames = bundleNames + ", " + build.assetBundleName;
                }
            }
        Debug.Log("Created Asset Bundle"+ s + bundleNames + " in " + dir.FullName);
        }
    }
}
