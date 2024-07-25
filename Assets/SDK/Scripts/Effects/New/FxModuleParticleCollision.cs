using UnityEngine;
using System.Collections.Generic;

namespace ThunderRoad
{
    public class FxModuleParticleCollision : MonoBehaviour
    {
        public FxModuleParticle spawnFxModuleParticle;

        public float maxGroundAngle = 45;
        public float emitRate = 0.2f;
        public float minIntensity = 0;
        public float maxIntensity = 1;
        public bool useMainGradient;
        public bool useSecondaryGradient;

        protected ParticleSystem particle;
        protected List<ParticleCollisionEvent> collisionEvents;

        private float lastEmitTime;
        private int aliveEvents;

    }
}
