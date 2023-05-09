using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    public interface IShopCustomer 
    {
        void BoughtItem(List<WeaponItem> weaponItems, List<FlaskItem> healthItems, List<ManaItem> manaItems);
    }
}