using System;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ThunderRoad.AssetSorcery
{
    [ScriptedImporter(0, AssetSorceryArray.ASSETSORCERY_FILE_EXTENSION)]

    // AssetSorceryAssetCommon<FilterCommon<TObject>,TObject>
    public class AssetSorceryArray : AssetSorceryCommon<AssetSorceryArrayAsset, Texture2DArray>
    {
        public const string ASSETSORCERY_FILE_EXTENSION = "ASarray";


        public override void OnImportAsset(AssetImportContext ctx)
        {
            var package = GetPackage(ctx);

            /*
            foreach (var entryCommon in package.GetFilterEntries())
            {
                if (entryCommon.GetItem() != null)
                {
                    ctx.DependsOnArtifact(AssetDatabase.GetAssetPath(entryCommon.GetItem()));
                }
            }*/

            foreach (var filterPlatform in package.filters)
            {
                if(filterPlatform.item==null) continue;
                //Debug.Log("Depend on path:" + AssetDatabase.GetAssetPath(filterPlatform.item) + " : " + filterPlatform.item.name);
                ctx.DependsOnArtifact(AssetDatabase.GetAssetPath(filterPlatform.item));
            }

            base.OnImportAsset(ctx);
        }

        public override Object ProcessSourceToObject(AssetImportContext ctx, AssetSorceryArrayAsset package, AFilterCommon<Texture2DArray> entry, string source, Texture2DArray entryItem)
        {
            var newName = package.customName;
            if (newName.Contains("*")) newName = newName.Replace("*", entryItem.name);

#if ENABLE_UNITYPIPELINE
            if (newName.Contains("@")) newName = newName.Replace("@", entry.srpTarget.ToString());
#endif


            //var ts = package.textureSettings; 


            return entryItem;
        }

       
    }
}
