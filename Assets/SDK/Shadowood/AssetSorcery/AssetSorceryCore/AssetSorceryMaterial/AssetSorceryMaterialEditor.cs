using UnityEditor;
using UnityEngine;

namespace ThunderRoad.AssetSorcery
{
    [CustomEditor(typeof(AssetSorceryMaterial)), CanEditMultipleObjects]
    public class AssetSorceryMaterialEditor : AssetSorceryEditorCommon<AssetSorceryMaterialAsset, Material, AFilterCommon<Material>>
    {
    }
}
