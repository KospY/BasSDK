using UnityEngine;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/DisableOnCondition")]
    [AddComponentMenu("ThunderRoad/Misc/Disable on condition")]
    public class DisableOnCondition : MonoBehaviour
    {
        public Condition condition = Condition.OnPlay;
        public enum Condition
        {
            OnPlay,
            IsBuildRelease,
            OnRoomCulled,
            ContentFilterSetting
        }

        public BuildSettings.SingleContentFlag contentFlag = BuildSettings.SingleContentFlag.Blood;

    }
}
