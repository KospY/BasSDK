using UnityEngine;
#if ODIN_INSPECTOR
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/WhooshPoint")]
	[AddComponentMenu("ThunderRoad/Whoosh")]
    public class WhooshPoint : ThunderBehaviour
    {
        //Experimental timeslicing test
        //Whooshpoints just update their sound intensity every frame, but it could be done every second frame
        //but we dont want all of them running every second frame, so we split it so half are on odd frames and half are on even frames
        public static int timeSlice = 0;
        //split between 2 frames
        public static int timeSliceNumFrames = 2;
        private int myTimeSlice = 0;
        
        [Tooltip("Depicts how the whoosh is triggered.\n\"Always\" makes it so whoosh happens when velocity is met.\n\"On Grab\" makes Whoosh only play when grabbed.\n\"On Fly\" makes Whoosh only play when thrown/in air.")]
        public Trigger trigger = Trigger.Always;
        [Tooltip("Minimum velocity of which the Whoosh plays")]
        public float minVelocity = 5;
        [Tooltip("Maximum velocity of item to play at max volume.")]
        public float maxVelocity = 12;
        [Tooltip("Depicts the volume of which the whoosh sound will play, depending on velocity. The slower the speed, the quieter the whoosh sound, while the faster the speed, the higher the volume. Lowest speed is depicted by minimunm velocity and highest speed is depicted by maximum velocity.")]
        public float dampening = 0.1f;
        [Tooltip("Stops the whoosh sound once the item is connected to a holder.")]
        public bool stopOnSnap = true;

        public enum Trigger
        {
            Always,
            OnGrab,
            OnFly,
        }

    }
}
