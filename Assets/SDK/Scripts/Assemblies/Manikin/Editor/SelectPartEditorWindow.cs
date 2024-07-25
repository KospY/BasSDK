using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

namespace ThunderRoad.Manikin
{
    public class SelectPartEditorWindow : EditorWindow
    {
        private static int CompareAddressableEntries(AddressableAssetEntry a, AddressableAssetEntry b)
        {
            return a.address.CompareTo(b.address);
        }

        public enum ViewStyle
        {
            MeshPreview,
            Name
        }

        public delegate void SelectionPickedHandler(string guid);
        public delegate EventArgs CustomFilterGUIHandler(EventArgs e);
        public delegate bool CustomFilterHandler(UnityEngine.Object obj, EventArgs e);

        public static EventArgs customFilterArgs;

        public static event SelectionPickedHandler SelectionEvent;
        public static event CustomFilterHandler CustomFilterEvent;
        public static event CustomFilterGUIHandler CustomFilterGUIEvent;

        public static Type requiredComponent;
        public static Type assetType;

        static AddressableAssetSettings aaSettings;
        static List<AddressableAssetEntry> assetEntries;
        static string[] allLabels;
        static string[] allGroups;

        static HashSet<string> selectedLabels;
        static int selectedGroup = 0;
        static string addressFilter;

        static bool closeOnSelect;
        static Rect windowPosition = new Rect(Screen.currentResolution.width * 0.4f, Screen.currentResolution.height * 0.4f, 500, 500);

        Vector2 scrollPos;
        float previewScale = 1.0f;
        ViewStyle viewStyle = ViewStyle.MeshPreview;        

        static SelectPartEditorWindow window;

        static GUIStyle fixedStyle = new GUIStyle("Button");

        List<GUIContent> buttons = new List<GUIContent>();
        List<string> guids = new List<string>();

        bool dirtyFilter = true;
        bool focused = false;

        static ManikinSettingsProvider manikinSettings;

        public static void Init(Type assetType, Type requiredComponent, SelectionPickedHandler selectionhandler, CustomFilterGUIHandler customFilterGUIHandler = null, CustomFilterHandler customFilterHandler = null)
        {
            // Get existing open window or if none, make a new one:
            window = (SelectPartEditorWindow)GetWindow(typeof(SelectPartEditorWindow), true, "Selection", true);

            SelectionEvent = selectionhandler;
            CustomFilterEvent = customFilterHandler;
            CustomFilterGUIEvent = customFilterGUIHandler;
            customFilterArgs = null;
            SelectPartEditorWindow.requiredComponent = requiredComponent;
            SelectPartEditorWindow.assetType = assetType;

            manikinSettings = ManikinSettingsProvider.GetSettings();

            aaSettings = AddressableAssetSettingsDefaultObject.Settings;
            if (aaSettings != null)
            {
                if (assetEntries == null)
                {
                    assetEntries = new List<AddressableAssetEntry>();
                }
                else
                {
                    assetEntries.Clear();
                }

                List<string> labels = aaSettings.GetLabels();
                labels.Insert(0, "None");
                allLabels = labels.ToArray();

                allGroups = new string[aaSettings.groups.Count + 1];
                allGroups[0] = "None";
                for(int i = 0; i < aaSettings.groups.Count; i++)
                {
                    allGroups[i+1] = aaSettings.groups[i].Name;
                }

                if(selectedLabels == null) { selectedLabels = new HashSet<string>(); }

                aaSettings.GetAllAssets(assetEntries, false, GroupFilter, EntryFilter);

                window.position = windowPosition;
                //window.ShowModal();
                window.Show(true);
            }
            else
            {
                Debug.LogWarning("Use 'Window->Addressables' to initialize addressables.");
            }
        }

