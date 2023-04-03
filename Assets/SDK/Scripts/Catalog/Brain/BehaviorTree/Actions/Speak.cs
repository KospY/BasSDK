using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad.AI.Action
{
	public class Speak : ActionNode
    {
        public enum PlayMode
        {
            Default,
            WithCooldown,
            OnlyOnce,
        }
        public string dialogId;
        public bool abnormalNoise = false;
        public bool checkMuffled = true;
        public PlayMode playMode = PlayMode.Default;
        public bool ignoreIfFromWave = true;
#if ODIN_INSPECTOR
        [ShowIf("playMode", optionalValue: PlayMode.WithCooldown)]
#endif
        public Vector2 cooldownMinMax = new Vector2(3f, 5f);
#if ODIN_INSPECTOR
        [ShowIf("playMode", optionalValue: PlayMode.WithCooldown)]
#endif
        public bool succeedOnCooldown = true;

    }
}
