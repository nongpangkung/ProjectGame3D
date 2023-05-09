using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    public class AICharacterStatsManager : CharacterStatsManager
    {
        AICharacterManager aiCharacter;
        public UIAICharacterHealthBar aiCharacterHealthBar;

        public bool isBoss;

        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponent<AICharacterManager>();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        void Start()
        {
            if (!isBoss)
            {
                aiCharacterHealthBar.SetMaxHealth(maxHealth);
            }
        }

        public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
        {
            if (aiCharacter.isDead)
                return;

            base.TakeDamageNoAnimation(physicalDamage, fireDamage);

            if (!isBoss)
            {
                aiCharacterHealthBar.SetHealth(currentHealth);
            }
            else if (isBoss && aiCharacter.aiCharacterBossManager != null)
            {
                aiCharacter.aiCharacterBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
            }

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                aiCharacter.isDead = true;
                aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation("Dead_01", true);
                AudioManager.instance.Play("EnemyDead");
            }
        }

        public override void TakePoisonDamage(int damage)
        {
            if (aiCharacter.isDead)
                return;

            base.TakePoisonDamage(damage);

            if (!isBoss)
            {
                aiCharacterHealthBar.SetHealth(currentHealth);
            }
            else if (isBoss && aiCharacter.aiCharacterBossManager != null)
            {
                aiCharacter.aiCharacterBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
            }

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                aiCharacter.isDead = true;
                aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation("Dead_01", true);
                AudioManager.instance.Play("EnemyDead");
            }
        }

        public void BreakGuard()
        {
            aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation("Break Guard", true);
        }

        public override void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation, CharacterManager enemyCharacterDamagingMe)
        {
            if (aiCharacter.isDead)
                return;

            base.TakeDamage(physicalDamage, fireDamage, damageAnimation, enemyCharacterDamagingMe);

            if (!isBoss)
            {
                aiCharacterHealthBar.SetHealth(currentHealth);
            }
            else if (isBoss && aiCharacter.aiCharacterBossManager != null)
            {
                aiCharacter.aiCharacterBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
            }

            aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                HandleDeath();
                AudioManager.instance.Play("EnemyDead");
            }
        }

        private void HandleDeath()
        {
            currentHealth = 0;
            aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation("Dead_01", true);
            aiCharacter.isDead = true;
            AudioManager.instance.Play("EnemyDead");
        }
    }
}