using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FN
{
    public class ShopItemUI : MonoBehaviour
    {
        public Item item;
        //public Button button;
        public List<Item> items;

        public void SetItem(Item item, System.Action<ShopItemUI, itemType> onClickHandler)
        {
            this.item = item;
            //button.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName + " (" + item.price + " gold)";
            //button.onClick.AddListener(() => onClickHandler(this, item.itemType));
        }
    }
}
