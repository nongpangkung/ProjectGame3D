using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FN
{
    public class HeadPickUp : Interactable
    {
        //  This is a unique ID for this item spawn in the game world, each item you place in your world should have it's own UNIQUE ID
        [Header("Item Information")]
        [SerializeField] int itemPickUpID;
        [SerializeField] bool hasBeenLooted;

        [Header("item")]
        public HelmetEquipment helmet;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            //  Place the item in the players Inventory
            PickUpItem(playerManager);
        }

        private void PickUpItem(PlayerManager playerManager)
        {
            PlayerInventoryManager playerInventory;
            PlayerLocomotionManager playerLocomotion;
            PlayerAnimatorManager animatorHandler;

            playerInventory = playerManager.GetComponent<PlayerInventoryManager>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotionManager>();
            animatorHandler = playerManager.GetComponentInChildren<PlayerAnimatorManager>();

            playerLocomotion.rigidbody.velocity = Vector3.zero; //Stops the player from moving whilst picking up item
            animatorHandler.PlayTargetAnimation("Pick Up Item", true); //Plays the animation of looting the item
            playerInventory.headEquipmentInventory.Add(helmet);
            playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = helmet.itemName;
            playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = helmet.itemIcon.texture;
            playerManager.itemInteractableGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
