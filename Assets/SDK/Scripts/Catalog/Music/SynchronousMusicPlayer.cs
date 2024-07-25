using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

public class SynchronousMusicPlayer : ThunderBehaviour
{
    private AudioSource[] sources = Array.Empty<AudioSource>();
    private AudioClip[] clips = Array.Empty<AudioClip>();
    private float[] targetVolumes;
    public float volumeMultiplier = 1;
    public float transition = 2f;

}
