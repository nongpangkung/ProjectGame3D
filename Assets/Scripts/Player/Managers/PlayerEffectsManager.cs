using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        PlayerManager player;

        PoisonBuildUpBar poisonBuildUpBar;
        PoisonAmountBar poisonAmountBar;

        public GameObject currentParticleFX; //The particles that will play of the current effect that is effecting the player (drinking estus, poison ect)
        public GameObject instantiatedFXModel;
        public int amountToBeHealed;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();

            poisonBuildUpBar = FindObjectOfType<PoisonBuildUpBar>();
            poisonAmountBar = FindObjectOfType<PoisonAmountBar>();
        }

        public void HealPlayerFromEffect()
        {
            player.playerStatsManager.HealPlayer(amountToBeHealed);
            GameObject healParticles = Instantiate(currentParticleFX, player.playerStatsManager.transform);
            Destroy(instantiatedFXModel.gameObject);
            player.playerWeaponSlotManager.LoadBothWeaponsOnSlots();
        }

        protected override void HandlePoisonBuildUp()
        {
            if (poisonBuildup <= 0)
            {
                poisonBuildUpBar.gameObject.SetActive(false);
            }
            else
            {
                poisonBuildUpBar.gameObject.SetActive(true);
            }


            base.HandlePoisonBuildUp();
            poisonBuildUpBar.SetPoisonBuildUpAmount(Mathf.RoundToInt(poisonBuildup));
        }

        protected override void HandleIsPoisonedEffect()
        {
            if (isPoisoned == false)
            {
                poisonAmountBar.gameObject.SetActive(false);
            }
            else
            {
                poisonAmountBar.gameObject.SetActive(true);
            }

            base.HandleIsPoisonedEffect();
            poisonAmountBar.SetPoisonAmount(Mathf.RoundToInt(poisonAmount));
        }
    }
}
