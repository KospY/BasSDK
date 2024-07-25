using UnityEditor;
using System.IO;
using System;
using UnityEngine;
using System.Collections.Generic;

//namespace ThunderRoad
//{
//    public class WeaponJsonBuilder : EditorWindow
//    {
//        public string modName;
//        public string modDirectory;
//        JSONData jsondata = new JSONData();
//        bool initializedWithFile = false;

//        Vector3 scroll4, scroll5;
//        bool baseSelected = false;
//        bool expandBaseItemList = false;
//        string selectedBase;
//        string selectedBaseDir;
//        bool loadedPrefab = false;
//        string json, jsonModules;
//        string jsonName;
//        GameObject tempPrefab;
//        GameObject itemPrefab;
//        public static string prefabKey;
//        string[] slotOptions, bookCategories, itemCategories, weaponClass, weaponHandling, dodgeBehaviour, knockoutHit, knockoutThrow, damagerTypes, handleTypes, handlePoses, whooshSounds;
//        [Serializable]
//        public class JSONData
//        {
//            public string MONEYSIGNtype = "BS.ItemData, Assembly-CSharp";
//            public string id;
//            public int version;
//            public int pooledCount;
//            public string displayName;
//            public string description;
//            public string author;
//            public double price;
//            public bool purchasable;
//            public int tier;
//            public int levelRequired;
//            public int slot;
//            public bool isStackable;
//            public int category;
//            public string storageCategory;
//            public string prefabName;
//            public double mass;
//            public double drag;
//            public double angularDrag;
//            public bool useGravity;
//            public bool overrideInertiaTensor;
//            public InertiaTensor inertiaTensor;
//            public int collisionMaxOverride;
//            public bool collisionEnterOnly;
//            public List<object> ignoredCollisionRagdollParts;
//            public int forceLayer;
//            public string audioSnapPath;
//            public bool flyFromThrow;
//            public double flyRotationSpeed;
//            public double flyThrowAngle;
//            public double telekinesisSafeDistance;
//            public bool telekinesisSpinEnabled;
//            public double telekinesisThrowRatio;
//            public int damageTransfer;
//            public bool grippable;
//            public List<object> customSnaps = new List<object>();
//            public List<Module> modules = new List<Module>();
//            public List<Damager> damagers = new List<Damager>();
//            public List<Interactable> Interactables = new List<Interactable>();
//            public List<Whoosh> whooshs = new List<Whoosh>();
//            [Serializable]
//            public class InertiaTensor
//            {
//                public double x;
//                public double y;
//                public double z;
//            }
//            [Serializable]
//            public class Module
//            {
//                public string MONEYSIGNtype = "BS.ItemModuleAI, Assembly-CSharp";
//                public int weaponClass;
//                public int weaponHandling;
//                public bool parryIgnoreRotation;
//                public double parryRotation;
//                public double parryDualRotation;
//                public double armResistanceMultiplier;
//                public ParryRevertAngleRange parryRevertAngleRange;
//                public ParryDefaultPosition parryDefaultPosition;
//                public ParryDefaultLeftRotation parryDefaultLeftRotation;
//                public ParryDefaultRightRotation parryDefaultRightRotation;
//                public bool allowDynamicHeight;
//                public int dodgeBehaviour;
//                public bool defenseHasPriority;
//                public bool attackIgnore;
//                public bool attackForceParryIgnoreRotation;
//                [Serializable]
//                public class ParryRevertAngleRange
//                {
//                    public double x;
//                    public double y;
//                }
//                [Serializable]
//                public class ParryDefaultPosition
//                {
//                    public double x;
//                    public double y;
//                    public double z;
//                }
//                [Serializable]
//                public class ParryDefaultLeftRotation
//                {
//                    public double x;
//                    public double y;
//                    public double z;
//                }
//                [Serializable]
//                public class ParryDefaultRightRotation
//                {
//                    public double x;
//                    public double y;
//                    public double z;
//                }
//            }
//            [Serializable]
//            public class Damager
//            {
//                public string transformName;
//                public string damagerID;
//            }
//            [Serializable]
//            public class Interactable
//            {
//                public string transformName;
//                public string interactableId;
//                public bool overrideHandPose;
//                public string handPoseId;
//            }
//            [Serializable]
//            public class Whoosh
//            {
//                public string transformName;
//                public string fxId;
//                public int trigger;
//                public double minVelocity = 5;
//                public double maxVelocity = 12;
//            }
//        }

//        private void OnFocus()
//        {

