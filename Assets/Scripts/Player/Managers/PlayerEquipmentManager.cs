using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        PlayerManager player;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            EquipAllEquipmentModelsOnStart();
        }

        private void EquipAllEquipmentModelsOnStart()
        {
            //HELMET EQUIPMENT
            if (player.playerInventoryManager.currentHelmetEquipment != null)
            {
                player.playerStatsManager.physicalDamageAbsorptionHead = player.playerInventoryManager.currentHandEquipment.physicalDefense;
            }
            else
            {
                player.playerStatsManager.physicalDamageAbsorptionHead = 0;
            }

            //TORSO EQUIPMENT
            if (player.playerInventoryManager.currentBodyEquipment != null)
            {
                player.playerStatsManager.physicalDamageAbsorptionBody = player.playerInventoryManager.currentBodyEquipment.physicalDefense;
            }
            else
            {
                player.playerStatsManager.physicalDamageAbsorptionBody = 0;
            }

            //LEG EQUIPMENT
            if (player.playerInventoryManager.currentLegEquipment != null)
            {
                player.playerStatsManager.physicalDamageAbsorptionLegs = player.playerInventoryManager.currentLegEquipment.physicalDefense;
            }
            else
            {
                player.playerStatsManager.physicalDamageAbsorptionLegs = 0;
            }

            //HAND EQUIPMENT
            if (player.playerInventoryManager.currentHandEquipment != null)
            {
                player.playerStatsManager.physicalDamageAbsorptionHands = player.playerInventoryManager.currentHandEquipment.physicalDefense;
            }
            else
            {
                player.playerStatsManager.physicalDamageAbsorptionHands = 0;
            }

            player.playerStatsManager.armorPoiseBonus = player.playerStatsManager.physicalDamageAbsorptionHead
                + player.playerStatsManager.physicalDamageAbsorptionBody
                + player.playerStatsManager.physicalDamageAbsorptionLegs
                + player.playerStatsManager.physicalDamageAbsorptionHands;
        }
    }
}
