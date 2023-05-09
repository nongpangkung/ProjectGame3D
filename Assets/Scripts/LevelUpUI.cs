using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FN
{
    public class LevelUpUI : MonoBehaviour
    {
        public PlayerManager playerManager;
        public Button confirmLevelUpButton;

        [Header("Player Level")]
        public int currentPlayerLevel;      //THE CURRENT LEVEL WE ARE BEFORE LEVELING UP
        public int projectedPlayerLevel;    //THE POSSIBLE LEVEL WE WILL BE IF WE ACCEPT LEVELING UP
        public Text currentPlayerLevelText;     //THE UI TEXT FOR THE NUMBER OF THE CURRENT PLAYER LEVEL
        public Text projectedPlayerLevelText;   //THE UI TEXT FOR THE PROJECTED PLAYER LEVEL NUMBER

        [Header("Souls")]
        public Text currentSoulsText;
        public Text soulsRequiredToLevelUpText;
        private int soulsRequiredToLevelUp;
        public int baseLevelUpCost = 5;

        [Header("Health")]
        public Slider healthSlider;
        public Text currentHealthLevelText;
        public Text projectedHealthLevelText;

        [Header("Stamina")]
        public Slider staminaSlider;
        public Text currentStaminaLevelText;
        public Text projectedStaminaLevelText;

        [Header("Poise")]
        public Slider poiseSlider;
        public Text currentPoiseLevelText;
        public Text projectedPoiseLevelText;

        //Update all of the stats on the UI to the player's current Stats
        private void OnEnable()
        {
            currentPlayerLevel = playerManager.playerStatsManager.playerLevel;
            currentPlayerLevelText.text = currentPlayerLevel.ToString();

            projectedPlayerLevel = playerManager.playerStatsManager.playerLevel;
            projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

            healthSlider.value = playerManager.playerStatsManager.healthLevel;
            healthSlider.minValue = playerManager.playerStatsManager.healthLevel;
            healthSlider.maxValue = 99;
            currentHealthLevelText.text = playerManager.playerStatsManager.healthLevel.ToString();
            projectedHealthLevelText.text = playerManager.playerStatsManager.healthLevel.ToString();

            staminaSlider.value = playerManager.playerStatsManager.staminaLevel;
            staminaSlider.minValue = playerManager.playerStatsManager.staminaLevel;
            staminaSlider.maxValue = 99;
            currentStaminaLevelText.text = playerManager.playerStatsManager.staminaLevel.ToString();
            projectedStaminaLevelText.text = playerManager.playerStatsManager.staminaLevel.ToString();

            poiseSlider.value = playerManager.playerStatsManager.poiseLevel;
            poiseSlider.minValue = playerManager.playerStatsManager.poiseLevel;
            poiseSlider.maxValue = 99;
            currentPoiseLevelText.text = playerManager.playerStatsManager.poiseLevel.ToString();
            projectedPoiseLevelText.text = playerManager.playerStatsManager.poiseLevel.ToString();

            currentSoulsText.text = playerManager.playerStatsManager.currentSoulCount.ToString();

            UpdateProjectedPlayerLevel();
        }

        //Updates the player's stats to the projected stats, providing they have enough souls to confirm
        public void ConfirmPlayerLevelUpStats()
        {
            playerManager.playerStatsManager.playerLevel = projectedPlayerLevel;
            playerManager.playerStatsManager.healthLevel = Mathf.RoundToInt(healthSlider.value);
            playerManager.playerStatsManager.staminaLevel = Mathf.RoundToInt(staminaSlider.value);
            playerManager.playerStatsManager.poiseLevel = Mathf.RoundToInt(poiseSlider.value);

            playerManager.playerStatsManager.maxHealth = playerManager.playerStatsManager.SetMaxHealthFromHealthLevel();
            playerManager.playerStatsManager.maxStamina = playerManager.playerStatsManager.SetMaxStaminaFromStaminaLevel();
            playerManager.playerStatsManager.totalPoiseDefence = playerManager.playerStatsManager.SetMaxPoisePointsFromPoiseLevel();

            playerManager.playerStatsManager.currentSoulCount = playerManager.playerStatsManager.currentSoulCount - soulsRequiredToLevelUp;
            playerManager.uiManager.soulCount.text = playerManager.playerStatsManager.currentSoulCount.ToString();

            gameObject.SetActive(false);
        }

        private void CalculateSoulCostToLevelUp()
        {
            for (int i = currentPlayerLevel; i < projectedPlayerLevel; i++)
            {
                soulsRequiredToLevelUp += baseLevelUpCost;
            }
        }

        //Updates the projected player's total level, by adding up all the projected level up stats
        private void UpdateProjectedPlayerLevel()
        {
            soulsRequiredToLevelUp = 0;

            projectedPlayerLevel = currentPlayerLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(healthSlider.value) - playerManager.playerStatsManager.healthLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(staminaSlider.value) - playerManager.playerStatsManager.staminaLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(poiseSlider.value) - playerManager.playerStatsManager.poiseLevel;
            projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

            CalculateSoulCostToLevelUp();
            soulsRequiredToLevelUpText.text = soulsRequiredToLevelUp.ToString();

            if (playerManager.playerStatsManager.currentSoulCount < soulsRequiredToLevelUp)
            {
                confirmLevelUpButton.interactable = false;
            }
            else
            {
                confirmLevelUpButton.interactable = true;
            }
        }

        public void UpdateHealthLevelSlider()
        {
            projectedHealthLevelText.text = healthSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateStaminaLevelSlider()
        {
            projectedStaminaLevelText.text = staminaSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdatePoiseLevelSlider()
        {
            projectedPoiseLevelText.text = poiseSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

    }
}
