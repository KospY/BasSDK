using UnityEngine;

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Misc/Disable on condition")]
    public class DisableOnCondition : MonoBehaviour
    {
        public Condition condition = Condition.OnPlay;
        public enum Condition
        {
            OnPlay,
            DebugAdvancedOff,
        }

        protected void OnEnable()
        {
            if (condition == Condition.OnPlay)
            {
                this.gameObject.SetActive(false);
            }
#if PrivateSDK
            else if (condition == Condition.DebugAdvancedOff && !Catalog.gameData.debugAdvanced)
            {
                this.gameObject.SetActive(false);
            }
#endif
        }
    }
}
