using UnityEditor;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class EditorTools
{
    [MenuItem("Blade and Sorcery/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/AssetBundles";
        DirectoryInfo dir = new DirectoryInfo(assetBundleDirectory);
        if (!Directory.Exists(assetBundleDirectory)) Directory.CreateDirectory(assetBundleDirectory);

        foreach (FileInfo file in dir.GetFiles("*.*"))
        {
            file.Delete();
        }

        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

        List<string> bundleNames = new List<string>(AssetDatabase.GetAllAssetBundleNames());

        foreach (FileInfo file in dir.GetFiles("*.*"))
        {
            if (bundleNames.Contains(file.Name))
            {
                file.MoveTo(file.FullName + ".bundle");
            }
            else
            {
                file.Delete();
            }
        }
        AssetDatabase.Refresh();
        Debug.Log("Asset bundles created in " + dir.FullName);
    }
}