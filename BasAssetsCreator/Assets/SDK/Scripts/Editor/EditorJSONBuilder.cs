using UnityEditor;
using System.IO;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace BS
{
    public class JsonBuilder : EditorWindow
    {
        public string modName;
        public string modDirectory;
        public static JSONData jsondata = new JSONData();
        bool initializedWithFile = false;

        Vector3 scroll4, scroll5;
        bool baseSelected = false;
        bool expandBaseItemList = false;
        string selectedBase;
        string selectedBaseDir;
        bool loadedPrefab = false;
        string json;
        string jsonName;
        GameObject tempPrefab;
        GameObject itemPrefab;
        public static string prefabKey;
        string[] slotOptions, bookCategories, itemCategories, weaponClass, weaponHandling, dodgeBehaviour, knockoutHit, knockoutThrow, damagerTypes, handleTypes, handlePoses, whooshSounds;
        [Serializable]
        public class JSONData
        {
            [Serializable]
            public class DisplayName
            {
                public string language;
                public string text;
            }
            [Serializable]
            public class Description
            {
                public string language;
                public string text;
            }
            public string MONEYSIGNtype = "BS.ItemData, Assembly-CSharp";
            public string id;
            public int version;
            public int pooledCount;
            public List<DisplayName> displayNames = new List<DisplayName>();
            public List<Description> descriptions = new List<Description>();
            public string author;
            public double price;
            public bool purchasable;
            public int tier;
            public int levelRequired;
            public int slot;
            public bool isStackable;
            public int category;
            public string storageCategory;
            public string prefabName;
            public double mass;
            public double drag;
            public double angularDrag;
            public bool useGravity;
            public bool overrideInertiaTensor;
            public InertiaTensor inertiaTensor;
            public int collisionMaxOverride;
            public bool collisionEnterOnly;
            public List<object> ignoredCollisionRagdollParts;
            public int forceLayer;
            public string audioSnapPath;
            public bool flyFromThrow;
            public double flyRotationSpeed;
            public double flyThrowAngle;
            public double telekinesisSafeDistance;
            public bool telekinesisSpinEnabled;
            public double telekinesisThrowRatio;
            public int damageTransfer;
            public bool paintable;
            public bool grippable;
            public List<object> customSnaps = new List<object>();
            public List<Module> modules = new List<Module>();
            public List<Damager> damagers = new List<Damager>();
            public List<Interactable> Interactables = new List<Interactable>();
            public List<Whoosh> whooshs = new List<Whoosh>();
            [Serializable]
            public class InertiaTensor
            {
                public double x;
                public double y;
                public double z;
            }
            [Serializable]
            public class Module
            {
                public string MONEYSIGNtype = "BS.ItemModuleAI, Assembly-CSharp";
                public int weaponClass;
                public int weaponHandling;
                public bool parryIgnoreRotation;
                public double parryRotation;
                public double parryDualRotation;
                public double armResistanceMultiplier;
                public ParryRevertAngleRange parryRevertAngleRange;
                public ParryDefaultPosition parryDefaultPosition;
                public ParryDefaultLeftRotation parryDefaultLeftRotation;
                public ParryDefaultRightRotation parryDefaultRightRotation;
                public bool allowDynamicHeight;
                public int dodgeBehaviour;
                public bool defenseHasPriority;
                public bool attackIgnore;
                public bool attackForceParryIgnoreRotation;
                [Serializable]
                public class ParryRevertAngleRange
                {
                    public double x;
                    public double y;
                }
                [Serializable]
                public class ParryDefaultPosition
                {
                    public double x;
                    public double y;
                    public double z;
                }
                [Serializable]
                public class ParryDefaultLeftRotation
                {
                    public double x;
                    public double y;
                    public double z;
                }
                [Serializable]
                public class ParryDefaultRightRotation
                {
                    public double x;
                    public double y;
                    public double z;
                }
            }
            [Serializable]
            public class Damager
            {
                public string transformName;
                public string damagerID;
            }
            [Serializable]
            public class Interactable
            {
                public string transformName;
                public string interactableId;
                public bool overrideHandPose;
                public string handPoseId;
            }
            [Serializable]
            public class Whoosh
            {
                public string transformName;
                public string fxId;
                public int trigger;
                public double minVelocity;
                public double maxVelocity;
            }
        }

        private void OnFocus()
        {
            
            if (EditorToolsJSONConfig.openJson != "") 
            {
                initializedWithFile = true;
                selectedBaseDir = EditorToolsJSONConfig.openJson;
                selectedBase = EditorToolsJSONConfig.openJson.Substring(EditorToolsJSONConfig.openJson.LastIndexOf('\\') + 1, EditorToolsJSONConfig.openJson.Length - EditorToolsJSONConfig.openJson.LastIndexOf('\\') - 1);
                jsonName = selectedBase.Remove(selectedBase.IndexOf('.'));
                baseSelected = true;
                loadedPrefab = true;
                LoadJSON(selectedBaseDir); 
                itemPrefab = EditorToolsJSONConfig.ModObject[prefabKey];
            } 

            if (EditorToolsJSONConfig.selectedModDirectory != null)
            {
                modName = EditorToolsJSONConfig.selectedModDirectory.Substring(EditorToolsJSONConfig.selectedModDirectory.LastIndexOf('\\') + 1, EditorToolsJSONConfig.selectedModDirectory.Length - EditorToolsJSONConfig.selectedModDirectory.LastIndexOf('\\') - 1);

                modDirectory = EditorToolsJSONConfig.selectedModDirectory;
            }
        }

        private void OnGUI()
        {
            if (!itemPrefab && initializedWithFile && prefabKey != null)
            {
                itemPrefab = EditorToolsJSONConfig.ModObject[prefabKey];
            }
            if (itemPrefab && !loadedPrefab && !initializedWithFile)   
            {
                LoadPrefab();
                loadedPrefab = true;
                tempPrefab = itemPrefab;
            }
            else if (((!itemPrefab && loadedPrefab) || tempPrefab != itemPrefab)&& !initializedWithFile) 
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
                    foreach (FileInfo file in new DirectoryInfo(EditorToolsJSONConfig.streamingassetsDirectory + "\\Default\\Items").GetFiles())
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

                if (initializedWithFile)
                {
                    GUI.enabled = false;
                }
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Base: " + selectedBase))
                {
                }
                if (!initializedWithFile)
                {
                    if (GUILayout.Button("Change Base"))
                    {
                        selectedBase = "";
                        baseSelected = false;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(10);
                AddField("File Name", ref jsonName);
                foreach (FileInfo file in new DirectoryInfo(modDirectory).GetFiles())
                {
                    if (!initializedWithFile && file.Name == jsonName + ".json")
                    {
                        EditorGUILayout.HelpBox("A file with that name already exists. It will be replaced", MessageType.Warning);
                    }
                }
                GUI.enabled = true;
                GUILayout.Space(10);
                AddField("Prefab", ref itemPrefab);
                scroll5 = GUILayout.BeginScrollView(scroll5);
                GUILayout.Space(5);
                
                try { 
                DisplayWeaponInfo();
                DisplayWeaponConfig();
                DisplayWeaponDamagers();
                DisplayWeaponHandles();
                DisplayWeaponWhooshpoints(); 
                } catch (Exception) {Close();}

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
                else if (noWeaponName)
                {
                    EditorGUILayout.HelpBox("Weapon Name can not be empty.", MessageType.Error);
                    if (GUILayout.Button("Retry"))
                    {
                        noWeaponName = false;
                        CreateJSON();
                    }
                } else if (noDamagerType)
                {
                    EditorGUILayout.HelpBox("Damager Types can not be empty.", MessageType.Error);
                    if (GUILayout.Button("Retry"))
                    {
                        noDamagerType = false;
                        CreateJSON();
                    }
                }
                else if (noHandleType)
                {
                    EditorGUILayout.HelpBox("Interactable Types can not be empty.", MessageType.Error);
                    if (GUILayout.Button("Retry"))
                    {
                        noHandleType = false;
                        CreateJSON();
                    }
                }
                else if (noHandlePose)
                {
                    EditorGUILayout.HelpBox("Hand Poses can not be empty and enabled.", MessageType.Error);
                    if (GUILayout.Button("Retry"))
                    {
                        noHandlePose = false;
                        CreateJSON();
                    }
                }
                else if (noWhooshFX)
                {
                    EditorGUILayout.HelpBox("Whoosh FXs can not be empty.", MessageType.Error);
                    if (GUILayout.Button("Retry"))
                    {
                        noWhooshFX = false;
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
            JsonUtility.FromJsonOverwrite(json, jsondata);
            if (!initializedWithFile) { 
            jsondata.author = EditorPrefs.GetString("modAuthor");
            }
            if (jsondata.displayNames.Count < 1)
            {
                jsondata.displayNames.Add(new JSONData.DisplayName());
            }
            if (jsondata.descriptions.Count < 1)
            {
                jsondata.descriptions.Add(new JSONData.Description());
            }
            if (jsondata.modules.Count < 1)
            {
                jsondata.modules.Add(new JSONData.Module());
            }


            slotOptions = new string[10] { "None", "Potion", "Small", "Medium", "Large", "Head", "Shield", "Arrow", "Bolt", "Cork" };
            bookCategories = new string[16] { "Daggers", "Swords", "Axes", "Blunt", "Spears", "Bows", "Crossbows", "Shields", "Exotics", "Wands", "Firearms", "Potions", "Traps", "Funny", "Unknown", "Misc" };
            itemCategories = new string[7] { "Misc", "Apparel", "Weapon", "Quiver", "Potion", "Prop", "Body" };
            weaponClass = new string[10] { "None", "Melee", "Bow", "Arrow", "Shield", "Wand", "Crossbow", "Bolt", "Fire Arm", "Rock" };
            weaponHandling = new string[3] { "None", "One Handed", "Two Handed" };
            dodgeBehaviour = new string[4] { "None", "Low Only", "Not Parrying", "Always" };
            knockoutHit = new string[3] { "None", "Always", "Sometimes" };
            knockoutThrow = new string[3] { "None", "Always", "Sometimes" };

            List<string> dmgrTyps = new List<string>();
            foreach (FileInfo file in new DirectoryInfo(EditorToolsJSONConfig.streamingassetsDirectory + "\\Default\\Damagers").GetFiles())
            {
                dmgrTyps.Add(file.Name.Substring(8, file.Name.Length - 13));
            }
            damagerTypes = dmgrTyps.ToArray();

            List<string> hndlTyps = new List<string>();
            foreach (FileInfo file in new DirectoryInfo(EditorToolsJSONConfig.streamingassetsDirectory + "\\Default\\Interactables").GetFiles())
            {
                hndlTyps.Add(file.Name.Substring(13, file.Name.Length - 18));
            }
            handleTypes = hndlTyps.ToArray();

            List<string> hndlPse = new List<string>();
            foreach (FileInfo file in new DirectoryInfo(EditorToolsJSONConfig.streamingassetsDirectory + "\\Default\\HandPoses").GetFiles())
            {
                hndlPse.Add(file.Name.Substring(9, file.Name.Length - 14));
            }
            handlePoses = hndlPse.ToArray();

            List<string> wshFx = new List<string>();
            foreach (FileInfo file in new DirectoryInfo(EditorToolsJSONConfig.streamingassetsDirectory + "\\Default\\FXs").GetFiles())
            {
                wshFx.Add(file.Name.Substring(3, file.Name.Length - 8));
            }
            whooshSounds = wshFx.ToArray();
        }

        private void LoadPrefab()
        {
            noAssetBundleOnPrefab = false;
            jsondata.id = itemPrefab.GetComponentInChildren<ItemDefinition>().itemId;
            jsondata.damagers = new List<JSONData.Damager>(itemPrefab.GetComponentInChildren(typeof(ItemDefinition)).GetComponentsInChildren<BS.DamagerDefinition>().Length);
            jsondata.Interactables = new List<JSONData.Interactable>(itemPrefab.GetComponentInChildren(typeof(ItemDefinition)).GetComponentsInChildren<HandleDefinition>().Length);
            jsondata.whooshs = new List<JSONData.Whoosh>(itemPrefab.GetComponentInChildren<ItemDefinition>().whooshPoints.Count);


            int damagerIndex = 0;
            foreach (DamagerDefinition damager in itemPrefab.GetComponentInChildren(typeof(ItemDefinition)).GetComponentsInChildren<BS.DamagerDefinition>())
            {
                jsondata.damagers.Add(new JSONData.Damager());
                jsondata.damagers[damagerIndex].transformName = damager.name;
                damagerIndex++;
            }
            int interactableIndex = 0;
            foreach (HandleDefinition handle in itemPrefab.GetComponentInChildren(typeof(ItemDefinition)).GetComponentsInChildren<HandleDefinition>())
            {
                jsondata.Interactables.Add(new JSONData.Interactable());
                jsondata.Interactables[interactableIndex].transformName = handle.name;
                interactableIndex++;
            }
            int wooshIndex = 0;
            foreach (Transform woosh in itemPrefab.GetComponentInChildren<ItemDefinition>().whooshPoints)
            {
                jsondata.whooshs.Add(new JSONData.Whoosh());
                jsondata.whooshs[wooshIndex].transformName = woosh.name;
                wooshIndex++;
            }
        }

        private void DisplayWeaponInfo()
        {
            GUILayout.Label("Weapon Info", new GUIStyle("BoldLabel"));
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.BeginVertical();

            AddField("Displayed Name", ref jsondata.displayNames[0].text);
            AddField("Description", ref jsondata.descriptions[0].text, true);
            AddField("Mod Author", ref jsondata.author);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
        }
        bool slotDropdown = false;
        bool bookDropdown = false;
        bool itemDropdown = false;
        bool wpnDropdown = false;
        bool hndlngDropdown = false;
        //bool dodgeDropdown = false;
        private void DisplayWeaponConfig()
        {
            GUILayout.Label("Weapon Config", new GUIStyle("BoldLabel"));
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.BeginVertical();
            AddField("Available in book", ref jsondata.purchasable);

            if (!jsondata.purchasable) { GUI.enabled = false; }
            GUILayout.BeginHorizontal();
            GUILayout.Label("Storage Book Category");
            GUILayout.Space(20);
            AddFieldDropdown("Select Category", ref jsondata.storageCategory, bookCategories, ref bookDropdown);
            GUI.enabled = true;
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Holster Slot");
            GUILayout.Space(89);
            string slot = slotOptions[jsondata.slot];
            AddFieldDropdown("Select Slot", ref slot, slotOptions, ref slotDropdown);
            jsondata.slot = Array.IndexOf(slotOptions, slot);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Item Type");
            GUILayout.Space(96);
            string category = itemCategories[jsondata.category];
            AddFieldDropdown("Select Category", ref category, itemCategories, ref itemDropdown);
            jsondata.category = Array.IndexOf(itemCategories, category);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            if (jsondata.category != 2) { GUI.enabled = false; }
            GUILayout.BeginHorizontal();
            GUILayout.Label("Weapon Type");
            GUILayout.Space(77);
            string weapon = weaponClass[jsondata.modules[0].weaponClass];
            AddFieldDropdown("Select Weapon Type", ref weapon, weaponClass, ref wpnDropdown);
            jsondata.modules[0].weaponClass = Array.IndexOf(weaponClass, weapon);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUI.enabled = true;
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Weapon Handling");
            GUILayout.Space(56);
            string hndlng = weaponHandling[jsondata.modules[0].weaponHandling];
            AddFieldDropdown("Select Handling Mode", ref hndlng, weaponHandling, ref hndlngDropdown);
            jsondata.modules[0].weaponHandling = Array.IndexOf(weaponHandling, hndlng);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            AddField("Mass", ref jsondata.mass);
            AddField("Drag", ref jsondata.drag);
            AddField("Angular Drag", ref jsondata.angularDrag);
            AddField("Use Gravity", ref jsondata.useGravity);
            AddField("Fly From Throw", ref jsondata.flyFromThrow);

            AddField("Dodge Behaviour", ref jsondata.modules[0].dodgeBehaviour, 3);
            GUILayout.Space(10);
            AddField("Enable Decals", ref jsondata.paintable);
            AddField("Enable Climbing Grip", ref jsondata.grippable);

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
        }
        Dictionary<string, bool> damDropdown = new Dictionary<string, bool>();
        Dictionary<string, bool> hndlDropdown = new Dictionary<string, bool>();
        Dictionary<string, bool> hndlHandDropdown = new Dictionary<string, bool>();
        Dictionary<string, bool> wshSoundDropdown = new Dictionary<string, bool>();
        Dictionary<string, bool> wshTriggDropdown = new Dictionary<string, bool>();
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
                    if (jsondata.damagers.Count > 0)
                    {
                        foreach (JSONData.Damager damager in jsondata.damagers)
                        {

                            GUILayout.BeginHorizontal();

                            if (!damDropdown.ContainsKey(damager.transformName))
                            {
                                damDropdown[damager.transformName] = false;
                            }
                            bool drpdwn = damDropdown[damager.transformName];
                            GUILayout.Label(damager.transformName);
                            AddFieldDropdown("Select Damager Type", ref damager.damagerID,damagerTypes, ref drpdwn);
                            damDropdown[damager.transformName] = drpdwn;

                            GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                        }
                    }
                    else
                    {
                        GUILayout.Label("There are no Damagers on this prefab.");
                    }
                }
                catch (Exception) { }
                //}
                //catch (Exception)
                //{
                //    LoadPrefab();
                //}
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
                    if (jsondata.Interactables.Count > 0)
                    {
                        foreach (JSONData.Interactable handle in jsondata.Interactables)
                        {
                            GUILayout.BeginHorizontal();
                            if (!hndlDropdown.ContainsKey(handle.transformName))
                            {
                                hndlDropdown[handle.transformName] = false;
                            }
                            bool drpdwn = hndlDropdown[handle.transformName];
                            GUILayout.Label(handle.transformName);
                            AddFieldDropdown("Select Interactable Type", ref handle.interactableId, handleTypes, ref drpdwn);
                            hndlDropdown[handle.transformName] = drpdwn;
                            if (handle.overrideHandPose)
                            {
                                handle.overrideHandPose = GUILayout.Toggle(handle.overrideHandPose, "");
                                if (!hndlHandDropdown.ContainsKey(handle.transformName))
                                {
                                    hndlHandDropdown[handle.transformName] = false;
                                }
                                bool drpdwn2 = hndlHandDropdown[handle.transformName];
                                AddFieldDropdown("Select Hand Pose", ref handle.handPoseId, handlePoses, ref drpdwn2);
                                hndlHandDropdown[handle.transformName] = drpdwn2;
                            }
                            else
                            {
                                handle.overrideHandPose = GUILayout.Toggle(handle.overrideHandPose, "Custom Hand Pose");
                                hndlHandDropdown[handle.transformName] = false;
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
                catch (Exception) { }
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
                    if (jsondata.whooshs.Count > 0)
                    {
                        foreach (JSONData.Whoosh whoosh in jsondata.whooshs)
                        {
                            GUILayout.BeginHorizontal();
                            if (!wshSoundDropdown.ContainsKey(whoosh.transformName))
                            {
                                wshSoundDropdown[whoosh.transformName] = false;
                            }
                            bool whsh = wshSoundDropdown[whoosh.transformName];
                            GUILayout.Label(whoosh.transformName);
                            AddFieldDropdown("Sound Effect", ref whoosh.fxId, whooshSounds, ref whsh);
                            wshSoundDropdown[whoosh.transformName] = whsh;
                            
                            GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                        }
                    }
                    else
                    {
                        GUILayout.Label("There are no Whoosh points on this prefab.");
                    }
                }
                catch (Exception) { }
            }
            else
            {
                EditorGUILayout.HelpBox("Please assign a Prefab   ", MessageType.Warning);
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
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
                EditorStyles.textField.wordWrap = true;
                var = EditorGUILayout.TextArea(var);
                GUILayout.EndVertical();
            }
            else
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
        private void AddField(string label, ref double var, int max = -1)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label);
            var = EditorGUILayout.DoubleField(var);
            if (max > 0 && var > max)
            {
                var = max;
            }
            else if (max > 0 && var < 0)
            {
                var = 0;
            }
            GUILayout.EndHorizontal();
        }
        private void AddField(string label, ref int var, int max = -1)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label);
            var = EditorGUILayout.IntField(var);
            if (max > 0 && var > max)
            {
                var = max;
            }
            else if (max > 0 && var < 0)
            {
                var = 0;
            }
            GUILayout.EndHorizontal();
        }
        private void AddFieldDropdown(string label, ref string var, string[] options, ref bool toggle)
        {
            GUILayout.BeginVertical();
            if (var == "" || var == null)
            {
                if (GUILayout.Button(label, new GUIStyle("DropDownButton")))
                {
                    if (toggle) { toggle = false; } else { toggle = true; }
                }
            }
            else
            {
                if (GUILayout.Button(var, new GUIStyle("DropDownButton")))
                {
                    if (toggle) { toggle = false; } else { toggle = true; }
                }
            }
            if (toggle)
            {
                foreach (string option in options)
                {
                    if (GUILayout.Button(option, new GUIStyle("Radio")))
                    {
                        var = option;
                        toggle = false;
                    }
                }
                GUILayout.Space(10);

            }
            GUILayout.EndVertical();
        }

        bool noAssetBundleOnPrefab, noAuthorName, noWeaponName, noDamagerType, noHandleType, noHandlePose, noWhooshFX;
        private void CreateJSON()
        {
            noAssetBundleOnPrefab = false;
            noAuthorName = false;
            noDamagerType = false;
            noHandleType = false;
            noHandlePose = false;
            noWhooshFX = false;

            json = File.ReadAllText(selectedBaseDir);

            if (jsondata.displayNames[0].text == null || jsondata.displayNames[0].text == "")
            {
                noWeaponName = true;
            }
            foreach (JSONData.Damager damager in jsondata.damagers)
            {
                if (damager.damagerID == null || damager.damagerID == "")
                {
                    noDamagerType = true;
                }
            }
            foreach (JSONData.Interactable handle in jsondata.Interactables)
            {
                if (handle.interactableId == null || handle.interactableId == "")
                {
                    noHandleType = true;
                }
                if ((handle.handPoseId == null || handle.handPoseId == "") && handle.overrideHandPose)
                {
                    noHandlePose = true;
                }
            }
            foreach (JSONData.Whoosh whoosh in jsondata.whooshs)
            {
                if (whoosh.fxId == null || whoosh.fxId == "")
                {
                    noWhooshFX = true;
                }
            }

            json = JsonUtility.ToJson(jsondata, true).Replace("MONEYSIGN", "$");

            EditorToolsJSONConfig.AddPrefab(jsonName + ".json", AssetDatabase.GetAssetPath(itemPrefab));
            if (!initializedWithFile)
            {
                EditorPrefs.SetString("modAuthor", jsondata.author);
                if (!noAssetBundleOnPrefab && !noAuthorName && !noWeaponName && !noDamagerType && !noHandlePose && !noHandleType && !noWhooshFX)
                {
                    Debug.Log("JSON File \"" + jsonName + ".json\" created in directory " + modDirectory);
                    File.WriteAllText(modDirectory + "\\" + jsonName + ".json", json);
                    Close();
                }
            } else
            {
                if (!noAssetBundleOnPrefab && !noAuthorName && !noWeaponName && !noDamagerType && !noHandlePose && !noHandleType && !noWhooshFX)
                {
                    Debug.Log("JSON File \"" + jsonName + ".json\" created in directory " + modDirectory);
                    File.WriteAllText(selectedBaseDir, json);
                    Close();
                }
            }
            
            
            
        }
        
    }
}