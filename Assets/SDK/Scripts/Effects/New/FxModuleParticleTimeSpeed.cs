using UnityEngine;
using System.Collections.Generic;

namespace ThunderRoad
{
    public class FxModuleParticleTimeSpeed : MonoBehaviour
    {
        public float startSpeed = 300;
        public float gravity = 1;

        public float minTimeScale = 0.25f;
        public float slowTimeMultiplier = 0.1f;
#pragma warning disable 0109 // unity this.particleSystem is obsolete
        protected new ParticleSystem particleSystem;
#pragma warning restore 0109
        protected ParticleSystem.MainModule particleSystemMain;
        ParticleSystem.Particle[] particles;
        protected float previousTimeScale = 1;

    }
}
