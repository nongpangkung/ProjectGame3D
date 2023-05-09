using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    public class AmbushState : State
    {
        public bool isSleeping;
        public float detectionRadius = 2;
        public string sleepAnimation;
        public string wakeAnimation;

        public LayerMask detectionLayer;

        public PursueTargetState pursueTargetState;

        private void Awake()
        {
            pursueTargetState = GetComponent<PursueTargetState>();
        }

        public override State Tick(AICharacterManager aiCharacter)
        {
            if (isSleeping && aiCharacter.isInteracting == false)
            {
                aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation(sleepAnimation, true);
            }

            Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager characterStats = colliders[i].transform.GetComponent<CharacterManager>();

                if (characterStats != null)
                {
                    Vector3 targetsDirection = characterStats.transform.position - aiCharacter.transform.position;
                    float viewableAngle = Vector3.Angle(targetsDirection, aiCharacter.transform.forward);

                    if (viewableAngle > aiCharacter.minimumDetectionAngle
                        && viewableAngle < aiCharacter.maximumDetectionAngle)
                    {
                        aiCharacter.currentTarget = characterStats;
                        isSleeping = false;
                        aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation(wakeAnimation, true);
                    }
                }
            }

            if (aiCharacter.currentTarget != null)
            {
                return pursueTargetState;
            }
            else
            {
                return this;
            }
        }
    }
}