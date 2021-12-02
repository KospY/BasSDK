using System;
using System.Collections.Generic;
using UnityEngine;


namespace ThunderRoad
{
    public class EventLoadLevel : MonoBehaviour
    {
        public string levelId;
        public string modeName;
        public List<LevelOption> levelOptions;

        public float fadeInDuration = 2;

        public LevelSaveOptions saveOptions;

        [Serializable]
        public class LevelOption
        {
            public string name;
            public double value;
        }

        public void LoadLevel()
        {
        }

        public void LoadLevel(string levelId)
        {
        }


    }
}
