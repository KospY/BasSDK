---
parent: JSON
grand_parent: ThunderRoad
title: Damager
---
# Damager 

## Overview
This data describes the properties of a damager component. Read more about damagers [here][damagers].

## Properties

| Property                                  | Description
| ---                                       | ---
| ID                                        | Unique identifier used when referencing this data from another source. 
| sensitiveContent                          | Flags that remark what (if any) elements of this data contain content that is sensitive to some users.
| sensitiveContentFlags                     | <details>- None<br>- Blood<br>- Burns<br>- Dismemberment<br>- Desecration<br>- Skeleton<br>- Spider<br>- Insect<br>- Snake<br>- Bird<br>- Fright</details>
| sensitiveFilterBehavior                   | How this data should be processed if any of the above flags are forbidden by the game.
| sensitiveFilterBehavior \[Flags\]         | <details>- Discard [This data will be skipped]<br>- Keep [This data will be included]</details>
| version                                   | The version of the JSON code this data is compatible with. For damager data set this to `1`.
| damageModifierID                          | The ID of the Damage Modifier Data this damager uses.
| tiers                                     | An ordered list of modifiers to the damager's behavior, respective to the tier of the weapon this damager is attached to.
| velocityDamageCurve                       | Represents a curve which describes the relation between the force of an impact and the damage inflicted.
| minSelfVelocity                           | Impacts occurring when the damager object's velocity is below this value will be ignored.
| intensityMinVelocity                      | The minimum velocity required for an impact to have an effect.
| intensityMaxVelocity                      | The maximum velocity that will be considered when calculating the intensity of an impact.
| hitDelayByCollider                        | Time in seconds before this damager can collide with the same object again.
| playerMinDamage                           | The minimum amount of damage dealt to a player on impact.
| playerMaxDamage                           | The maximum amount of damage dealt to a player on impact.
| selfDamage                                | Enables this damager be able to recieve damage from impacting the environment. Only relevant to ragdoll parts.
| staticVelocityDamageCurve                 | Represents a curve which describes the relation between the force of impact and the damage inflicted when colliding with the environment. Requires `Self Damage` to be enabled.
| throwedMultiplier                         | Damage dealt will be multiplied by this value if the object is in a thrown state on impact
| handleDamager                             | Determines if this damager is intended for a handle, and if so allows it to be used for the pommel strike skill.
| badAngleBluntFallback                     | When true, the material used will be taken from `badAngleMaterialDamageId` instead of `damageModifierId`.
| badAngleDamage                            | The damage dealt when an impact lands at a bad angle.
| ~~badAngleRecoilMultiplier~~              | Unused property.
| badAngleMaterialDamageId                  | When `badAngleBluntFallback` is true, the material from this damage modifier will be used in place of the one specified in `damageModifierId`.
| dyingAnimationMaxVelocity                 | If a killing blow with this damager has a velocity higher than this value, the target's death animation will be skipped.
| addForce                                  | On impact, this much force will be applied to the hit object.
| addForceState                             | The state this object must be in for `addForce` to apply.
| addForceState \[Flags\]                   | <details>- Flying<br>- Handled<br>- All</details>
| addForceTargetType                        | The target must be of this type for `addForce` to apply.
| addForceTargetType \[Flags\]              | <details>- Object<br>- Creature<br>- All</details>
| addForceMode                              | The way in which force is applied to the target when `addForce` is applied.
| addForceMode \[Flags\]                    | <details>- Force<br>- Impulse<br>- VelocityChange<br>- Acceleration</details>
| addForceDuration                          | How long in seconds `addForce` is applied for.
| addForceRagdollPartMultiplier             | If the target is a ragdoll part, `addForce` will be multiplied by this value.
| addForceRagdollOtherMultiplier            | If the target is a ragdoll part, force applied to adjacent parts will be multiplied by this value.
| addForceSlowMoMultiplier                  | If slow motion is active, `addForce` will be multiplied by this value.
| addForceNormalize                         | If enabled, the velocity of this impact will be normalized to magnitude of 1 before `addForce` is applied.
| penetrationAllowed                        | When true, this damager will be able to penetrate objects with the correct materials.
| penetrationDeepDepthMultiplier            | Multiplies the deep penetration threshold by this value. A lower value will have piercing do more damage at less depth.
| penetrationDamage                         | The amount of damage dealt on penetration start.
| penetrationEffect                         | If true, the ragdoll part's `penetrationDeepEffect` will play when the deep penetration threshold is reached.
| penetrationInitialVelocityMultiplier      | When penetration occurs, the damager's object velocity will be multiplied by this value.
| penetrationSkewerDetection                | When enabled, checks will be done to see if any part of the damager has passed through the other side of the penetrated objects.
| penetrationSkewerDamage                   | The amount of extra damage to deal when skewering has occurred.
| penetrationHeldDamperIn                   | Dampening applied to the penetration force while the item is held.
| penetrationHeldDamperOut                  | Dampening applied when trying to withdraw a penetrated object, while it is held.
| penetrationDamper                         | Dampening applied to penetration and withdrawal when the object is not held.
| penetrationShortDepth                     | Determines the threshold before an object is too deep to be moved around.
| penetrationShortDepthAngle                | The angle this object can still pivot in while penetrating below the short depth threshold.
| penetrationAllowSlide                     | If true, the object can still slide while penetrating. Used for penetrating slashes, allowing the blade to slice out.
| penetrationSlideDamper                    | Dampening applied when sliding the object while penetrating.
| penetrationTempModifier                   | The condition in which the temporary dampening modifier will be applied. 
| penetrationTempModifier \[Flags\]         | <details>- OnHit<br>- OnThrow</details>
| penetrationTempModifierDuration           | The time in seconds the temp modifier will apply for.
| penetrationTempModifierDamperIn           | Dampening applied temporarily when an object is penetrating.
| penetrationTempModifierDamperOut          | Dampening applied temporarily when an object is withdrawing.
| penetrationPressureAllowed                | If true, penetration can occur by resting this object against a surface and applying force.
| penetrationPressureForceCurve             | Describes a curve which is sampled to determine if enough pressure is applied to penetrate.
| penetrationPressureMaxDot                 | How close this damager needs to be perpendicular to the surface in order for pressure-penetration to work. Where 1 is perfectly perpendicular and 0 is any angle.
| dismemebermentAllowed                     | Whether this damager should be capable of dismembering limbs.
| dismembermentMinVelocity                  | The minimum amount of force required to dismember limbs.
| dismembermentNoPenetrationDuration        | The time in seconds before this damager can penetrate a surface again, after dismembering a limb.
        

[damagers]:           {{ site.baseurl }}{% link Components/ThunderRoad/Items/Damager.md %}
[damageModifierData]: {{ site.baseurl }}{% link Components/ThunderRoad/JSON/DamageModifier.md %}