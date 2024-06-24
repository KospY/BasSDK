---
parent: Levels
grand_parent: ThunderRoad
---
# Audio Loader

The Audio Loader is a script used to load audio from an addressables rather than placing the Audio Source in the scene directly. This script requires an Audio Source with this script, however it should be left blank (however you can adjust Audio settings (like volume, reverb filter etc)). With this, you can reference in-game audio addressables that may not be available to you in raw form, loading it straight from the addressables.

![AudioLoader][AudioLoader]

## Components

| Field                       | Description
| ---                         | ---
| Audio Clip Reference        | You can drag an audio source from your addressables to here. Will load this audio when in game.
| Use Audio Clip Address      | When ticked, does not use `Audio Clip Reference` and uses `Audio Clip Address` instead.
| Audio Clip Address          | Allows you to reference an Audio clip from the Addressables name. Allows external addressables from different bundles.
| Audio Mixer                 | Allows you to adjust volume for sound via in-game audio settings. See [AudioMixerLinker][AudioMixerLinker] for more information.

[AudioLoader]: {{ site.baseurl }}/assets/components/AudioLoader/AudioLoader.PNG
[AudioMixerLinker]: {{ site.baseurl }}{% link Components/ThunderRoad/Levels/AudioMixerLinker.md %}