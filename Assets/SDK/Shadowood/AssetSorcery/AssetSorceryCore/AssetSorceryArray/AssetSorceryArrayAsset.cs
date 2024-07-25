using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ThunderRoad.AssetSorcery
{
    public class AssetSorceryArrayAsset : AssetSorceryAssetCommon<AFilterCommon<Texture2DArray>, Texture2DArray>
    {
        [Tooltip("Rename the asset using this string")]
        public string customName = "AssetSorceryArray";

        public List<FilterPlatform<Texture2DArray>> filters = new List<FilterPlatform<Texture2DArray>>();


        public override void DrawCustomInspector(SerializedObject extraDataSerializedObject)
        {
            //
        }

        public override AFilterCommon<Texture2DArray> ReturnFilterMatch()
        {
            AFilterCommon<Texture2DArray> match = null;

            foreach (var o in filters)
            {
                if (!o.FilterTest()) continue;
                match = o;
                break;
            }

            return match;
        }

        public override List<AFilterCommon<Texture2DArray>> GetFilterEntries()
        {
            return new List<AFilterCommon<Texture2DArray>>(filters);
        }
    }
}
