using UnityEngine;

#if ODIN_INSPECTOR
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Levels/DisableOnPlatform.html")]
    [AddComponentMenu("ThunderRoad/Misc/Disable or Strip GameObject relative to platform")]
    public class DisableOnPlatform : MonoBehaviour
    {
        public PlatformFilter platformFilter = PlatformFilter.OnlyOn;
        public QualityLevel platform = QualityLevel.Windows;
        
        
        [Tooltip("Strip can happen when generating platform specific scene or room")]
        public bool allowStrip = false;
        

        public enum PlatformFilter
        {
            OnlyOn = 1,
            ExcludeOn = 2,
        }

        protected void Start()
        {
            if (platformFilter == PlatformFilter.OnlyOn)
            {
                if (Common.GetQualityLevel() == platform
                    
                   )
                {
                    this.gameObject.SetActive(false);
                }
            }
            else if (platformFilter == PlatformFilter.ExcludeOn)
            {
                if (Common.GetQualityLevel() != platform
                   )
                {
                    this.gameObject.SetActive(false);
                }
            }
        }

        public bool ConcernQualityLevel(QualityLevel qualityLevel)
        {
            if (platformFilter == PlatformFilter.OnlyOn)
            {
                if (qualityLevel == this.platform 
                    
                   )
                {
                    return true;
                }
            }
            else if (platformFilter == PlatformFilter.ExcludeOn)
            {
                if (qualityLevel != this.platform
                
                   )
                {
                    return true;
                }
            }
            return false;
        }
    }
}