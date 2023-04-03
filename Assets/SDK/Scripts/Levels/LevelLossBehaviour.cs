using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace ThunderRoad
{
    [CreateAssetMenu(menuName = "ThunderRoad/Level/Loss Behaviour")]
    public class LevelLossBehaviour : ScriptableObject
    {
        [Flags]
        public enum PopupOptions
        {
            SkippablePopup = 0,
            RespawnButton = 1,
            ReloadLevel = 2,
            ReloadSameSeed = 4,
            LoadHome = 8,
        }

        public enum StepType
        {
            EndRunningWave,
            EndMusic,
            ResumeMusic,
            PlayEffect,
            EnableGrayscale,
            DisableGrayscale,
            FadeToBlack,
            FadeIn,
            StartSlowmo,
            StartSlowmoWithEffect,
            EndSlowmo,
            DisableLocomotion,
            EnableLocomotion,
            ShowPopup,
            HidePopup,
            Wait,
            RealtimeWait,
            Clean,
            WaitClean,
            Respawn,
            CleanRespawn,
            Reload,
            ReloadSameSeed,
            LoadHome,
            ActivateEventGroup,
        }

        [System.Serializable]
        public class Step
        {
            public StepType action;
            public string parameter;

            public Step(StepType t)
            {
                action = t;
            }

            public Step(StepType t, string p)
            {
                action = t;
                parameter = p;
            }
        }

        public PopupOptions popupOptions = PopupOptions.ReloadLevel | PopupOptions.LoadHome;
        public List<Step> actionSteps = new List<Step>()
        {
            new Step(StepType.EndMusic),
            new Step(StepType.EndRunningWave),
            new Step(StepType.DisableLocomotion),
            new Step(StepType.StartSlowmoWithEffect, "Death"),
            new Step(StepType.EnableGrayscale),
            new Step(StepType.ShowPopup,"{YouAreDead}"),
            new Step(StepType.RealtimeWait, "10"),
            new Step(StepType.EndSlowmo),
        };

        public static Dictionary<string, LevelLossBehaviour> loadedLossBehaviours;

        public static void LoadAll(List<string> addresses)
        {
            loadedLossBehaviours ??= new Dictionary<string, LevelLossBehaviour>();
            foreach (string address in addresses)
            {
                if (!loadedLossBehaviours.ContainsKey(address))
                {
                    Catalog.LoadAssetAsync<LevelLossBehaviour>(address, behaviour =>
                    {
                        loadedLossBehaviours[address] = behaviour;
                    }, "LossBehaviourLoad");
                }
            }
        }
    }
}
