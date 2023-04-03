using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
	public class ItemModuleBow : ItemModule
    {
        public float velocityMultiplier = 1.5f;
        public float stringSpring = 500;

        public static bool forceAutoSpawnArrow;
        public bool autoSpawnArrow;
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllItemID")] 
#endif
        public string arrowProjectileID;

        public float audioShootMinPull = 0.1f;
        public float audioDrawPullSpeed = 0.02f;

#if ODIN_INSPECTOR
        [BoxGroup("Audio")] 
#endif
        public string audioStringAddress;

#if ODIN_INSPECTOR
        [BoxGroup("Audio")] 
#endif
        public string audioGroupDrawAddress;

#if ODIN_INSPECTOR
        [BoxGroup("Audio")] 
#endif
        public string audioGroupShootAddress;

#if ODIN_INSPECTOR
        [BoxGroup("AI")] 
#endif
        public float maxPullRatioAI = 0.75f;

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
