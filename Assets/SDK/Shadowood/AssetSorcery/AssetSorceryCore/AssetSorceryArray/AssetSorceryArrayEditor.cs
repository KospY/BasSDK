using UnityEditor;
using UnityEngine;

namespace ThunderRoad.AssetSorcery
{
    [CustomEditor(typeof(AssetSorceryArray)), CanEditMultipleObjects]
    public class AssetSorceryArrayEditor : AssetSorceryEditorCommon<AssetSorceryArrayAsset, Texture2DArray, AFilterCommon<Texture2DArray>>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10);

            AssetSorceryPlatform.DrawPlatformButtons(target);
        }
    }
}
