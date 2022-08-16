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
            GoreNotAllowed,
        }

        protected void OnEnable()
        {
            if (condition == Condition.OnPlay)
            {
                this.gameObject.SetActive(false);
            }
            else if (condition == Condition.GoreNotAllowed)
            {
                if (!GameSettings.instance.activeContent.HasFlag(GameSettings.ContentFlag.Blood))
                {
                    this.gameObject.SetActive(false);
                }
            }
#if PrivateSDK
            else if (condition == Condition.IsBuildRelease && !Debug.isDebugBuild)
            {
                this.gameObject.SetActive(false);
            }
#endif
        }
    }
}
