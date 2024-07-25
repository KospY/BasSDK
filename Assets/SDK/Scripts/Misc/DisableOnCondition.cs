using UnityEngine;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Levels/DisableOnCondition.html")]
    [AddComponentMenu("ThunderRoad/Misc/Disable on condition")]
    public class DisableOnCondition : MonoBehaviour
    {
        public Condition condition = Condition.OnPlay;
        [Tooltip("Specifies the point that this gameObject should be disabled.\n\nOnPlay: Disables when the game is played.\nIsBuildReleased: Disables when the build is ran.\nOnRoomCulled: Disables when the Dungeon room this gameobject resides in is culled.\nContentFilterSetting: Disabled based on the content filter (like gore being disabled).\nLevelOption: Disabled based on what LevelOption is selected in the Level (e.g. Survival).")]
        public enum Condition
        {
            OnPlay,
            IsBuildRelease,
            OnRoomCulled,
            ContentFilterSetting,
            LevelOption,
        }

        [Tooltip("Specifies the content flag for the ContentFilterSetting Condition.\n\nSettings such as Blood will make it so this gameObject will be disabled if Gore is disabled (but only if the Condition is ContentFilterSettings).")]
        public BuildSettings.SingleContentFlag contentFlag = BuildSettings.SingleContentFlag.Blood;
        [Tooltip("Specifies the levelOption of which this gameObject is disabled if the Condition is set to LevelOption.\n\nDISABLE_IF_OPTION_EXISTS is a placeholder level option, and does not exist in any level.")]
        public string levelOption = "DISABLE_IF_OPTION_EXISTS";
    }
}
