#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class AddDefineUtility
{
    /// <summary>
    /// Add define symbols as soon as Unity gets done compiling.
    /// </summary>
    public static void AddDefines(IEnumerable<string> symbols)
    {
        Debug.Log($"AddDefines: {string.Join(", ", symbols)}");

        string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        var uniqueDefines = new HashSet<string>(definesString.Split(';'));
 
        bool recompile = false;
        foreach (var symbol in symbols)
        {
            if (uniqueDefines.Contains(symbol))
            {
                Debug.Log($"Symbol {symbol} already defined");
            }
            else
            {
                uniqueDefines.Add(symbol);
                recompile = true;
            }
        }

        if (recompile)
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", uniqueDefines));
        }
    }
}
#endif

/*
// Example use:

/// <summary>
/// Adds the given define symbols to PlayerSettings define symbols.
/// Just add your own define symbols to the Symbols property at the below.
/// </summary>
#if UNITY_EDITOR
[InitializeOnLoad]
public class SomeClass
{
    static SomeClass()
    {
        AddDefineUtility.AddDefines(new[] {"_SomeDefine1", "_SomeDefine2"});
    }
}
#endif
*/
