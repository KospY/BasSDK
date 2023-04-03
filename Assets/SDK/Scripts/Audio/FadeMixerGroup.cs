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

    public void StartFade(int indexData)
    {
        if(indexData < 0)
        {
            Debug.LogError("indexData is <�0");
            return;
        }

        if(indexData >= fadeDataArray.Length)
        {
            Debug.LogError("indexData is >�fade data list");
            return;
        }

        fadeDataArray[indexData].NeedClear = true;
        StartCoroutine(StartFade(fadeDataArray[indexData].audioMixer, 
                                                fadeDataArray[indexData].exposedParam, 
                                                fadeDataArray[indexData].duration, 
                                                fadeDataArray[indexData].targetVolumeDb, 
                                                fadeDataArray[indexData].overrideInitialVolume,
                                                fadeDataArray[indexData].initialVolumeOverrideDb));
    }

    public IEnumerator StartFade(AudioMixer audioMixer, string exposedParam, float duration, float targetVolumeDb, bool overrideInitialVolume, float initialVolumeOverrideDb)
    {
        float currentTime = 0;
        float currentVol = initialVolumeOverrideDb;
        if(!overrideInitialVolume)
        {
            audioMixer.GetFloat(exposedParam, out currentVol);
        }

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetVolumeDb, currentTime / duration);
            audioMixer.SetFloat(exposedParam, newVol);
            yield return null;
        }
        yield break;
    }

    public void ClearExposedParameter()
    {
        for (int i = 0; i < fadeDataArray.Length; i++)
        {
            FadeData data = fadeDataArray[i];
            if (data.NeedClear)
            {
                data.audioMixer.ClearFloat(data.exposedParam);
            }
        }
    }

    private void OnDestroy()
    {
        ClearExposedParameter();
    }
}