//            if (EditorToolsJSONConfig.openJson != "")
//            {
//                initializedWithFile = true;
//                selectedBaseDir = EditorToolsJSONConfig.openJson;
//                selectedBase = EditorToolsJSONConfig.openJson.Substring(EditorToolsJSONConfig.openJson.LastIndexOf('\\') + 1, EditorToolsJSONConfig.openJson.Length - EditorToolsJSONConfig.openJson.LastIndexOf('\\') - 1);
//                jsonName = selectedBase.Remove(selectedBase.IndexOf('.'));
//                baseSelected = true;
//                loadedPrefab = true;
//                LoadJSON(selectedBaseDir);
//                itemPrefab = EditorToolsJSONConfig.ModObject[prefabKey];
//            }

//            if (EditorToolsJSONConfig.selectedModDirectory != null)
//            {
//                modName = EditorToolsJSONConfig.selectedModDirectory.Substring(EditorToolsJSONConfig.selectedModDirectory.LastIndexOf('\\') + 1, EditorToolsJSONConfig.selectedModDirectory.Length - EditorToolsJSONConfig.selectedModDirectory.LastIndexOf('\\') - 1);

//                modDirectory = EditorToolsJSONConfig.selectedModDirectory;
//            }
//        }

//        private void OnGUI()
//        {
//            if (!itemPrefab && initializedWithFile && prefabKey != null)
//            {
//                itemPrefab = EditorToolsJSONConfig.ModObject[prefabKey];
//            }
//            if (itemPrefab && !loadedPrefab && !initializedWithFile)
//            {
//                LoadPrefab();
//                loadedPrefab = true;
//                tempPrefab = itemPrefab;
//            }
//            else if (((!itemPrefab && loadedPrefab) || tempPrefab != itemPrefab) && !initializedWithFile)
//            {
//                loadedPrefab = false;
//            }

//            GUILayout.Label("Creating item JSON for " + modName);

//            EditorGUILayout.HelpBox("Base item is the file that will be used as a base to start editing your weapon JSON. Please select the item that more closely resembles your weapon.", MessageType.Info);


//            if (!baseSelected)
//            {
//                if (GUILayout.Button("Select Base Item", new GUIStyle("DropDownButton")))
//                {
//                    if (expandBaseItemList)
//                    {
//                        expandBaseItemList = false;
//                    }
//                    else
//                    {
//                        expandBaseItemList = true;
//                    }
//                }
//                if (expandBaseItemList)
//                {
//                    scroll4 = GUILayout.BeginScrollView(scroll4);
//                    foreach (FileInfo file in new DirectoryInfo(EditorToolsJSONConfig.streamingAssetsDirectory + "\\Default\\Items").GetFiles())
//                    {
//                        if (file.Name.StartsWith("Item_Weapon_"))
//                        {
//                            if (GUILayout.Button(file.Name, new GUIStyle("Radio")))
//                            {
//                                selectedBase = file.Name;
//                                selectedBaseDir = file.FullName;
//                                jsonName = file.Name.Remove(file.Name.IndexOf('.'));
//                                baseSelected = true;
//                                LoadJSON(selectedBaseDir);
//                            }
//                        }
//                    }
//                    GUILayout.EndScrollView();
//                }
//            }
//            else
//            {

//                if (initializedWithFile)
//                {
//                    GUI.enabled = false;
//                    if (itemPrefab.GetComponentInChildren(typeof(ItemDefinition)).GetComponentsInChildren<WhooshPoint>().Length != jsondata.whooshs.Count)
//                    {
//                        int whooshIndex = 0;
//                        jsondata.whooshs.Clear();
//                        foreach (WhooshPoint whoosh in itemPrefab.GetComponentInChildren(typeof(ItemDefinition)).GetComponentsInChildren<WhooshPoint>())
//                        {
//                            jsondata.whooshs.Add(new JSONData.Whoosh());
//                            jsondata.whooshs[whooshIndex].transformName = whoosh.name;
//                            jsondata.whooshs[whooshIndex].fxId = "";
//                            whooshIndex++;
//                        }
//                    }
//                }
//                GUILayout.BeginHorizontal();
//                if (GUILayout.Button("Base: " + selectedBase))
//                {
//                }
//                if (!initializedWithFile)
//                {
//                    if (GUILayout.Button("Change Base"))
//                    {
//                        selectedBase = "";
//                        baseSelected = false;
//                    }
//                }
//                GUILayout.EndHorizontal();
//                GUILayout.Space(10);
//                AddField("File Name", ref jsonName);
//                foreach (FileInfo file in new DirectoryInfo(modDirectory).GetFiles())
//                {
//                    if (!initializedWithFile && file.Name == jsonName + ".json")
//                    {
//                        EditorGUILayout.HelpBox("A file with that name already exists. It will be replaced", MessageType.Warning);
//                    }
//                }
//                GUI.enabled = true;
//                GUILayout.Space(10);
//                AddField("Prefab", ref itemPrefab);
//                scroll5 = GUILayout.BeginScrollView(scroll5);
//                GUILayout.Space(5);

