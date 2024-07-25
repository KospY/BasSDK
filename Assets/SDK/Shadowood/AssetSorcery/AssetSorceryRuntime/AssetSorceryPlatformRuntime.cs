using System;
using UnityEngine;

namespace ThunderRoad.AssetSorcery
{
    public static class AssetSorceryPlatformRuntime
    {
        public enum ePlatformAS
        {
            Auto,
            Desktop,
            Mobile,
        }
        
        private static bool platformCached;
        private static ePlatformAS currentPlatform;

        public static ePlatformAS AssetSorceryGetBuildPlatform(bool ignoreCache = false)
        {
            if (ignoreCache) platformCached = false;
            if (platformCached) return currentPlatform;

            if (QualitySettings.names[QualitySettings.GetQualityLevel()] == "Windows")
            {
                currentPlatform = ePlatformAS.Desktop;
                platformCached = Application.isPlaying;
                return currentPlatform;
            }
            if (QualitySettings.names[QualitySettings.GetQualityLevel()] == "Android")
            {
                currentPlatform = ePlatformAS.Mobile;
                platformCached = Application.isPlaying;
                return currentPlatform;
            }
            Debug.LogError("Quality Settings names don't match platform enum!");
            return ePlatformAS.Auto;
        }
    }
}
