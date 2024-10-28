using UnityEngine;

namespace ThunderRoad
{
    /// <summary>
    /// This component applies custom gravity to a collisionHandlers physic body
    /// </summary>
    public class Gravity : ThunderBehaviour
    {
        public CollisionHandler collisionHandler;
        public float gravityMultiplier;
        
        public override ManagedLoops EnabledManagedLoops => ManagedLoops.FixedUpdate;

    }
}
