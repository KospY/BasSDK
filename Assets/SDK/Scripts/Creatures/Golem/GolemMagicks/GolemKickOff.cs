using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [CreateAssetMenu(menuName = "ThunderRoad/Creatures/Golem/Kick off config")]
    public class GolemKickOff : GolemAbility
    {
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("General")]
#endif
        public GolemController.AttackMotion motion;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("General")]
#endif
        public bool launchClimbers = true;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("General")]
#endif
        public float launchSpeed = 2f;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("General")]
#endif
        public bool launchVertical = false;

    }
}
