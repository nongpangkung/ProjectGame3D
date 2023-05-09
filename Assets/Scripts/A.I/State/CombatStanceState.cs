using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    public class CombatStanceState : State
    {
        public AttackState attackState;
        public AICharacterAttackAction[] aiCharacterAttacks;
        public PursueTargetState pursueTargetState;

        protected bool randomDestinationSet = false;
        protected float verticalMovementValue = 0;
        protected float horizontalMovementValue = 0;

        private void Awake()
        {
            pursueTargetState = GetComponent<PursueTargetState>();
        }

        public override State Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isDead)
                return this;
            attackState.hasPerformedAttack = false;

            if (aiCharacter.isInteracting)
            {
                aiCharacter.animator.SetFloat("Vertical", 0);
                aiCharacter.animator.SetFloat("Horizontal", 0);
                return this;
            }

            if (aiCharacter.distanceFromTarget > aiCharacter.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            if (!randomDestinationSet)
            {
                randomDestinationSet = true;
                DecideCirclingAction(aiCharacter.aiCharacterAnimatorManager);
            }

            HandleRotateTowardsTarget(aiCharacter);

            if (aiCharacter.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
                randomDestinationSet = false;
                return attackState;
            }
            else
            {
                GetNewAttack(aiCharacter);
            }

            CheckIfWeAreTooClose(aiCharacter);
            return this;
        }

        protected void HandleRotateTowardsTarget(AICharacterManager aiCharacter)
        {
            //Rotate manually
            if (aiCharacter.isPreformingAction)
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
            //Rotate with pathfinding (navmesh)
            else
            {
                Vector3 relativeDirection = transform.InverseTransformDirection(aiCharacter.navmeshAgent.desiredVelocity);
                Vector3 targetVelocity = aiCharacter.enemyRigidBody.velocity;

                aiCharacter.navmeshAgent.enabled = true;
                aiCharacter.navmeshAgent.SetDestination(aiCharacter.currentTarget.transform.position);
                aiCharacter.enemyRigidBody.velocity = targetVelocity;
                aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, aiCharacter.navmeshAgent.transform.rotation, aiCharacter.rotationSpeed / Time.deltaTime);
            }
        }

        protected void DecideCirclingAction(AICharacterAnimatorManager aiCharacterAnimatorManager)
        {
            //Circle with only forward vertical movement
            //Circle with running
            WalkAroundTarget(aiCharacterAnimatorManager);
        }

        protected void WalkAroundTarget(AICharacterAnimatorManager aiCharacterAnimatorManager)
        {
            verticalMovementValue = 0.5f;

            horizontalMovementValue = Random.Range(-1, 1);

            if (horizontalMovementValue <= 1 && horizontalMovementValue >= 0)
            {
                horizontalMovementValue = 0.5f;
            }
            else if (horizontalMovementValue >= -1 && horizontalMovementValue < 0)
            {
                horizontalMovementValue = -0.5f;
            }
        }

        protected virtual void GetNewAttack(AICharacterManager aiCharacter)
        {
            int maxScore = 0;

            for (int i = 0; i < aiCharacterAttacks.Length; i++)
            {
                AICharacterAttackAction enemyAttackAction = aiCharacterAttacks[i];

                if (aiCharacter.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && aiCharacter.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (aiCharacter.viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && aiCharacter.viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }


            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < aiCharacterAttacks.Length; i++)
            {
                AICharacterAttackAction enemyAttackAction = aiCharacterAttacks[i];

                if (aiCharacter.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && aiCharacter.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (aiCharacter.viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && aiCharacter.viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (attackState.currentAttack != null)
                            return;

                        temporaryScore += enemyAttackAction.attackScore;

                        if (temporaryScore > randomValue)
                        {
                            attackState.currentAttack = enemyAttackAction;
                        }
                    }
                }
            }

        }

        private void CheckIfWeAreTooClose(AICharacterManager aiCharacter)
        {
            if (aiCharacter.distanceFromTarget <= aiCharacter.stoppingDistance)
            {
                aiCharacter.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                aiCharacter.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            }
            else
            {
                aiCharacter.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
                aiCharacter.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            }
        }
    }
}