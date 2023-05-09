using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    public class PursueTargetState : State
    {
        public CombatStanceState combatStanceState;
        //public PatrolState patrolState;

        private void Awake()
        {
            combatStanceState = GetComponent<CombatStanceState>();
            //patrolState = GetComponent<PatrolState>();
        }

        public override State Tick(AICharacterManager aiCharacter)
        {
            HandleRotateTowardsTarget(aiCharacter);

            if (aiCharacter.isInteracting)
                return this;

            if (aiCharacter.isPreformingAction)
            {
                aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            if (aiCharacter.distanceFromTarget > aiCharacter.maximumAggroRadius)
            {
                aiCharacter.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            if (aiCharacter.distanceFromTarget <= aiCharacter.maximumAggroRadius)
            {
                return combatStanceState;
            }
            else
            {
                return this;
            }
        }

        private void HandleRotateTowardsTarget(AICharacterManager aiCharacter)
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
                aiCharacter.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, aiCharacter.rotationSpeed / Time.deltaTime);
            }
            //Rotate with pathfinding (navmesh)
            else
            {
                Vector3 relativeDirection = transform.InverseTransformDirection(aiCharacter.navmeshAgent.desiredVelocity);
                Vector3 targetVelocity = aiCharacter.enemyRigidBody.velocity;

                aiCharacter.navmeshAgent.enabled = true;
                aiCharacter.navmeshAgent.SetDestination(aiCharacter.currentTarget.transform.position);
                aiCharacter.enemyRigidBody.velocity = targetVelocity;
                aiCharacter.transform.rotation = Quaternion.Lerp(aiCharacter.transform.rotation, aiCharacter.navmeshAgent.transform.rotation, aiCharacter.rotationSpeed / Time.deltaTime);
            }
        }
    }
}