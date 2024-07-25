using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ThunderRoad.AssetSorcery
{
    public class AssetSorceryMaterialAsset : AssetSorceryAssetCommon<AFilterCommon<Material>, Material>
    {
        [Tooltip("Rename the shader using this string")]
        public string customMatName = "AssetSorceryMaterial";

        public override void DrawCustomInspector(SerializedObject extraDataSerializedObject)
        {
            //SerializedProperty textureProps = extraDataSerializedObject.FindProperty("textureSettings");
            //EditorGUILayout.PropertyField(textureProps);
        }

        public List<FilterPlatform<Material>> filters = new List<FilterPlatform<Material>>();

        public override AFilterCommon<Material> ReturnFilterMatch()
        {
            AFilterCommon<Material> match = null;

            foreach (var o in filters)
            {
                if (!o.FilterTest()) continue;
                match = o;
                break;
            }

            return match;
        }

        public override List<AFilterCommon<Material>> GetFilterEntries()
        {
            return new List<AFilterCommon<Material>>(filters);
        }
    }
}
