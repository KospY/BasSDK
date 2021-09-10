using UnityEngine;

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Misc/Disable on platform")]
    public class DisableOnPlatform : MonoBehaviour
    {
        public Platform platform;

        protected void Awake()
        {
            if (Common.GetPlatform() == platform)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
