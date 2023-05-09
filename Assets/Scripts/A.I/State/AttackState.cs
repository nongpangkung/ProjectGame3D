using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    public class AttackState : State
    {
        public RotateTowardsTargetState rotateTowardsTargetState;
        public CombatStanceState combatStanceState;
        public PursueTargetState pursueTargetState;
        public AICharacterAttackAction currentAttack;

        bool willDoComboOnNextAttack = false;
        public bool hasPerformedAttack = false;

        private void Awake()
        {
            rotateTowardsTargetState = GetComponent<RotateTowardsTargetState>();
            combatStanceState = GetComponent<CombatStanceState>();
            pursueTargetState = GetComponent<PursueTargetState>();
        }

        public override State Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isDead)
                return this;
            float distanceFromTarget = Vector3.Distance(aiCharacter.currentTarget.transform.position, aiCharacter.transform.position);

            RotateTowardsTargetWhilstAttacking(aiCharacter);

            if (distanceFromTarget > aiCharacter.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            if (willDoComboOnNextAttack && aiCharacter.canDoCombo)
            {
                AttackTargetWithCombo(aiCharacter);
            }

            if (!hasPerformedAttack)
            {
                AttackTarget(aiCharacter);
                RollForComboChance(aiCharacter);
            }

            if (willDoComboOnNextAttack && hasPerformedAttack)
            {
                return this; //GOES BACK UP TO PREFORM THE COMBO
            }

            return rotateTowardsTargetState;
        }

        private void AttackTarget(AICharacterManager aiCharacter)
        {
            aiCharacter.isUsingRightHand = currentAttack.isRightHandedAction;
            aiCharacter.isUsingLeftHand = !currentAttack.isRightHandedAction;
            aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            aiCharacter.aiCharacterAnimatorManager.PlayWeaponTrailFX();
            aiCharacter.currentRecoveryTime = currentAttack.recoveryTime;
            hasPerformedAttack = true;
        }

        private void AttackTargetWithCombo(AICharacterManager aiCharacter)
        {
            aiCharacter.isUsingRightHand = currentAttack.isRightHandedAction;
            aiCharacter.isUsingLeftHand = !currentAttack.isRightHandedAction;
            willDoComboOnNextAttack = false;
            aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            aiCharacter.aiCharacterAnimatorManager.PlayWeaponTrailFX();
            aiCharacter.currentRecoveryTime = currentAttack.recoveryTime;
            currentAttack = null;
        }

        private void RotateTowardsTargetWhilstAttacking(AICharacterManager aiCharacter)
        {
            //Rotate manually
            if (aiCharacter.canRotate && aiCharacter.isInteracting)
            {
                Vector3 direction = aiCharacter.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                aiCharacter.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, aiCharacter.rotationSpeed / Time.deltaTime);
            }
        }

        private void RollForComboChance(AICharacterManager aiCharacter)
        {
            float comboChance = Random.Range(0, 100);

            if (aiCharacter.allowAIToPerformCombos && comboChance <= aiCharacter.comboLikelyHood)
            {
                if (currentAttack.comboAction != null)
                {
                    willDoComboOnNextAttack = true;
                    currentAttack = currentAttack.comboAction;
                }
                else
                {
                    willDoComboOnNextAttack = false;
                    currentAttack = null;
                }
            }
        }
    }
}