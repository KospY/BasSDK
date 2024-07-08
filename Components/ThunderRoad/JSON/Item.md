---
parent: JSON
grand_parent: ThunderRoad
---

# Item

The Item JSON is the primary JSON used to spawn items, from swords and axes to crates and lanterns. 

| Keys                          | Description |
| ---                           | --- |
| id                            | The Item ID of the Item. Make sure this is unique so that it does not conflict with other items. |
| sensitiveContent              | Depicts if the item contains sensitive content, disabling it with certain in-game settings. Current functioning sensitiveContent values are: Blood, Dismemberment. |
| sensitiveFilterBehavior       | Depicts the behavior if it is altered by sensitiveContent. "Discard" removes the item, "Keep" keeps the item. |
| version                       | Version of JSON. Must be "4" |
| localizationId                | ID that matches the localization JSON, so it can change the name and description to different languages |
| displayName                   | The name the item will have in the spawn book and holder UI |
| description                   | The description the item will have in the Item Spawner book |
| author                        | The "author" in the Item Spawner book |
| valueType                     | What currency will be used to buy the item in the shop. This can either be crystalShard or Gold. |
| value                         | How much the item will cost (by valueType) |
| rewardValue                   | Utilised by the reward loot tables to define rarity |
| tier                          | Up to 5, will determine the power of the weapon in its slicing, decapitation and imbue power/strength. |
| flags                         | Used for Spell flags. Current integrated flags are:<details>• *None (no flags)*<br>• *Throwable - Orients itself when thrown, in the direction of the FlyRef*<br>• *Heavy*<br>• *Spinnable - Can spin with Telekinesis*<br>• *Jabbing - Works with Dive Strike skill*<br>• *Defensive*<br>• *Ricochet - Works with Boomerang skill*<br>• *Piercing - Works with Ranged Expertise skill*<br>• *Electromagnetic - Works with Electromagnetic skill*<br>• *Blunt - Determines if the item is fully blunt, for the Dive Strike skill*</details> |
| levelRequired                 | The level required for the item to show up in the shop. |
| category                      | The category the item will appear in in the item spawner book. The categories can be found in the game.json |
| iconEffectId                  | ID for the effect used for Spell Orbs |
| preferredItemCenter           | Sets the item center to specified. List is:<details>• *Mass*<br>• *Root*<br>• *Renderer*<br>• *Main Handle*<br>• *Holder*</details> |
| drainImbueWhenIdle            | When "true", the imbue will drain when stored in holders (like back holder, hip holder etc) |
| prefabAddress                 | The addressable asset name for the item |
| iconAddress                   | The addressable asset name for the item icon |
| closeUpIconAddress            | The addressable asset name for the item close-up icon |
| pooledCount                   | How many of this item is stored in the item pool (recommended for quiver contents, having too many can cause memory problems) |
| androidPooledCount            | How many of this item is stored in the item pool, on android |
| type                          | What type of item this is, this key has many different uses for each type, so ensure this is accurate to your item. Item type list is:<details>• *Misc*<br>• *Weapon*<br>• *Quiver*<br>• *Potion*<br>• *Body*<br>• *Shield*<br>• *Wardrobe*<br>• *Spell*<br>• *Crystal*<br>• *Valuable*<br>• *Tool*<br>• *Food*<br>• *Note* </details> |
| allowedStorage                 | Depict that storage this item is allowed in:<details>• *Inventory*<br>• *SandboxAllitems*<br>• *Container* </details> |
| despawnOnStoredInInventory     | Despawns the item when placed in the inventory. Mainly used for Gold/crystal additions |
| isStackable                    | If the item is stackable in inventory (like apples) |
| consumableID                   | ***obsolete*** |
| inventoryAudioContainer        | The Audio Container address for the sound the inventory item makes |
| inventoryAudioVolume_dB        | Volume of the sound that occurs the inventory item is grabbed |
| slot                           | What slot the item goes in to:<details>• *None*<br>• *Small (hips, back)*<br>• *Medium (hips, back)*<br>• *Large (back)*<br>• *Potion*<br>• *Head*<br>• *Shield*<br>• *ShieldSmall*<br>• *Bow*<br>• *Quiver*<br>• *Arrow*<br>• *Throwables*<br>• *Bolt*<br>• *Cork*<br>• *Torch*<br>• *SkillTreeReceptacle*<br>• *InventoryBag*<br>• *Gigantic*</details> |
| snapAudioContainerAddress      | The Audio Container address for the sound an item makes when going in to your inventory/holder |
| snapAudioVolume_dB             | Volume of the sound that occurs when an item is stored in your inventory/holder |
| overrideMassAndDrag            | When ticked, the mass and drag of the item's rigidbody will be overwritten by the JSON |
| mass                           | Mass of the item's rigidbody |
| drag                           | Drag of the item's rigidbody |
| angularDrag                    | Angular Drag of the item's rigidbody |
| focusRegenMultiplier           | For armor, depending on your armor determines the focus regen. 1 is the "default regen" |
| spellChargeSpeedPlayerMultiplier  | For armor, depending on your armor determines the spell charge speed multiplier. 1 is the "default" speed. |
| spellChargeSpeedNPCMultiplier  | For armor, depending on NPC armor determines the spell charge speed multiplier for NPC. 1 is the "default" speed. |
| collisionMaxOverride           | Maximum collissions the item can make (0 = disabled) |
| collisionEnterOnly             | Must be disabled for any behaviors that require continuous collision between this item and another. For example draw-cuts/slashing |
| collisionNoMinVelocityCheck   | If collisionEnterOnly is enabled, and this is enabled, it will do a velocity check before being able to an enter only collision like draw cuts. |
| forceLayer                    | Will force the Unity Layer of the object. |
| diffForceLayerWhenHeld        | If enabled, will change the layer of the object when it is held |
| forceLayerHeld                | If diffforceLayerWhenHeld is set to **true**, it will change its' layer to this. |
| waterHandSpringMultiplierCurve  | Curve of the hand spring while the item is in water  |
| waterDragMultiplierCurve      | Changes the item drag when underwater (how hard it is to move underwater) |
| waterSampleMinRadius          | How much of the item in a radius has to be underwater before the item has water spring/drag changes |
| throwMultiplier               | Increases item throw speed/velocity |
| runSpeedMulitplier            | For armor: increases speed multiplier when equipped |
| flyRotationSpeed              | The speed of the item rotation when flying |
| flyThrowingAngle              | The angle the item has to be thrown before it is classed as flying |
| allowFlyBackwards             | Allow the item to be able to be "flying" if thrown not in the flyThrowingAngle |
| telekinesisSafeDistance       | Distance between item and player that is considered safe, without being able to hit and damage the player |
| telekinesisThrowRatio         | Speed of the item that is thrown with telekinesis |
| telekinesisAutoGrabAnyHandle  | When pulled back to hand with telekinesis, will automatically grab any handle on the weapon. |
| grippable                     | When enabled, item can be gripped (grabing an area that does not have a handle) |
| grabAndGripClimb              | When enabled, item can be "climbed" with when grabbed or gripped |
| playerGrabAndGripChangeLayer  | When grabAndGripClimb is enabled, and the player tries grip climbing, will change the item layer |
| customSnaps                   | Custom Item Snaps |
| drainImbueOnSnap              | When enabled, item will drain imbue to 0 when stored in a holder |
| imbueEnergyOverTimeOnSnap     | Curve of how fast imbue will drain on snap |


