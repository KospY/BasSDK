using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using ThunderRoad.Skill.SpellPower;
using UnityEngine.Profiling;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class LevelModuleCleaner : LevelModule
    {
        public static int cleanMaxDeadCount = 6;
        public static int cleanMaxDropCount = 8;
        public static bool cleanVisibleDead = false;
        public static bool cleanVisibleDroppedItems = false;
        public float cleanerRate = 5;

    }
}
