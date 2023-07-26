using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class EffectModuleAudio : EffectModule
    {
#if ODIN_INSPECTOR
        [TabGroup("Tab", "Audio"), BoxGroup("Tab/Audio/General")]
#endif
        public string audioContainerAddress;
        [NonSerialized]
        public AudioContainer audioContainer;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/General")]
#endif
        public float loopFadeDelay = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/General")]
#endif
        public AudioMixerName audioMixer = AudioMixerName.Effect;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/General"), Range(0, 1)]
#endif
        public float spatialBlend = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/General")]
#endif
        public bool globalOnPlayerOnly = false;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/General")]
#endif
        public float playDelay;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/General")]
#endif
        public float dopplerLevel = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/General")]
#endif
        public bool useAudioForHaptic;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Smoothing")]
#endif
        public int intensitySmoothingSampleCount = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Smoothing")]
#endif
        public int speedSmoothingSampleCount = 0;

#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Volume")]
#endif
        public float cullMinVolume = 0.1f;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Volume")]
#endif
        public float volumeDb = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Volume")]
#endif
        public bool useVolumeIntensity;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Volume"), ShowIf("useVolumeIntensity")]
#endif
        public AnimationCurve volumeIntensityCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Volume")]
#endif
        public bool useVolumeSpeed;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Volume"), ShowIf("useVolumeSpeed")]
#endif
        public AnimationCurve volumeSpeedCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Volume"), ShowIf("useVolumeIntensity"), ShowIf("useVolumeSpeed")]
#endif
        public BlendMode volumeBlendMode = BlendMode.Min;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Volume")]
#endif
        public float reverbZoneMix = 1.0f;

#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/AI")]
#endif
        public bool isNoiseForAI;

#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Dynamic Music")]
#endif
        public bool onDynamicMusic;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Dynamic Music"), ShowIf("onDynamicMusic")]
#endif
        public Music.MusicTransition.TransitionType dynamicMusicTiming;

#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Random play")]
#endif
        public bool randomPlay;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Random play"), ShowIf("randomPlay")]
#endif
        public float randomMinTime = 2;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Random play"), ShowIf("randomPlay")]
#endif
        public float randomMaxTime = 5;

#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Pitch")]
#endif
        public bool randomPitch;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Pitch")]
#endif
        public EffectLink pitchEffectLink = EffectLink.Intensity;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Pitch")]
#endif
        public AnimationCurve pitchCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Low pass filter")]
#endif
        public bool useLowPassFilter;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Low pass filter"), ShowIf("useLowPassFilter")]
#endif
        public EffectLink lowPassEffectLink = EffectLink.Intensity;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Low pass filter"), ShowIf("useLowPassFilter")]
#endif
        public AnimationCurve lowPassCutoffFrequencyCurve = new AnimationCurve(new Keyframe(0, 22000), new Keyframe(1, 22000));
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Low pass filter"), ShowIf("useLowPassFilter")]
#endif
        public AnimationCurve lowPassResonanceQCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/High pass filter")]
#endif
        public bool useHighPassFilter;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/High pass filter"), ShowIf("useHighPassFilter")]
#endif
        public EffectLink highPassEffectLink = EffectLink.Intensity;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/High pass filter"), ShowIf("useHighPassFilter")]
#endif
        public AnimationCurve highPassCutoffFrequencyCurve = new AnimationCurve(new Keyframe(0, 10), new Keyframe(1, 10));
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/High pass filter"), ShowIf("useHighPassFilter")]
#endif
        public AnimationCurve highPassResonanceQCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Reverb filter")]
#endif
        public bool useReverbFilter;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Reverb filter"), ShowIf("useReverbFilter")]
#endif
        public EffectLink reverbEffectLink = EffectLink.Intensity;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Reverb filter"), ShowIf("useReverbFilter")]
#endif
        public AnimationCurve reverbDryLevelCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));

#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Distance")]
#endif
        public float minDistance = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Tab/Audio/Distance")]
