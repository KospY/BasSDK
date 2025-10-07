using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace ThunderRoad
{
    public class AddressableGroupAssetProcessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            //find all of the AddressableAssetGroup assets that were imported
            foreach (string assetPath in importedAssets)
            {
                ProcessAssets(assetPath);
            }
            //and moved ones in case they were moved to a git tracked folder
            foreach (string assetPath in movedAssets)
            {
                ProcessAssets(assetPath);
            }
        }
        private static void ProcessAssets(string assetPath)
        {
            AddressableAssetGroup assetGroup = null;
            if (!assetPath.EndsWith(".asset")) return;
            var mainObj = AssetDatabase.LoadMainAssetAtPath(assetPath);
            if (mainObj is AddressableAssetGroupSchema assetGroupSchema)
            {
                //get the group from the schema
                assetGroup = assetGroupSchema.Group;
                mainObj = assetGroup;
            }
            if (mainObj is not AddressableAssetGroup obj) return;
            MoveAddressableGroup(obj);
        }
        private static void MoveAddressableGroup(AddressableAssetGroup assetGroup)
        {
            string assetPath = AssetDatabase.GetAssetPath(assetGroup);
            var trSchema = assetGroup.GetSchema<ThunderRoadAAGroupSchema>();
            bool isShared = trSchema != null && trSchema.sharedBundle;
            //get the name of the group
            string groupName = assetGroup.Name;
            //if the groupname is ProtoAssets we can skip it
            if (groupName == "ProtoAssets") return;
            //if the groupname is Default we can skip it
            if (groupName == "Default") return;
            if (groupName == "Built In Data") return;
            // if its a shared bundle (bas base game) we can skip it
            if (isShared) return;
                    
            //create the folder Assets/Personal/AddressableAssetsData if it does not exist
            if (!AssetDatabase.IsValidFolder("Assets/Personal"))
            {
                AssetDatabase.CreateFolder("Assets", "Personal");
            }
            if(!AssetDatabase.IsValidFolder("Assets/Personal/AddressableAssetsData"))
            {
                AssetDatabase.CreateFolder("Assets/Personal", "AddressableAssetsData");
            }
            //check the path, if the path does not contain Assets/Personal or Assets/Private or Assets/Repos then we need to move it to Assets/Personal/AddressableAssetsData
            if (!assetPath.Contains("Assets/Personal") && !assetPath.Contains("Assets/Private") && !assetPath.Contains("Assets/Repos"))
            {
                string newPath = $"Assets/Personal/AddressableAssetsData/{groupName}.asset";
                string uniquePath = AssetDatabase.GenerateUniqueAssetPath(newPath);
                AssetDatabase.MoveAsset(assetPath, uniquePath);
                Debug.Log($"[ThunderRoad] Moved AddressableAssetGroup {groupName} from: {assetPath} to: {uniquePath}");
                assetPath = uniquePath;
            }
            
            foreach (var schema in assetGroup.Schemas)
            {
                string path = AssetDatabase.GetAssetPath(schema);
                if (string.IsNullOrEmpty(path)) continue;
                if (path.Contains("Assets/Personal") || path.Contains("Assets/Private") || path.Contains("Assets/Repos")) continue;
                string schemaName = System.IO.Path.GetFileName(path);
                string newSchemaPath = $"Assets/Personal/AddressableAssetsData/{schemaName}";
                string uniqueSchemaPath = AssetDatabase.GenerateUniqueAssetPath(newSchemaPath);
                AssetDatabase.MoveAsset(path, uniqueSchemaPath);
                Debug.Log($"[ThunderRoad] Moved AddressableAssetGroup schema {schemaName} from: {path} to: {uniqueSchemaPath}");
            }
        }
    }
}
