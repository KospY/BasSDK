using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public abstract class StanceData : CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Stance basics")]
#endif
        public BrainModuleStance.Stance baseStance = BrainModuleStance.Stance.Melee1H;

    }
}
