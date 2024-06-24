---
parent: DynamicMusic
grand_parent: Guides
---
# Music Group

Dynamic music is composed of different music groups. 
There is two type of groups:

- The music types that loop over and over until type change
- the transition one that are played on type change (wave start and end). They are both made the same way.

A music Group is always composed of segments which share the same BPM, beats per Bar and per Grid.

Each segment of a group will be play with no interruption. The order can be set by hand or randomized. You can also use only one long segment that will loop. (this will reduce variation in the music however)

Groups for transition usually use only one segment. (if multiple, only one will be randomized, but doesn’t loop)

To create a Music Group first you need to have all your audio clips (music segments) set as addressables.

Then create a JSON (copy one from game data) and edit:

**id** : The id of your musicgroup (will be used to specify which group to play)

**musicSegments** : List of music segments 
"**$type"**:  DO NOT CHANGE (should be "ThunderRoad.MusicSegment, ThunderRoad")
**"volumeDb"**: Change the volume of the segment in dB (decibels)
**"musicAddress":** Addressables of your musics,
**"timesPlayedInSerie"**: if played in serie the number of time to repeat this segment before switching to next segment

"**volumeDb**": Change the volume of next the segment in dB (decibels),
**"musicAddress"**: Addressables of your next segment,
**"timesPlayedInSerie"**: if played in series, the number of times it takes to repeat this segment before switching to next segment

**"shuffleMode"**: if true, it will randomly shuffle between segments. If false, segments will be played in order.
**"restartSequenceIndex"**: if play in series, use this index to ignore all segments before this index when looping (So the segment before this index will be played only once when group is selected). Use 0 if you don’t need this.
**"volumeDb**": the volume dB for all segments (multiply to the segment specific ones)
**"BPM"**: the BPM of the music group (all segments needs to have the same)
**"beatsPerBar"**: the beats per bar of the music group (all segments needs to have the same)
**"barPerGrid"**: the bar per Grid of the music group (all segments needs to have the same)
**"SegmentOrderCount": do not use**