#endif
        public float maxDistance = 500;

        /*
        [TabGroup("Tab", "Steam Audio")]
        public bool overrideSteamAudio = false;
        [BoxGroup("Tab/Steam Audio/Direct Sound"), ShowIf("overrideSteamAudio", true)]
        public bool steamDirectBinaural = true;
        [BoxGroup("Tab/Steam Audio/Direct Sound"), ShowIf("directBinaural", true), ShowIf("overrideSteamAudio", true)]
        public SteamAudio.HRTFInterpolation steamInterpolation = SteamAudio.HRTFInterpolation.Nearest;
        [BoxGroup("Tab/Steam Audio/Direct Sound"), ShowIf("overrideSteamAudio", true)]
        public bool steamPhysicsBasedAttenuation = true;
        [BoxGroup("Tab/Steam Audio/Direct Sound"), ShowIf("overrideSteamAudio", true)]
        public SteamAudio.OcclusionMode steamOcclusionMode = SteamAudio.OcclusionMode.OcclusionWithFrequencyDependentTransmission;
        [BoxGroup("Tab/Steam Audio/Direct Sound"), ShowIf("overrideSteamAudio", true)]
        public SteamAudio.OcclusionMethod steamOcclusionMethod = SteamAudio.OcclusionMethod.Partial;
        [BoxGroup("Tab/Steam Audio/Direct Sound"), Range(0.1f, 10.0f), ShowIf("overrideSteamAudio", true)]
        public float steamSourceRadius = 1.0f;
        [BoxGroup("Tab/Steam Audio/Direct Sound"), Range(2, 256), ShowIf("overrideSteamAudio", true)]
        public int steamOcclusionSamples = 16;
        [BoxGroup("Tab/Steam Audio/Direct Sound"), ShowIf("overrideSteamAudio", true)]
        public bool steamAirAbsorption = true;
        [BoxGroup("Tab/Steam Audio/Direct Sound"), Range(0.0f, 1.0f), ShowIf("overrideSteamAudio", true)]
        public float steamDirectMixLevel = 1.0f;

        [BoxGroup("Tab/Steam Audio/Directivity"), Range(0.0f, 1.0f), ShowIf("overrideSteamAudio", true)]
        public float steamDipoleWeight = 0.0f;
        [BoxGroup("Tab/Steam Audio/Directivity"), Range(0.0f, 4.0f), ShowIf("overrideSteamAudio", true)]
        public float steamDipolePower = 0.0f;

        [BoxGroup("Tab/Steam Audio/Indirect Sound"), ShowIf("overrideSteamAudio", true)]
        public bool steamReflections = true;
        [BoxGroup("Tab/Steam Audio/Indirect Sound"), ShowIf("reflections", true), ShowIf("overrideSteamAudio", true)]
        public SteamAudio.SourceSimulationType steamSimulationType = SteamAudio.SourceSimulationType.Realtime;
        [BoxGroup("Tab/Steam Audio/Indirect Sound"), ShowIf("reflections", true), ShowIf("overrideSteamAudio", true)]
        public bool steamPhysicsBasedAttenuationForIndirect = true;
        [BoxGroup("Tab/Steam Audio/Indirect Sound"), Range(0.0f, 10.0f), ShowIf("reflections", true), ShowIf("overrideSteamAudio", true)]
        public float steamIndirectMixLevel = 0.5f;
        [BoxGroup("Tab/Steam Audio/Indirect Sound"), ShowIf("reflections", true), ShowIf("overrideSteamAudio", true)]
        public bool steamIndirectBinaural = true;
        */

        public static void Despawn(EffectAudio effect)
        {
            GameObject.Destroy(effect.gameObject);
        }



        public EffectAudio Configure(EffectAudio effectAudio)
        {

            if (step == Effect.Step.Custom) effectAudio.stepCustomHashId = stepCustomIdHash;

            effectAudio.useVolumeIntensity = useVolumeIntensity;
            effectAudio.useVolumeSpeed = useVolumeSpeed;
            effectAudio.volumeBlendMode = volumeBlendMode;
            effectAudio.highPassEffectLink = highPassEffectLink;
            effectAudio.lowPassEffectLink = lowPassEffectLink;
            effectAudio.reverbEffectLink = reverbEffectLink;
            effectAudio.pitchEffectLink = pitchEffectLink;
            effectAudio.audioSource.minDistance = minDistance;
            effectAudio.audioSource.maxDistance = maxDistance;
            effectAudio.audioSource.pitch = 1;
            effectAudio.audioSource.volume = 1;
            effectAudio.audioSource.spatialBlend = spatialBlend;
            effectAudio.loopFadeDelay = loopFadeDelay;
            effectAudio.audioContainer = audioContainer;
            effectAudio.randomPitch = randomPitch;
            effectAudio.audioSource.dopplerLevel = dopplerLevel;
            effectAudio.doNoise = isNoiseForAI;
            effectAudio.playDelay = playDelay;
            effectAudio.onDynamicMusic = onDynamicMusic;
            effectAudio.dynamicMusicTiming = dynamicMusicTiming;
            effectAudio.randomPlay = randomPlay;
            effectAudio.randomMaxTime = randomMaxTime;
            effectAudio.randomMinTime = randomMinTime;

            if (volumeIntensityCurve != null) effectAudio.volumeIntensityCurve = volumeIntensityCurve;
            else effectAudio.volumeIntensityCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

            if (volumeSpeedCurve != null) effectAudio.volumeSpeedCurve = volumeSpeedCurve;
            else effectAudio.volumeSpeedCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

            if (pitchCurve != null) effectAudio.pitchCurve = pitchCurve;
            else effectAudio.pitchCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

            /*
            if (effectAudio.steamAudioSource)
            {
                effectAudio.steamAudioSource.airAbsorption = steamAirAbsorption;
                effectAudio.steamAudioSource.dipolePower = steamDipolePower;
                effectAudio.steamAudioSource.dipoleWeight = steamDipoleWeight;
                effectAudio.steamAudioSource.directBinaural = steamDirectBinaural;
                effectAudio.steamAudioSource.directMixLevel = steamDirectMixLevel;
                effectAudio.steamAudioSource.indirectBinaural = steamIndirectBinaural;
                effectAudio.steamAudioSource.indirectMixLevel = steamIndirectMixLevel;
                effectAudio.steamAudioSource.interpolation = steamInterpolation;
                effectAudio.steamAudioSource.occlusionMethod = steamOcclusionMethod;
                effectAudio.steamAudioSource.occlusionMode = steamOcclusionMode;
                effectAudio.steamAudioSource.occlusionSamples = steamOcclusionSamples;
                effectAudio.steamAudioSource.physicsBasedAttenuation = steamPhysicsBasedAttenuation;
                effectAudio.steamAudioSource.physicsBasedAttenuationForIndirect = steamPhysicsBasedAttenuationForIndirect;
                effectAudio.steamAudioSource.reflections = steamReflections;
                effectAudio.steamAudioSource.simulationType = steamSimulationType;
                effectAudio.steamAudioSource.sourceRadius = steamSourceRadius;
            }
            */

            effectAudio.useLowPassFilter = useLowPassFilter;
            effectAudio.useHighPassFilter = useHighPassFilter;
            effectAudio.useReverbFilter = useReverbFilter;


            // Adding audio filters has a performance cost when playing audio - even if the filters are disabled.
            // Only add the filter components if the effectModuleAudio says it needs it.
            if (useLowPassFilter)
            {
                if (!effectAudio.lowPassFilter)
                {
                    effectAudio.lowPassFilter = effectAudio.gameObject.AddComponent<AudioLowPassFilter>();
                    effectAudio.lowPassFilter.enabled = true;
                }
                effectAudio.lowPassCutoffFrequencyCurve = lowPassCutoffFrequencyCurve;
                effectAudio.lowPassResonanceQCurve = lowPassResonanceQCurve;
            }

            if (useHighPassFilter)
            {
                if (!effectAudio.highPassFilter)
                {
                    effectAudio.highPassFilter = effectAudio.gameObject.AddComponent<AudioHighPassFilter>();
                    effectAudio.highPassFilter.enabled = true;
                }
                effectAudio.highPassCutoffFrequencyCurve = highPassCutoffFrequencyCurve;
                effectAudio.highPassResonanceQCurve = highPassResonanceQCurve;
            }

            if (useReverbFilter)
            {
                if (!effectAudio.highPassFilter)
                {
                    effectAudio.reverbFilter = effectAudio.gameObject.AddComponent<AudioReverbFilter>();
                    effectAudio.reverbFilter.enabled = true;
                    effectAudio.reverbFilter.reverbPreset = AudioReverbPreset.Off;
                }
                effectAudio.reverbDryLevelCurve = reverbDryLevelCurve;
            }

            return effectAudio;
        }
    }

}
