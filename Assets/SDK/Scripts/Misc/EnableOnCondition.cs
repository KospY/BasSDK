using UnityEngine;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/EnableOnCondition")]
    [AddComponentMenu("ThunderRoad/Misc/Enable on condition")]
    public class EnableOnCondition : MonoBehaviour
    {
        public Condition condition = Condition.OnPlay;
        public enum Condition
        {
            OnPlay,
            IsBuildRelease,
            OnRoomCulled,
            ContentFilterSetting,
            LevelOption,
        }

        
        public BuildSettings.SingleContentFlag contentFlag = BuildSettings.SingleContentFlag.Blood;
        public string levelOption = "ENABLE_IF_OPTION_EXISTS";
    }
}
