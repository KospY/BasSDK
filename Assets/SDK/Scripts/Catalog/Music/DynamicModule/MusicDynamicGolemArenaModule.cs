using System;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif //ODIN_INSPECTOR

namespace ThunderRoad
{
    public class GolemMusicMapping : Dictionary<Golem.State, string> { }


    [Serializable]
    public class MusicDynamicGolemArenaModule : MusicDynamicStingerModule
    {
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllEffectID))]
#endif //ODIN_INSPECTOR
        public string headshotStingerId;
        [NonSerialized]
        public EffectData headshotStinger;
        
        public List<Golem.State> stingerOnGolemStateChange;
        public GolemMusicMapping golemStateActiveMusicId = new GolemMusicMapping();

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllEffectID() => Catalog.GetDropdownAllID(Category.Effect);
#endif //ODIN_INSPECTOR


        public string[] dynamicModuleToDisconnectOnGolemActivation;
        public string[] dynamicModuleToConnectOnGolemActivation;

    }
}
