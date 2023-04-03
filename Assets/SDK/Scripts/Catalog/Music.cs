using System;
using System.Collections;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class MusicDynamicModuleMap : Dictionary<string, MusicDynamicModule> { }

    [Serializable]
    public class Music : CatalogData
    {
        [Serializable]
        public class MusicTransition
        {
            public enum TransitionType
            {
                OnBeat,
                OnBar,
                Immediate,
                OnGrid
            }

#if ODIN_INSPECTOR
            [BoxGroup("Transition")]
            [ValueDropdown("GetAllMusicGroupID")] 
#endif
            public string sourceGroup;

#if ODIN_INSPECTOR
            [BoxGroup("Transition")]
            [ValueDropdown("GetAllMusicGroupID")] 
#endif
            public string destinationGroup;

#if ODIN_INSPECTOR
            [BoxGroup("Transition"), ShowInInspector]
            [ValueDropdown("GetAllMusicGroupID")] 
#endif
            public string musicGroup;

#if ODIN_INSPECTOR
            [BoxGroup("TransitionIn")] 
#endif
            public int timeBeforeTransition;

#if ODIN_INSPECTOR
            [BoxGroup("TransitionIn")] 
#endif
            public TransitionType transitionType;

#if ODIN_INSPECTOR
            [BoxGroup("TransitionOut")] 
#endif
            public int timeBeforeDestStart;

#if ODIN_INSPECTOR
            [BoxGroup("TransitionOut")] 
#endif
            public TransitionType transitionDestStartType;

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllMusicGroupID()
            {
                return Catalog.GetDropdownAllID(Category.MusicGroup);
            } 
#endif
        }

        [NonSerialized]
        public List<MusicGroup> loadedMusicGroup;

        public float volumeDb = 0.0f;

#if ODIN_INSPECTOR
        [BoxGroup("Music")]
        [ValueDropdown("GetAllMusicGroupID")] 
#endif
        public List<string> groupsToLoad;

#if ODIN_INSPECTOR
        [BoxGroup("Music")] 
#endif
        public List<MusicTransition> transitions;

#if ODIN_INSPECTOR
        [BoxGroup("Dynamic Module")]
        [ShowInInspector] 
#endif
        public MusicDynamicModuleMap dynamicModules = new MusicDynamicModuleMap();

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllMusicGroupID()
        {
            return Catalog.GetDropdownAllID(Category.MusicGroup);
        } 
#endif

    }
}