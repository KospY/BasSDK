---
parent: DynamicMusic
grand_parent: Guides
---
# MusicDynamicPlayerBrainTransitionModule

This module will change music type depending on the player brain exposure change. 
So when a player get attack or detected.

Copy this data and change red text with yours.

{
"$type": "ThunderRoad.MusicDynamicPlayerBrainTransitionModule, ThunderRoad",
"exposureMusicGroupId": {
"$type": "ThunderRoad.BrainMusicMapping, ThunderRoad",
"None": "music group type ID", 
"Danger":  "music group type ID", 
"Alert": "music group type ID", 
"RangedCombat": "music group type ID",
"CloseCombat":  "music group type ID", 
}

Player brain exposure type : 

**None**: No NPC near the player.
**Danger**: NPC near the player but none of them are alert (player not detected) .
**Alert**: At least 1 NPC is near the player is alert.
**RangedCombat**: If not in “CloseCombat” mode, start when an NPC attack the player from range and stop when no more NPC in alert near.
**CloseCombat**: Start when an NPC attack the player in close combat and stop when no more NPC in alert near.