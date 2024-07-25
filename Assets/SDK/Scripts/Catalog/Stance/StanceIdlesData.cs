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
    public abstract class StanceIdlesData<T> : StanceData where T : IdlePose
    {
#if ODIN_INSPECTOR
        [BoxGroup("Stance basics"), ListDrawerSettings(Expanded = true)]
#endif
        public List<T> idles = new List<T>();

    }
}