//                try
//                {
//                    DisplayWeaponInfo();
//                    DisplayWeaponConfig();
//                    DisplayWeaponDamagers();
//                    DisplayWeaponHandles();
//                    DisplayWeaponWhooshpoints();
//                }
//                catch (Exception e) { Debug.Log("Window closed. " + e); Close(); }

//                GUILayout.EndScrollView();
//                GUILayout.BeginHorizontal();
//                GUILayout.FlexibleSpace();
//                GUILayout.BeginVertical();
//                if (!itemPrefab)
//                {
//                    EditorGUILayout.HelpBox("Please assign a Prefab before creating file.", MessageType.Warning);
//                    EditorGUI.BeginDisabledGroup(true);
//                    GUILayout.Button("Finish and Create");
//                    EditorGUI.EndDisabledGroup();
//                }
//                else if (noAssetBundleOnPrefab)
//                {
//                    EditorGUILayout.HelpBox("The prefab is not assigned to an AssetBundle.", MessageType.Error);
//                    if (GUILayout.Button("Retry"))
//                    {
//                        CreateJSON();
//                    }
//                }
//                else if (noAuthorName)
//                {
//                    EditorGUILayout.HelpBox("Author Name can not be empty.", MessageType.Error);
//                    if (GUILayout.Button("Retry"))
//                    {
//                        noAuthorName = false;
//                        CreateJSON();
//                    }
//                }
//                else if (noWeaponName)
//                {
//                    EditorGUILayout.HelpBox("Weapon Name can not be empty.", MessageType.Error);
//                    if (GUILayout.Button("Retry"))
//                    {
//                        noWeaponName = false;
//                        CreateJSON();
//                    }
//                }
//                else if (noDamagerType)
//                {
//                    EditorGUILayout.HelpBox("Damager Types can not be empty.", MessageType.Error);
//                    if (GUILayout.Button("Retry"))
//                    {
//                        noDamagerType = false;
//                        CreateJSON();
//                    }
//                }
//                else if (noHandleType)
//                {
//                    EditorGUILayout.HelpBox("Interactable Types can not be empty.", MessageType.Error);
//                    if (GUILayout.Button("Retry"))
//                    {
//                        noHandleType = false;
//                        CreateJSON();
//                    }
//                }
//                else if (noHandlePose)
//                {
//                    EditorGUILayout.HelpBox("Hand Poses can not be empty and enabled.", MessageType.Error);
//                    if (GUILayout.Button("Retry"))
//                    {
//                        noHandlePose = false;
//                        CreateJSON();
//                    }
//                }
//                else if (noWhooshFX)
//                {
//                    EditorGUILayout.HelpBox("Whoosh FXs can not be empty.", MessageType.Error);
//                    if (GUILayout.Button("Retry"))
//                    {
//                        noWhooshFX = false;
//                        CreateJSON();
//                    }
//                }
//                else
//                {
//                    if (GUILayout.Button("Finish and Create"))
//                    {
//                        CreateJSON();
//                    }
//                }
//                GUILayout.EndVertical();
//                GUILayout.EndHorizontal();
//            }

//        }

//        private void LoadJSON(string dir)
//        {
//            json = File.ReadAllText(dir);
//            jsonModules = json.Substring(json.IndexOf("modules")-1).Substring(0, json.Substring(json.IndexOf("modules") - 1).IndexOf("],")+2);
//            JsonUtility.FromJsonOverwrite(json.Replace("$", "MONEYSIGN"), jsondata);
//            if (jsondata.MONEYSIGNtype == "BS.ItemData, Assembly-CSharp")
//            {
//                if (!initializedWithFile)
//                {
//                    jsondata.author = EditorPrefs.GetString("modAuthor");
//                }
//                if (jsondata.modules.Count < 1)
//                {
//                    jsondata.modules.Add(new JSONData.Module());
//                }


//                slotOptions = new string[10] { "None", "Potion", "Small", "Medium", "Large", "Head", "Shield", "Arrow", "Bolt", "Cork" };
//                bookCategories = new string[16] { "Daggers", "Swords", "Axes", "Blunt", "Spears", "Bows", "Crossbows", "Shields", "Exotics", "Wands", "Firearms", "Potions", "Traps", "Funny", "Unknown", "Misc" };
//                itemCategories = new string[7] { "Misc", "Apparel", "Weapon", "Quiver", "Potion", "Prop", "Body" };
//                weaponClass = new string[10] { "None", "Melee", "Bow", "Arrow", "Shield", "Wand", "Crossbow", "Bolt", "Fire Arm", "Rock" };
//                weaponHandling = new string[3] { "None", "One Handed", "Two Handed" };
//                dodgeBehaviour = new string[4] { "None", "Low Only", "Not Parrying", "Always" };
//                knockoutHit = new string[3] { "None", "Always", "Sometimes" };
//                knockoutThrow = new string[3] { "None", "Always", "Sometimes" };

