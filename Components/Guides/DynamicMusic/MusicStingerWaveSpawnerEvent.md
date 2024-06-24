---
parent: DynamicMusic
grand_parent: Guides
---
# MusicStingerWaveSpawnerEvent

Play an effect on wave event selected.

Effect should have audio module. You can add a dynamyc music type for the effect play on next beat (so it’s according to the music rythm).

Copy this data and change red text with yours.

{
"$type": "ThunderRoad.MusicStingerWaveSpawnerEvent, ThunderRoad",
"_waveEventType": "WaveEnumTypeEvent",
"stingerEffectIdList": [
"effectId"
]
}

**_waveEventType** : an enum to define the type of event :

**WaveBegin** : When wave start.
**WaveAnyEndEvent** = When wave end.
**WaveWin** = When wave ended as won.
**WaveLost** = When wave ended as lost.
**WaveCancel** = When wave ended as canceled.
**WaveLoop** = when a new wave start.