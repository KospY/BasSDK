using ThunderRoad;
using UnityEngine;
using UnityEngine.UI;

[HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/ItemShelf")]
public class ItemShelf : MonoBehaviour
{
    public Text priceTag = null;
    public Image tierImage = null;
    public string symbol = "gold";

    private int quantity = 0;
    private Item firstItem = null;

    public extern void OnZoneEnter(Object obj);

    public extern void OnZoneExit(Object obj);
}
