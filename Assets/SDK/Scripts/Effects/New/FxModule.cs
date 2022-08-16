using System;
using UnityEngine;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/FxModule")]
    public class FxModule : MonoBehaviour
    {
        public bool useControllerDirection;

        public enum Link
        {
            None,
            Intensity,
            Speed,
        }

        [NonSerialized]
        public FxController controller;

        public virtual void Play()
        {
            if (useControllerDirection)
            {
                if (controller.direction != Vector3.zero)
                {
                    this.transform.rotation = Quaternion.LookRotation(controller.direction, controller.transform.forward);
                }
                else
                {
                    this.transform.localRotation = Quaternion.identity;
                }
            }
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
