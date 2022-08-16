using UnityEngine;
using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/SpellCaster")]
    [AddComponentMenu("ThunderRoad/Creatures/Spell caster")]
    public class SpellCaster : MonoBehaviour
    {
        public Transform fire;
        public Transform magicSource;
        public Transform rayDir;

    }
}