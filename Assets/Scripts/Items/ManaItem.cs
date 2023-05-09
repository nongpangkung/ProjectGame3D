using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    [CreateAssetMenu(menuName = "Items/Consumables/Mana")]
    public class ManaItem : ConsumableItem
    {
        [Header("Flask Type")]
        public bool estusFlask;
        public bool ashenFlask;

        [Header("Recovery Amount")]
        public int healthRecoverAmount;
        public int focusPointsRecoverAmount;

        [Header("Recovery FX")]
        public GameObject recoveryFX;

        public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager, PlayerManager player)
        {
            if (currentItemAmount > 0)
            {
                base.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager, player);
                GameObject flask = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);
                playerEffectsManager.currentParticleFX = recoveryFX;
                playerEffectsManager.amountToBeHealed = focusPointsRecoverAmount;
                playerEffectsManager.instantiatedFXModel = flask;
                playerAnimatorManager.PlayTargetAnimation(consumeAnimation, isInteracting, true);
                weaponSlotManager.rightHandSlot.UnloadWeapon();
                currentItemAmount -= 1;
            }
            else if (currentItemAmount <= 0)
            {
                playerAnimatorManager.PlayTargetAnimation("Shrug", true);
            }
        }
    }
}
