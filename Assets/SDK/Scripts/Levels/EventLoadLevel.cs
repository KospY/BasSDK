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
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Levels/EventLoadLevel.html")]
    public class EventLoadLevel : MonoBehaviour
    {
        //mock the LoadingCamera state
        public class LoadingCamera {
            public enum State
            {
                Disabled,
                Black,
                White,
                Enabled,
                EnabledNoTips,
                Error,
            }
        }
        public string levelId;
        public string modeName;
        public List<LevelOption> levelOptions;

        public float fadeInDuration = 2;
        public LoadingCamera.State loadingUIState = LoadingCamera.State.Enabled;

        [Serializable]
        public class LevelOption
        {
            public string name;
            public string value;
        }

#if ProjectX
        [Button]
#endif
        public void LoadLevel()
        {
        }

    }
}
