using System;

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
