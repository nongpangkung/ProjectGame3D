using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    public class AICharacterBossManager : MonoBehaviour
    {
        public string bossName;
        public GameObject teleportObject;

        AICharacterManager enemy;
        UIBossHealthBar bossHealthBar;
        AICharacterStatsManager enemyStats;
        AICharacterAnimatorManager enemyAnimatorManager;
        BossCombatStanceState bossCombatStanceState;

        [Header("Second Phase FX")]
        public GameObject particleFX;

        private void Awake()
        {
            enemy = GetComponent<AICharacterManager>();
            bossHealthBar = FindObjectOfType<UIBossHealthBar>();
            enemyStats = GetComponent<AICharacterStatsManager>();
            enemyAnimatorManager = GetComponentInChildren<AICharacterAnimatorManager>();
            bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
        }

        private void Start()
        {
            bossHealthBar.SetBossName(bossName);
            bossHealthBar.SetBossMaxHealth(enemyStats.maxHealth);
        }

        public void UpdateBossHealthBar(int currentHealth, int maxHealth)
        {
            bossHealthBar.SetBossCurrentHealth(currentHealth);

            if (currentHealth <= maxHealth / 2 && !bossCombatStanceState.hasPhaseShifted)
            {
                Debug.Log("hasPhaseShifted");
                bossCombatStanceState.hasPhaseShifted = true;
                ShiftToSecondPhase();
            }

            if (currentHealth <= 0)
            {
                Die();
                AudioManager.instance.Play("BossDead");
            }
        }

        public void ShiftToSecondPhase()
        {
            if (enemy.isPhaseShifting)
                return;

            enemy.animator.SetBool("isInvulnerable", true);
            enemy.animator.SetBool("isPhaseShifting", true);
            enemyAnimatorManager.PlayTargetAnimation("Phase Shift", true);
            bossCombatStanceState.hasPhaseShifted = true;
        }

        public void Die()
        {
            teleportObject.SetActive(true);
        }
    }
}