## Modules

<details markdown="block">

<summary><b>ItemModuleAI</b></summary>

The ItemModuleAI module specifies how an NPC uses this weapon. It displays what type of weapon this is, and how the AI handle it.

| Keys                          | Description |
| ---                           | --- |
| primaryClass                  | States what type of class this weapon is. The choice is:<details>• None<br>• Melee<br>• Bow<br>• Arrow<br>• Shield<br>• Wand<br>• Crossbow<br>• Bolt<br>• Firearm<br>• Throwable </details> |
| secondaryClass               | States the secondary class, seprate from the primary class. This means a sword can be both a melee weapon and classed as a shield. |
| weaponHandling               | States what handling it uses. "oneHanded" or "twoHanded" (Two Handed is unused) |
| secondaryHandling            | Depicts the secondary use of handling. |
| weaponAttackTypes            | What attacks can be used with this weapon. Supports "Swing" and "Thrust" |
| ignoredByDefense             | if true, it is ignored by AI, they wont try to parry it |
| alwaysPrimary                | If true, will ignore the Secondary Class/Handling |
| armResistanceMultiplier      | States the resistance multiplier enemies will use to defend with the weapon |
| allowDynamicHeight           | Utilises the Enemy's Animator height variable to take the weapon in to account for defence (unused) |
| defenceHasPriority           | When ticked, defence has priority over attacking. |

