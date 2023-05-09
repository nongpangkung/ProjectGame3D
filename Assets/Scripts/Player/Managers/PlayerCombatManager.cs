using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager player;

        [Header("Attack Animations")]
        public string oh_light_attack_01 = "OH_Light_Attack_01";
        public string oh_light_attack_02 = "OH_Light_Attack_02";
        public string oh_heavy_attack_01 = "OH_Heavy_Attack_01";
        public string oh_heavy_attack_02 = "OH_Heavy_Attack_02";
        public string oh_running_attack_01 = "OH_Running_Attack_01";
        public string oh_jumping_attack_01 = "OH_Jumping_Attack_01";

        public string oh_charge_attack_01 = "OH_Charging_Attack_Charge_01";
        public string oh_charge_attack_02 = "OH_Charging_Attack_Charge_02";

        public string th_light_attack_01 = "TH_Light_Attack_01";
        public string th_light_attack_02 = "TH_Light_Attack_02";
        public string th_heavy_attack_01 = "TH_Heavy_Attack_01";
        public string th_heavy_attack_02 = "TH_Heavy_Attack_02";
        public string th_running_attack_01 = "TH_Running_Attack_01";
        public string th_jumping_attack_01 = "TH_Jumping_Attack_01";

        public string th_charge_attack_01 = "TH_Charging_Attack_Charge_01";
        public string th_charge_attack_02 = "TH_Charging_Attack_Charge_02";

        public string weapon_art = "Weapon_Art";

        public string lastAttack;

        LayerMask backStabLayer = 1 << 12;
        LayerMask riposteLayer = 1 << 13;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        public override void DrainStaminaBasedOnAttack()
        {
            if (player.isUsingRightHand)
            {
                if (currentAttackType == AttackType.light)
                {
                    player.playerStatsManager.DeductStamina(player.playerInventoryManager.rightWeapon.baseStaminaCost * player.playerInventoryManager.rightWeapon.lightAttackStaminaMultiplier);
                }
                else if (currentAttackType == AttackType.heavy)
                {
                    player.playerStatsManager.DeductStamina(player.playerInventoryManager.rightWeapon.baseStaminaCost * player.playerInventoryManager.rightWeapon.heavyAttackStaminaMultiplier);
                }
            }
            else if (player.isUsingLeftHand)
            {
                if (currentAttackType == AttackType.light)
                {
                    player.playerStatsManager.DeductStamina(player.playerInventoryManager.leftWeapon.baseStaminaCost * player.playerInventoryManager.rightWeapon.lightAttackStaminaMultiplier);
                }
                else if (currentAttackType == AttackType.heavy)
                {
                    player.playerStatsManager.DeductStamina(player.playerInventoryManager.leftWeapon.baseStaminaCost * player.playerInventoryManager.rightWeapon.heavyAttackStaminaMultiplier);
                }
            }
        }

        public override void AttemptBlock(DamageCollider attackingWeapon, float physicalDamage, float fireDamage, string blockAnimation)
        {
            base.AttemptBlock(attackingWeapon, physicalDamage, fireDamage, blockAnimation);
            player.playerStatsManager.staminaBar.SetCurrentStamina(Mathf.RoundToInt(player.playerStatsManager.currentStamina));
        }
    }
}