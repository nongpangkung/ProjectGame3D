using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    public class BonfireInteractable : Interactable
    {
        //LOCATION OF BONFIRE (For teleporting)

        [Header("Bonfire Teleport Transform")]
        public Transform bonfireTeleportTransform;

        [Header("Activation Status")]
        public bool hasBeenActivated;

        //BONFIRE UNIQUE ID (For saving which bonfires you have activated)

        [Header("Bonfire FX")]
        public ParticleSystem activationFX;
        public ParticleSystem fireFX;
        public AudioClip bonfireActivationSoundFX;

        AudioSource audioSource;

        protected override void Awake()
        {
            base.Awake();
            //If the bonfire has already been activated by the player, play the "Fire" FX when the bonfire is loaded into the scene
            if (hasBeenActivated)
            {
                fireFX.gameObject.SetActive(true);
                fireFX.Play();
                interactableText = "Rest";
            }
            else
            {
                interactableText = "Light Bonfire";
            }

            audioSource = GetComponent<AudioSource>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            Debug.Log("BONFIRE INTERACTED WITH");

            if (hasBeenActivated)
            {
                //OPEN THE TELEPORT MENU
            }
            else
            {
                //ACTIVATE BONFIRE
                playerManager.playerAnimatorManager.PlayTargetAnimation("Bonfire_Idle", true);
                playerManager.uiManager.ActivateBonfirePopUp();
                hasBeenActivated = true;
                interactableText = "Rest";
                activationFX.gameObject.SetActive(true);
                activationFX.Play();
                fireFX.gameObject.SetActive(true);
                fireFX.Play();
                audioSource.PlayOneShot(bonfireActivationSoundFX);
            }
        }
    }
}
