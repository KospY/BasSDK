using System;
using System.IO;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ThunderRoad.AssetSorcery
{
    //[ScriptedImporter(version: 1, AssetSorceryTexture.ASSETSORCERY_FILE_EXTENSION)]
    [Serializable]
    public abstract class AssetSorceryCommon<TSISAssetCommon, TObject> : ScriptedImporter where TSISAssetCommon : AssetSorceryAssetCommon<AFilterCommon<TObject>, TObject> where TObject : Object
    {
        public static TSISAssetCommon ReadFromPath(string path, bool silent = false)
        {
            if (!File.Exists(path))
            {
                if (!silent) Debug.LogError("ReadFromPath: File does not exist: " + path);
                return null;
            }

            string fileContent = File.ReadAllText(path);
            var package = ObjectFactory.CreateInstance<TSISAssetCommon>();
            try
            {
                if (!string.IsNullOrEmpty(fileContent)) EditorJsonUtility.FromJsonOverwrite(fileContent, package);
            }
            catch (Exception e)
            {
                Debug.LogError("ReadFromPath: JSON Failed: " + e +" : " + path);
                return null;
            }
    
            return package;
        }


        // public  abstract string GetExtension();
        //public  abstract void ReloadAll();

        public TSISAssetCommon GetPackage(AssetImportContext ctx)
        {
            return GetPackageFromPath(ctx.assetPath);
        }

        public static TSISAssetCommon GetPackageFromPath(string pathIn)
        {
            return ReadFromPath(pathIn);
        }

        public override void OnImportAsset(AssetImportContext ctx)
        {
            Debug.Log("OnImportAsset: " + ctx.assetPath);
            //var package = ReadFromPath(ctx.assetPath);
            var package = GetPackage(ctx);

            /* // TODO fix
            foreach (var entryCommon in package.GetEntries())
            {
                //if( entryCommon.srpTarget != AssetSorceryCommonItems.SRPTarget.Standard)
                if (entryCommon.GetItem() != null)
                {
                    ctx.DependsOnSourceAsset(AssetDatabase.GetAssetPath(entryCommon.GetItem()));
                    //ctx.DependsOnArtifact(AssetDatabase.GetAssetPath(entryCommon.item));
                }
            }*/

            string source = null;
            string en = "";

            var entry = package.GetEntryForCurrentRP(false);
            TObject entryItem = null;

            if (entry == null)
            {
                Debug.LogError("AssetSorceryCommon - No matching item available", this);
                //return;
            }
            else
            {
                if (entry.GetItem() == null)
                {
                    Debug.LogError("AssetSorceryCommon - entry.item null");
                }
                else
                {
                    entryItem = entry.GetItem();
                    source = package.GetSource();
                    en = entry.GetItem().name;
                }
            }

            Object dataObject = ProcessSourceToObject(ctx, package, entry, source, entryItem);

            if (dataObject == null) Debug.LogError("AssetSorceryCommon: dataObject is null from ProcessSourceToObject()");

            ctx.AddObjectToAsset("MainAsset", dataObject);
            ctx.SetMainObject(dataObject);
        }

        public abstract Object ProcessSourceToObject(AssetImportContext ctx, TSISAssetCommon package, AFilterCommon<TObject> entry, string source, TObject entryItem);
    }
}
