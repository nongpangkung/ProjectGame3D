using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FN
{
    public class InteractiveShop : Interactable
    {
        public override void Interact(PlayerManager playerManager)
        {
            playerManager.uiManager.shopUI.SetActive(true);
        }
    }
}
