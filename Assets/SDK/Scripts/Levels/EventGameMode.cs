using UnityEngine;
using UnityEngine.Events;

namespace ThunderRoad
{
    public class EventGameMode : MonoBehaviour
    {
        public string gameModeId;
        public UnityEvent onGameModeEqual;
        public UnityEvent onGameModeNotEqual;

        private void Awake()
        {
        }
    }
}
