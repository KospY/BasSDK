using UnityEngine;

#if ODIN_INSPECTOR
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/DisableOnPlatform")]
	[AddComponentMenu("ThunderRoad/Misc/Disable or Strip GameObject relative to platform")]
    public class DisableOnPlatform : MonoBehaviour
    {
        public PlatformFilter platformFilter = PlatformFilter.OnlyOn;
        public Platform platform = Platform.Windows;

        [Tooltip("Strip can happen when generating platform specific scene or room")]
        public bool allowStrip = false;

        public enum PlatformFilter
        {
            OnlyOn = 1,
            ExcludeOn = 2,
        }

        protected void Awake()
        {
            if (platformFilter == PlatformFilter.OnlyOn)
            {
                if (Common.GetPlatform() == platform)
                {
                    this.gameObject.SetActive(false);
                }
            }
            else if (platformFilter == PlatformFilter.ExcludeOn)
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
            return false;
        }
    }
}