using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [RequireComponent(typeof(PlayableDirector))]
    public class PlayablePlatformBinder : MonoBehaviour
    {
        public string trackName;
        public Object referencePC;
        public Object referenceAndroid;

        private Object track;

        void Awake()
        {
            BindDependingOnPlatform();
        }

        void FindTrack()
        {
            var playableDirector = GetComponent<PlayableDirector>();
            if (playableDirector != null && playableDirector.playableAsset != null)
            {
                var bindings = playableDirector.playableAsset.outputs.ToArray();
                for (int i = 0; i < bindings.Length; i++)
                {
                    Debug.Log(bindings[i].streamName);
                    if (bindings[i].streamName == trackName)
                    {
                        track = bindings[i].sourceObject;
                        break;
                    }
                }
            }
        }

        [Button]
        public void BindDependingOnPlatform()
        {
            FindTrack();
            PlayableDirector playableDirector = GetComponent<PlayableDirector>();
            if (Common.GetQualityLevel() == QualityLevel.Windows)
            {
                playableDirector.SetGenericBinding(track, referencePC);
            }
            else if (Common.GetQualityLevel() == QualityLevel.Android)
            {
                playableDirector.SetGenericBinding(track, referenceAndroid);
            }
            else
            {
                playableDirector.SetGenericBinding(track, referencePC);
            }
        }
    }
}