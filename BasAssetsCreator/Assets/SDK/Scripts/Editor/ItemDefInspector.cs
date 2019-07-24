using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BS.ItemDefinition))]
public class ItemDefInspector : Editor
{
    public override void OnInspectorGUI()
    {
        BS.ItemDefinition item = (BS.ItemDefinition)target;

        base.OnInspectorGUI();

        bool noRenderers = false;
        if (item.renderers.Count == 0)
        {
            noRenderers = true;
        }
        for (int i = 0; i < item.renderers.Count; i++)
        {
            if (!item.renderers[i])
            {
                noRenderers = true;
            }
        }

        bool hasColliders = false;
        for (int i = 0; i < item.colliderGroups.Count; i++)
        {
            for (int j = 0; j < item.colliderGroups[i].colliders.Count; j++)
            {
                if (item.colliderGroups[i].colliders[j])
                {
                    hasColliders = true;
                }
            }
        }

        bool noWhooshPoints = false;
        if (item.whooshPoints.Count == 0)
        {
            noWhooshPoints = true;
        }
        for (int i = 0; i < item.whooshPoints.Count; i++)
        {
            if (!item.whooshPoints[i])
            {
                noWhooshPoints = true;
            }
        }

        if (item.itemId == "")
        {
            EditorGUILayout.HelpBox("Do not forget to assign a valid Item ID.", MessageType.Error);
        }

        if (noRenderers)
        {
            EditorGUILayout.HelpBox("There are no renderers assigned to this object. The item will be invisible.", MessageType.Error);
        }

        foreach (MeshCollider col in item.gameObject.GetComponentsInChildren<MeshCollider>())
        {
            if (!col.convex)
            {
                EditorGUILayout.HelpBox("The mesh collider " + col.name + " has to be set to convex.", MessageType.Error);
            }
        }

        if (!hasColliders)
        {
            EditorGUILayout.HelpBox("There are no colliders assigned to this object. It will fall through the ground.", MessageType.Error);
        }

        GameObject[] collObj = new GameObject[item.GetComponentsInChildren<Collider>().Length];
        List<GameObject> hasRepeatedColliders = new List<GameObject>(item.GetComponentsInChildren<Collider>().Length);
        for (int i = 0; i < item.GetComponentsInChildren<Collider>().Length; i++)
        {
            bool isRepeated = false;
            foreach (GameObject ExistingCol in collObj)
            {
                if (item.GetComponentsInChildren<Collider>()[i] && item.GetComponentsInChildren<Collider>()[i].gameObject == ExistingCol)
                {
                    isRepeated = true;
                    hasRepeatedColliders.Add(item.GetComponentsInChildren<Collider>()[i].gameObject);
                }
            }
            if (!isRepeated && collObj[i] == null)
            {
                collObj[i] = item.GetComponentsInChildren<Collider>()[i].gameObject;

            }
        }
        foreach (GameObject col in collObj)
        {
            if (col)
            {
                if (hasRepeatedColliders.Contains(col))
                {
                    EditorGUILayout.HelpBox("Collider object " + col.name + " has more than one collider script.", MessageType.Error);
                }
            }
        }

        if (hasColliders)
        {
            for (int a = 0; a < item.colliderGroups.Count; a++)
            {
                for (int i = 0; i < item.colliderGroups[a].colliders.Count; i++)
                {
                    if (!hasRepeatedColliders.Contains(item.colliderGroups[a].colliders[i].gameObject) && item.colliderGroups[a].colliders[i] && !item.colliderGroups[a].colliders[i].sharedMaterial)
                    {
                        EditorGUILayout.HelpBox("Collider "+ item.colliderGroups[a].colliders[i].name + " is missing a physics material and will not make sounds nor decals on impact.", MessageType.Warning);
                    }
                }
            }
        }

        foreach (GameObject col in collObj)
        {
            if (col)
            {
                bool colliderNotAssigned = true;
                for (int a = 0; a < item.colliderGroups.Count; a++)
                {
                    for (int b = 0; b < item.colliderGroups[a].colliders.Count; b++)
                    {
                        if (item.colliderGroups[a].colliders[b] && col == item.colliderGroups[a].colliders[b].gameObject)
                        {
                            colliderNotAssigned = false;
                        }
                    }
                }
                if (colliderNotAssigned && col)
                {
                    EditorGUILayout.HelpBox("Collider " + col.name + " has not been assigned to any Collider Group.", MessageType.Warning);
                }
            }
        }

        if (noWhooshPoints)
        {
            EditorGUILayout.HelpBox("There are no whoosh points assigned to this object. The item will now make air sounds when moving.", MessageType.Warning);
        }

        if (item.holderPoint == null)
        {
            EditorGUILayout.HelpBox("There is no holder point assigned to this object. It will not be able to be holstered.", MessageType.Warning);
        }

        if (item.parryPoint == null)
        {
            EditorGUILayout.HelpBox("There is no parry point assigned to this object. Enemies will not know how to weild it.", MessageType.Warning);
        }

        if (item.transform.lossyScale != Vector3.one)
        {
            EditorGUILayout.HelpBox("The scale of the Item definition object should set to 1. Sercale the Mesh object instead.", MessageType.Warning);
        }

        if (!item.mainHandleLeft || !item.mainHandleRight)
        {
            EditorGUILayout.HelpBox("There are no handles on this object. It will not be able to be picked up from the spawn book.", MessageType.Warning);
        }

    }

    private void OnSceneGUI()
    {
        BS.ItemDefinition item = (BS.ItemDefinition)target;
        if (item.customCenterOfMass)
        {
            item.transform.GetComponent<Rigidbody>().centerOfMass = Handles.PositionHandle(item.transform.GetComponent<Rigidbody>().centerOfMass + new Vector3(-0.025f,0.008f,0), Quaternion.identity) - new Vector3(-0.025f, 0.008f, 0);
            item.centerOfMass = item.transform.GetComponent<Rigidbody>().centerOfMass * 10;
        }
    }
    private void OnDisable()
    {
        EditorUtility.SetDirty(target);
    }
}

[CustomEditor(typeof(BS.Preview))]
public class PreviewInspector : Editor
{
    public override void OnInspectorGUI()
    {
        BS.Preview preview = (BS.Preview)target;
        EditorGUILayout.HelpBox("The dark blue arrow points towards the viewer.", MessageType.Info);
    }
}

