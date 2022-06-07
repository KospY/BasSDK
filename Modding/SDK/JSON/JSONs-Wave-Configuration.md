# JSONs: Wave Configuration

_This page will be updated if there are any changes to the JSON. However, this will not be instant, and may take some time to update._

## Introduction

Waves are rounds of enemies that spawn when you select it in the Sandbox gamemode. There are many customizable options inside the wave JSON that allows the creator to adjust the waves to their pleasing, and can provide a variety of unique waves. _Please note that this JSON relies a lot on other JSONs, and those will not be explained fully here until their own page has been created._

_Please note that if the author does not understand a particular definition inside the JSON, it will be left out until the author finds out. These will be marked with <?>_

## Components

_Note: The JSON used for this is the Gladiator1_

"**$type**" : This is to reference the Wave data. Do not change this, it should be "ThunderRoad.WaveData, Assembly-CSharp,"

"**ID**" : This is the Unique Identifier for the wave. This must be unique to ensure that the ID does not conflict with other mods or the default game. For errors, the Console will be able to define what wave is at fault.

"**saveFolder**" : This is for internal use, and does not need to be changed. This can be left to Bas

"**version**" : This must match the in-game JSON version for the Wave JSON. At the current moment (Update 9) this should remain at "2". Do not change this.

"**category**" : The category defines what your list of waves will be under. For example, Gladiator 1 will be under the "Gladiator" category. The ordering of the categories goes 0-9 A-Z, these letters/numbers can be put at the start of these names and they will be hidden, however two-character starts will not work. As used in the Gladiator1 JSON category is "2Gladiator"

"**title**" : Title is the name of the wave itself under its category. This also applies with the ordering sequence of "category". In Gladiator1, the title is "Wave I"

"**description**" : Displays what information is displayed on the book when you select the wave

"**loop**" : When the wave ends, will it start again automatically? Wave length goes by how many groups are inside the JSON. Use "true" and "false" for this function

"**maxAlive**" : Displays how many characters can be alive at once. Only accepts numbers.

"**alwaysAvailable**" : Displays if this wave is available of all maps, regardless of "waveSelectors". Only accepts "true" and "false".

"**waveSelectors**" : Displays what maps these waves are available for. By default, this is set to:

"waveSelectors": [

    "Arena",

    "Ruins",

    "Market",

    "Canyon",

    "Citadel"

  ],

To add more to this list, add a comma after "Citadel" then underneath add your map ID, surrounded by quotation marks.

"**playerHealthMultiplier**" : Multiplies your health by this amount when the wave is started. So for example, 2.0 would double your health, 0.5 would half your health.

"**enemiesHealthMultiplier**" : Multiplies NPC health by this amount when wave is started. 

## Groups

Groups are the enemy information. The more there are, the longer the wave lasts.

"**references**" : This will reference the Creature part of the NPC. If you use the term "Creature" it will define the exact creature JSON, for example "Humans". If you use the term "Table" it will go from a "CreatureTable" JSON which lists different creatures. For example, "GladiatorsMelee".

"**referenceID**" : The ID of the reference used above.

"**overrideFaction**" : This is to ask if you want to override the Faction of the NPC. The faction of the NPC is already listed in the Creature/CreatureTable JSON, however this will be used to override those references. Only accepts "true" and "false"

"**factionID**" : This is the ID of the faction used. This is in a numberacle form;

* **-1** : Passive (does not attack anyone, will be attacked by other factions)
* **0** : None (will attack player and other factions)
* **1** : Ignore (Passive, not attacked by other factions)
* **2** : Player (will attack all other factions except for the player, also known as "friendly")
* **3** : Bandits (will attack all other factions except its own)
* **4** : Cult (will attack all other factions except its own)
* **5** : Mercenary (will attack all other factions except its own)

"**overrideContainer**" : This will override the container referenced by the Creature JSON

"**overrideContainerID**" : References the ID of the container JSON you want the NPC to use

"**overrideBrain**" : This will override the Brain JSON referenced by the Creature JSON

"**overrideBrainID**" : References the ID of the container JSON you want the NPC to use

"**overrideMaxMelee**2 : When set to true, it limits the amount of enemies that can attack you at a time. This limit is set by "overrideMaxMeleeCount" and "minMaxCount"

"**spawnPointIndex**" : The spawn point they spawn at (Like at arena there is 4, -1 sets them to all)

"**conditionStepIndex**" : Index of the spawn group to use as a condition for the group

"**conditionThreshold**" : How few must be left in order for the condition to be met
