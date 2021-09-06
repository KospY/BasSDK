using UnityEngine;

namespace ThunderRoad
{
    public class FxModule : MonoBehaviour
    {
        public enum Link
        {
            None,
            Intensity,
            Speed,
        }

        public virtual void Play()
        {

        }

        public virtual void SetIntensity(float intensity)
        {

        }

        public virtual void SetSpeed(float speed)
        {

        }

        public virtual void Stop()
        {

        }

        public virtual bool IsPlaying()
        {
            return false;
        }
    }
}
