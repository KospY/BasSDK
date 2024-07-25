using UnityEngine;
using UnityEngine.VFX;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class VfxPlayer : MonoBehaviour
    {
        public bool playOnStart;

        [Header("Default")]
        [InspectorName("ParticleSystem")]
        public ParticleSystem defaultParticleSystem;
        [InspectorName("VisualEffect")]
        public VisualEffect defaultVisualEffect;

        [Header("Mobile")]
        [InspectorName("ParticleSystem")]
        public ParticleSystem mobileParticleSystem;
        [InspectorName("VisualEffect")]
        public VisualEffect mobileVisualEffect;

        private void Awake()
        {
            if (Common.GetQualityLevel() == QualityLevel.Windows)
            {
                if (defaultParticleSystem) defaultParticleSystem.gameObject.SetActive(true);
                if (defaultVisualEffect) defaultVisualEffect.gameObject.SetActive(true);
                if (mobileParticleSystem) mobileParticleSystem.gameObject.SetActive(false);
                if (mobileVisualEffect) mobileVisualEffect.gameObject.SetActive(false);
            }
            else if (Common.GetQualityLevel() == QualityLevel.Android)
            {
                if (defaultParticleSystem) defaultParticleSystem.gameObject.SetActive(false);
                if (defaultVisualEffect) defaultVisualEffect.gameObject.SetActive(false);
                if (mobileParticleSystem) mobileParticleSystem.gameObject.SetActive(true);
                if (mobileVisualEffect) mobileVisualEffect.gameObject.SetActive(true);
            }
        }

        private void Start()
        {
            if (playOnStart)
            {
                Play();
            }
        }

        [Button]
        public void Play()
        {
            if (Common.GetQualityLevel() == QualityLevel.Windows)
            {
                if (defaultParticleSystem) defaultParticleSystem.Play();
                if (defaultVisualEffect) defaultVisualEffect.Play();
            }
            else if (Common.GetQualityLevel() == QualityLevel.Android)
            {
                if (mobileParticleSystem) mobileParticleSystem.Play();
                if (mobileVisualEffect) mobileVisualEffect.Play();
            }
        }

        [Button]
        public void Stop()
        {
            if (Common.GetQualityLevel() == QualityLevel.Windows)
            {
                if (defaultParticleSystem) defaultParticleSystem?.Stop();
                if (defaultVisualEffect) defaultVisualEffect?.Stop();
            }
            else if (Common.GetQualityLevel() == QualityLevel.Android)
            {
                if (mobileParticleSystem) mobileParticleSystem.Stop();
                if (mobileVisualEffect) mobileVisualEffect.Stop();
            }
        }
    }
}
