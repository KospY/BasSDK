using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
	public class ItemModuleCrossbow : ItemModule
    {
        public float velocityMultiplier = 1.5f;
        public float baseShootVelocity = 20f;
        public float stringSpring = 1000;

        public static bool forceAutoSpawnBolt;
        public bool autoSpawnBolt;
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllItemID")] 
#endif
        public string boltProjectileID;

        public float pullRatioToLock = 0.95f;

#if ODIN_INSPECTOR
        [BoxGroup("Audio")] 
#endif
        public string audioPullAddress;

#if ODIN_INSPECTOR
        [BoxGroup("Audio")] 
#endif
        public string audioLockAddress;

#if ODIN_INSPECTOR
        [BoxGroup("Audio")] 
#endif
        public string audioShootAddress;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllItemID()
        {
            return Catalog.GetDropdownAllID(Category.Item);
        }

        public List<ValueDropdownItem<string>> GetAllEffectID()
        {
            return Catalog.GetDropdownAllID(Category.Effect);
        } 
#endif
    }
}
