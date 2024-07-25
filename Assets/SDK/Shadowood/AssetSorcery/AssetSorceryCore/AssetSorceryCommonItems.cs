
//#define ENABLE_UNITYVERSION // Enable filtering by Unity version

using System.Collections.Generic;

namespace ThunderRoad.AssetSorcery
{
    public static class AssetSorceryCommonItems
    {
        public static IEnumerable<T> FindAssetsByType<T>() where T : UnityEngine.Object
        {
            var guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            foreach (var guid in guids)
            {
                var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null) yield return asset;
            }
        }

        public static void SaveProject()
        {
            UnityEditor.AssetDatabase.SaveAssets(); // Open materials might not be saved to disk so force a 'Save Project' operation, same as ExecuteMenuItem("File/Save Project")
        }

#if ENABLE_UNITYPIPELINE
        /// <summary>
        /// Any - match any render pipeline
        /// Standard - used when pointing to a Shader we want to match against to swap from ( eg Standard shader ) but we do not want to copy the source from and swap to it
        /// StandardSource - Built-In / DIRP / DRP / Default Render Pipeline - must point to version built from source code
        /// URP - Universal Render Pipeline
        /// HDRP - High Definition Render Pipeline
        /// -
        /// Only 'Standard' is upgradeable using the Unity upgrader
        /// </summary>
        public enum SRPTarget
        {
            Any,
            Standard,
            StandardSource,
            URP,
            HDRP
        }
#endif
      
#if ENABLE_UNITYVERSION
        public enum UnityVersion
        {
            Min = 0,
            Unity2018_4 = 20184,
            Unity2019_1 = 20191,
            Unity2019_2 = 20192,
            Unity2019_3 = 20193,
            Unity2019_4 = 20194,
            Unity2020_1 = 20201,
            Unity2020_2 = 20202,
            Unity2020_3 = 20203,
            Unity2021_1 = 20211,
            Unity2021_2 = 20212,
            Unity2021_3 = 20213,
            Unity2022_1 = 20221,
            Unity2022_2 = 20222,
            Unity2022_3 = 20223,
            Max = 30000
        }

        public static UnityVersion GetUnityVersion()
        {
            UnityVersion curVersion = UnityVersion.Min;
#if UNITY_2018_4_OR_NEWER
            curVersion = UnityVersion.Unity2018_4;
#endif
#if UNITY_2019_1_OR_NEWER
            curVersion = UnityVersion.Unity2019_1;
#endif
#if UNITY_2019_2_OR_NEWER
            curVersion = UnityVersion.Unity2019_2;
#endif
#if UNITY_2019_3_OR_NEWER
            curVersion = UnityVersion.Unity2019_3;
#endif
#if UNITY_2019_4_OR_NEWER
            curVersion = UnityVersion.Unity2019_4;
#endif
#if UNITY_2020_1_OR_NEWER
            curVersion = UnityVersion.Unity2020_1;
#endif
#if UNITY_2020_2_OR_NEWER
            curVersion = UnityVersion.Unity2020_2;
#endif
#if UNITY_2020_3_OR_NEWER
            curVersion = UnityVersion.Unity2020_3;
#endif
#if UNITY_2021_1_OR_NEWER
      curVersion = UnityVersion.Unity2021_1;
#endif
#if UNITY_2021_2_OR_NEWER
      curVersion = UnityVersion.Unity2021_2;
#endif
#if UNITY_2021_3_OR_NEWER
      curVersion = UnityVersion.Unity2021_3;
#endif
#if UNITY_2022_1_OR_NEWER
      curVersion = UnityVersion.Unity2022_1;
#endif
#if UNITY_2022_2_OR_NEWER
      curVersion = UnityVersion.Unity2022_2;
#endif
#if UNITY_2022_3_OR_NEWER
      curVersion = UnityVersion.Unity2022_3;
#endif

            return curVersion;
        }
#endif
    }
}
