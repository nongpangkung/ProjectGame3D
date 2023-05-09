using CodeMonkey.Utils;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FN
{
    public class StoreUI : MonoBehaviour
    {
        public TextMeshProUGUI itemNameText;
        public Image itemIconImage;

        private Transform container;
        private Transform shopItemTemplate;

        PlayerManager player;

        [Header("Weapon Stats")]
        public TextMeshProUGUI priceText;

        public List<Item> Items;

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
            container = transform.Find("container");
            shopItemTemplate = container.Find("shopItemTemplate");
            shopItemTemplate.gameObject.SetActive(false);
        }

        private void Update()
        {
            player.uiManager.soulCount.text = player.playerStatsManager.currentSoulCount.ToString();
            player.uiManager.goldCount.text = player.playerStatsManager.currentGoldCount.ToString();
        }

        private void Start()
        {
            ShopItemUI(Items);
        }

        private void ShopItemUI(List<Item> Items)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                Item Item = Items[i];

                if (Item != null)
                {
                    Transform shopItemTransform = Instantiate(shopItemTemplate, container);
                    shopItemTransform.gameObject.SetActive(true);
                    RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

                    float shopItemHeight = 90f;
                    shopItemRectTransform.anchoredPosition = new Vector2(0, -shopItemHeight * i);

                    TextMeshProUGUI itemNameTextInstance = shopItemTransform.Find("nameText").GetComponent<TextMeshProUGUI>();
                    Image itemIconImageInstance = shopItemTransform.Find("itemImage").GetComponent<Image>();
                    TextMeshProUGUI priceTextInstance = shopItemTransform.Find("costText").GetComponent<TextMeshProUGUI>();

                    if (Item.itemName != null)
                    {
                        itemNameTextInstance.text = Item.itemName;
                    }
                    else
                    {
                        itemNameTextInstance.text = "";
                    }

                    if (Item.itemIcon != null)
                    {
                        itemIconImageInstance.gameObject.SetActive(true);
                        itemIconImageInstance.enabled = true;
                        itemIconImageInstance.sprite = Item.itemIcon;
                    }
                    else
                    {
                        itemIconImageInstance.gameObject.SetActive(false);
                        itemIconImageInstance.enabled = false;
                        itemIconImageInstance.sprite = null;
                    }

                    priceTextInstance.text = Item.price.ToString();

                    Button_UI button = shopItemTransform.GetComponent<Button_UI>();
                    button.ClickFunc = () => { Buy(Item); };
                }
            }
        }

        public void Buy(Item item)
        {
            switch (item.itemType)
            {
                case itemType.health:
                    if (item is FlaskItem)
                    {
                        BuyFlaskItem(player, (FlaskItem)item);
                    }
                    break;
                case itemType.weapon:
                    if (item is WeaponItem)
                    {
                        BuyWeaponItem(player, (WeaponItem)item);
                    }
                    break;
            }
        }

        private void BuyFlaskItem(PlayerManager player, FlaskItem health)
        {
            if (player.playerStatsManager.currentGoldCount >= health.price)
            {
                int amountToAdd = 1;
                if (health.currentItemAmount + amountToAdd <= health.maxItemAmount)
                {
                    Debug.Log("You have purchased " + health.itemName + " for " + health.price + " coins!");
                    health.currentItemAmount += amountToAdd;
                    player.playerStatsManager.currentGoldCount -= health.price;
                }
                else
                {
                    Debug.Log("HealthItem inventory is full.");
                }
            }
            else
            {
                Debug.Log("Your gold is not enough..");
            }
        }

        private void BuyWeaponItem(PlayerManager player, WeaponItem weapon)
        {
            if (player.playerStatsManager.currentGoldCount >= weapon.price)
            {
                Debug.Log("You have purchased " + weapon.itemName + " for " + weapon.price + " coins!");
                player.playerInventoryManager.weaponsInventory.Add(weapon);
                player.playerStatsManager.currentGoldCount -= weapon.price;
            }
            else
            {
                Debug.Log("Your gold is not enough..");
            }
        }
    }
}
