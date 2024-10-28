using UnityEngine;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class BrainModuleFear : BrainData.Module
    {
        public bool receiving = true;
        public string panicScreamDialogID = "falling";
        public float cowerDuration = 10f;
        public string cowerAnimationID = "HumanCower";

        [NonSerialized]
        public AnimationData cowerAnimation;
        [JsonIgnore]
        public bool isCowering => Time.time <= cowerEndTime;
        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public float cowerEndTime = -1f;
        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public float endPanicBrainActiveTime = -1f;

    }
}
