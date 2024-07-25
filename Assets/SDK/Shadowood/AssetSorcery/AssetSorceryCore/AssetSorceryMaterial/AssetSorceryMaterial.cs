using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace ThunderRoad.AssetSorcery
{
    [ScriptedImporter(0, AssetSorceryMaterial.ASSETSORCERY_FILE_EXTENSION)]
    public class AssetSorceryMaterial : AssetSorceryCommon<AssetSorceryMaterialAsset, Material>
    {
        public const string ASSETSORCERY_FILE_EXTENSION = "ASmat";
        
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var package = GetPackage(ctx);
            foreach (var filterPlatform in package.filters)
            {
                if(filterPlatform.item==null) continue;
                ctx.DependsOnArtifact(AssetDatabase.GetAssetPath(filterPlatform.item));
            }
          
            base.OnImportAsset(ctx);
        }
   
        public override Object ProcessSourceToObject(AssetImportContext ctx, AssetSorceryMaterialAsset package, AFilterCommon<Material> entry, string source, Material entryItem)
        {
            var newName = package.customMatName;
            if (newName.Contains("*")) newName = newName.Replace("*", entryItem.name);
            
#if ENABLE_UNITYPIPELINE
            if (newName.Contains("@")) newName = newName.Replace("@", entry.srpTarget.ToString());
#endif

            if (entryItem == null)
            {
                Shader shader = Shader.Find("Hidden/MissingShader");
                if (shader == null)
                {
                    Debug.LogError("AssetSorceryMaterial: could not find Hidden/MissingShader");
                    shader = Shader.Find("Lit");
                }

                return new Material(shader)
                {
                    name = newName
                };
            }

            return new Material(entryItem)
            {
                name = newName
            };
        }
    }
}
