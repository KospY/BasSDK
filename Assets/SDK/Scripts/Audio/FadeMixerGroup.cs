using System.Collections;
using UnityEngine.Audio;
using UnityEngine;

public class FadeMixerGroup : MonoBehaviour
{
    [System.Serializable]
    public class FadeData
    {
        public AudioMixer audioMixer;
        public string exposedParam;
        public float duration;
        public float targetVolumeDb;
        public bool overrideInitialVolume;
        public float initialVolumeOverrideDb;

        private bool _needClear = false;
        public bool NeedClear
        {
            get { return _needClear; }
            set { _needClear = value; }
        }
    }

    public FadeData [] fadeDataArray;

}