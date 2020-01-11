using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace BS
{
    public class EffectModule
    {
        [BoxGroup("Effect")]
        public Effect.Step step = Effect.Step.Start;

        public virtual void Refresh()
        {

        }

        public virtual Effect Spawn()
        {
            return null;
        }
    }
}
