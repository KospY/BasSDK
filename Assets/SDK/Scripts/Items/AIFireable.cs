using ThunderRoad.AI.Action;
using UnityEngine;
using UnityEngine.Events;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Exclude/AIFireball.html")]
	public class AIFireable : MonoBehaviour
    {
        public Transform aimTransform;

        [Header("Fireable events")]
        public UnityEvent aimEvent;
        public UnityEvent fireEvent;
        public UnityEvent reloadEvent;

    }
}
