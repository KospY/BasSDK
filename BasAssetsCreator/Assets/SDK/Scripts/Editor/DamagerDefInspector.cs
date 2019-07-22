using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BS.DamagerDefinition))]
public class DamagerDefInspector : Editor
{
    string damagerType = "";
    public override void OnInspectorGUI()
    {
        BS.DamagerDefinition damager = (BS.DamagerDefinition)target;
        BS.ItemDefinition item = damager.transform.GetComponentInParent<BS.ItemDefinition>();

        EditorGUILayout.HelpBox("This Damager is set up for "+damagerType+" damage.", MessageType.Info);

        base.OnInspectorGUI();

        if (damager.colliderGroupName == "")
        {
            EditorGUILayout.HelpBox("Do not forget to assign a valid Collider Group.", MessageType.Error);
        }
        else
        {
            bool ColliderGroupExists = false;
            for (int i = 0; i < item.colliderGroups.Count; i++)
            {
                if (item.colliderGroups[i].name == damager.colliderGroupName)
                {
                    ColliderGroupExists = true;
                }
            }
            if (!ColliderGroupExists)
            {
                EditorGUILayout.HelpBox("The assigned Collider Group does not exist.", MessageType.Error);
            }
        }

        if (damager.penetrationDepth < 0)
        {
            EditorGUILayout.HelpBox("Penetration Depth can not be a negative number.", MessageType.Error);
        }
        if (damager.penetrationLength < 0)
        {
            EditorGUILayout.HelpBox("Penetration Length can not be a negative number.", MessageType.Error);
        }

        if (damager.penetrationLength == 0 && damager.penetrationDepth == 0)
        {
            damagerType = "Blunt";
            if (damager.direction != BS.DamagerDefinition.Direction.All)
            {
                EditorGUILayout.HelpBox("Direction for Blunt damagers should be set to 'All'", MessageType.Warning);
            }
        }
        else if (damager.penetrationLength == 0 && damager.penetrationDepth != 0)
        {
            damagerType = "Piercing";
            if (damager.direction != BS.DamagerDefinition.Direction.Forward)
            {
                EditorGUILayout.HelpBox("Direction for Piercing damagers should be set to 'Forward'", MessageType.Warning);
            }
        }
        else
        {
            damagerType = "Slashing";
            if (damager.direction == BS.DamagerDefinition.Direction.All)
            {
                EditorGUILayout.HelpBox("Direction for Slicing damagers should be set to 'Forward' or 'Forward and Backward'", MessageType.Warning);
            }
            if (damager.penetrationDepth == 0)
            {
                EditorGUILayout.HelpBox("Slashing damagers must have Depth greater than zero.", MessageType.Warning);
            }
        }


    }
}
