using UnityEditor;
using System.IO;
using System;
using UnityEngine;
using System.Collections.Generic;

public class JsonBuilder : EditorWindow
{
    public string modName;
    public string modDirectory;

    private void OnFocus()
    {
        if (EditorTools.selectedModDirectory != null)
        {
            modName = EditorTools.selectedModDirectory.Substring(EditorTools.selectedModDirectory.LastIndexOf('\\') + 1, EditorTools.selectedModDirectory.Length - EditorTools.selectedModDirectory.LastIndexOf('\\') - 1);

            modDirectory = EditorTools.selectedModDirectory;
        }
    }

    Vector3 scroll4,scroll5;
    bool baseSelected = false;
    bool expandBaseItemList = false;
    string selectedBase;
    string selectedBaseDir;
    bool customHand = false;
    bool loadedPrefab = false;
    GameObject tempPrefab;
    string json;

    string jsonName;
    GameObject itemPrefab;
    string id, displayName, description, author, storageCategory;
    bool purchasable, paintable, grippable, useGravity, flyFromThrow;
    float mass, drag, angularDrag;
    int version, slot, damagerCount, handleCount, whooshCount;
    Damagers[] damagers;
    Interactables[] interactables;
    Whooshs[] whooshs;
    [Serializable]
    public class Damagers
    {
        public string transformName, damagerID;
        public Damagers(string name)
        {
            transformName = name;
        }
    }
    [Serializable]
    public class Interactables
    {
        public string transformName, interactableID, handPoseID; bool overrideHandlePose;
        public Interactables(string name)
        {
            transformName = name;
        }
    }
    [Serializable]
    public class Whooshs
    {
        public string transformName, fxID, trigger, minVelocity, maxVelocity;
        public Whooshs(string name)
        {
            transformName = name;
        }
    }


