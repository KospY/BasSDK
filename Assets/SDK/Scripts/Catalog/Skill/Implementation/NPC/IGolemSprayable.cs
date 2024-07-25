using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad.Skill.SpellMerge;
using UnityEngine;
using Object = UnityEngine.Object;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public interface IGolemSprayable
    {
        public abstract void GolemSprayStart(GolemSpray ability, out Action end);
    }
}
