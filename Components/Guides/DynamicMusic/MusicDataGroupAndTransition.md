---
parent: DynamicMusic
grand_parent: Guides
---
# Music Data : Group and transition

Create a musicData JSon in the Musics folder in your mod data. (copy one from game data)

Set up the Music ID.

**"volumeDb"**: the volume in dB (will be multiply  to the one from group and segment) (in decibels)

**"groupsToLoad**": List of all music group IDs that can be played (music type and transition)

**"transitions"**: List of transition between one type to an other. Note that the empty type (silence) is always the first one in play so you will need transition for this type also.

## Transition data :

**"sourceGroup"**: ID of the music group use for music type from. use “” if it’s from silence.

**"destinationGroup"**: ID of the music group use for music type to destination. use “” if it’s from silence.

**"musicGroup"**: ID of the music groupe that does the transition.

**"timeBeforeTransition"**: X number of beat/bar/grid to wait before transition start when ask for transition. Depend on source group Data (BPM).
Note that source group will STOP playing when transition start.

**"transitionType":**

"Immediate" : will start the transition when ask (always use this if it’s from silence)
”OnBeat” : Transition will start on X beat. If X is 0 transition will start on next beat.
”OnBar” : Transition will start on X bar. If X is 0 transition will start on next bar.
”OnGrid” : Transition will start on X grid. If X is 0 transition will start on next grid.

**"timeBeforeDestStart**": Y number of beat/bar/grid to wait once transition segment started to play. Depend on transition group Data (BPM).
Note that transition segment will CONTINUE playing until it end. So you can have transition and destination sources.
**"transitionDestStartType":** 

**"Immediate"** : will start the transition when ask (always use this if it’s from silence)
**”OnBeat”** : Transition will start on Y beat. If Y is 0 next group will start immediatly.
**”OnBar”** : Transition will start on Y bar. If Y is 0 next group will start immediatly.
**”OnGrid”** : Transition will start on Y grid. If Y is 0 next group will start immediatly.

![Untitled](Music%20Data%20Group%20and%20transition%20bc7eef1175c74fe78134da8adfb9ff80/Untitled.png)