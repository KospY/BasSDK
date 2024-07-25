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
    public class MeleeIdle : IdlePose
    {
#if ODIN_INSPECTOR
        [LabelWidth(150), HorizontalGroup("$prettifiedID/Horiz"), VerticalGroup("$prettifiedID/Horiz/Fields"), HorizontalGroup("$prettifiedID/Horiz/Fields/Row3", Order = 2, Width = 165)]
#endif
        public bool requireThrustWeapon = false;

        public override bool showDifficulty => true;

        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly, ShowIf("isPlaying")]
#endif
        public List<AttackMotion> attackMotions;
        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly, ShowIf("isPlaying")]
#endif
        public AttackMotion lastPick;

        [NonSerialized]
        public float minRange = 0f;
        [NonSerialized]
        public float maxRange = 0f;
        [NonSerialized]
        public float minReachRange = 0f;
        [NonSerialized]
        public float maxReachRange = 0f;



    }
}
