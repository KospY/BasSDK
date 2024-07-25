using UnityEngine;
using UnityEngine.Audio;

namespace ThunderRoad
{
    public static class SnapshotTool
    {
        #region Fields
        public static AudioMixerSnapshot currentSnapshot;
        public static float transitionEndTime = 0.0f;
        #endregion Fields

        public static bool InTransition
        {
            get
            {
                return Time.unscaledTime < transitionEndTime;
            }
        }
        public static void DoSnapshotTransition(AudioMixerSnapshot snapshot, float timeTransition)
        {
            snapshot.TransitionTo(timeTransition);
            currentSnapshot = snapshot;
            transitionEndTime = Time.unscaledTime + timeTransition;
        }
    }
}
