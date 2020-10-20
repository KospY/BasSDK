using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace ThunderRoad
{
    public class FileManager
    {
        public static string defaultFolderName = "Default";
        public static string modFolderName = "Mods";
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

        public static string obbPath
        {
            get { return ("jar:file:///storage/emulated/0/Android/obb/" + Application.identifier); }
        }

        public static string obbPathEnd
        {
            get { return ".1." + Application.identifier + ".obb!/"; }
        }

        ///////////////////////////////////////////////////////////

        public static string[] GetObbFilePaths()
        {
            return Directory.GetFiles(obbPath, "*.obb", SearchOption.TopDirectoryOnly);
        }

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
                            return Path.Combine(Application.dataPath.Replace("/Assets", ""), "BuildStaging/AddressableAssets/Android", relativePath);
                        }
#endif
                        return Path.Combine(Application.dataPath.Replace("/Assets", ""), "BuildStaging/AddressableAssets/Windows", relativePath);
                    }
                    else if (type == Type.JSONCatalog)
                    {
                        string path = Path.Combine(Application.dataPath.Replace("/Assets", ""), "BuildStaging/Catalog", relativePath);
                        return path;
                    }
                }
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                if (source == Source.Default)
                {
                    return Path.Combine(Application.streamingAssetsPath, defaultFolderName, relativePath);
                }
                else if (source == Source.Mods)
                {
                    return Path.Combine(Application.persistentDataPath, modFolderName, relativePath);
                }
            }
            else
            {
                if (source == Source.Default)
                {
                    return Path.Combine(Application.streamingAssetsPath, defaultFolderName, relativePath);
                }
                else if (source == Source.Mods)
                {
                    return Path.Combine(Application.streamingAssetsPath, modFolderName, relativePath);
                }
            }
            return null;
        }

        public static string[] GetFolderNames(Type type, Source source, string localPath = "")
        {
            if (Application.platform == RuntimePlatform.Android && source == Source.Default)
            {
                Debug.LogError("Listing folders in APK/OBB is not supported!");
                return null;
            }
            else
            {
                string fullPath = GetFullPath(type, source, localPath);
                return Directory.GetDirectories(fullPath).Select(Path.GetFileName).ToArray();
            }
        }

        public static List<string> GetValidModJsons(string searchPattern = "*.*")
        {
            List<string> validJsons = new List<string>();
            string[] modFolders = GetFolderNames(Type.JSONCatalog, Source.Mods);
            foreach (string modFolder in modFolders)
            {
                if (Catalog.loadModFolders.Count > 0)
                {
                    if (!Catalog.loadModFolders.Contains(modFolder.ToLower())) continue;
                }
                else if (modFolder.StartsWith("_") || (!FileExist(Type.JSONCatalog, Source.Mods, modFolder + "/manifest.json")))
                {
                    continue;
                }
                string[] files = GetFilePaths(Type.JSONCatalog, Source.Mods, modFolder, searchPattern);
                foreach (string modJson in files)
                {
                    validJsons.Add(modJson);
                }
            }
            return validJsons;
        }


        public static ReadFile[] ReadFiles(Type type, Source source, string localPath = "", string searchPattern = "*.*")
        {
            string folderPath = GetFullPath(type, source, localPath);

            if (folderPath.StartsWith("jar:"))
            {
                // Looking into a jar file
                if (source == Source.Mods)
                {
                    Debug.LogError("Listing jar files outside of default folders is not supported!");
                    return null;
                }
                if (BetterStreamingAssets.Root == null) BetterStreamingAssets.Initialize();
                string typeSourceLocalPath = Path.Combine(defaultFolderName, localPath);
                List<ReadFile> readFiles = new List<ReadFile>();
                foreach (string path in BetterStreamingAssets.GetFiles(typeSourceLocalPath, searchPattern, SearchOption.AllDirectories))
                {
                    readFiles.Add(new ReadFile(BetterStreamingAssets.ReadAllText(path), path));
                }
                return readFiles.ToArray();
            }
            else
            {
                List<ReadFile> readFiles = new List<ReadFile>();
                if (Directory.Exists(folderPath))
                {
                    foreach (string filePath in Directory.GetFiles(folderPath, searchPattern, SearchOption.AllDirectories))
                    {
                        readFiles.Add(new ReadFile(File.ReadAllText(filePath), filePath));
                    }
                }
                return readFiles.ToArray();
            }
        }

        public static string[] GetFilePaths(Type type, Source source, string localPath = "", string searchPattern = "*.*")
        {
            if (Application.platform == RuntimePlatform.Android && source == Source.Default)
            {
                if (BetterStreamingAssets.Root == null) BetterStreamingAssets.Initialize();
                string typeSourceLocalPath = Path.Combine(source == Source.Default ? defaultFolderName : modFolderName, localPath);
                List<string> filePaths = new List<string>();
                foreach (string path in BetterStreamingAssets.GetFiles(typeSourceLocalPath, searchPattern, SearchOption.AllDirectories))
                {
                    string relativePath = path.Substring(path.IndexOf('/') + 1);
                    filePaths.Add(relativePath);
                }
                return filePaths.ToArray();
            }
            else
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
            if (Application.platform == RuntimePlatform.Android && source == Source.Default)
            {
                if (BetterStreamingAssets.Root == null) BetterStreamingAssets.Initialize();
                string typeSourceLocalPath = Path.Combine(source == Source.Default ? defaultFolderName : modFolderName, localPath);
                List<string> filePaths = new List<string>();
                foreach (string path in BetterStreamingAssets.GetFiles(typeSourceLocalPath, searchPattern, SearchOption.AllDirectories))
                {
                    filePaths.Add(GetStreamingAssetFullPath(path));
                }
                return filePaths.ToArray();
            }
            else
            {
                string fullPath = GetFullPath(type, source, localPath);
                return Directory.GetFiles(fullPath, searchPattern, SearchOption.AllDirectories);
            }
        }

        protected static string GetStreamingAssetFullPath(string localPath)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                //jar:file:///data/app/com.Warpfrog.BladeAndSorcery-1/base.apk!/assets/aa/Android
                return ("jar:file://" + Application.dataPath + "!/assets/" + localPath);
            }
            else
            {
                return (BetterStreamingAssets.Root + "/" + localPath);
            }
        }

        public static bool FileExist(Type type, Source source, string localPath)
        {
            if (Application.platform == RuntimePlatform.Android && source == Source.Default)
            {
                if (BetterStreamingAssets.Root == null) BetterStreamingAssets.Initialize();
                string typeSourceLocalPath = Path.Combine(source == Source.Default ? defaultFolderName : modFolderName, localPath);
                return BetterStreamingAssets.FileExists(typeSourceLocalPath);
            }
            else
            {
                return File.Exists(GetFullPath(type, source, localPath));
            }
        }

        public static string ReadAllText(Type type, Source source, string localPath)
        {
            if (Application.platform == RuntimePlatform.Android && source == Source.Default)
            {
                if (BetterStreamingAssets.Root == null) BetterStreamingAssets.Initialize();
                string typeSourceLocalPath = Path.Combine(source == Source.Default ? defaultFolderName : modFolderName, localPath);
                return BetterStreamingAssets.ReadAllText(typeSourceLocalPath);
            }
            else
            {
                return File.ReadAllText(GetFullPath(type, source, localPath));
            }
        }

    }
}