        private static bool EntryFilter(AddressableAssetEntry entry)
        {
            if (string.IsNullOrEmpty(entry.AssetPath) || !System.IO.File.Exists(entry.AssetPath))
            {
                return false;
            }
            
            //Filter by selected Labels
            foreach (string label in selectedLabels)
            {
                if (!entry.labels.Contains(label)) { return false; }
            }

            //Filter by name search
            if (!string.IsNullOrWhiteSpace(addressFilter))
            {
                if (entry.address.IndexOf(addressFilter, StringComparison.OrdinalIgnoreCase) == -1)
                {
                    return false;
                }
            }

            //Filters out assets not of the selected AssetType
            if (AssetDatabase.GetMainAssetTypeAtPath(entry.AssetPath) != assetType)
            {
                return false;
            }

            UnityEngine.Object loadedAsset = null;
            //Only get assets with the required component
            if (requiredComponent != null)
            {
                loadedAsset = AssetDatabase.LoadAssetAtPath(entry.AssetPath, requiredComponent);
                if (loadedAsset == null)
                {
                    return false;
                }
            }

            if(CustomFilterEvent != null && CustomFilterGUIEvent != null)
            {
                return CustomFilterEvent.Invoke(loadedAsset, customFilterArgs);
            }

            return true;
        }

        private static bool GroupFilter(AddressableAssetGroup group)
        {
            if (selectedGroup == 0)
                return true;

            if (selectedGroup > 0 && (selectedGroup - 1) < aaSettings.groups.Count)
            {
                return (group.Name.Equals(aaSettings.groups[selectedGroup - 1].Name));
            }

            return false;
        }

        private void Awake()
        {
            previewScale = EditorPrefs.GetFloat("Manikin_PreviewScale", 1.0f);
            viewStyle = (ViewStyle)EditorPrefs.GetInt("Manikin_ViewStyle", 0);
            selectedGroup = EditorPrefs.GetInt("Manikin_SelectedGroup", -1);
            closeOnSelect = EditorPrefs.GetBool("Manikin_CloseOnSelect", true);
            windowPosition.x = EditorPrefs.GetFloat("Manikin_PositionX", (Screen.currentResolution.width * 0.4f));
            windowPosition.y = EditorPrefs.GetFloat("Manikin_PositionY", (Screen.currentResolution.height * 0.4f));
            windowPosition.width = EditorPrefs.GetFloat("Manikin_PositionWidth", 500);
            windowPosition.height = EditorPrefs.GetFloat("Manikin_PositionHeight", 500);
        }

        void OnGUI()
        {
            float buttonSize = 100f;
            List<string> removeSelectedLabels = new List<string>();

            if(!position.Equals(windowPosition))
            {
                EditorPrefs.SetFloat("Manikin_PositionX", position.x);
                EditorPrefs.SetFloat("Manikin_PositionY", position.y);
                EditorPrefs.SetFloat("Manikin_PositionWidth", position.width);
                EditorPrefs.SetFloat("Manikin_PositionHeight", position.height);
                windowPosition = position;
            }

            EditorGUI.BeginChangeCheck();
            viewStyle = (ViewStyle)EditorGUILayout.EnumPopup("View Style", viewStyle);
            if(EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetInt("Manikin_ViewStyle", (int)viewStyle);
            }

            EditorGUI.BeginChangeCheck();
            closeOnSelect = EditorGUILayout.Toggle("Close On Select", closeOnSelect);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool("Manikin_CloseOnSelect", closeOnSelect);
            }

            EditorGUI.BeginChangeCheck();
            previewScale = EditorGUILayout.Slider("Preview Scale", previewScale, 0.2f, 2.0f);
            if(EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetFloat("Manikin_PreviewScale", previewScale);
            }

            EditorGUI.BeginChangeCheck();
            selectedGroup = EditorGUILayout.Popup("Group Filter", selectedGroup, allGroups);
            if(EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetInt("Manikin_SelectedGroup", selectedGroup);
                dirtyFilter = true;
            }

            EditorGUI.BeginChangeCheck();
            int selectedLabel = EditorGUILayout.Popup("Add Label Filter", 0, allLabels);
            if(selectedLabel != 0)
            {
                selectedLabels.Add(allLabels[selectedLabel]);
            }

