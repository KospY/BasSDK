#if UNITY_EDITOR
using System;
using ThunderRoad.AssetSorcery;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public class AssetSorceryBuildTargetListener : IActiveBuildTargetChanged
{
    public int callbackOrder
    {
        get { return 0; }
    }

    public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
    {
        

        var platform = AssetSorceryPlatformRuntime.AssetSorceryGetBuildPlatform();
        
        Debug.Log("AssetSorceryBuildTargetListener: Switched: " + platform + " -> " + newTarget);

        switch (platform)
        {
            case AssetSorceryPlatformRuntime.ePlatformAS.Desktop:
                AssetSorceryPlatform.AssetSorceryShaderSetPlatform(AssetSorceryPlatformRuntime.ePlatformAS.Desktop);
                break;
            case AssetSorceryPlatformRuntime.ePlatformAS.Mobile:
                AssetSorceryPlatform.AssetSorceryShaderSetPlatform(AssetSorceryPlatformRuntime.ePlatformAS.Mobile);
                break;
        }

        /*
        switch (newTarget)
        {
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                AssetSorceryPlatform.AssetSorceryShaderSetPlatform(AssetSorceryPlatform.ePlatformAS.desktop);
                break;
            case BuildTarget.Android:
            case BuildTarget.PS3:
            case BuildTarget.PS4:
            case BuildTarget.PS5:
                AssetSorceryPlatform.AssetSorceryShaderSetPlatform(AssetSorceryPlatform.ePlatformAS.mobile);
                break;
        }*/
    }
}

#endif