```json
{
      "$type": "ThunderRoad.ItemModuleAI, ThunderRoad",
      "primaryClass": "None",
      "secondaryClass": "None",
      "weaponHandling": "None",
      "secondaryHandling": "None",
      "weaponAttackTypes": "Swing, Thrust",
      "ignoredByDefense": false,
      "alwaysPrimary": false,
      "defaultStanceInfo": {
        "$type": "ThunderRoad.ItemModuleAI+StanceInfo, ThunderRoad",
        "offhand": "Empty",
        "grabAIHandleRadius": 0.0,
        "stanceDataID": null
  },
      "stanceInfosByOffhand": [],
      "rangedWeaponData": {
        "$type": "ThunderRoad.ItemModuleAI+RangedWeaponData, ThunderRoad",
        "ammoType": "",
        "tooCloseDistance": 5.0,
        "spread": {
          "x": 1.0,
          "y": 1.0
    },
        "projectileSpeed": 20.0,
        "accountForGravity": true,
        "weaponAimAngleOffset": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
    },
        "weaponHoldPositionOffset": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
    },
        "weaponHoldAngleOffset": {
          "x": 0.31,
          "y": 0.31,
          "z": 0.31
    },
        "customRangedAttackAnimationData": ""
  },
      "armResistanceMultiplier": 3.0,
      "allowDynamicHeight": false,
      "defenseHasPriority": false
}
```

### Stance

| Keys                          | Description |
| ---                           | --- |
| offhand                       | Depicts how it uses an off-hand weapon. When selected, the AI will only pickup specified items:<br>• Empty<br>• Same Item<br>• Item Duplicate<br>• Any Melee<br>• Any Shield<br>• Any Firearm<br>• Any Throwable<br>• Anything |
| grabAIHandleRadius            | The Handle radius for the NPC to pick up the item |
| stanceDataID                  | The Stance Data ID for the NPC to use when handling this item |

### Ranged Weapon Data

| Keys                          | Description |
| ---                           | --- |
| ammoType                      | What type of ammo can be used with this weapon, if it is a ranged weapon. Supports all different type of "slots" |
| tooCloseDistance              | What classifies as "too close" when handling the item, of which they will switch to their secondary/melee item on the hips |
| spread                        | Changes accuracy spread of the projectile. Higher the spread, the less accurate the projectile |
| projectileSpeed               | The speed of the projectiles the NPC will fire from this item |
| accountForGravity             | Account the item to have gravity imbued items/ammo |
| weaponAimAngleOffset          | The offset of the aiming angle |
| weaponHoldPositionOffset      | The offset of the position when holding the item |
| weaponHoldAngleOffset         | The offset of the angular rotation when holding the item |
| customRangedAttackAnimationData   | Add custom attack animation data. |

</details>

<details markdown="block">

<summary><b>ItemModuleBow</b></summary>

| Keys                          | Description |
| ---                           | --- |
| velocityMultiplier            | The velocity of which the bow shoots the arrow |
| stringSpring                  | The spring of the bowstring |
| autoSpawnArrow                | When ticked, the bow will automatically spawn the arrow when pulled back
| arrowProjectileID             | ID of the item spawned on the bow when autoSpawnArrow is enabled |
| audioShootMinPull             | The minimum pull of the string before the audio can play |
| audioDrawPullSpeed            | The speed required for the draw audio to play |
| audioSpringAddress            | Used to reference the audio for the spring pull. Our Default: `Bas.Audio.Item.BowString` |
| audioGroupDrawAddress         | Used to reference the audio group for the spring pul. Our Default: `Bas.AudioGroup.Item.BowDraw` |
| audioGroupShootAddress        | Used to reference the audio group used for shooting the bow. Our Default: `Bas.AudioGroup.Item.BowShoot` |
| maxPullRadioAI                | Used to reference the maximum pull NPCs can use on this bow. |