    private void OnGUI()
    {
        if (itemPrefab && !loadedPrefab)
        {
            LoadPrefab();
            loadedPrefab = true;
            tempPrefab = itemPrefab;
        }
        else if ((!itemPrefab && loadedPrefab) || tempPrefab != itemPrefab)
        {
            loadedPrefab = false;
        }

        GUILayout.Label("Creating item JSON for " + modName);

        EditorGUILayout.HelpBox("Base item is the file that will be used as a base to start editing your weapon JSON. Please select the item that more closely resembles your weapon.", MessageType.Info);
        if (!baseSelected)
        {
            if (GUILayout.Button("Select Base Item", new GUIStyle("DropDownButton")))
            {
                if (expandBaseItemList)
                {
                    expandBaseItemList = false;
                }
                else
                {
                    expandBaseItemList = true;
                }
            }
            if (expandBaseItemList)
            {
                scroll4 = GUILayout.BeginScrollView(scroll4);
                foreach (FileInfo file in new DirectoryInfo(EditorTools.streamingassetsDirectory + "\\Default\\Items").GetFiles())
                {
                    if (file.Name.StartsWith("Item_Weapon_"))
                    {
                        if (GUILayout.Button(file.Name, new GUIStyle("Radio")))
                        {
                            selectedBase = file.Name;
                            selectedBaseDir = file.FullName;
                            jsonName = file.Name.Remove(file.Name.IndexOf('.'));
                            baseSelected = true;
                            LoadJSON(selectedBaseDir);
                        }
                    }
                }
                GUILayout.EndScrollView();
            }
        }
        else
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Base: " + selectedBase))
            {
            }
            if (GUILayout.Button("Change Base"))
            {
                selectedBase = "";
                baseSelected = false;
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            AddField("File Name", ref jsonName);

            foreach (FileInfo file in new DirectoryInfo(modDirectory).GetFiles())
            {
                if (file.Name == jsonName + ".json")
                {
                    EditorGUILayout.HelpBox("A file with that name already exists. It will be replaced", MessageType.Warning);
                }
            }

            GUILayout.Space(10);
            AddField("Prefab", ref itemPrefab);
            scroll5 = GUILayout.BeginScrollView(scroll5);
            GUILayout.Space(5);
            
            DisplayWeaponInfo();
            DisplayWeaponConfig();
            DisplayWeaponDamagers();
            DisplayWeaponHandles();
            DisplayWeaponWhooshpoints();

            GUILayout.EndScrollView();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            if (!itemPrefab)
            {
                EditorGUILayout.HelpBox("Please assign a Prefab before creating file.", MessageType.Warning);
                EditorGUI.BeginDisabledGroup(true);
                GUILayout.Button("Finish and Create");
                EditorGUI.EndDisabledGroup();
            }
            else if (noAssetBundleOnPrefab)
            {
                EditorGUILayout.HelpBox("The prefab is not assigned to an AssetBundle.", MessageType.Error);
                if (GUILayout.Button("Retry"))
                {
                    CreateJSON();
                }
            }
            else if (noAuthorName)
            {
                EditorGUILayout.HelpBox("Author Name can not be empty.", MessageType.Error);
                if (GUILayout.Button("Retry"))
                {
                    noAuthorName = false;
                    CreateJSON();
                }
            }
            else if (noModName)
            {
                EditorGUILayout.HelpBox("Mod Name can not be empty.", MessageType.Error);
                if (GUILayout.Button("Retry"))
                {
                    noModName = false;
                    CreateJSON();
                }
            }
            else 
            {
                if (GUILayout.Button("Finish and Create"))
                {
                    CreateJSON();

                }
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
    }

    private void LoadJSON(string dir)
    {
        json = File.ReadAllText(dir);
        author = EditorPrefs.GetString("modAuthor");

        FindInJSON(json, "purchasable", out purchasable);
        FindInJSON(json, "slot", out slot);
        FindInJSON(json, "storageCategory", out storageCategory);
        FindInJSON(json, "mass", out mass);
        FindInJSON(json, "drag", out drag);
        FindInJSON(json, "angularDrag", out angularDrag);
        FindInJSON(json, "flyFromThrow", out flyFromThrow);
        FindInJSON(json, "useGravity", out useGravity);
        FindInJSON(json, "paintable", out paintable);
        FindInJSON(json, "grippable", out grippable);
    }

    private void LoadPrefab()
    {
        noAssetBundleOnPrefab = false;
        id = itemPrefab.GetComponentInChildren<BS.ItemDefinition>().itemId;
        damagers = new Damagers[itemPrefab.GetComponentInChildren(typeof(BS.ItemDefinition)).GetComponentsInChildren<BS.DamagerDefinition>().Length];
        interactables = new Interactables[itemPrefab.GetComponentInChildren(typeof(BS.ItemDefinition)).GetComponentsInChildren<BS.HandleDefinition>().Length];
        whooshs = new Whooshs[itemPrefab.GetComponentInChildren<BS.ItemDefinition>().whooshPoints.Count];
    }

    private void DisplayWeaponInfo()
    {
        GUILayout.Label("Weapon Info", new GUIStyle("BoldLabel"));
        GUILayout.BeginHorizontal();
        GUILayout.Space(20);
        GUILayout.BeginVertical();
        AddField("Displayed Name", ref displayName);
        AddField("Description", ref description, true);
        AddField("Mod Author", ref author);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
    }
    private void DisplayWeaponConfig()
    {
        GUILayout.Label("Weapon Config", new GUIStyle("BoldLabel"));
        GUILayout.BeginHorizontal();
        GUILayout.Space(20);
        GUILayout.BeginVertical();
        AddField("Available in book", ref purchasable);
        AddField("Holster Slot", ref slot);
        AddField("Storage Category", ref storageCategory);
        AddField("Mass", ref mass);
        AddField("Drag", ref drag);
        AddField("Angular Drag", ref angularDrag);
        AddField("Use Gravity", ref useGravity);
        AddField("Fly From Throw", ref flyFromThrow);
        AddField("Enable Decals", ref paintable);
        AddField("Enable Climbing Grip", ref grippable);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
    }
    private void DisplayWeaponDamagers()
    {
        GUILayout.Label("Damagers", new GUIStyle("BoldLabel"));
        GUILayout.BeginHorizontal();
        GUILayout.Space(20);
        GUILayout.BeginVertical();
        if (itemPrefab)
        {
            try
            {
                int damagerIndex = 0;
                foreach (BS.DamagerDefinition damager in itemPrefab.GetComponentInChildren(typeof(BS.ItemDefinition)).GetComponentsInChildren<BS.DamagerDefinition>())
                {
                    damagers[damagerIndex] = new Damagers(damager.name);
                    damagerIndex++;
                }

                if (damagers.Length > 0) { 
                foreach (Damagers damager in damagers)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(damager.transformName);
                    if (damager.damagerID != null)
                    {
                        if (GUILayout.Button(damager.damagerID, new GUIStyle("DropDownButton")))
                        {
                            //DAMAGER TYPE DROPDOWN
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("Select Damager Type", new GUIStyle("DropDownButton")))
                        {
                            //DAMAGER TYPE DROPDOWN
                        }
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
                }
                else
                {
                    GUILayout.Label("There are no Damagers on this prefab.");
                }
            }
            catch (Exception) {
                LoadPrefab();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Please assign a Prefab   ", MessageType.Warning);
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
    private void DisplayWeaponHandles()
    {
        GUILayout.Label("Handles", new GUIStyle("BoldLabel"));
        GUILayout.BeginHorizontal();
        GUILayout.Space(20);
        GUILayout.BeginVertical();
        if (itemPrefab)
        {
            try
            {
                int handleIndex = 0;
                foreach (BS.HandleDefinition handle in itemPrefab.GetComponentInChildren(typeof(BS.ItemDefinition)).GetComponentsInChildren<BS.HandleDefinition>())
                {
                    interactables[handleIndex] = new Interactables(handle.name);
                    handleIndex++;
                }

                if (interactables.Length > 0) { 
                foreach (Interactables handle in interactables)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(handle.transformName);
                    if (handle.interactableID != null)
                    {
                        if (GUILayout.Button(handle.interactableID, new GUIStyle("DropDownButton")))
                        {
                            //INTERACTABLE TYPE DROPDOWN
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("Select Interactable Type", new GUIStyle("DropDownButton")))
                        {
                            //INTERACTABLE TYPE DROPDOWN
                        }
                    }
                    if (customHand)
                    {
                        customHand = GUILayout.Toggle(customHand, "");
                        if (GUILayout.Button("Select Hand Pose", new GUIStyle("DropDownButton")))
                        {
                            //HANDLE POSE DROPDOWN
                        }
                    }
                    else
                    {
                        customHand = GUILayout.Toggle(customHand, "Custom Hand Pose");
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
                }
                else
                {
                    GUILayout.Label("There are no Handles on this prefab.");
                }
            }
            catch (Exception)
            {
                LoadPrefab();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Please assign a Prefab   ", MessageType.Warning);
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
    private void DisplayWeaponWhooshpoints()
    {
        GUILayout.Label("Whoosh Points", new GUIStyle("BoldLabel"));
        GUILayout.BeginHorizontal();
        GUILayout.Space(20);
        GUILayout.BeginVertical();
        if (itemPrefab)
        {
            try
            {
                int whooshIndex = 0;
                foreach (Transform whoosh in itemPrefab.GetComponentInChildren<BS.ItemDefinition>().whooshPoints)
                {
                    whooshs[whooshIndex] = new Whooshs(whoosh.name);
                    whooshIndex++;
                }

                if (whooshs.Length > 0)
                {
                    foreach (Whooshs whoosh in whooshs)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(whoosh.transformName);
                        if (GUILayout.Button("Sound Effect", new GUIStyle("DropDownButton")))
                        {

                        }
                        if (GUILayout.Button("Trigger Type", new GUIStyle("DropDownButton")))
                        {

                        }
                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();
                    }
                }
                else
                {
                    GUILayout.Label("There are no Whoosh points on this prefab.");
                }
            }
            catch (Exception)
            {
                LoadPrefab();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Please assign a Prefab   ", MessageType.Warning);
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    //FindInJSON Overloading
    private void FindInJSON(string json, string key, out string value)
    {
        string text = json.Substring(json.IndexOf("\"" + key + "\": \"") + key.Length + 5).Substring(0, json.Substring(json.IndexOf("\"" + key + "\": \"") + key.Length + 5).IndexOf("\""));
        //Debug.Log(text);
        if (text != "$type" && text != "")
        {
            value = text;
        }
        else
        {
            value = "key not found";
        }

    }
    private void FindInJSON(string json, string key, out int value)
    {
        string text = json.Substring(json.IndexOf("\"" + key + "\": ") + key.Length + 4).Substring(0, json.Substring(json.IndexOf("\"" + key + "\": ") + key.Length + 4).IndexOf(","));
        //Debug.Log(text);
        if (text != "$type" && text != "")
        {
            Int32.TryParse(text, out value);
        }
        else
        {
            value = 0;
        }

    }
    private void FindInJSON(string json, string key, out float value)
    {
        string text = json.Substring(json.IndexOf("\"" + key + "\": ") + key.Length + 4).Substring(0, json.Substring(json.IndexOf("\"" + key + "\": ") + key.Length + 4).IndexOf(","));
        //Debug.Log(text);
        if (text != "$type" && text != "")
        {
            value = float.Parse(text.Replace(".",","));
        }
        else
        {
            value = 0;
        }

    }
    private void FindInJSON(string json, string key, out bool value)
    {
        string text = json.Substring(json.IndexOf("\"" + key + "\": ") + key.Length + 4).Substring(0, json.Substring(json.IndexOf("\"" + key + "\": ") + key.Length + 4).IndexOf("e") + 1);
        //Debug.Log(text);
        if (text == "true")
        {
            value = true;
        } else 
        {
            value = false;
        }

    }

    //AddField Overloading
    private void AddField(string label, ref GameObject var)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label);
        var = (GameObject)EditorGUILayout.ObjectField(itemPrefab, typeof(GameObject), false);
        GUILayout.EndHorizontal();
    }
    private void AddField(string label, ref string var, bool alt = false)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label);
        if (alt)
        {
            GUILayout.BeginVertical();
            GUILayout.ExpandWidth(false);
            GUILayout.ExpandHeight(true);
            var = EditorGUILayout.TextArea(var);
            GUILayout.EndVertical();
        } else
        {
            var = EditorGUILayout.TextField(var);
        }
        
        GUILayout.EndHorizontal();
    }
    private void AddField(string label, ref bool var)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label);
        var = EditorGUILayout.Toggle(var);
        GUILayout.EndHorizontal();
    }
    private void AddField(string label, ref float var)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label);
        var = EditorGUILayout.FloatField(var);
        GUILayout.EndHorizontal();
    }
    private void AddField(string label, ref int var)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label);
        var = EditorGUILayout.IntField(var);
        GUILayout.EndHorizontal();
    }

    bool noAssetBundleOnPrefab;
    bool noAuthorName;
    bool noModName;
    private void CreateJSON()
    {
        noAssetBundleOnPrefab = false;
        noAuthorName = false;

        json = File.ReadAllText(selectedBaseDir);

        FindInJSON(json, "id", out string tempString);
        json = json.Replace("\"id\": \"" + tempString+"\"", "\"id\": \"" + id + "\"");

        if (displayName != null && displayName != "")
        {
            //NAME
        }
        else
        {
            noModName = true;
        }
        
        
            //DESCRIPTION
        
        
        if (author != null && author != "")
        {
            FindInJSON(json, "author", out tempString);
            json = json.Replace("\"author\": \"" + tempString + "\"", "\"author\": \"" + author + "\"");
        }
        else
        {
            noAuthorName = true;
        }
        //PRICE
        FindInJSON(json, "purchasable", out bool tempBool);
        json = json.Replace("\"purchasable\": " + tempBool, "\"purchasable\": " + purchasable);
        //TIER
        //LEVEL REQUIRED
        FindInJSON(json, "slot", out int tempInt);
        json = json.Replace("\"slot\": " + tempInt, "\"slot\": " + slot);
        //IS STACKABLE
        //CATEGORY
        FindInJSON(json, "storageCategory", out tempString);
        json = json.Replace("\"storageCategory\": \"" + tempString + "\"", "\"storageCategory\": \"" + storageCategory + "\"");
        if (AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(itemPrefab)).assetBundleName != "")
        {
            FindInJSON(json, "prefabName", out tempString);
            json = json.Replace("\"prefabName\": \"" + tempString + "\"", "\"prefabName\": \"@" + AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(itemPrefab)).assetBundleName + ":" + AssetDatabase.GetAssetPath(itemPrefab).Substring(AssetDatabase.GetAssetPath(itemPrefab).LastIndexOf('/') + 1, AssetDatabase.GetAssetPath(itemPrefab).Length - AssetDatabase.GetAssetPath(itemPrefab).LastIndexOf('/') - 1) + "\"");
        }
        else
        {
            noAssetBundleOnPrefab = true;
        }
        FindInJSON(json, "mass", out float tempFloat);
        json = json.Replace(("\"mass\": " + tempFloat).Replace(',', '.'), ("\"mass\": " + mass).Replace(',', '.'));
        FindInJSON(json, "drag", out tempFloat);
        json = json.Replace(("\"drag\": " + tempFloat).Replace(',', '.'), ("\"drag\": " + drag).Replace(',', '.'));
        FindInJSON(json, "angularDrag", out tempFloat);
        json = json.Replace(("\"angularDrag\": " + tempFloat).Replace(',', '.'), ("\"angularDrag\": " + angularDrag).Replace(',', '.'));
        FindInJSON(json, "useGravity", out tempBool);
        json = json.Replace("\"useGravity\": " + tempBool, "\"useGravity\": " + useGravity);
        FindInJSON(json, "flyFromThrow", out tempBool);
        json = json.Replace("\"flyFromThrow\": " + tempBool, "\"uflyFromThrow\": " + flyFromThrow);
        //FLY ROTATION SPEED
        //FLY THROW ANGLE
        //TELE SAFE DISTANCE
        //TELE SPIN ENABLED
        //TELE THROW RATIO
        //DAMAGE TRANSFER
        FindInJSON(json, "paintable", out tempBool);
        json = json.Replace("\"paintable\": " + tempBool, "\"paintable\": " + paintable);
        FindInJSON(json, "grippable", out tempBool);
        json = json.Replace("\"grippable\": " + tempBool, "\"grippable\": " + grippable);

        //DAMAGERS

        //INTERACTABLES

        //WHOOSHS

        EditorTools.AddPrefab(jsonName+".json", AssetDatabase.GetAssetPath(itemPrefab));

        EditorPrefs.SetString("modAuthor", author);

        if (!noAssetBundleOnPrefab && !noAuthorName && !noModName) {
            Debug.Log("JSON File \"" + jsonName + ".json\" created in directory " + modDirectory);
            File.WriteAllText(modDirectory + "\\" + jsonName + ".json", json);
            this.Close();
        }
    }
}
