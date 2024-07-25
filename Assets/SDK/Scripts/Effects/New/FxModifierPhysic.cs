using UnityEngine;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Effects/FxModifierPhysic.html")]
    public class FxModifierPhysic : ThunderBehaviour
    {
        [Header("References")]
        public FxController fxController;
        public Rigidbody rb;

        [Header("Velocity")]
        public Link velocityLink = Link.None;
        public Transform velocityPointTransform;
        public Vector2 velocityRange = new Vector2(0, 5);
        public float velocityDampening = 1f;

        [Header("Torque")]
        public Link torqueLink = Link.None;
        public Vector2 torqueRange = new Vector2(2, 8);
        public float torqueDampening = 1f;

        public enum Link
        {
            None,
            Intensity,
            Speed,
        }

        protected float dampenedVelocity;
        protected float dampenedTorque;

    }
}