```json
{
      "$type": "ThunderRoad.ItemModuleBow, ThunderRoad",
      "velocityMultiplier": 1.5,
      "stringSpring": 500.0,
      "autoSpawnArrow": false,
      "arrowProjectileID": null,
      "audioShootMinPull": 0.1,
      "audioDrawPullSpeed": 0.02,
      "audioStringAddress": null,
      "audioGroupDrawAddress": null,
      "audioGroupShootAddress": null,
      "maxPullRatioAI": 0.75,
      "spawnArrow": false
}
```

</details>

<details markdown="block">

<summary><b>ItemModuleConvertToCurrency</b></summary>

The ConvertToCurrency module converts items to currency when the item is added to the inventory.

The collectSoundEffectId field plays the audio when it is placed in the inventory. Our Default value is `CollectCurrency`.

```json
{
      "$type": "ThunderRoad.ItemModuleConvertToCurrency, ThunderRoad",
      "CollectSoundEffectId": "CollectCurrency"
}
```
</details>

<details markdown="block">

<summary><b>ItemModuleCrystal</b></summary>

The Crystal Module is used to create custom crystals. This references all effects that happen with merges, hovering etc.

| Keys                          | Description |
| ---                           | --- |
| treeName                      | Name of the skill tree this crystal is for |
| overrideCrystalColors         | If true, you can use the color changer to change the color of the crystal to this color. |
| higherTierCrystalId           | Reference the ID of the higher tier crystal so this can merge in to this ID |
| shardId                       | ID of the shard if you merge two crystals which are the highest tiers. |
| mergeBeginEffectId            | ID of the effect used for the beginning of the merge. Our Default: `CrystalMergeBegin` |
| sparkleEffectId               | ID of the effect used for the crystal sparkling effect. Our Default: `CrystalSparkle` |
| hoverEffectId                 | ID of the effect used for the crystal hover effect. Our Default: `CrystalCoreHover` |
| mergeEffectId                 | ID of the effect used for the merge effect. Our Default: `CrystalMerge` |
| mergeStartEffectId            | ID of the effect used for the start of the merge effect. Our Default: `CrystalMergeStart` |
| mergeCompleteEffectId         | ID of the effect used for when the merge is complete. Our Default: `CrystalMergeNewTier` |
| endMergeTime                  | The time it takes for the merge to end.
| activateEffectId              | The effect that plays when crystal is activated (trigger held). Our Default: `CrystalActivate` |

```json
{
      "$type": "ThunderRoad.ItemModuleCrystal, ThunderRoad",
      "overridePrefabForceValues": false,
      "treeName": null,
      "overrideCrystalColors": false,
      "baseColor": {
        "r": 0.0,
        "g": 0.0,
        "b": 0.0,
        "a": 0.0
  },
      "internalColor": {
        "r": 0.0,
        "g": 0.0,
        "b": 0.0,
        "a": 0.0
  },
      "animatedColor": {
        "r": 0.0,
        "g": 0.0,
        "b": 0.0,
        "a": 0.0
  },
      "emissionColor": {
        "r": 0.0,
        "g": 0.0,
        "b": 0.0,
        "a": 0.0
  },
      "linkVfxColor": {
        "r": 0.0,
        "g": 0.0,
        "b": 0.0,
        "a": 0.0
  },
      "mergeVfxColor": {
        "r": 0.0,
        "g": 0.0,
        "b": 0.0,
        "a": 0.0
  },
      "higherTierCrystalId": null,
      "shardId": null,
      "mergeBeginEffectId": "CrystalMergeBegin",
      "sparkleEffectId": "CrystalSparkle",
      "hoverEffectId": "CrystalCoreHover",
      "mergeEffectId": "CrystalMerge",
      "mergeStartEffectId": "CrystalMergeStart",
      "mergeCompleteEffectId": "CrystalMergeNewTier",
      "endMergeTime": 1.0,
      "activateEffectId": "CrystalActivate"
}
```
</details>