//                List<string> dmgrTyps = new List<string>();
//                foreach (FileInfo file in new DirectoryInfo(EditorToolsJSONConfig.streamingAssetsDirectory + "\\Default\\Damagers").GetFiles())
//                {
//                    dmgrTyps.Add(file.Name.Substring(8, file.Name.Length - 13));
//                }
//                damagerTypes = dmgrTyps.ToArray();

//                List<string> hndlTyps = new List<string>();
//                foreach (FileInfo file in new DirectoryInfo(EditorToolsJSONConfig.streamingAssetsDirectory + "\\Default\\Interactables").GetFiles())
//                {
//                    hndlTyps.Add(file.Name.Substring(13, file.Name.Length - 18));
//                }
//                handleTypes = hndlTyps.ToArray();

//                List<string> hndlPse = new List<string>();
//                foreach (FileInfo file in new DirectoryInfo(EditorToolsJSONConfig.streamingAssetsDirectory + "\\Default\\HandPoses").GetFiles())
//                {
//                    hndlPse.Add(file.Name.Substring(9, file.Name.Length - 14));
//                }
//                handlePoses = hndlPse.ToArray();

//                List<string> wshFx = new List<string>();
//                foreach (FileInfo file in new DirectoryInfo(EditorToolsJSONConfig.streamingAssetsDirectory + "\\Default\\FXs").GetFiles())
//                {
//                    wshFx.Add(file.Name.Substring(3, file.Name.Length - 8));
//                }
//                whooshSounds = wshFx.ToArray();
//            }
//        }

//        private void LoadPrefab()
//        {
//            noAssetBundleOnPrefab = false;
//            jsondata.id = itemPrefab.GetComponentInChildren<ItemDefinition>().itemId;
//            string defaultFx = "";
//            if (jsondata.whooshs.Count > 0)
//            {
//                defaultFx = jsondata.whooshs[0].fxId;
//            }
//            jsondata.damagers = new List<JSONData.Damager>(itemPrefab.GetComponentInChildren(typeof(ItemDefinition)).GetComponentsInChildren<ThunderRoad.DamagerDefinition>().Length);
//            jsondata.Interactables = new List<JSONData.Interactable>(itemPrefab.GetComponentInChildren(typeof(ItemDefinition)).GetComponentsInChildren<HandleDefinition>().Length);


//            int damagerIndex = 0;
//            foreach (DamagerDefinition damager in itemPrefab.GetComponentInChildren(typeof(ItemDefinition)).GetComponentsInChildren<ThunderRoad.DamagerDefinition>())
//            {
//                jsondata.damagers.Add(new JSONData.Damager());
//                jsondata.damagers[damagerIndex].transformName = damager.name;
//                damagerIndex++;
//            }
//            int interactableIndex = 0;
//            foreach (HandleDefinition handle in itemPrefab.GetComponentInChildren(typeof(ItemDefinition)).GetComponentsInChildren<HandleDefinition>())
//            {
//                jsondata.Interactables.Add(new JSONData.Interactable());
//                jsondata.Interactables[interactableIndex].transformName = handle.name;
//                interactableIndex++;
//            }
//            int whooshIndex = 0;
//            jsondata.whooshs.Clear();
//            foreach (WhooshPoint whoosh in itemPrefab.GetComponentInChildren(typeof(ItemDefinition)).GetComponentsInChildren<WhooshPoint>())
//            {
//                jsondata.whooshs.Add(new JSONData.Whoosh());
//                jsondata.whooshs[whooshIndex].transformName = whoosh.name;
//                jsondata.whooshs[whooshIndex].fxId = defaultFx;
//                whooshIndex++;
//            }
//        }

//        private void DisplayWeaponInfo()
//        {
//            GUILayout.Label("Weapon Info", new GUIStyle("BoldLabel"));
//            GUILayout.BeginHorizontal();
//            GUILayout.Space(20);
//            GUILayout.BeginVertical();

//            AddField("Displayed Name", ref jsondata.displayName);
//            AddField("Description", ref jsondata.description, true);
//            AddField("Mod Author", ref jsondata.author);
//            GUILayout.EndVertical();
//            GUILayout.EndHorizontal();
//            GUILayout.Space(5);
//        }

//        bool slotDropdown = false;
//        bool bookDropdown = false;
//        bool itemDropdown = false;
//        bool wpnDropdown = false;
//        bool hndlngDropdown = false;
//        bool ddgDropdown = false;
//        private void DisplayWeaponConfig()
//        {
//            GUILayout.Label("Weapon Config", new GUIStyle("BoldLabel"));
//            GUILayout.BeginHorizontal();
//            GUILayout.Space(20);
//            GUILayout.BeginVertical();
//            AddField("Available in book", ref jsondata.purchasable);

