using UnityEditor;
using UnityEngine;

namespace ThunderRoad
{
    public class ItemEditorTools
    {
        [MenuItem("ThunderRoad (SDK)/Items/Remove Preview References")]
        static void RemovePreviewReferences()
        {
            // Get the selected objects from the Project window
            Object[] selectedPrefabs = Selection.GetFiltered(typeof(GameObject), SelectionMode.Assets);

            // Check if any prefabs are selected
            if (selectedPrefabs.Length > 0)
            {
                // Iterate over each selected prefab
                foreach (Object prefab in selectedPrefabs)
                {
                    if (prefab is not GameObject gameObjectPrefab) continue;
                    string assetPath = AssetDatabase.GetAssetPath(gameObjectPrefab);
 
                    using (var editingScope = new PrefabUtility.EditPrefabContentsScope(assetPath))
                    {
                        var prefabRoot = editingScope.prefabContentsRoot;
                        if(!prefabRoot.TryGetComponent(out Item item)) continue;
                        var previews = prefabRoot.GetComponentsInChildren<Preview>();
                        foreach (var preview in previews)
                        {
                            if (preview.generatedIcon == null) continue;
                            Debug.Log($"{prefab.name} has preview component and a generated icon - removing icon reference");
                            preview.generatedIcon = null;
                        }
                    }
                }
            }

        }
    }
}