<details markdown="block">

<summary><b>ItemModuleEdible</b></summary>

The Edible Item Module is used for edible items, such as apples.

| Keys                          | Description |
| ---                           | --- |
| nextStageItemID               | ID for the item this turns in to when eaten. Set to `null` if you want the item to just disappear. |
| onConsumeAudioContainerAddress    | Addressable of the Audio Container containing the eating sounds. |
| healthGain                    | Gain this amount of health when item is eaten |
| focusGain                     | Gain this amount of focus when item is eaten |
| consumeTime                   | Takes this amount of seconds of the food being near the mouth to consume the item |
| transferCustomData            | Can transfer custom data from this item to the nextStage Item. |

```json
{
      "$type": "ThunderRoad.ItemModuleEdible, ThunderRoad",
      "nextStageItemID": null,
      "onConsumeAudioContainerAddress": null,
      "healthGain": 0.0,
      "focusGain": 0.0,
      "consumeTime": 0.0,
      "transferCustomData": true
}
```
</details>

<details markdown="block">

<summary><b>ItemModulePotion</b></summary>

The Potion Item Module is used to create your own potions.

| Keys                          | Description |
| ---                           | --- |
| contents                      | References the Liquid ID and level of the liquid by default |
| maxLevel                      | Maximum level of liquid, how much is stored in the potion |
| popCorkOnAltUse               | Pops the Potion's cork/holder when Alt Use (spell select button) |
| corkPopForce                  | Depicts the force of the cork popping out |
| corkPopForceMode              | Depicts the force mode of the cork. |
| collissionLayer               | Depicts the layer of the collision for the liquid of the potion. Default is: Default, LiquidFlow. | 
| healthIndicatorGradient       | Gradient of the health indicator on the Potion |
| levelIndicatorGradient        | Gradient of the amount of liquid inside the potion |
| flowSpeed                     | Depicts the speed of liquid flow: how fast it empties
| flowMinAngle                  | The minimum angle the potion needs to be (pointing down) to flow |
| flowMaxAngle                  | The maximum angle the potion needs to flow the most amount
| effectFlowId                  | The Potion effect ID used for the flowing effect. |
| healthEffectId                | Effect used for the Health Effect. |
| levelEffectId                 | Effect used to display potion liquid level Effect. |

```json
{
      "$type": "ThunderRoad.ItemModulePotion, ThunderRoad",
      "id": null,
      "sensitiveContent": "None",
      "sensitiveFilterBehaviour": "Discard",
      "version": 0,
      "contents": [
        {
          "$type": "ThunderRoad.LiquidData+Content, ThunderRoad",
          "liquidId": "PotionHealth",
          "level": 8.0
    }
      ],
      "maxLevel": 8.0,
      "popCorkOnAltUse": true,
      "corkPopForce": 0.3,
      "corkPopForceMode": "Impulse",
      "collisionLayer": 16385,
      "healthIndicatorGradient": {
        "$type": "UnityEngine.Gradient, UnityEngine.CoreModule",
        "colorKeys": [
          {
            "$type": "UnityEngine.GradientColorKey, UnityEngine.CoreModule",
            "color": {
              "r": 0.8962264,
              "g": 0.274283141,
              "b": 0.190236762,
              "a": 1.0
        },
            "time": 0.0
      },
          {
            "$type": "UnityEngine.GradientColorKey, UnityEngine.CoreModule",
            "color": {
              "r": 2.980994,
              "g": 1.0708425,
              "b": 0.7452485,
              "a": 1.0
        },
            "time": 0.997055
      }
        ],
        "alphaKeys": [
          {
            "$type": "UnityEngine.GradientAlphaKey, UnityEngine.CoreModule",
            "alpha": 1.0,
            "time": 0.0
      },
          {
            "$type": "UnityEngine.GradientAlphaKey, UnityEngine.CoreModule",
            "alpha": 1.0,
            "time": 1.0
      }
        ],
        "mode": "Blend"
  },
      "levelIndicatorGradient": {
        "$type": "UnityEngine.Gradient, UnityEngine.CoreModule",
        "colorKeys": [
          {
            "$type": "UnityEngine.GradientColorKey, UnityEngine.CoreModule",
            "color": {
              "r": 0.291966468,
              "g": 0.155660391,
              "b": 1.0,
              "a": 1.0
        },
            "time": 0.0
      },
          {
            "$type": "UnityEngine.GradientColorKey, UnityEngine.CoreModule",
            "color": {
              "r": 1.3569051,
              "g": 0.999786556,
              "b": 3.163504,
              "a": 1.0
        },
            "time": 1.0
      }
        ],
        "alphaKeys": [
          {
            "$type": "UnityEngine.GradientAlphaKey, UnityEngine.CoreModule",
            "alpha": 1.0,
            "time": 0.0
      },
          {
            "$type": "UnityEngine.GradientAlphaKey, UnityEngine.CoreModule",
            "alpha": 1.0,
            "time": 1.0
      }
        ],
        "mode": "Blend"
  },
      "flowSpeed": 1.2,
      "flowMinAngle": 70.0,
      "flowMaxAngle": 100.0,
      "effectFlowId": "PotionFlow",
      "healthEffectId": "WristHealth",
      "levelEffectId": "WristMana",
      "groupPath": null
}
```