//            if (!jsondata.purchasable) { GUI.enabled = false; }
//            GUILayout.BeginHorizontal();
//            GUILayout.Label("Storage Book Category");
//            GUILayout.Space(20);
//            AddFieldDropdown("Select Category", ref jsondata.storageCategory, bookCategories, ref bookDropdown);
//            GUI.enabled = true;
//            GUILayout.FlexibleSpace();
//            GUILayout.EndHorizontal();

//            GUILayout.BeginHorizontal();
//            GUILayout.Label("Holster Slot");
//            GUILayout.Space(89);
//            string slot = slotOptions[jsondata.slot];
//            AddFieldDropdown("Select Slot", ref slot, slotOptions, ref slotDropdown);
//            jsondata.slot = Array.IndexOf(slotOptions, slot);
//            GUILayout.FlexibleSpace();
//            GUILayout.EndHorizontal();

//            GUILayout.BeginHorizontal();
//            GUILayout.Label("Item Type");
//            GUILayout.Space(96);
//            string category = itemCategories[jsondata.category];
//            AddFieldDropdown("Select Category", ref category, itemCategories, ref itemDropdown);
//            jsondata.category = Array.IndexOf(itemCategories, category);
//            GUILayout.FlexibleSpace();
//            GUILayout.EndHorizontal();

//            //if (jsondata.category != 2) { GUI.enabled = false; }
//            //GUILayout.BeginHorizontal();
//            //GUILayout.Label("Weapon Type");
//            //GUILayout.Space(77);
//            //string weapon = weaponClass[jsondata.modules[0].weaponClass];
//            //AddFieldDropdown("Select Weapon Type", ref weapon, weaponClass, ref wpnDropdown);
//            //jsondata.modules[0].weaponClass = Array.IndexOf(weaponClass, weapon);
//            //GUILayout.FlexibleSpace();
//            //GUILayout.EndHorizontal();
//            //GUI.enabled = true;

//            //GUILayout.BeginHorizontal();
//            //GUILayout.Label("Weapon Handling");
//            //GUILayout.Space(56);
//            //string hndlng = weaponHandling[jsondata.modules[0].weaponHandling];
//            //AddFieldDropdown("Select Handling Mode", ref hndlng, weaponHandling, ref hndlngDropdown);
//            //jsondata.modules[0].weaponHandling = Array.IndexOf(weaponHandling, hndlng);
//            //GUILayout.FlexibleSpace();
//            //GUILayout.EndHorizontal();

//            //GUILayout.Space(10);
//            //GUILayout.BeginHorizontal();
//            //GUILayout.Label("Dodge Behaviour");
//            //GUILayout.Space(56);
//            //string dodge = dodgeBehaviour[jsondata.modules[0].dodgeBehaviour];
//            //AddFieldDropdown("Dodge Behaviour", ref dodge, dodgeBehaviour, ref ddgDropdown);
//            //jsondata.modules[0].dodgeBehaviour = Array.IndexOf(dodgeBehaviour, dodge);
//            //GUILayout.FlexibleSpace();
//            //GUILayout.EndHorizontal();

//            AddField("Mass", ref jsondata.mass);
//            AddField("Drag", ref jsondata.drag);
//            AddField("Angular Drag", ref jsondata.angularDrag);
//            AddField("Use Gravity", ref jsondata.useGravity);
//            AddField("Fly From Throw", ref jsondata.flyFromThrow);
//            AddField("Enable Climbing Grip", ref jsondata.grippable);

//            GUILayout.EndVertical();
//            GUILayout.EndHorizontal();
//            GUILayout.Space(5);
//        }

//        Dictionary<string, bool> damDropdown = new Dictionary<string, bool>();
//        Dictionary<string, bool> hndlDropdown = new Dictionary<string, bool>();
//        Dictionary<string, bool> hndlHandDropdown = new Dictionary<string, bool>();
//        Dictionary<string, bool> wshSoundDropdown = new Dictionary<string, bool>();
//        Dictionary<string, bool> wshTriggDropdown = new Dictionary<string, bool>();
//        private void DisplayWeaponDamagers()
//        {
//            GUILayout.Label("Damagers", new GUIStyle("BoldLabel"));
//            GUILayout.BeginHorizontal();
//            GUILayout.Space(20);
//            GUILayout.BeginVertical();
//            if (itemPrefab)
//            {
//                try
//                {
//                    if (jsondata.damagers.Count > 0)
//                    {
//                        foreach (JSONData.Damager damager in jsondata.damagers)
//                        {

//                            GUILayout.BeginHorizontal();

//                            if (!damDropdown.ContainsKey(damager.transformName))
//                            {
//                                damDropdown[damager.transformName] = false;
//                            }
//                            bool drpdwn = damDropdown[damager.transformName];
//                            GUILayout.Label(damager.transformName);
//                            AddFieldDropdown("Select Damager Type", ref damager.damagerID, damagerTypes, ref drpdwn);
//                            damDropdown[damager.transformName] = drpdwn;

