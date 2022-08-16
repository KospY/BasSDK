using System;
using System.Collections.Generic;
using UnityEngine;


namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/EventLoadLevel")]
    public class EventLoadLevel : MonoBehaviour
    {
        public string levelId;
        public string modeName;
        public List<LevelOption> levelOptions;

        public float fadeInDuration = 2;

        public bool forceSavePlayerInventory;

        [Serializable]
        public class LevelOption
        {
            public string name;
            public string value;
        }

        public void LoadLevel()
        {
        }

        public void LoadLevel(string levelId)
        {
        }


    }
}
