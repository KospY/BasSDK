using UnityEngine;
using UnityEngine.Events;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Event-Linkers/CollisionEventsBridge.html")]
	public class CollisionEventsBridge : MonoBehaviour
    {
        public UnityEvent<Collision> onCollisionEnter = new UnityEvent<Collision>();
        public UnityEvent<Collision> onCollisionExit = new UnityEvent<Collision>();
        public UnityEvent<Collision> onCollisionStay = new UnityEvent<Collision>();

    }
}