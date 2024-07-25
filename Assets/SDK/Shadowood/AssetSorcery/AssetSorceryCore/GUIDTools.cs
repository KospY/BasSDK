using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ThunderRoad.AssetSorcery
{
    /// <summary>
    /// https://github.com/jeffjadulco/unity-guid-regenerator/blob/60c99435cc23adfc734beb770ed677d6c7fad14c/Editor/AssetGUIDRegenerator.cs#L62
    /// </summary>
    public static class GUIDTools
    {
        /*
          public static string SetGUID(string guidTarget, string guidReplaceWith, long originalFileID=0, long replaceFileID=0)
          {
              AssetDatabase.StartAssetEditing();
              var guid = ReplaceGUID(guidTarget, guidReplaceWith, originalFileID, replaceFileID);
              AssetDatabase.StopAssetEditing();
              AssetDatabase.SaveAssets();
              AssetDatabase.Refresh();
              return guid;
          }
  
          public static string SetGUID(Object asset, string guidReplaceWith, long originalFileID=0, long replaceFileID=0)
          {
              AssetDatabase.StartAssetEditing();
              var guid = ReplaceShaderGUID(GetGUID(asset), guidReplaceWith, originalFileID, replaceFileID);
              AssetDatabase.StopAssetEditing();
              AssetDatabase.SaveAssets();
              AssetDatabase.Refresh();
              return guid;
          }*/
/*
        public static string GetGUID(string path)
        {
            return AssetDatabase.AssetPathToGUID(path, AssetPathToGUIDOptions.OnlyExistingAssets);
        }

        public static string GetGUID(Object asset)
        {
            var path = AssetDatabase.GetAssetPath(asset);
            var guid = GetGUID(path);
            Debug.Log("GetGUID: " + path +" : " + guid);
            return guid;
        }*/


        public static void ReplaceShaderGUID(Shader oldAsset, Shader newAsset)
        {
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier<Shader>(oldAsset, guid: out var originalGUID, localId: out var originalFileID);
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier<Shader>(newAsset, guid: out var replaceGUID, localId: out var replaceFileID);

            Debug.Log("ReplaceShaderGUID: Replace: " + originalGUID + " : " + originalFileID + " -- " + replaceGUID + " : " + replaceFileID);

            string rootfolder = Application.dataPath;

            var allowedExtensions = new[] {".mat", ".asset", ".prefab"};
            var files = Directory
                .GetFiles(rootfolder, "*.*", SearchOption.AllDirectories)
                .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
                .ToList();

            Debug.Log("ReplaceShaderGUID: Files to search: " + files.Count);
            foreach (string file in files)
            {
                try
                {
                    string contents = File.ReadAllText(file);
                    var needle = $"fileID: {originalFileID}, guid: {originalGUID}"; // note 'type: 3}' can be on a new line and have various whitespace before it
                    var replace = $"fileID: {replaceFileID}, guid: {replaceGUID}";
                    var found = contents.Contains(needle);
                    if (found)
                    {
                        Debug.Log("ReplaceShaderGUID: Found: " + found + " : " + file);
                        contents = contents.Replace(needle, replace);
                        // Make files writable
                        File.SetAttributes(file, FileAttributes.Normal);
                        File.WriteAllText(file, contents);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }

/*
        private static string ReplaceGUIDDontUse(string orignalGUID, string replaceGUID, long originalFileID, long replaceFileID)
        {
            var path = AssetDatabase.GUIDToAssetPath(orignalGUID);
            var metaPath = AssetDatabase.GetTextMetaFilePathFromAssetPath(path);
            
            if (string.IsNullOrEmpty(replaceGUID)) replaceGUID = GUID.Generate().ToString();
            
            Debug.Log("ReplaceGUID: " + orignalGUID +":"+replaceGUID+":"+path+":"+metaPath);

            try
            {
                if (!File.Exists(metaPath)) throw new FileNotFoundException($"The meta file of selected asset cannot be found. Asset: {orignalGUID + " : " + path}");

                var metaContents = File.ReadAllText(metaPath);

                // Check if guid in .meta file matches the guid of selected asset
                if (!metaContents.Contains(orignalGUID)) throw new ArgumentException($"The GUID of [{path}] does not match the GUID in its meta file.");


                if (originalFileID != 0 && !metaContents.Contains(originalFileID.ToString())) throw new ArgumentException($"The FileID [{originalFileID}] cannot be found in its meta file.");


                // if (assetPath.EndsWith(".unity")) return;

                var metaAttributes = File.GetAttributes(metaPath);
                var bIsInitiallyHidden = false;

                if (metaAttributes.HasFlag(FileAttributes.Hidden))
                {
                    bIsInitiallyHidden = true;
                    HideFile(metaPath, metaAttributes);
                }

                metaContents = metaContents.Replace(orignalGUID, replaceGUID);
                if (originalFileID != 0 && replaceFileID != 0) metaContents = metaContents.Replace(originalFileID.ToString(), replaceFileID.ToString());

                File.WriteAllText(metaPath, metaContents);

                if (bIsInitiallyHidden) UnhideFile(metaPath, metaAttributes);
            }
            catch (Exception e)
            {
                Debug.LogError("RegenerateGUIDs: " + e);
            }
            

            return replaceGUID;
        }

        private static void HideFile(string path, FileAttributes attributes)
        {
            attributes &= ~FileAttributes.Hidden;
            File.SetAttributes(path, attributes);
        }

        private static void UnhideFile(string path, FileAttributes attributes)
        {
            attributes |= FileAttributes.Hidden;
            File.SetAttributes(path, attributes);
        }*/
    }
}
