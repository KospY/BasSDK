using UnityEngine;

namespace ThunderRoad
{
    public class AreaAssetPostProcessor : UnityEditor.AssetPostprocessor
    {
        public static bool SetUpAreaOnImport = true;
        
        private void OnPostprocessPrefab(GameObject gameObject)
        {
            if (!SetUpAreaOnImport) return;

            Area tempArea;
            if(!gameObject.TryGetComponent<Area>(out tempArea)) return;

            Debug.Log("AP: AreaAssetPostProcessor: OnPostprocessPrefab: " + gameObject.name);
            
            tempArea.gameObject.hideFlags = HideFlags.DontSaveInEditor;
            tempArea.OnImport(this);
        }
    }
}