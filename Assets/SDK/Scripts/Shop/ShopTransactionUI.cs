using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{

    public class ShopTransactionUI : MonoBehaviour
    {
        public Shop shop;
        public ReceptType type;

        public Transform rootLines;
        public ShopTransactionUILine shopReceptLinePrefab;

        public TextMeshProUGUI playerMoneyText;
        public TextMeshProUGUI totalText;
        public Color notEnoughMoneyTotalColor = Color.red;

        public UICustomisableButton buyOrSellButton;
        public UICustomisableButton sellAllButton;

        public UIText titleUiText;
        public UIText emptyMessage;

        public string buyString = "{Buy}";
        public string sellString = "{Sell}";

        public string boughtItemsString = "{BoughtItems}";
        public string SoldItemsString = "{SoldItems}";

        public string emptyBuyString = "{EmptyBuy}";
        public string emptySellString = "{EmptySell}";

        protected List<ShopTransactionUILine> lines;

        public Transform[] autoDropPoints;

        public UnityEvent onBuySuccess;
        public UnityEvent onSellSuccess;
        public UnityEvent onBuyFail;
        public UnityEvent onSellFail;

        protected Color totalOrignalColor;

        public enum ReceptType
        {
            Buy,
            Sell,
        }

 // ProjectCore
    }
}