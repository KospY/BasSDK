using UnityEngine;
using System;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Effects/EffectShader.html")]
    public class EffectShader : Effect
    {
        public EffectTarget linkBaseColor = EffectTarget.None;
        public EffectTarget linkEmissionColor = EffectTarget.None;
        public EffectTarget linkExtraColor = EffectTarget.None;

        public float lifeTime = 0;
        public float refreshSpeed = 0.1f;
        public bool useSecondaryRenderer;

        public int extraPropertyId;

        [NonSerialized]
        public float playTime;

        protected MaterialInstance materialInstance;

        protected static int colorPropertyID;
        protected static int emissionPropertyID;
        protected static int useEmissionPropertyID;

        protected float currentValue;
        protected Gradient currentMainGradient;
        protected Gradient currentSecondaryGradient;

    }
}