            if(CustomFilterEvent != null && CustomFilterGUIEvent != null)
            {
                customFilterArgs = CustomFilterGUIEvent.Invoke(customFilterArgs);
            }

            GUI.SetNextControlName("NameFilter");
            addressFilter = EditorGUILayout.TextField("Name Filter", addressFilter);

            //Draw selected labels
            EditorGUILayout.BeginHorizontal();
            foreach(string label in selectedLabels)
            {
                if(GUILayout.Button("(X) " + label, GUILayout.MaxWidth(100)))
                {
                    removeSelectedLabels.Add(label);
                }
            }
            EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                dirtyFilter = true;
            }

            if (dirtyFilter)
            {
                //remove labels that have been clicked.
                foreach (string label in removeSelectedLabels)
                {
                    selectedLabels.Remove(label);
                }

                assetEntries.Clear();
                aaSettings.GetAllAssets(assetEntries, false, GroupFilter, EntryFilter);
            }

            if(dirtyFilter)
            {
                buttons.Clear();
                guids.Clear();

                assetEntries.Sort(CompareAddressableEntries);

                EditorUtility.DisplayProgressBar("Loading Part Previews", "", 0f);
                for(int i = 0; i < assetEntries.Count; i++)
                {
                    var entry = assetEntries[i];

                    if (string.IsNullOrEmpty(entry.AssetPath) || !System.IO.File.Exists(entry.AssetPath))
                    {
                        continue;
                    }

                    EditorUtility.DisplayProgressBar("Loading Part Previews", assetEntries[i].address, i / assetEntries.Count);

                    var obj = AssetDatabase.LoadAssetAtPath(entry.AssetPath, requiredComponent);

                    if (obj != null)
                    {
                        Texture2D preview;
                        if (manikinSettings != null)
                        {
                            preview = ((IManikinPartPreview)obj).GetOrCreatePreview(entry.guid.ToString(), manikinSettings.previewsFolder, manikinSettings.previewDimensions, manikinSettings.previewDimensions);
                        }
                        else
                        {
                            preview = ((IManikinPartPreview)obj).GetOrCreatePreview(entry.guid.ToString(), "Assets/ManikinPreviews/", 128, 128);
                        }

                        if(preview != null)
                        {
                            buttons.Add(new GUIContent(preview, entry.address));
                        }
                        else
                        {
                            buttons.Add(new GUIContent("No Preview", entry.address));
                        }

                        guids.Add(entry.guid);
                    }
                }
                EditorUtility.ClearProgressBar();
            }

            if (viewStyle == ViewStyle.MeshPreview)
            {
                fixedStyle.fixedHeight = buttonSize * previewScale;
                fixedStyle.fixedWidth = buttonSize * previewScale;

                int size = Mathf.FloorToInt((window.position.width / (buttonSize * previewScale)));
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                int selected = GUILayout.SelectionGrid(-1, buttons.ToArray(), size, fixedStyle);
                EditorGUILayout.EndScrollView();

                if (selected >= 0)
                {
                    if (SelectionEvent != null)
                    {
                        SelectionEvent.Invoke(guids[selected]);
                    }

                    if (closeOnSelect)
                    {
                        Close();
                    }
                }
            }

            if(viewStyle == ViewStyle.Name)
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                for (int i = 0; i < buttons.Count; i++)
                {
                    if(GUILayout.Button(buttons[i].tooltip))
                    {
                        if (SelectionEvent != null)
                        {
                            SelectionEvent.Invoke(guids[i]);
                        }

                        if (closeOnSelect)
                        {
                            Close();
                        }
                    }
                }
                EditorGUILayout.EndScrollView();
            }

            if(!focused)
            {
                EditorGUI.FocusTextInControl("NameFilter");
                focused = true;
            }

            dirtyFilter = false;
        }

        void OnInspectorUpdate()
        {
            if (AssetPreview.IsLoadingAssetPreviews() && viewStyle == ViewStyle.MeshPreview)
            {
                Repaint();
            }
        }
    }
}
