using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    [CreateAssetMenu(menuName = "Items/Consumables/Cure Effect Clump")]
    public class ClumpConsumeableItem : ConsumableItem
    {
        [Header("Recovery FX")]
        public GameObject clumpConsumeFX;

        [Header("Cure FX")]
        public bool curePoison;
        //Cure Bleed
        //Cure Curse

        public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager, PlayerManager player)
        {
            base.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager, player);
            GameObject clump = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);
            playerEffectsManager.currentParticleFX = clumpConsumeFX;
            playerEffectsManager.instantiatedFXModel = clump;

            if (curePoison)
            {
                playerEffectsManager.poisonBuildup = 0;
                playerEffectsManager.poisonAmount = playerEffectsManager.defaultPoisonAmount;
                playerEffectsManager.isPoisoned = false;

                if (playerEffectsManager.currentPoisonParticleFX != null)
                {
                    Destroy(playerEffectsManager.currentPoisonParticleFX);
                }
            }

            weaponSlotManager.rightHandSlot.UnloadWeapon();
        }
    }
}
