using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace ThunderRoad
{
    public class GolemCrystal : SimpleBreakable
    {
        [Header("Protection")]
        public GameObject shield;
        public UnityEvent onShieldEnable;
        public UnityEvent onShieldDisable;
        public Transform linkEffect;
        public Transform linkEffectTarget;
        public VfxPlayer passiveVfxPlayer;
        public string hitEffectID;
        public string emissiveEffectID;
        public float emissiveToggleTime = 0.5f;
        public List<Renderer> emissiveRenderers = new List<Renderer>();

    }
}
