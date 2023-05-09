using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        protected CharacterManager character;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false, bool mirrorAnim = false)
        {
            character.animator.applyRootMotion = isInteracting;
            character.animator.SetBool("canRotate", canRotate);
            character.animator.SetBool("isInteracting", isInteracting);
            character.animator.SetBool("isMirrored", mirrorAnim);
            character.animator.CrossFade(targetAnim, 0.2f);
        }

        public void PlayTargetAnimationWithRootRotation(string targetAnim, bool isInteracting)
        {
            character.animator.applyRootMotion = isInteracting;
            character.animator.SetBool("isRotatingWithRootMotion", true);
            character.animator.SetBool("isInteracting", isInteracting);
            character.animator.CrossFade(targetAnim, 0.2f);
        }

        public virtual void CanRotate()
        {
            character.animator.SetBool("canRotate", true);
        }

        public virtual void StopRotation()
        {
            character.animator.SetBool("canRotate", false);
        }

        public virtual void EnableCombo()
        {
            character.animator.SetBool("canDoCombo", true);
        }

        public virtual void DisableCombo()
        {
            character.animator.SetBool("canDoCombo", false);
        }

        public virtual void EnableIsInvulnerable()
        {
            character.animator.SetBool("isInvulnerable", true);
        }

        public virtual void DisableIsInvulnerable()
        {
            character.animator.SetBool("isInvulnerable", false);
        }

        public virtual void EnableIsParrying()
        {
            character.isParrying = true;
        }

        public virtual void DisableIsParrying()
        {
            character.isParrying = false;
        }

        public virtual void EnableCanBeRiposted()
        {
            character.canBeRiposted = true;
        }

        public virtual void DisableCanBeRiposted()
        {
            character.canBeRiposted = false;
        }

        public virtual void OnAnimatorMove()
        {
            if (character.isInteracting == false)
                return;

            Vector3 velocity = character.animator.deltaPosition;
            character.characterController.Move(velocity);
            character.transform.rotation *= character.animator.deltaRotation;
        }
    }
}