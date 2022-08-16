using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ThunderRoad
{
    public static class EditorCommon
    {
        public static void ShowCheckbox(ref bool variable, string title, string editorPrefName, string toolTip = "")
        {
            bool newVariable = GUILayout.Toggle(variable, new GUIContent(title, toolTip));
            if (newVariable != variable)
            {
                EditorPrefs.SetBool(editorPrefName, newVariable);
                variable = newVariable;
            }
        }

        public static void ShowCheckbox(ref bool variable, string title, string editorPrefName, float width, string toolTip = "")
        {
            bool newVariable = GUILayout.Toggle(variable, new GUIContent(title, toolTip), GUILayout.Width(width));
            if (newVariable != variable)
            {
                EditorPrefs.SetBool(editorPrefName, newVariable);
                variable = newVariable;
            }
        }

        private delegate void GetWidthAndHeight(TextureImporter importer, ref int width, ref int height);
        private static GetWidthAndHeight getWidthAndHeightDelegate;

        public static T[] GetAllInstances<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
            T[] a = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }
            return a;

        }

        public struct Size
        {
            public int width;
            public int height;
        }

        public static Size GetOriginalTextureSize(this Texture2D texture, bool includePowerOfTwo)
        {
            if (texture == null)
                throw new NullReferenceException();

            var path = AssetDatabase.GetAssetPath(texture);
            if (string.IsNullOrEmpty(path))
                throw new Exception("Texture2D is not an asset texture.");

            var importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null)
                throw new Exception("Failed to get Texture importer for " + path);

            return GetOriginalTextureSize(importer, includePowerOfTwo);
        }

        public static Size GetOriginalTextureSize(this TextureImporter importer, bool includePowerOfTwo)
        {
            if (getWidthAndHeightDelegate == null)
            {
                var method = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
                getWidthAndHeightDelegate = Delegate.CreateDelegate(typeof(GetWidthAndHeight), null, method) as GetWidthAndHeight;
            }

            var size = new Size();
            getWidthAndHeightDelegate(importer, ref size.width, ref size.height);

            if (includePowerOfTwo)
            {
                if (importer.npotScale == TextureImporterNPOTScale.ToNearest)
                {
                    size.width = Mathf.ClosestPowerOfTwo(size.width);
                    size.height = Mathf.ClosestPowerOfTwo(size.height);
                }
                else if (importer.npotScale == TextureImporterNPOTScale.ToLarger)
                {
                    size.width = Mathf.NextPowerOfTwo(size.width);
                    size.height = Mathf.NextPowerOfTwo(size.height);
                }
                else if (importer.npotScale == TextureImporterNPOTScale.ToSmaller)
                {
                    // TODO
                }
            }

            return size;
        }

        public static T[] GetAllProjectAssets<T>() where T : UnityEngine.Object
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
            T[] a = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }
            return a;
        }
    }
}