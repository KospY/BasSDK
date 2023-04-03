using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class SpellData : CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Spell")] 
#endif
        public float manaConsumption;
#if ODIN_INSPECTOR
        [BoxGroup("Spell")] 
#endif
        public float activeManaConsumption;
#if ODIN_INSPECTOR
        [BoxGroup("Spell")] 
#endif
        public float manaWaste;
        [NonSerialized]
        public Level level;

        public class Level
        {
            public int castThrow;
            public int crystalShockwave;
        }

        public virtual void DrawGizmos() { }

        public virtual void DrawGizmosSelected() { }
    }
}
