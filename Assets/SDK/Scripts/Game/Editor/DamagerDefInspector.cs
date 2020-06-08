using UnityEngine;
using UnityEditor;
using System;

namespace ThunderRoad
{
    [CustomEditor(typeof(DamagerDefinition))]
    public class DamagerDefInspector : PointToPoint
    {
        Vector3 depthPoint;
        string damagerType = "";
        DamagerDefinition damager;

        public override void OnInspectorGUI()
        {
            script = damager = (DamagerDefinition)target;
            ItemDefinition item = damager.transform.GetComponentInParent<ThunderRoad.ItemDefinition>();
            damager.transform.localScale = Vector3.one;

            PointToPointButton(damager.penetrationLength);

            EditorGUILayout.HelpBox("This Damager is set up for " + damagerType + " damage.", MessageType.Info);

            base.OnInspectorGUI();

            if (damager.colliderGroup == null)
            {
                EditorGUILayout.HelpBox("Do not forget to assign a valid Collider Group.", MessageType.Error);
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
                if (damager.direction != ThunderRoad.DamagerDefinition.Direction.All)
                {
                    EditorGUILayout.HelpBox("Direction for Blunt damagers should be set to 'All'", MessageType.Warning);
                }
            }
            else if (damager.penetrationLength == 0 && damager.penetrationDepth != 0)
            {
                damagerType = "Piercing";
                if (damager.direction != ThunderRoad.DamagerDefinition.Direction.Forward)
                {
                    EditorGUILayout.HelpBox("Direction for Piercing damagers should be set to 'Forward'", MessageType.Warning);
                }
            }
            else
            {
                damagerType = "Slashing";
                if (damager.direction == ThunderRoad.DamagerDefinition.Direction.All)
                {
                    EditorGUILayout.HelpBox("Direction for Slicing damagers should be set to 'Forward' or 'Forward and Backward'", MessageType.Warning);
                }
                if (damager.penetrationDepth == 0)
                {
                    EditorGUILayout.HelpBox("Slashing damagers must have Depth greater than zero.", MessageType.Warning);
                }
            }
        }

        private void OnEnable()
        {
            script = damager = (DamagerDefinition)target;
            depthPoint = damager.GetMaxDepthPosition(false);
            ResetPoints(damager.penetrationLength);
        }
        
        private void OnSceneGUI()
        {
            script = damager = (DamagerDefinition)target;
            UpdatePoints(ref damager.penetrationLength);
        }
    }
}