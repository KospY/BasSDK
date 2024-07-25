using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace ThunderRoad.Manikin
{
    [InitializeOnLoad]
    public class ManikinPrefabStage
    {
        static ManikinPrefabStage()
        {
            UnityEditor.SceneManagement.PrefabStage.prefabStageOpened += OnPrefabStageOpened;
            UnityEditor.SceneManagement.PrefabStage.prefabStageClosing += OnPrefabStageClosing;
        }

        static void OnPrefabStageOpened(UnityEditor.SceneManagement.PrefabStage prefabStage)
        {
            var root = prefabStage.prefabContentsRoot;

            if(root.TryGetComponent(out ManikinPart part))
            {
                part.PrefabStageOpened();
            }
        }

        static void OnPrefabStageClosing(UnityEditor.SceneManagement.PrefabStage prefabStage) { }
    }
}
