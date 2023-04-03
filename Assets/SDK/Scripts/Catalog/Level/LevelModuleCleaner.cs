using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Profiling;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
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
