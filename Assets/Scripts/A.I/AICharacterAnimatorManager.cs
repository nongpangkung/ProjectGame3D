using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    public class AICharacterAnimatorManager : CharacterAnimatorManager
    {
        AICharacterManager aiCharacter;

        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponent<AICharacterManager>();
        }

        public void AwardSoulsOnDeath()
        {
            PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();
            SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();

            if (playerStats != null)
            {
                playerStats.AddSouls(aiCharacter.aiCharacterStatsManager.soulsAwardedOnDeath);

                if (soulCountBar != null)
                {
                    soulCountBar.SetSoulCountText(playerStats.currentSoulCount);
                }
            }
        }

        public void AwardGoldsOnDeath()
        {
            PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();
            GoldCountBar goldCountBar = FindObjectOfType<GoldCountBar>();

            if (playerStats != null)
            {
                playerStats.AddGold(aiCharacter.aiCharacterStatsManager.goldAwardedOnDeath);

                if (goldCountBar != null)
                {
                    goldCountBar.SetGoldCountText(playerStats.currentGoldCount);
                }
            }
        }

        public void InstantiateBossParticleFX()
        {
            BossFXTransform bossFxTransform = GetComponentInChildren<BossFXTransform>();
            GameObject phaseFX = Instantiate(aiCharacter.aiCharacterBossManager.particleFX, bossFxTransform.transform);
        }

        public void PlayWeaponTrailFX()
        {
            aiCharacter.aiCharacterEffectsManager.PlayWeaponFX(false);
        }

        public override void OnAnimatorMove()
        {
            Vector3 velocity = character.animator.deltaPosition;
            character.characterController.Move(velocity);

            if (aiCharacter.isRotatingWithRootMotion)
            {
                character.transform.rotation *= character.animator.deltaRotation;
            }
        }
    }
}