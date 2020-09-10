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
    public string prefix0;
    public string prefix1;
    public string prefix2;
    public string postfix0;

    public bool prependFolderName = false;

    static string info = "Example format, all optional; {prefix0}.{prefix1}.{prefix2}.{FolderName}.{AssetName}.{postfix}";

    [MenuItem("Assets/Addressable Rename Wizard")]
    static void CreateWizard()
    {
        AddressableRenameWizard wizard = ScriptableWizard.DisplayWizard<AddressableRenameWizard>("Rename Adressables", "Apply");
        wizard.prefix0 = EditorPrefs.GetString("ARW_Prefix0");
        wizard.prefix1 = EditorPrefs.GetString("ARW_Prefix1");
        wizard.prefix2 = EditorPrefs.GetString("ARW_Prefix2");
        wizard.postfix0 = EditorPrefs.GetString("ARW_Postfix0");
        wizard.prependFolderName = EditorPrefs.GetBool("ARW_Prepend", false);
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
        prefix0 = EditorGUILayout.TextField(new GUIContent("Prefix0"), prefix0);
        prefix1 = EditorGUILayout.TextField(new GUIContent("Prefix1"), prefix1);
        prefix2 = EditorGUILayout.TextField(new GUIContent("Prefix2"), prefix2);
        postfix0 = EditorGUILayout.TextField(new GUIContent("Postfix0"), postfix0);
        prependFolderName = EditorGUILayout.Toggle(new GUIContent("Prepend Folder Name"),prependFolderName);
        EditorGUILayout.HelpBox(info, MessageType.Info);
        return EditorGUI.EndChangeCheck();
    }

    void OnWizardCreate()
    {
        //save/update any editor prefs we have changed
        EditorPrefs.SetString("ARW_Prefix0", prefix0);
        EditorPrefs.SetString("ARW_Prefix1", prefix1);
        EditorPrefs.SetString("ARW_Prefix2", prefix2);
        EditorPrefs.SetString("ARW_Postfix0", postfix0);
        EditorPrefs.SetBool("ARW_Prepend", prependFolderName);

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
            StringBuilder newAddress = new StringBuilder(50);

            foreach (string assetGUID in assetGUIDs)
            {
                AddressableAssetEntry entry = settings.FindAssetEntry(assetGUID);
                if (entry != null)
                {
                    string assetName = Path.GetFileNameWithoutExtension(entry.AssetPath);
                    string folderName = Path.GetFileName(Path.GetDirectoryName(entry.AssetPath));
                    newAddress.Clear();

                    if (!string.IsNullOrEmpty(prefix0)) { newAddress.Append(prefix0); newAddress.Append("."); }
                    if (!string.IsNullOrEmpty(prefix1)) { newAddress.Append(prefix1); newAddress.Append("."); }
                    if (!string.IsNullOrEmpty(prefix2)) { newAddress.Append(prefix2); newAddress.Append("."); }
                    if (prependFolderName) { newAddress.Append(folderName); newAddress.Append("."); }
                    newAddress.Append(assetName);
                    if (!string.IsNullOrEmpty(postfix0)) { newAddress.Append("."); newAddress.Append(postfix0); }

                    entry.SetAddress(newAddress.ToString(), false);
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
