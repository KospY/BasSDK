---
parent: Levels
grand_parent: ThunderRoad
---
# Audio Container

The audio container is a `ScriptableObject` which contains a `List` of `AudioClip` objects.

The intended use of the `AudioContainer` for sound effects is to group together similar sounds, a random one is chosen from the container when the effect is played. This helps to reduce repetitiveness in the sound effects.

The `AudioContainer` can be accessed directly by script mods to specifically choose the `AudioClip` they want to play.

## How to create an audio container
<video autoplay="autoplay" loop="loop">
  <source src="{{ site.baseurl }}/assets/components/AudioContainer/create-audio-container.mp4" type="video/mp4">
</video>