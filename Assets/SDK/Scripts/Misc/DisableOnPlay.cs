using UnityEngine;

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Misc/Disable on play")]
    public class DisableOnPlay : MonoBehaviour
    {
        protected void Awake()
        {
            this.gameObject.SetActive(false);
        }

    }
}
