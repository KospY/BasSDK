using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Misc/Repeater.html")]
    public class Repeater : MonoBehaviour
    {
        public float startTime;
        public float interval = 1;

        public UnityEvent onRepeat;

        private void OnEnable()
        {
            InvokeRepeating(nameof(Repeat), startTime, interval);
        }

        private void OnDisable()
        {
            CancelInvoke(nameof(Repeat));
        }

        private void Repeat()
        {
            onRepeat?.Invoke();
        }
    }
}