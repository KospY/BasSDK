#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace ThunderRoad.AssetSorcery
{/*
    public class AssetSorceryPlatformPostprocessor : AssetPostprocessor
    {
        public static Action callback = () => { };

        public static void Subscribe(Action actionIn)
        {
            callback -= actionIn;
            callback += actionIn;
        }

        //public static void UnSubscribe(Action actionIn)
        //{
        //    callback -= actionIn;
        //}

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            Debug.Log("OnPostprocessAllAssets");
            foreach (string str in importedAssets)
            {
                Debug.Log("AssetSorceryPlatformPostprocessor: Reimported Asset: " + str);
            }

            callback.Invoke();
            callback = () => { };
        }
    }*/
}
#endif
