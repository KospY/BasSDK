#if UNITY_EDITOR

using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;

namespace ThunderRoad
{
    [InitializeOnLoad]
    public class ThunderRoadDefine : IActiveBuildTargetChanged
    {

        static void CheckDefines()
        {
            if (HasType("ThunderRoad.GameManager"))
            {
                // Game project
                AddDefine("ProjectCore");
                RemoveDefine("PrivateSDK");
                Debug.Log("Project defines set to game project");
            }
            else if (HasType("ThunderRoad.CatalogEditor"))
            {
                // Private SDK project
                RemoveDefine("ProjectCore");
                AddDefine("PrivateSDK");
                Debug.Log("Project defines set to private SDK");
            }
            else
            {
                // Public SDK project
                RemoveDefine("ProjectCore");
                RemoveDefine("PrivateSDK");
                Debug.Log("Project defines set to public SDK");
            }
            if (NamespaceExists("DunGen"))
            {
                AddDefine("DUNGEN");
            }
            else
            {
                RemoveDefine("DUNGEN");
            }
        }

        public static void AddDefine(string value)
        {
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
            if (!defines.Contains(value))
            {
                if (defines.Length > 0) defines += ";";
                defines += value;
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, defines);
            }
            defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
            if (!defines.Contains(value))
            {
                if (defines.Length > 0) defines += ";";
                defines += value;
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, defines);
            }
        }

        public static void RemoveDefine(string value)
        {
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
            if (defines.Contains(value))
            {
                defines = defines.Replace(";" + value, "");
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, defines);
            }
            defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
            if (defines.Contains(value))
            {
                defines = defines.Replace(";" + value, "");
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, defines);
            }
        }

        public static bool HasType(string typeName)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.FullName == typeName)
                        return true;
                }
            }
            return false;
        }
        public static bool NamespaceExists(string desiredNamespace)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Namespace == desiredNamespace)
                        return true;
                }
            }
            return false;
        }
        
        static ThunderRoadDefine()
        {
            CheckDefines();
        }

        public int callbackOrder { get { return 0; } }
        public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
        {
            CheckDefines();
        }
    }
}
#endif