//                            GUILayout.FlexibleSpace();
//                            GUILayout.EndHorizontal();
//                        }
//                    }
//                    else
//                    {
//                        GUILayout.Label("There are no Damagers on this prefab.");
//                    }
//                }
//                catch (Exception e)
//                {
//                    Debug.Log(e);
//                }
//            }
//            else
//            {
//                EditorGUILayout.HelpBox("Please assign a Prefab   ", MessageType.Warning);
//            }
//            GUILayout.EndVertical();
//            GUILayout.EndHorizontal();
//        }

//        private void DisplayWeaponHandles()
//        {
//            GUILayout.Label("Handles", new GUIStyle("BoldLabel"));
//            GUILayout.BeginHorizontal();
//            GUILayout.Space(20);
//            GUILayout.BeginVertical();
//            if (itemPrefab)
//            {
//                try
//                {
//                    if (jsondata.Interactables.Count > 0)
//                    {
//                        foreach (JSONData.Interactable handle in jsondata.Interactables)
//                        {
//                            GUILayout.BeginHorizontal();
//                            if (!hndlDropdown.ContainsKey(handle.transformName))
//                            {
//                                hndlDropdown[handle.transformName] = false;
//                            }
//                            bool drpdwn = hndlDropdown[handle.transformName];
//                            GUILayout.Label(handle.transformName);
//                            AddFieldDropdown("Select Interactable Type", ref handle.interactableId, handleTypes, ref drpdwn);
//                            hndlDropdown[handle.transformName] = drpdwn;
//                            if (handle.overrideHandPose)
//                            {
//                                handle.overrideHandPose = GUILayout.Toggle(handle.overrideHandPose, "");
//                                if (!hndlHandDropdown.ContainsKey(handle.transformName))
//                                {
//                                    hndlHandDropdown[handle.transformName] = false;
//                                }
//                                bool drpdwn2 = hndlHandDropdown[handle.transformName];
//                                AddFieldDropdown("Select Hand Pose", ref handle.handPoseId, handlePoses, ref drpdwn2);
//                                hndlHandDropdown[handle.transformName] = drpdwn2;
//                            }
//                            else
//                            {
//                                handle.overrideHandPose = GUILayout.Toggle(handle.overrideHandPose, "Custom Hand Pose");
//                                hndlHandDropdown[handle.transformName] = false;
//                            }
//                            GUILayout.FlexibleSpace();
//                            GUILayout.EndHorizontal();
//                        }
//                    }
//                    else
//                    {
//                        GUILayout.Label("There are no Handles on this prefab.");
//                    }
//                }
//                catch (Exception) { }
//            }
//            else
//            {
//                EditorGUILayout.HelpBox("Please assign a Prefab   ", MessageType.Warning);
//            }
//            GUILayout.EndVertical();
//            GUILayout.EndHorizontal();
//        }

//        private void DisplayWeaponWhooshpoints()
//        {
//            GUILayout.Label("Whoosh Points", new GUIStyle("BoldLabel"));
//            GUILayout.BeginHorizontal();
//            GUILayout.Space(20);
//            GUILayout.BeginVertical();
//            if (itemPrefab)
//            {
//                if (jsondata.whooshs.Count > 0)
//                {

//                    foreach (JSONData.Whoosh whoosh in jsondata.whooshs)
//                    {
//                        GUILayout.BeginHorizontal();
//                        if (whoosh.transformName != null)
//                        {
//                            if (!wshSoundDropdown.ContainsKey(whoosh.transformName))
//                            {
//                                wshSoundDropdown[whoosh.transformName] = false;
//                            }
//                            bool whsh = wshSoundDropdown[whoosh.transformName];
//                            GUILayout.Label(whoosh.transformName);
//                            AddFieldDropdown("Sound Effect", ref whoosh.fxId, whooshSounds, ref whsh);
//                            wshSoundDropdown[whoosh.transformName] = whsh;
//                        }
//                        GUILayout.FlexibleSpace();
//                        GUILayout.EndHorizontal();
//                    }

//                }
//                else
//                {
//                    GUILayout.Label("There are no Whoosh points on this prefab.");
//                }

//            }
//            else
//            {
//                EditorGUILayout.HelpBox("Please assign a Prefab   ", MessageType.Warning);
//            }
//            GUILayout.EndVertical();
//            GUILayout.EndHorizontal();
//        }

//        //AddField Overloading
//        private void AddField(string label, ref GameObject var)
//        {
//            GUILayout.BeginHorizontal();
//            GUILayout.Label(label);
//            var = (GameObject)EditorGUILayout.ObjectField(itemPrefab, typeof(GameObject), false);
//            GUILayout.EndHorizontal();
//        }
//        private void AddField(string label, ref string var, bool alt = false)
//        {
//            GUILayout.BeginHorizontal();
//            GUILayout.Label(label);
//            if (alt)
//            {
//                GUILayout.BeginVertical();
//                GUILayout.ExpandWidth(false);
//                GUILayout.ExpandHeight(true);
//                EditorStyles.textField.wordWrap = true;
//                var = EditorGUILayout.TextArea(var);
//                GUILayout.EndVertical();
//            }
//            else
//            {
//                var = EditorGUILayout.TextField(var);
//            }

