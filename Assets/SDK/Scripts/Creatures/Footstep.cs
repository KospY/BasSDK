using UnityEngine;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Footstep")]
    public class Footstep : ThunderBehaviour
    {
        /// <summary>
        /// Id of the material to retrieve the sounds from (in catalog).
        /// </summary>
        [Tooltip("Id of the material to retrieve the sounds from (in catalog).")]
        public string materialId = "Footstep";

        /// <summary>
        /// If true, on each step we trigger a raycast sampling above and under the foot.
        /// We only keep the higher collider and play its effects. We discard the others.
        /// </summary>
        [Tooltip(
            "Enables on each step a ray sampling above and under the foot. Plays only the effects of the highest collider. (Needs to be true for water planes)")]
        public bool usePerFootRaycastCheck = true;

        /// <summary>
        /// Velocity thresholds, used in an inverse lerp when walking and running.
        /// </summary>
        [Tooltip("Velocity thresholds, used in an inverse lerp when walking and running.")]
        public Vector2 minMaxStandingVelocity = new Vector2(0f, 8);

        /// <summary>
        /// Velocity thresholds, used in an inverse lerp when falling.
        /// </summary>
        [Tooltip("Velocity thresholds, used in an inverse lerp when falling.")]
        public Vector2 minMaxFallingVelocity = new Vector2(.1f, 10);

        /// <summary>
        /// Factor used to tweak the intensity of the footsteps when falling.
        /// </summary>
        [Tooltip("Factor used to tweak the intensity of the footsteps when falling.")]
        public float fallingIntensityFactor = 1.25f;

        /// <summary>
        /// Factor used to tweak the intensity of the footsteps when crouching.
        /// </summary>
        [Tooltip("Factor used to tweak the intensity of the footsteps when crouching.")]
        public float crouchingIntensityFactor = 0.35f;

        /// <summary>
        /// Cool-down delay in second between two steps. Different timers are used for each foot.
        /// </summary>
        [Tooltip("Cool-down delay in second between two steps. Different timers are used for each foot.")]
        public float stepMinDelay = 0.2f;

        /// <summary>
        /// Cool-down delay in second between two falls.
        /// </summary>
        [Tooltip("Cool-down delay in second between two falls.")]
        public float fallMinDelay = 0.2f;

        /// <summary>
        /// Height used to detect footsteps, it's added to the locomotion ground point (in meters).
        /// </summary>
        [Tooltip("Height used to detect footsteps, it's added to the locomotion ground point (in meters).")]
        public float footstepDetectionHeightThreshold = 0.045f;

        /// <summary>
        /// Height used to detect footsteps while running, it's added to the locomotion ground point (in meters).
        /// </summary>
        [Tooltip(
            "Height used to detect footsteps while running, it's added to the locomotion ground point (in meters).")]
        public float footstepDetectionRunningHeightThreshold = 0.08f;

        /// <summary>
        /// RaycastHit buffer, used to sample the floor material per foot.
        /// </summary>
        private readonly RaycastHit[] hits = new RaycastHit[4];


        private int waterMaterialHash = Animator.StringToHash("Water (Instance)");

    }
}