using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    public class RotateTowardsTargetState : State
    {
        public CombatStanceState combatStanceState;

        private void Awake()
        {
            combatStanceState = GetComponent<CombatStanceState>();
        }

        public override State Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isDead)
                return this;
            aiCharacter.animator.SetFloat("Vertical", 0);
            aiCharacter.animator.SetFloat("Horizontal", 0);

            if (aiCharacter.isInteracting)
                return this;

            if (aiCharacter.viewableAngle >= 100 && aiCharacter.viewableAngle <= 180 && !aiCharacter.isInteracting)
            {
                aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Behind", true);
                return combatStanceState; 
            }
            else if (aiCharacter.viewableAngle <= -101 && aiCharacter.viewableAngle >= -180 && !aiCharacter.isInteracting)
            {
                aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Behind", true);
                return combatStanceState;
            }
            else if (aiCharacter.viewableAngle <= -45 && aiCharacter.viewableAngle >= -100 && !aiCharacter.isInteracting)
            {
                aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Right", true);
                return combatStanceState;
            }
            else if (aiCharacter.viewableAngle >= 45 && aiCharacter.viewableAngle <= 100 && !aiCharacter.isInteracting)
            {
                aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Left", true);
                return combatStanceState;
            }

            return combatStanceState;
        }
    }
}