//            GUILayout.EndHorizontal();
//        }
//        private void AddField(string label, ref bool var)
//        {
//            GUILayout.BeginHorizontal();
//            GUILayout.Label(label);
//            var = EditorGUILayout.Toggle(var);
//            GUILayout.EndHorizontal();
//        }
//        private void AddField(string label, ref double var, int max = -1)
//        {
//            GUILayout.BeginHorizontal();
//            GUILayout.Label(label);
//            var = EditorGUILayout.DoubleField(var);
//            if (max > 0 && var > max)
//            {
//                var = max;
//            }
//            else if (max > 0 && var < 0)
//            {
//                var = 0;
//            }
//            GUILayout.EndHorizontal();
//        }
//        private void AddField(string label, ref int var, int max = -1)
//        {
//            GUILayout.BeginHorizontal();
//            GUILayout.Label(label);
//            var = EditorGUILayout.IntField(var);
//            if (max > 0 && var > max)
//            {
//                var = max;
//            }
//            else if (max > 0 && var < 0)
//            {
//                var = 0;
//            }
//            GUILayout.EndHorizontal();
//        }
//        private void AddFieldDropdown(string label, ref string var, string[] options, ref bool toggle)
//        {
//            GUILayout.BeginVertical();
//            if (var == "" || var == null)
//            {
//                if (GUILayout.Button(label, new GUIStyle("DropDownButton")))
//                {
//                    if (toggle) { toggle = false; } else { toggle = true; }
//                }
//            }
//            else
//            {
//                if (GUILayout.Button(var, new GUIStyle("DropDownButton")))
//                {
//                    if (toggle) { toggle = false; } else { toggle = true; }
//                }
//            }
//            if (toggle)
//            {
//                foreach (string option in options)
//                {
//                    if (GUILayout.Button(option, new GUIStyle("Radio")))
//                    {
//                        var = option;
//                        toggle = false;
//                    }
//                }
//                GUILayout.Space(10);

//            }
//            GUILayout.EndVertical();
//        }

//        bool noAssetBundleOnPrefab, noAuthorName, noWeaponName, noDamagerType, noHandleType, noHandlePose, noWhooshFX;
//        private void CreateJSON()
//        {
//            noAssetBundleOnPrefab = false;
//            noAuthorName = false;
//            noDamagerType = false;
//            noHandleType = false;
//            noHandlePose = false;
//            noWhooshFX = false;
            
//            json = File.ReadAllText(selectedBaseDir);
            
//            if (jsondata.displayName == null || jsondata.displayName == "")
//            {
//                noWeaponName = true;
//            }
//            foreach (JSONData.Damager damager in jsondata.damagers)
//            {
//                if (damager.damagerID == null || damager.damagerID == "")
//                {
//                    noDamagerType = true;
//                }
//            }
//            foreach (JSONData.Interactable handle in jsondata.Interactables)
//            {
//                if (handle.interactableId == null || handle.interactableId == "")
//                {
//                    noHandleType = true;
//                }
//                if ((handle.handPoseId == null || handle.handPoseId == "") && handle.overrideHandPose)
//                {
//                    noHandlePose = true;
//                }
//            }
//            foreach (JSONData.Whoosh whoosh in jsondata.whooshs)
//            {
//                if (whoosh.fxId == null || whoosh.fxId == "")
//                {

//                    noWhooshFX = true;
//                }
//            }

//            if (AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(itemPrefab)).assetBundleName != "")
//            {
//                jsondata.prefabName = "@" + AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(itemPrefab)).assetBundleName + ":" + AssetDatabase.GetAssetPath(itemPrefab).Substring(AssetDatabase.GetAssetPath(itemPrefab).LastIndexOf('/') + 1, AssetDatabase.GetAssetPath(itemPrefab).Length - AssetDatabase.GetAssetPath(itemPrefab).LastIndexOf('/') - 1);
//            }

//            if (initializedWithFile)
//            {
//                jsondata.modules.Clear();
//                json = JsonUtility.ToJson(jsondata, true).Replace("MONEYSIGN", "$").Replace("\"modules\": [],", jsonModules);
//            }
//            else
//            {
//                json = JsonUtility.ToJson(jsondata, true).Replace("MONEYSIGN", "$");
//            }

//            //EditorToolsJSONConfig.AddPrefab(jsonName + ".json", AssetDatabase.GetAssetPath(itemPrefab));

