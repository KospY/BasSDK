using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Playables;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
	public class TestEffectManager : MonoBehaviour
    {
        public List<EffectCollection> effects = new List<EffectCollection>();


            [System.Serializable]
        public class EffectCollection
        {
            public string name = "Effect Name";
            public List<TestEffect> effects = new List<TestEffect>();

            [Header("Timelines")]
            public PlayableDirector director;


            [Button]
            public void Play_Pause()
            {
                if (director == null) { return; }

                if (director.state == PlayState.Playing)
                    director.Pause();
                else
                    director.Play();
            }

            [Button]
            public void Stop()
            {
                if (director == null) { return; }
                director.Stop();
                foreach (TestEffect effect in effects)
                {
                    effect.Stop();
                }
            }


            [Range(0, 1)]
            public float intensity = 1f;
            private float lastIntensity;

            public void CheckIntensityChange()
            {
                if (intensity == lastIntensity) { return; }
                foreach (TestEffect effect in effects)
                {
                    effect.intensity = intensity;
                    effect.OnValidate();
                }

                lastIntensity = intensity;
            }
        }

        private void Update()
        {
            foreach (EffectCollection effect in effects)
            {
                effect.CheckIntensityChange();
            }
        }


    }
}
