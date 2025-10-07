﻿using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace ThunderRoad
{
    public class FileManager
    {
        public static string defaultFolderName = "Default";
        public static string modsFolderName = "Mods";
        public static string logFolderName = "Logs";
        public static bool useObb = false;

        public class ReadFile
        {
            public string text;
            public string path;
            public ReadFile(string text, string path)
            {
                this.text = text;
                this.path = path;
            }
        }

        public enum Source
        {
            Default,
            Mods,
            Logs
        }

        public enum Type
        {
            AddressableAssets,
            JSONCatalog,
        }

        // Mod builder use this paths for addressable (LocalLoadPath)

        public static string aaDefaultPath
        {
            get { return GetFullPath(Type.AddressableAssets, Source.Default); }
        }

        public static string aaModPath
        {
            get { return GetFullPath(Type.AddressableAssets, Source.Mods); }
        }

        ///////////////////////////////////////////////////////////

        public static string GetFullPath(Type type, Source source, string relativePath = "")
        {
            if (Application.isEditor)
            {
                if (source == Source.Default || source == Source.Mods)
                {
                    if (type == Type.AddressableAssets)
                    {
#if UNITY_EDITOR
                        if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
                        {
                            string path = Path.Combine(Application.dataPath.Replace("/Assets", ""), Path.Combine(ThunderRoadSettings.current.addressableEditorPath, "Android"));
                            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                            return Path.Combine(path, relativePath);
                        }
                        else
                        {
                            string path = Path.Combine(Application.dataPath.Replace("/Assets", ""), Path.Combine(ThunderRoadSettings.current.addressableEditorPath, "Windows"));
                            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                            return Path.Combine(path, relativePath);
                        }
#endif                  
                    }
                    else if (type == Type.JSONCatalog)
                    {
                        string path = Path.Combine(Application.dataPath.Replace("/Assets", ""), ThunderRoadSettings.current.catalogsEditorPath);
                        if (source == Source.Default) path = Path.Combine(path, defaultFolderName);
                        else if (source == Source.Mods) path = Path.Combine(path, ModManager.modFolderName);
                        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                        return Path.Combine(path, relativePath);
                    }
                }
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                if (source == Source.Default)
                {
                    return Path.Combine("/sdcard/Android/obb/" + Application.identifier, relativePath);
                }
                else if (source == Source.Mods)
                {
                    return Path.Combine(Application.persistentDataPath, ModManager.modFolderName, relativePath);
                }
                else if (source == Source.Logs)
                {
                    return Path.Combine(Application.persistentDataPath, logFolderName, relativePath);
                }
            }
            else
            {
                if (source == Source.Default)
                {
                    string pathFolder = Path.Combine(Application.streamingAssetsPath, defaultFolderName);
                    string path = Path.Combine(pathFolder, relativePath);
                    if (!Directory.Exists(path)) Directory.CreateDirectory(pathFolder);
                    return path;
                }
                else if (source == Source.Mods)
                {
                    if (Application.platform == RuntimePlatform.PS5)
                    {
                        return Path.Combine(Application.persistentDataPath, ModManager.modFolderName, relativePath);
                    }
                    else
                    {
                        string pathFolder = Path.Combine(Application.streamingAssetsPath, ModManager.modFolderName);
                        string path = Path.Combine(pathFolder, relativePath);
                        if (!Directory.Exists(path)) Directory.CreateDirectory(pathFolder);
                        return path;
                    }
                }
                else if (source == Source.Logs)
                {
                    if (Application.platform == RuntimePlatform.PS5)
                    {
                        return Path.Combine(Application.persistentDataPath, logFolderName, relativePath);
                    }
                    else
                    {
                        string pathFolder = Path.Combine(Application.streamingAssetsPath, logFolderName);
                        string path = Path.Combine(pathFolder, relativePath);
                        if (!Directory.Exists(path)) Directory.CreateDirectory(pathFolder);
                        return path;
                    }
                }
            }
            return null;
        }

        public static string[] GetFolderNames(Type type, Source source, string localPath = "")
        {
            string fullPath = GetFullPath(type, source, localPath);
            return Directory.GetDirectories(fullPath).Select(Path.GetFileName).ToArray();
        }

        public static ReadFile[] ReadFiles(Type type, Source source, string localPath = "", string searchPattern = "*.*")
        {
            string folderPath = GetFullPath(type, source, localPath);
            if (!Directory.Exists(folderPath)) return Array.Empty<ReadFile>();
            
            string[] filePaths = Directory.GetFiles(folderPath, searchPattern, SearchOption.AllDirectories);
            ReadFile[] readFiles = new ReadFile[filePaths.Length];
            for (var i = 0; i < filePaths.Length; i++)
            {
                var filePath = filePaths[i];
                readFiles[i] = new ReadFile(File.ReadAllText(filePath), filePath);
            }
            return readFiles;
        }

        public static string[] GetFilePaths(Type type, Source source, string localPath = "", string searchPattern = "*.*")
        {
            List<string> relativePaths = new List<string>();
            string fullPath = GetFullPath(type, source, localPath);
            foreach (string filePath in Directory.GetFiles(fullPath, searchPattern, SearchOption.AllDirectories))
            {
                string relativePath = GetRelativePath(fullPath, filePath);
                relativePaths.Add(relativePath);
            }
            return relativePaths.ToArray();
        }

        public static string GetRelativePath(string relativeTo, string path)
        {
            var uri = new Uri(relativeTo);
            var rel = Uri.UnescapeDataString(uri.MakeRelativeUri(new Uri(path)).ToString()).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            if (rel.Contains(Path.DirectorySeparatorChar.ToString()) == false)
            {
                rel = $".{ Path.DirectorySeparatorChar }{ rel }";
            }
            return rel;
        }

        public static string[] GetFullFilePaths(Type type, Source source, string localPath = "", string searchPattern = "*.*")
        {
            string fullPath = GetFullPath(type, source, localPath);
            try
            {
                return Directory.GetFiles(fullPath, searchPattern, SearchOption.AllDirectories);
                
            }
            catch(DirectoryNotFoundException e)
            {
                Debug.LogWarning($"Directory Not Found: {e}");
            }
            return new string[0];
        }

        public static bool FileExist(Type type, Source source, string localPath)
        {
            return File.Exists(GetFullPath(type, source, localPath));
        }

        public static string ReadAllText(Type type, Source source, string localPath)
        {
            return File.ReadAllText(GetFullPath(type, source, localPath));
        }

    }
}