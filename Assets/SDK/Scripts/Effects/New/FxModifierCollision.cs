using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad
{
    public class FxModifierCollision : ThunderBehaviour
    {
        public FxController fxController;
        public List<Collider> colliders;
        public Link velocityLink = Link.None;
        public Vector2 velocityRange = new Vector2(1, 12);
        public float minDelay = 0.5f;

        private float lastFxTime;

        public enum Link
        {
            None,
            Intensity,
            Speed,
        }

    }
}
