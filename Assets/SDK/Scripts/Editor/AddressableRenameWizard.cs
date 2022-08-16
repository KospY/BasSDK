using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

public class AddressableRenameWizard : ScriptableWizard
{
    public string append;
    public string search;
    public string replace;

    static string info = "Append : Add text at begining, Search / Replace : Search and text and replace it";

    [MenuItem("Assets/ThunderRoad/Addressable Rename Wizard")]
    static void CreateWizard()
    {
        AddressableRenameWizard wizard = ScriptableWizard.DisplayWizard<AddressableRenameWizard>("Rename Adressables", "Apply");
        wizard.append = EditorPrefs.GetString("ARW_append");
        wizard.search = EditorPrefs.GetString("ARW_search");
        wizard.replace = EditorPrefs.GetString("ARW_replace");
    }

    void OnWizardUpdate()
    {
        string[] assetGUIDs = Selection.assetGUIDs;
        if(assetGUIDs == null || assetGUIDs.Length <= 0)
        {
            helpString = "Select at least one asset.";
        }
    }

    protected override bool DrawWizardGUI()
    {
        EditorGUI.BeginChangeCheck();
        append = EditorGUILayout.TextField(new GUIContent("Append"), append);
        search = EditorGUILayout.TextField(new GUIContent("Search"), search);
        replace = EditorGUILayout.TextField(new GUIContent("Replace"), replace);
        EditorGUILayout.HelpBox(info, MessageType.Info);
        return EditorGUI.EndChangeCheck();
    }

    void OnWizardCreate()
    {
        //save/update any editor prefs we have changed
        EditorPrefs.SetString("ARW_append", append);
        EditorPrefs.SetString("ARW_search", search);
        EditorPrefs.SetString("ARW_replace", replace);

        string[] assetGUIDs = Selection.assetGUIDs;
        if(assetGUIDs != null)
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            if(settings == null)
            {
                Debug.LogError("Could not find AddressableAssetSettings!");
                return;
            }

            var entries = new List<AddressableAssetEntry>();
            foreach (string assetGUID in assetGUIDs)
            {
                AddressableAssetEntry entry = settings.FindAssetEntry(assetGUID);
                if (entry != null)
                {
                    string newAddress = append + entry.address;
                    if (!string.IsNullOrEmpty(search))
                    {
                        newAddress = newAddress.Replace(search, replace);
                    }
                    entry.SetAddress(newAddress, false);
                    entries.Add(entry);
                }
                else
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(assetGUID);
                    if (string.IsNullOrEmpty(assetPath))
                    {
                        Debug.LogWarning(assetGUID + " is not marked as Addressable.");
                    }
                    else
                    {
                        Debug.LogWarning(assetPath + " is not marked as Addressable.");
                    }
                }
            }

            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryModified, entries, true, false);
        }
    }
}
