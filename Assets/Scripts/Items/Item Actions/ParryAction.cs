using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    [CreateAssetMenu(menuName = "Item Actions/Parry Action")]
    public class ParryAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.isInteracting)
                return;

            WeaponItem parryingWeapon = player.playerInventoryManager.currentItemBeingUsed as WeaponItem;

            //CHECK IF PARRYING WEAPON IS A FAST PARRY WEAPON OR A MEDIUM PARRY WEAPON
            if (parryingWeapon.weaponType == WeaponType.SmallShield)
            {
                //FAST PARRY ANIM
                player.playerAnimatorManager.PlayTargetAnimation("Parry", true);
                AudioManager.instance.Play("Shield");
            }
            else if (parryingWeapon.weaponType != WeaponType.Shield)
            {
                //NORMAL PARRY ANIM
                player.playerAnimatorManager.PlayTargetAnimation("Parry", true);
            }
        }
    }
}
