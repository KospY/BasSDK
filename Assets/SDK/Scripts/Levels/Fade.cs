using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class Fade : MonoBehaviour
    {
        public float fadeInDuration = 2;
        public float fadeOutDuration = 2;
        public Target target = Target.CameraAndAudio;

        public enum Target
        {
            CameraOnly,
            AudioOnly,
            CameraAndAudio,
        }

        public void Begin()
        {
        }

        public void End()
        {
        }
        public void BeginCamera()
        {
        }

        public void EndCamera()
        {
        }

        public void BeginAudio()
        {
        }

        public void EndAudio()
        {
        }

    }
}