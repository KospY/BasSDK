using ThunderRoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWorldMapLocation : MonoBehaviour
{
    public UICustomisableButton button;
    public TextMeshProUGUI label;
    [SerializeField] public RawImage defaultImage;
    [SerializeField] public RawImage highlightedImage;
    
    private LevelData levelData;

    public void Setup(LevelData levelData, ToggleGroup locationsToggleGroup)
    {
        this.levelData = levelData;

        defaultImage.texture = levelData.mapLocationIcon;
        highlightedImage.texture = levelData.mapLocationIconHover;

        button.toggle.group = locationsToggleGroup;

        UpdateLocalizedField();
    }

    public void UpdateLocalizedField()
    {
        if(levelData == null) return;
    }
}