using UnityEngine;
using System.Collections.Generic;
using System;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Creatures/Speak")]
    public class CreatureSpeak : MonoBehaviour
    {
        public Transform jaw;
        public Vector3 jawMaxRotation = new Vector3(0, -30, 0);
        public float lipSyncSpeed = 5f;

        private void OnValidate()
        {
            if (!jaw)
            {
                Creature creature = this.GetComponentInParent<Creature>();
                if (creature && creature.animator)
                {
                    jaw = creature.animator.GetBoneTransform(HumanBodyBones.Jaw);
                }
            }
        }

    }
}
