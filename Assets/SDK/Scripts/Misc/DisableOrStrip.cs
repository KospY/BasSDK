using UnityEngine;

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Misc/Disable or Strip GameObject")]
    public class DisableOrStrip : MonoBehaviour
    {
        public PlatformFilter platformFilter = PlatformFilter.None;
        public Platform platform = Platform.Windows;

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
            if (platformFilter == PlatformFilter.None)
            {
                this.gameObject.SetActive(false);
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