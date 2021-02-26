using UnityEngine;
using UnityEngine.XR;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Creatures/Creature tester")]
    [RequireComponent(typeof(Creature))]
    public class CreatureTester : MonoBehaviour
    {
        [Range(0.0f, 1.0f)]
        public float timeScale = 1;

        private Creature creature;

        private int castStep;

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                Time.timeScale = timeScale;
            }
        }

        private void Awake()
        {
            if (XRSettings.enabled)
            {
                Time.fixedDeltaTime = (Time.timeScale / UnityEngine.XR.XRDevice.refreshRate) / 2;              
            }
            else
            {
                Time.fixedDeltaTime = 0.006250078f;
            }          
            creature = this.GetComponent<Creature>();
        }

        [Button]
        public void Tpose()
        {
            creature.animator.SetBool("TStance", !creature.animator.GetBool("TStance"));
        }

        [Button]
        public void CastLeft()
        {
            if (castStep == 0)
            {
                creature.animator.SetInteger("CastSide", 2);
                creature.animator.SetTrigger("Cast");
                castStep = 1;
            }
            else if (castStep == 1)
            {
                creature.animator.SetTrigger("Cast");
                castStep = 2;
            }
            else if (castStep == 2)
            {
                creature.animator.SetInteger("CastSide", 0);
                castStep = 0;
            }
        }

        [Button]
        public void Shock()
        {
            if (creature.animator.GetInteger("EffectType") == (int)AnimEffectType.None)
            {
                creature.animator.SetInteger("EffectType", (int)AnimEffectType.Shock);
                creature.animator.SetTrigger("Effect");
            }
            else
            {
                creature.animator.SetInteger("EffectType", (int)AnimEffectType.None);
            }
        }

        [Button]
        public void ShockDead()
        {
            if (creature.animator.GetInteger("EffectType") == (int)AnimEffectType.None)
            {
                creature.animator.SetInteger("EffectType", (int)AnimEffectType.ShockDead);
                creature.animator.SetTrigger("Effect");
            }
            else if (creature.animator.GetInteger("EffectType") == (int)AnimEffectType.Shock)
            {
                creature.animator.SetInteger("EffectType", (int)AnimEffectType.ShockDead);
            }
            else
            {
                creature.animator.SetInteger("EffectType", (int)AnimEffectType.None);
            }
        }

        [Button]
        public void Burn()
        {
            if (creature.animator.GetInteger("EffectType") == (int)AnimEffectType.None)
            {
                creature.animator.SetInteger("EffectType", (int)AnimEffectType.Burning);
                creature.animator.SetTrigger("Effect");
            }
            else
            {
                creature.animator.SetInteger("EffectType", (int)AnimEffectType.None);
            }
        }

        [Button]
        public void Choke()
        {
            if (creature.animator.GetInteger("EffectType") == (int)AnimEffectType.None)
            {
                creature.animator.SetInteger("EffectType", (int)AnimEffectType.Choke);
                creature.animator.SetTrigger("Effect");
            }
            else
            {
                creature.animator.SetInteger("EffectType", (int)AnimEffectType.None);
            }
        }

        [Button]
        public void StanceIdle()
        {
            creature.animator.SetInteger("Stance", 0);
        }

        [Button]
        public void StanceOneHanded()
        {
            creature.animator.SetInteger("Stance", 3);
        }

        [Button]
        public void StanceShield()
        {
            creature.animator.SetInteger("Stance", 2);
        }

        [Button]
        public void StanceBow()
        {
            creature.animator.SetInteger("Stance", 6);
        }

        [Button]
        public void StanceStaff()
        {
            creature.animator.SetInteger("Stance", 5);
        }
    }
}