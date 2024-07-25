using System;
using System.Collections;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
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
            [ValueDropdown(nameof(GetAllMusicGroupID))] 
#endif
            public string sourceGroup;

#if ODIN_INSPECTOR
            [BoxGroup("Transition")]
            [ValueDropdown(nameof(GetAllMusicGroupID))] 
#endif
            public string destinationGroup;

#if ODIN_INSPECTOR
            [BoxGroup("Transition"), ShowInInspector]
            [ValueDropdown(nameof(GetAllMusicGroupID))] 
#endif
            public string musicGroup;

#if ODIN_INSPECTOR
            [BoxGroup("TransitionIn")] 
#endif
            /// Number of beat/bar/grid  to wait  to start transition (0 mean on next beat/bar/grid)
            public int timeBeforeTransition;

#if ODIN_INSPECTOR
            [BoxGroup("TransitionIn")] 
#endif
            // Type to define multiplier with timeBeforeTransition
            public TransitionType transitionType;

#if ODIN_INSPECTOR
            [BoxGroup("PreTransition")] 
#endif
            // Number of beat/bar/grid to start pre-transition. (before transition time) In  this  timing both source music and transition will be play during this time
            public int timePreTransition;

#if ODIN_INSPECTOR
            [BoxGroup("PreTransition")]
#endif
            // Type to define multiplier with timePreTransition
            public TransitionType preTransitionType = TransitionType.Immediate;

#if ODIN_INSPECTOR
            [BoxGroup("TransitionOut")]
#endif
            /// Number of beat/bar/grid to wait after transition time to start destination music (0 mean on next beat/bar/grid)
            public int timeBeforeDestStart;

#if ODIN_INSPECTOR
            [BoxGroup("TransitionOut")]
#endif
            // Type to define multiplier with timeBeforeDestStart
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
        [ValueDropdown(nameof(GetAllMusicGroupID))] 
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