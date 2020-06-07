using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    public class EffectModule
    {
#if ODIN_INSPECTOR
        [BoxGroup("Module")]
#endif
        public Effect.Step step = Effect.Step.Start;
#if ODIN_INSPECTOR
        [BoxGroup("Module"), ShowIf("step", Effect.Step.Custom)]
#endif
        public string stepCustomId;
#if ODIN_INSPECTOR
        [BoxGroup("Module")]
#endif
        public bool disabled;

        public virtual void Refresh()
        {

        }

#if ODIN_INSPECTOR

        public virtual Effect Spawn(EffectData effectData)
        {
            return null;
        }
#endif
    }
}
