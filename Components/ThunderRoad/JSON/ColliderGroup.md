---
parent: JSON
grand_parent: ThunderRoad
---

# Collider Group

The collider group JSON is used to determine imbues and weapon tiers. This is referenced in the "ColliderGroups" section of the Item JSON. Different collider groups have different settings related to each of their tier. The tier is set in in the "tier" of the item.

| Keys                          | Description |
| ---                           | --- |
| id                            | The ID of the ColliderGroup. This must match the "colliderGroupID" in the item json. |
| version                       | Version of the JSON. MUST be 0.
| collisionHandling             | Determines how the collisions are handled. <details>ByGroup: Handles the collision by group, of which if the item is pierced inside the object, it can't collide with the part with the same collider group.<br>ByCollider: Handles the collision per collider. This allows an item to pierce the same object multiple times and still hold collisions against it. For example, if a spike wall was one ColliderGroup, the spikes could pierce once and then also pierce the same part again with a different spike. </details> | 
| allowPenetration              | When true, it allows other items to be able to penetrate the colliders under this colliderGroup. |
| ***Tier***                    |
| tierFilter                    | For tiers, you can only go to a maximum of "Tier6". This determines all the stats below for that specific tier on this colliderGroup. This also supports stacking, so "Tier1, Tier3" also works. Also supports "None". |
| imbueType                     | Determines what type of imbue this colliderGroup affects. <details>• None - Doesn't Imbue <br>• Metal - Unused <br>• Blade - Used for all blades, able to be imbued. <br>• Crystal - Able to be imbued and cast magic effects (like slams). <br>• Custom - Allows custom coded imbue effects (like torch lighting up dispersing with velocity). </details>
| imbueMax                      | The amount of imbue the colliderGroup can hold. Default: 100 |
| imbueRate                     | The amount of imbue the item can obtain per tick when a spell is near it |
| imbueConstantLoss             | The amount of imbue that is lost constandly per tick. |
| staffSlamLoss                 | The amount of imbue that is lost when the collider group does a spell slam.
| imbueHitLoss                  | The amount of imbue that is lost when the collider hits anything |
| imbueVelocityLossPerSecond    | The amount of imbue that is lost per second when the collider is moving at velocity |
| imbueEffectiveness            | used to determine the power of specific spells for staffs. Higher the effectiveness, the stronger the skill used. <details>  Spells affected by this are: <br>• Gravity Slam <br>• Gravity Hammer <br>• Fire Slam <br>• Arcwire </details> |
| waterLossRateMultiplier       | Multiplier of the amount of imbue lost while the collider is underwater |
| deflectMaxAngleCollision      | The max angle the spell ball (fireball) will hit to deflect |
| deflectMAxAngleTarget         | ***unused/obsolete*** |
| deflectSpeedMultiplier        | Multiplier of the speed of the deflected projectile |
| deflectSpreadHeight           | The accuracy of the height of the deflection. Higher the number, the less accurate |
| deflectSpreadWidth            | The accuracy of the widge of the deflection. Higher the number, the less accurate |
| spellFilterLogic              | Allows you to either affect "AnyExcept" (where you specify skills that are not affected by imbue) or "NoneExcept" where you specify the only skills of which can imbue with. |
| ignoredImbueEffectModules     | You can specify what imbue effect modules are ignored. |
| customSpellEffects            | You can specify unique effect IDs to have custom effects on imbue.