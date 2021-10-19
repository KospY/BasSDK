using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Misc/Disable or Strip GameObject")]
    public class DisableOrStrip : MonoBehaviour
    {
        public PlatformFilter platformFilter = PlatformFilter.None;
#if ODIN_INSPECTOR
        [HideIf("platformFilter", PlatformFilter.None)]
#endif
        public Platform platform = Platform.Windows;

        public bool ifDebugAdvancedOff = false;

        [Tooltip("Strip can happen when generating platform specific scene or room")]
        public bool allowStrip = false;

        public enum PlatformFilter
        {
            None,
            OnlyOn,
            ExcludeOn,
        }

        protected void Awake()
        {
#if PrivateSDK
            if (ifDebugAdvancedOff && !Catalog.gameData.debugAdvanced)
            {
                this.gameObject.SetActive(false);
                return;
            }
#endif
            if (platformFilter == PlatformFilter.OnlyOn)
            {
                if (Common.GetPlatform() == platform)
                {
                    this.gameObject.SetActive(false);
                }
            }
            if (platformFilter == PlatformFilter.ExcludeOn)
            {
                if (Common.GetPlatform() != platform)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }

        public bool ConcernPlatform(Platform platform)
        {
            if (platformFilter == PlatformFilter.OnlyOn)
            {
                if (platform == this.platform)
                {
                    return true;
                }
            }
            else if (platformFilter == PlatformFilter.ExcludeOn)
            {
                if (platform != this.platform)
                {
                    return true;
                }
            }
            else if (platformFilter == PlatformFilter.None)
            {
                return true;
            }
            return false;
        }
    }
}