</details>

<details markdown="block">

<summary><b>ItemModuleReturnInInventory</b></summary>

Items with the ItemModuleReturnInInventory module will return the item to the player's inventory when under certain conditions.

| Keys                          | Description |
| ---                           | --- |
| returnsToLastHolder           | Returns the item to the last holder slot (inventory, hips etc) |
| returnsOnItemRelease          | Returns the item when the item handle is released |
| returnsWhenNotLookingAtIt     | Returns the item when the item is not in sight |
| returnsAfterReachingMaxDistance | Returns the item when the item has gone past the maximum distance |
| maxDistanceBeforeReturning    | The maximum distance the item can be before it returns
| timeAfterWhichItemReturn      | The delay of the return after reaching these return conditions |
| useReturningAnimation         | If true, will return the item by using the animation of which the item will physically come back to you |
| returningVelocityValue        | The velocity of the return animation. |

```json
{
      "$type": "ThunderRoad.ItemModuleReturnInInventory, ThunderRoad",
      "returnsToLastHolder": false,
      "returnsOnItemRelease": false,
      "returnsWhenNotLookingAtIt": true,
      "returnsAfterReachingMaxDistance": true,
      "maxDistanceBeforeReturning": 15.0,
      "timeAfterWhichItemReturn": 2.0,
      "useReturningAnimation": false,
      "returningVelocityValue": 0.0
}
```
</details>

<details markdown="block">

<summary><b>ItemModuleStats</b></summary>

This module allows you to add custom stats to your weapon which is displayed in the inventory when holding it.

The module allows four types of stats. They do not all need to be used.

| Stat                          | Description |
| ---                           | --- |
| ItemStatBool                  | Boolean Value (true/false)
| ItemStatFloat                 | Float Value (decimal)
| ItemStatInt                   | Integer Value (whole number)
| ItemStatString                | String Value (word)

You can specify the name and the value in these fields.

```json
{
      "$type": "ThunderRoad.ItemModuleStats, ThunderRoad",
      "stats": [
        {
          "$type": "ThunderRoad.ItemStatBool, ThunderRoad",
          "value": true,
          "name": "BoolStat"
    },
        {
          "$type": "ThunderRoad.ItemStatFloat, ThunderRoad",
          "value": 1.0,
          "name": "FloatStat"
    },
        {
          "$type": "ThunderRoad.ItemStatInt, ThunderRoad",
          "value": 1,
          "name": "IntStat"
    },
        {
          "$type": "ThunderRoad.ItemStatString, ThunderRoad",
          "value": "StringValue",
          "name": "StringStat"
    }
      ]
}
```
</details>

<details markdown="block">

