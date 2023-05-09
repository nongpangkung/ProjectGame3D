using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FN
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager player;

        public static event Action OnPlayerDeath;

        public HealthBar healthBar;
        public StaminaBar staminaBar;

        public float staminaRegenerationAmount = 1;
        public float staminaRegenerationAmountWhilstBlocking = 0.1f;
        public float staminaRegenTimer = 0;

        private float sprintingTimer = 0;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
            staminaBar = FindObjectOfType<StaminaBar>();
        }

        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetCurrentHealth(currentHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
            staminaBar.SetCurrentStamina(currentStamina);

            totalPoiseDefence = SetMaxPoisePointsFromPoiseLevel();
        }

        protected override void Update()
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetCurrentHealth(currentHealth);
            staminaBar.SetMaxStamina(maxStamina);
            staminaBar.SetCurrentStamina(currentStamina);
        }


        public override void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else if (poiseResetTimer <= 0 && !player.isInteracting)
            {
                totalPoiseDefence = armorPoiseBonus;
            }
        }

        public override void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation, CharacterManager enemyCharacterDamagingMe)
        {
            if (player.isDead)
                return;

            if (player.isInvulnerable)
                return;

            base.TakeDamage(physicalDamage, fireDamage, damageAnimation, enemyCharacterDamagingMe);
            healthBar.SetCurrentHealth(currentHealth);
            player.playerAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                player.isDead = true;
                player.playerAnimatorManager.PlayTargetAnimation("Dead_02", true);
                OnPlayerDeath?.Invoke();

                string sceneName = SceneManager.GetActiveScene().name;
                PlayerPrefs.SetString("LastSceneName", sceneName);
                Debug.Log("Saved last scene name: " + sceneName);

                AudioManager.instance.Play("PlayerDead");
                AudioManager.instance.Play("GameOver");
            }
        }

        public override void TakePoisonDamage(int damage)
        {
            if (player.isDead)
                return;

            base.TakePoisonDamage(damage);
            healthBar.SetCurrentHealth(currentHealth);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                player.isDead = true;
                player.playerAnimatorManager.PlayTargetAnimation("Dead_02", true);
                AudioManager.instance.Play("PlayerDead");
                AudioManager.instance.Play("GameOver");
            }
        }

        public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
        {
            if (player.isDead)
                return;

            base.TakeDamageNoAnimation(physicalDamage, fireDamage);
            healthBar.SetCurrentHealth(currentHealth);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                player.isDead = true;
                player.playerAnimatorManager.PlayTargetAnimation("Dead_02", true);
                OnPlayerDeath?.Invoke();

                string sceneName = SceneManager.GetActiveScene().name;
                PlayerPrefs.SetString("LastSceneName", sceneName);
                Debug.Log("Saved last scene name: " + sceneName);

                AudioManager.instance.Play("PlayerDead");
                AudioManager.instance.Play("GameOver");
            }
        }

        public override void DeductStamina(float staminaToDeduct)
        {
            base.DeductStamina(staminaToDeduct);
            staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
        }

        public void DeductSprintingStamina(float staminaToDeduct)
        {
            if (player.isSprinting)
            {
                sprintingTimer = sprintingTimer + Time.deltaTime;

                if (sprintingTimer > 0.1f)
                {
                    //  RESET TIMER
                    sprintingTimer = 0;
                    //  DEDUCT STAMINA
                    currentStamina = currentStamina - staminaToDeduct;
                    staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                }
            }
            else
            {
                sprintingTimer = 0;
            }
        }

        public void RegenerateStamina()
        {
            if (player.isInteracting || player.isSprinting) // DO NOT REGENERATE STAMINA
            {
                staminaRegenTimer = 0;
            }
            else
            {
                staminaRegenTimer += Time.deltaTime;

                if (currentStamina < maxStamina && staminaRegenTimer > 1f)
                {
                    if (player.isBlocking)
                    {
                        currentStamina += staminaRegenerationAmountWhilstBlocking * Time.deltaTime;
                        staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                    }
                    else
                    {
                        currentStamina += staminaRegenerationAmount * Time.deltaTime;
                        staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                    }
                }
            }
        }

        public void HealPlayer(int healAmount)
        {
            currentHealth = currentHealth + healAmount;

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            healthBar.SetCurrentHealth(currentHealth);
        }

        public void AddSouls(int souls)
        {
            currentSoulCount = currentSoulCount + souls;
        }

        public void AddGold(int gold)
        {
            currentGoldCount = currentGoldCount + gold;
        }

        public override int SetMaxHealthFromHealthLevel()
        {
            int HealthIncreasePerLevel = 300;

            if (healthLevel == 1)
            {
                maxHealth = 500;
            }
            else
            {
                maxHealth = 500 + (healthLevel - 1) * HealthIncreasePerLevel;
            }

            return maxHealth;
        }

        public override float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 100;
            return maxStamina;
        }

        public override float SetMaxPoisePointsFromPoiseLevel()
        {
            totalPoiseDefence = (poiseLevel * 100) + player.playerStatsManager.armorPoiseBonus;
            return totalPoiseDefence;
        }
    }
}