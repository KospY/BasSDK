using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;


#if ODIN_INSPECTOR
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class LiquidHealth : LiquidData
    {
        public float healthGain = 15;

        public float revealBlitDuration = 5;
        public float revealBlitStep = 0.1f;

    }
}