using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    public class EquipmentItem : Item
    {
        [Header("Defense Bonus")]
        public float physicalDefense;

        [Header("Resistances")]
        public float poisonResistance;
        //Fire Def
        //Lightning Def
        //Darkness Def
    }
}
