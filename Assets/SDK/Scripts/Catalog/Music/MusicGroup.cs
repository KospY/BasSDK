using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class MusicGroup : CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Container")] 
#endif
        public List<MusicSegment> musicSegments = null;

#if ODIN_INSPECTOR
        [BoxGroup("Container")] 
#endif
        public bool shuffleMode = true;

#if ODIN_INSPECTOR
        [BoxGroup("Container")] 
#endif
        public int restartSequenceIndex = 0;

#if ODIN_INSPECTOR
        [BoxGroup("Music")] 
#endif
        public float volumeDb = 0.0f;

#if ODIN_INSPECTOR
        [BoxGroup("Music")] 
#endif
        public int BPM = 0;

#if ODIN_INSPECTOR
        [BoxGroup("Music")] 
#endif
        public int beatsPerBar = 0;

#if ODIN_INSPECTOR
        [BoxGroup("Music")] 
#endif
        public int barPerGrid = 0;

        private List<MusicSegment> _musicSegmentsOrder = null;
    }
}