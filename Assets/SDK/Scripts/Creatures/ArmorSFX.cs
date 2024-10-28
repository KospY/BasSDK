using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad
{
    public class ArmorSFX : ThunderBehaviour
    {
        private Creature creature;
        private Footstep footStep;
        private float timeSinceLastPlayed = 0;
        private float blockTime = 0.1f;
        private int effectPrio;
        private EffectData armorSFXData;

    }
}

