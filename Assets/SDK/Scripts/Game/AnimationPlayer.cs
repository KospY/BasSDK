using UnityEngine;
using System.Collections.Generic;
using System;

using EasyButtons;


public class AnimationPlayer : MonoBehaviour
{
    public List<AnimationInfo> anims;
    protected new Animation animation;

    [Serializable]
    public class AnimationInfo
    {
        public string name;
        public AnimationClip animationClip;
        [NonSerialized]
        public AnimationPlayer animationPlayer;

        [Button]
        public void Play()
        {
            animationPlayer.Play(name, 1, 0);
        }
        [Button]
        public void Play(float speed = 1, float frame = 0)
        {
            animationPlayer.Play(name, speed, frame);
        }
    }

    private void OnValidate()
    {
        foreach (AnimationInfo anim in anims)
        {
            if (!anim.animationClip.legacy)
            {
                Debug.LogError("Animation clip [" + anim.animationClip.name + "] is not set to legacy, please change animation type");
            }
        }
    }

    void Awake()
    {
        animation = this.gameObject.GetComponent<Animation>();
        if (!animation) animation = this.gameObject.AddComponent<Animation>();
        animation.playAutomatically = false;
        foreach (AnimationInfo anim in anims)
        {
            anim.animationPlayer = this;
            animation.AddClip(anim.animationClip, anim.name);
        }
    }

    [Button]
    public void Play(string animationName, float speed = 1, float frame = 0)
    {
        animation[animationName].speed = speed;
        animation[animationName].time = frame;
        animation.Play(animationName);
    }

    public AnimationState GetAnimationState(string animationName)
    {
        return animation[animationName];
    }
}
