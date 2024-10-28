using System.Collections.Generic;
using UnityEngine;
using ThunderRoad;
using System;

public class CreatureAnimatorEventReceiver : MonoBehaviour
{
    public Dictionary<string, CreatureAbilityHitbox> hitboxDictionary { get; protected set; } = new Dictionary<string, CreatureAbilityHitbox>();
    public CreatureAbilityHitbox[] hitBoxes { get; protected set; }

    [NonSerialized]
    public CreatureAbility ability;

    protected Creature creature;

    void Start()
    {
        creature = GetComponentInParent<Creature>();
        hitBoxes = creature.GetComponentsInChildren<CreatureAbilityHitbox>(true);
        foreach (CreatureAbilityHitbox hitbox in hitBoxes)
        {
            hitboxDictionary.Add(hitbox.name, hitbox);
        }
    }

    public void EnableHitbox(string hitboxesString)
    {
        string[] hitboxes = hitboxesString.Split(",");
        for (int i = 0; i < hitboxes.Length; i++)
        {
            string s = hitboxes[i].Replace(" ", ""); //strip white space
            CreatureAbilityHitbox hitbox = hitboxDictionary[s];
            //Debug.Log($"golem enabling hitbox {s}, hitbox is {hitbox}");
            hitbox?.EnableHitBox(ability); //pass the ability data for damage calculation, force, etc.
        }
    }

    public void DisableHitbox(string hitboxesString)
    {
        string[] hitboxes = hitboxesString.Split(",");
        for (int i = 0; i < hitboxes.Length; i++)
        {
            string s = hitboxes[i].Replace(" ", ""); //strip white space
            CreatureAbilityHitbox hitbox = hitboxDictionary[s];
            //Debug.Log($"golem disabling hitbox {s}, hitbox is {hitbox}");
            hitbox?.DisableHitBox();
        }
    }

    public void DisableAllHitBoxes()
    {
        foreach (var hitbox in hitBoxes) hitbox.DisableHitBox();
    }

    //placeholder for shopkeeper voice lines during an animation?
    public virtual void PlaySyncedVoiceClip(string id)
    {

    }
}
