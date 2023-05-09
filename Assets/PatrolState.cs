using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    public class PatrolState : State
    {
        public PursueTargetState pursueTargetState;

        public LayerMask detectionLayer;
        public LayerMask layersThatBlockLineOfSight;

        public bool patrolComplete;
        public bool repeatPatrol;

        //  How long before next patrol
        [Header("Patrol Rest Time")]
        public float endOfPatrolRestTime;
        public float endOfPatrolTimer;

        [Header("Patrol Position")]
        public int patrolDestinationIndex;
        public bool hasPatrolDestination;
        public Transform currentPatrolDestination;
        public float distanceFromCurrentPatrolPoint;
        public List<Transform> listOfPatrolDestinations = new List<Transform>();

        public override State Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isDead)
                return this;

            SearchForTargetWhilstPatroling(aiCharacter);

            if (aiCharacter.isInteracting)
            {
                //  If the A.I is performing some action, hault all movement and return
                aiCharacter.animator.SetFloat("Vertical", 0);
                aiCharacter.animator.SetFloat("Horizontal", 0);
                return this;
            }

            if (aiCharacter.currentTarget != null)
            {
                return pursueTargetState;
            }

            //  If we've completed our patrol and we do want to repeat it, we do so
            if (patrolComplete && repeatPatrol)
            {
                // We count down our rest time, and reset all of our patrol flags
                if (endOfPatrolRestTime > endOfPatrolTimer)
                {
                    aiCharacter.animator.SetFloat("Vertical", 0f, 0.2f, Time.deltaTime);
                    endOfPatrolTimer = endOfPatrolTimer + Time.deltaTime;
                    return this;
                }
                else if (endOfPatrolTimer >= endOfPatrolRestTime)
                {
                    patrolDestinationIndex = -1;
                    hasPatrolDestination = false;
                    currentPatrolDestination = null;
                    patrolComplete = false;
                    endOfPatrolTimer = 0;
                }
            }
            else if (patrolComplete && !repeatPatrol)
            {
                aiCharacter.navmeshAgent.enabled = false;
                aiCharacter.animator.SetFloat("Vertical", 0f, 0.2f, Time.deltaTime);
                return this;
            }

            if (hasPatrolDestination)
            {
                if (currentPatrolDestination != null)
                {
                    distanceFromCurrentPatrolPoint = Vector3.Distance(aiCharacter.transform.position, currentPatrolDestination.transform.position);

                    if (distanceFromCurrentPatrolPoint > 1)
                    {
                        aiCharacter.navmeshAgent.enabled = true;
                        aiCharacter.navmeshAgent.destination = currentPatrolDestination.transform.position;
                        Quaternion targetRotation = Quaternion.Lerp(aiCharacter.transform.rotation, aiCharacter.navmeshAgent.transform.rotation, 5f);
                        aiCharacter.transform.rotation = targetRotation;
                        aiCharacter.animator.SetFloat("Vertical", 0.5f, 0.2f, Time.deltaTime);
                    }
                    else
                    {
                        currentPatrolDestination = null;
                        hasPatrolDestination = false;
                    }
                }
            }

            if (!hasPatrolDestination)
            {
                patrolDestinationIndex = patrolDestinationIndex + 1;

                if (patrolDestinationIndex > listOfPatrolDestinations.Count - 1)
                {
                    patrolComplete = true;
                    return this;
                }

                currentPatrolDestination = listOfPatrolDestinations[patrolDestinationIndex];
                hasPatrolDestination = true;
            }

            return this;
        }

        private void SearchForTargetWhilstPatroling(AICharacterManager aiCharacter)
        {
            //Searches for a potential target within the dectection radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, aiCharacter.detectionRadius, detectionLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

                //if a potential target is found, that is not on the same team as the A.I we proceed to the next step
                if (targetCharacter != null)
                {
                    if (targetCharacter.characterStatsManager.teamIDNumber != aiCharacter.aiCharacterStatsManager.teamIDNumber)
                    {
                        Vector3 targetDirection = targetCharacter.transform.position - transform.position;
                        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                        //if a potential target is found, it has to be standing infront of the A.I's field of view
                        if (viewableAngle > aiCharacter.minimumDetectionAngle && viewableAngle < aiCharacter.maximumDetectionAngle)
                        {
                            //if the A.I's potential target has an obstruction in between itself and the A.I, we do not add it as our current target
                            if (Physics.Linecast(aiCharacter.lockOnTransform.position, targetCharacter.lockOnTransform.position, layersThatBlockLineOfSight))
                            {
                                return;
                            }
                            else
                            {
                                AudioManager.instance.Play("Enemy");
                                aiCharacter.currentTarget = targetCharacter;
                            }
                        }
                    }
                }
            }

            if (aiCharacter.currentTarget != null)
            {
                return;
            }
            else
            {
                return;
            }
        }
    }
}
