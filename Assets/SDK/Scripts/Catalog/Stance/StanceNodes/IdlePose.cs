using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [System.Serializable]
    public abstract class IdlePose : StanceNode
    {
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly, ShowIf("isPlaying")]
#endif
        public List<IdlePose> nextIdles;

    }
}
