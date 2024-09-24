#if UNITY_EDITOR

using ThunderRoad.AssetSorcery;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class AssetSorceryInitOnLoad
{
    static AssetSorceryInitOnLoad()
    {
        //set the meta telemetry bool in the editor prefs so it doesnt annoy us, their codes broke
        EditorPrefs.SetBool("OVRTelemetry.TelemetryEnabled", false);
        var platform = AssetSorceryPlatformRuntime.AssetSorceryGetBuildPlatform();

        Debug.Log($"AssetSorceryInitOnLoad: {platform} <-> {AssetSorceryPlatform.AssetSorceryGetPlatform()}");

        switch (platform)
        {
            case AssetSorceryPlatformRuntime.ePlatformAS.Desktop:
                AssetSorceryPlatform.AssetSorceryShaderSetPlatform(AssetSorceryPlatformRuntime.ePlatformAS.Desktop);
                break;
            case AssetSorceryPlatformRuntime.ePlatformAS.Mobile:
                AssetSorceryPlatform.AssetSorceryShaderSetPlatform(AssetSorceryPlatformRuntime.ePlatformAS.Mobile);
                break;
        }
        Debug.Log($"AssetSorceryInitOnLoad Complete: {platform} <-> {AssetSorceryPlatform.AssetSorceryGetPlatform()}");

    }
}

#endif