//            if (!initializedWithFile)
//            {
//                EditorPrefs.SetString("modAuthor", jsondata.author);
//                if (!noAssetBundleOnPrefab && !noAuthorName && !noWeaponName && !noDamagerType && !noHandlePose && !noHandleType && !noWhooshFX)
//                {
//                    Debug.Log("JSON File \"" + jsonName + ".json\" created in directory " + modDirectory);
//                    File.WriteAllText(modDirectory + "\\" + jsonName + ".json", json);
//                    Close();
//                }
//            }
//            else
//            {
//                if (!noAssetBundleOnPrefab && !noAuthorName && !noWeaponName && !noDamagerType && !noHandlePose && !noHandleType && !noWhooshFX)
//                {
//                    Debug.Log("JSON File \"" + jsonName + ".json\" created in directory " + modDirectory);
//                    File.WriteAllText(selectedBaseDir, json);
//                    Close();
//                }
//            }
//        }
//    }

//    public class MapJsonBuilder : EditorWindow
//    {

//        public class JSONData
//        {
//            public string MONEYSIGNtype = "BS.LevelData, Assembly-CSharp";
//        }

//    }

//    public class ManifestBuilder : EditorWindow
//    {
//        public Manifest jsondata = new Manifest();
//        string json;
//        bool hasManifest = false;
//        string directory;

//        [Serializable]
//        public class Manifest
//        {
//            public string Name = "Mod Name";
//            public string Description = "Description of Mod";
//            public string Author = "Your Name";
//            public string ModVersion = "1.0";
//            public string GameVersion = "7.0";
//        }

//        public ManifestBuilder()
//        {
//            directory = EditorToolsJSONConfig.selectedModDirectory;
//            foreach (FileInfo file in new DirectoryInfo(directory).GetFiles())
//            {
//                if (file.Name == "manifest.json")
//                {
//                    hasManifest = true;
//                }
//            }
//            if (hasManifest)
//            {
//                LoadJSON(directory + "\\manifest.json");
//            }
//        }

//        private void LoadJSON(string dir)
//        {
//            json = File.ReadAllText(dir);
//            JsonUtility.FromJsonOverwrite(json, jsondata);
//        }

//        private void OnGUI()
//        {
//            GUILayout.Label("Editing mod info of " + directory.Substring(directory.LastIndexOf('\\') + 1, directory.Length - directory.LastIndexOf('\\') - 1));

//            GUILayout.Space(10);

//            AddField("Mod Name", ref jsondata.Name);
//            GUILayout.Space(5);
//            AddField("Description", ref jsondata.Description, true);
//            GUILayout.Space(5);
//            AddField("Mod Author", ref jsondata.Author);
//            GUILayout.Space(5);
//            AddField("Mod Version", ref jsondata.ModVersion);
//            GUILayout.Space(5);
//            AddField("Game Version", ref jsondata.GameVersion);

//            GUILayout.FlexibleSpace();
//            GUILayout.BeginHorizontal();
//            GUILayout.FlexibleSpace();
//            if (GUILayout.Button("Save Info"))
//            {
//                CreateJSON();
//            }
//            GUILayout.EndHorizontal();
//        }

//        private void AddField(string label, ref string var, bool alt = false)
//        {
//            GUILayout.BeginHorizontal();
//            GUILayout.Label(label);
//            if (alt)
//            {
//                GUILayout.BeginVertical();
//                GUILayout.ExpandWidth(false);
//                GUILayout.ExpandHeight(true);
//                EditorStyles.textField.wordWrap = true;
//                var = EditorGUILayout.TextArea(var);
//                GUILayout.EndVertical();
//            }
//            else
//            {
//                var = EditorGUILayout.TextField(var);
//            }

//            GUILayout.EndHorizontal();
//        }
//        public void CreateJSON()
//        {
//            json = JsonUtility.ToJson(jsondata, true);
//            EditorPrefs.SetString("modAuthor", jsondata.Author);
//            CreateFolder(jsondata.Name);
//            string NewModFolderPath = GetNewModDirecotry();
//            File.WriteAllText(NewModFolderPath + "\\manifest.json", json);
//            Close();
//        }
//        public void CreateFolder(string modName)
//        {
//            string directory;
//            directory = EditorToolsJSONConfig.streamingAssetsDirectory;
//            Debug.Log("Attempting to get directory...");
//            Directory.CreateDirectory(directory + "/" + modName);
//            string modDir = directory + "/" + modName;
//            Debug.Log("Successfully Created Mod Folder Called " + modName);
//            EditorToolsJSONConfig.selectedModDirectory = modDir;
//            EditorPrefs.SetString("selectedMod", modDir);
//        }
//        public string GetNewModDirecotry()
//        {
//            ManifestBuilder builder = GetWindow<ManifestBuilder>();
//            return EditorToolsJSONConfig.streamingAssetsDirectory + "/" + builder.jsondata.Name;
//        }
//    }
//}