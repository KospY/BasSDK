using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace ThunderRoad
{
    public class LevelLossController : ThunderBehaviour
    {
        public List<string> behaviourAddresses = new List<string>();

        protected Level level;
        protected Coroutine behaviourInProgress;
        protected string inProgressAddress;

        public delegate void LossTriggered(string lossAddress, EventTime eventTime);
        public event LossTriggered OnLossEvent;

        protected internal event Action<string> activateEventGroupers;

        private void Start()
        {
            
        }

        protected virtual void OnPossessionEvent(Creature creature, EventTime eventTime)
        {
            
        }


        public void InvokeLevelLossBehaviour(string address)
        {
            
        }

        public void EndMusic()
        {
            
        }

        public void ResumeMusic()
        {
            
        }

        public void EndRunningWave()
        {
            
        }

        public void StartSlowmo(string effectID)
        {
            
        }

        public void EndSlowmo()
        {
            
        }

        protected void ShowPopup(LevelLossBehaviour behaviour, string messageText)
        {
            
        }

        protected void HidePopup(LevelLossBehaviour behaviour)
        {
            
        }

        public static void CleanLevel() => Level.current.StartCoroutine(CleanCoroutine(false));

        protected static IEnumerator CleanCoroutine(bool wait)
        {
            yield break;
        }

        public void RespawnPlayer()
        {
            
        }
    }
}
