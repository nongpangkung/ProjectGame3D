using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    public class Item : ScriptableObject
    {
        [Header("Item Information")]
        public Sprite itemIcon;
        public string itemName;
        public int itemID;

        public itemType itemType;

        [Header("Price")]
        public int price;
    }
}