<summary><b>ItemModuleSpell</b></summary>

The Spell Item module adds the specified Spell Id to the player once the item has been placed in the player's inventory.

```json
{
      "$type": "ThunderRoad.ItemModuleSpell, ThunderRoad",
      "spellId": null
}
```
</details>

<details markdown="block">

<summary><b>ItemModuleTint</b></summary>

The Tint module allows you to change the color of any specific shader section in a shader to change its color. For example, you can put the _BaseColor shader property, to change the color of the _BaseColor of the material at runtime.

```json
{
      "$type": "ThunderRoad.ItemModuleTint, ThunderRoad",
      "tintColour": {
        "r": 1.0,
        "g": 1.0,
        "b": 1.0,
        "a": 1.0
  },
      "shaderProperty": "_BaseColor"
}
```
</details>

<details markdown="block">

<summary><b>ItemModuleWardrobe</b></summary>

The Wardrobe Item Module is used for Armor items.

| Keys                          | Description |
| ---                           | --- |
| Category                      | Use either "Apparel" for clothes, or "Body" for Body Parts |
| Cast Shadows                  | Can state if the armor casts shadows. Can set to "None" for no shadows, PlayerOnly for player only shadows, and "PlayerAndNpc" for both players and NPC |
| Wardrobes                     | Here, you specify what wardrobe data the item will apply to the player/NPC. Specify the creature name, whether it is default "HumanMale" or "HumanFemale", and then the Wardrobe data Addressable, to state what part to apply to the creature. |
| isMetal                       | States if the part is metal. This will utilise dents/scratches, as well as lightning effects off metal objects. |

```json
{
      "$type": "ThunderRoad.ItemModuleWardrobe, ThunderRoad",
      "category": "Apparel",
      "castShadows": "None",
      "wardrobes": [
        {
          "$type": "ThunderRoad.ItemModuleWardrobe+CreatureWardrobe, ThunderRoad",
          "creatureName": "HumanFemale",
          "wardrobeDataAddress": null
    },
        {
          "$type": "ThunderRoad.ItemModuleWardrobe+CreatureWardrobe, ThunderRoad",
          "creatureName": "HumanMale",
          "wardrobeDataAddress": null
    }
      ],
      "isMetal": false
}
```
</details>

## Collider Groups

The collider group field is to determine how the item interacts with many different in-game features. It will adjust how the item is imbued, as well as allow you to do staff effects and spell deflection. 

The name of the "transformName" MUST meet the same name as the collider group of your weapon.

```json
{
      "$type": "ThunderRoad.ItemData+ColliderGroup, ThunderRoad",
      "transformName": "Colliders",
      "colliderGroupId": "PropDefault"
}
```

## Interactables

Interactables are handles on the item. While most settings are altered in the component itself, the ID of the interactable determines how the player handles the handle. For example, if the handle accepts telekinesis or denies it. 

The name of the "transformName" MUST meet the same name as the handle transform.

```json
{
      "$type": "ThunderRoad.ItemData+Interactable, ThunderRoad",
      "transformName": "Handle",
      "interactableId": "ObjectHandleTKOnly"
}
```

## Effect Hinges

The effect hinge field is used for hinge effects on items that have hinges, such as wheels. In this field, you can determine the effect that is played, as well as the minimum torque the item needs to play the effect, and the maximum torque used so the effect at maximum volume/effect.

The name of the "transformName" MUST meet the same name as the Hinge transform.

## Whooshs

Whoosh is a component utilised to play a windy "whoosh" sound when the item is swung or thrown. 

| Keys                          | Description |
| ---                           | --- |
| trigger                       | Will trigger the whoosh under certain conditions. can be "Always" so it is will always play the sound, "onGrab" which is only playing the sound when the item is grabbed, and "OnFly" which plays the sound when the item is flying ONLY. |
| stopOnSnap                    | Will stop the whoosh sound when the item is snapped to a holder. |
| min/max velocity              | The minimum/maximum velocity of which the sound will start to play. Max velocity plays the whoosh at the maximum set volume. |
| dampening                     | The speed of which the whoosh sound will stop. |
