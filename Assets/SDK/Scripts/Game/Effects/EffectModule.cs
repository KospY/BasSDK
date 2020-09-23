using System;
using UnityEngine;
using System.Collections;
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

        public virtual IEnumerator Refresh(EffectData effectData)
        {
            yield return new WaitForEndOfFrame();
        }

        [Button]
        public virtual void Clean()
        {
         
        }

        public virtual bool IsEnabled()
        {
            return !disabled;
        }

#if ODIN_INSPECTOR

        public virtual Effect Spawn(EffectData effectData, bool pooled)
        {
            return null;
        }
#endif
    }
}
