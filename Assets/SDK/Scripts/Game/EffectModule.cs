using System;
using UnityEngine;

namespace ThunderRoad
{
    public class EffectModule
    {
        public Effect.Step step = Effect.Step.Start;
        public string stepCustomId;
        public bool disabled;

        public virtual void Refresh()
        {

        }

    }
}
