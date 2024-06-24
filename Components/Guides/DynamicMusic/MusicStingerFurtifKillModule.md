---
parent: DynamicMusic
grand_parent: Guides
---
# MusicStingerFurtifKillModule

Play an effect on player killing an NPC when no NPC are alert (player stealth).

Effect should have audio module. You can add a dynamyc music type for the effect play on next beat (so it’s according to the music rythm).

Copy this data and change red text with yours.

{
"$type": "ThunderRoad.MusicStingerFurtifKillModule, ThunderRoad",
"killType": "KillType",
"damageRatioThreshold": ratio,
"timeComboKill": time,
"stingerEffectIdList": [
"StingerIDCombo1",

"StingerIDCombo2",

"StingerIDCombo3"
]
}

**KillType** : Enum to check the kill type : 

**mele** : mele kill
**range** : from throwing item (arrow …)
**indirectCause** : indirect causes ( push the NPC and the fall kill him …)

**damageRatioThreshold** : ratio needed to triger. ratio is damage/NPC health. So 1 mean the damage cause need to be equal or higher than creature max health (only one shot would trigger).

**timeComboKill** : time to reset combo. each time triger play stingerEffect [comboIndex] then increment combo index. if no triger in timeComboKill index return to 0.

**stingerEffectIdList** : List of Effect to play.