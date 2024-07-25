using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class AssetSorceryDefine : MonoBehaviour
{
    static AssetSorceryDefine()
    {
        AddDefineUtility.AddDefines(new[] {"ASSET_SORCERY"});
    }
}
