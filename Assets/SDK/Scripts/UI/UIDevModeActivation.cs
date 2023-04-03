using UnityEngine;

namespace ThunderRoad
{
    public class UIDevModeActivation : MonoBehaviour
    {
        public GameObject devModeIndicator = null;
        public int clickToActivate = 5;
        public float timeToReset = 10.0f;
        private int _numberOfClick;
        private float _resetTime = 0.0f;
        
        public void OnClick()
        {
            
        }
    }
}
