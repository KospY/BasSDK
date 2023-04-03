using UnityEngine;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/AnimatorBundle")]
    [CreateAssetMenu(menuName = "ThunderRoad/Animation/Animator controller bundle")]
    public class AnimatorBundle : ScriptableObject
    {
        public RuntimeAnimatorController controller;
        public AnimationClip dynamicStartReplaceClipA;
        public AnimationClip dynamicStartReplaceClipB;
        public AnimationClip dynamicLoopReplaceClip;
        public AnimationClip dynamicEndReplaceClip;
        public AnimationClip upperBodyDynamicOneShotReplaceClipA;
        public AnimationClip upperBodyDynamicOneShotReplaceClipB;
        public AnimationClip upperBodyDynamicLoopReplaceClip;
        public AnimationClip subStanceAReplaceClip;
        public AnimationClip subStanceBReplaceClip;
    }
}
