using System.Collections.Generic;
using ThunderRoad.Modules;

namespace ThunderRoad
{
    public interface ILoreDisplay
    {
        public void SetLore(LoreModule loreModule, LoreSpawner spawner, LoreScriptableObject.LoreData loreData);
        public void SetMultipleLore(LoreModule loreModule, LoreSpawner spawner, List<LoreScriptableObject.LoreData> loreDatas);
        public void ReleaseLore();

#if UNITY_EDITOR
        public void EditorSetLore(LorePackScriptableObject.LoreItemData loreData);
        public void EditorSetMultipleLore(List<LorePackScriptableObject.LoreItemData> loreDatas);
#endif //UNITY_EDITOR
    }
}