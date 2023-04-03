using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class EffectModuleLight : EffectModule
    {
#if ODIN_INSPECTOR
        [HorizontalGroup("Intensity", width: 0.7f, LabelWidth = 150)]
        [BoxGroup("Intensity/Curve")]
#endif
        public new AnimationCurve intensityCurve = AnimationCurve.Constant(0, 1, 1);
#if ODIN_INSPECTOR
        [BoxGroup("Intensity/Smoothing")]
#endif
        public float intensitySmoothFactor = 0;

#if ODIN_INSPECTOR
        [HorizontalGroup("Range", width: 0.7f, LabelWidth = 150)]
        [BoxGroup("Range/Curve")]
#endif
        public AnimationCurve rangeCurve = AnimationCurve.Constant(0, 1, 1);

#if ODIN_INSPECTOR
        [BoxGroup("Range/Smoothing")]
#endif
        public float rangeSmoothFactor = 0;

#if ODIN_INSPECTOR
        [HorizontalGroup("Color", width: 0.7f, LabelWidth = 150)]
        [BoxGroup("Color/Curve")]
#endif
        public Color colorStart;

#if ODIN_INSPECTOR
        [BoxGroup("Color/Curve")]
#endif
        public Color colorEnd;

        [NonSerialized]
        protected Gradient colorCurve;

#if ODIN_INSPECTOR
        [BoxGroup("Color/Smoothing")]
#endif
        public float colorSmoothFactor = 0;
#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public float loopFadeDelay = 0;

#if ODIN_INSPECTOR
        [BoxGroup("Flicker")]
#endif

        public AnimationCurve flickerIntensityCurve = AnimationCurve.Constant(0, 1, 1);
#if ODIN_INSPECTOR
        [BoxGroup("Flicker")]
#endif

        public AnimationCurve flickerRateCurve = AnimationCurve.Constant(0, 1, 0);

    }
}
