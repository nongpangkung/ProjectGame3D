using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FN
{
    public class InputHandler : MonoBehaviour
    {
        PlayerControls inputActions;
        PlayerManager player;

        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_Input;
        public bool a_Input;
        public bool x_Input;
        public bool y_Input;
        public bool level_Input;
        public bool delete_Intput;

        public bool tap_rb_Input;
        public bool hold_rb_Input;
        public bool tap_rt_Input;
        public bool hold_rt_Input;

        public bool lb_Input;
        public bool tap_lb_Input;
        public bool tap_lt_Input;

        public bool jump_Input;
        public bool inventory_Input;
        public bool lockOnInput;
        public bool right_Stick_Right_Input;
        public bool right_Stick_Left_Input;

        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;

        public bool rollFlag;
        public bool twoHandFlag;
        public bool comboFlag;
        public bool lockOnFlag;
        public bool fireFlag;
        public bool inventoryFlag;
        public bool levelFlag;
        public bool SkillUIFlag;
        public float rollInputTimer;

        public bool input_Has_Been_Qued;
        public float current_Qued_Input_Timer;
        public float default_Qued_Input_Time;
        public bool qued_RB_Input;

        public Transform criticalAttackRayCastStartPoint;

        private List<Joycon> joycons;

        // Values made available via Unity
        public Vector3 accelLeft;
        public int jc_indLeft = 0;

        public Vector3 accelRight;
        public int jc_indRight = 1;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Start()
        {
            accelLeft = new Vector3(0, 0, 0);
            accelRight = new Vector3(0, 0, 0);
            // get the public Joycon array attached to the JoyconManager in scene
            joycons = JoyconManager.Instance.j;
        }

        private void Update()
        {
            if (joycons != null)
            {
                Joycon jLeft = joycons[jc_indLeft];
                Joycon jRight = joycons[jc_indRight];

                accelLeft = jLeft.GetAccel();
                accelRight = jRight.GetAccel();
            }
        }

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerActions.RB.performed += i => tap_rb_Input = true;

                inputActions.PlayerActions.HoldRB.performed += i => hold_rb_Input = true;
                inputActions.PlayerActions.HoldRB.canceled += i => hold_rb_Input = false;

                inputActions.PlayerActions.HoldRT.performed += i => hold_rt_Input = true;
                inputActions.PlayerActions.HoldRT.canceled += i => hold_rt_Input = false;

                inputActions.PlayerActions.close.performed += i => delete_Intput = true;
                inputActions.PlayerActions.RT.performed += i => tap_rt_Input = true;
                inputActions.PlayerActions.TapLB.performed += i => tap_lb_Input = true;
                inputActions.PlayerActions.LB.performed += i => lb_Input = true;
                inputActions.PlayerActions.LB.canceled += i => lb_Input = false;
                inputActions.PlayerActions.LT.performed += i => tap_lt_Input = true;
                inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
                inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
                inputActions.PlayerQuickSlots.DPadDown.performed += i => d_Pad_Down = true;
                inputActions.PlayerActions.A.performed += i => a_Input = true;
                inputActions.PlayerActions.X.performed += i => x_Input = true;
                inputActions.PlayerActions.Roll.performed += i => b_Input = true;
                inputActions.PlayerActions.Roll.canceled += i => b_Input = false;
                inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
                inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
                inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;
                inputActions.PlayerActions.Y.performed += i => y_Input = true;
                inputActions.PlayerActions.Level.performed += i => level_Input = true;

                inputActions.PlayerActions.QuedRB.performed += i => QueInput(ref qued_RB_Input);
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput()
        {
            if (player.isDead)
                return;

            HandleMoveInput();
            HandleRollInput();

            HandleHoldRBInput();
            HandleHoldLBInput();
            HandleHoldRTInput();

            HandleTapLBInput();
            HandleTapRBInput();
            HandleTapRTInput();
            HandleTapLTInput();

            HandleQuickSlotsInput();
            HandleInventoryInput();

            HandleLockOnInput();
            HandleTwoHandInput();
            HandleUseConsumableInput();
            //HandleQuedInput();
            HandleOpenLevel();
            HandleCloseWindow();
        }

        private void HandleMoveInput()
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        private void HandleRollInput()
        {
            if (b_Input)
            {
                rollInputTimer += Time.deltaTime;

                if (player.playerStatsManager.currentStamina <= 0)
                {
                    b_Input = false;
                    player.isSprinting = false;
                }

                if (moveAmount > 0.5f && player.playerStatsManager.currentStamina > 0)
                {
                    player.isSprinting = true;
                }
            }
            else
            {
                player.isSprinting = false;

                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }

        private void HandleOpenLevel()
        {
            if (level_Input)
            {
                levelFlag = !levelFlag; // toggle levelFlag

                if (levelFlag)
                {
                    player.uiManager.levelUpWindow.SetActive(true);
                }
                else
                {
                    player.uiManager.levelUpWindow.SetActive(false);
                }
            }
        }

        private void HandleCloseWindow()
        {
            if (delete_Intput)
            {
                delete_Intput = false;
                player.uiManager.CloseShop();   
            }
        }

        private void HandleTapRBInput()
        {
            if (tap_rb_Input || accelRight.sqrMagnitude >= 10f)
            {
                tap_rb_Input = false;

                if (player.playerInventoryManager.rightWeapon.oh_tap_RB_Action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.oh_tap_RB_Action.PerformAction(player);
                }
            }
        }

        private void HandleHoldRBInput()
        {
            if (hold_rb_Input)
            {
                if (player.playerInventoryManager.rightWeapon.oh_hold_RB_Action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.oh_hold_RB_Action.PerformAction(player);
                }
            }
        }

        private void HandleHoldRTInput()
        {
            player.animator.SetBool("isChargingAttack", hold_rt_Input);

            if (hold_rt_Input)
            {
                player.UpdateWhichHandCharacterIsUsing(true);
                player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;

                if (player.isTwoHandingWeapon)
                {
                    if (player.playerInventoryManager.rightWeapon.th_hold_RT_Action != null)
                    {
                        player.playerInventoryManager.rightWeapon.th_hold_RT_Action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.rightWeapon.oh_hold_RT_Action != null)
                    {
                        player.playerInventoryManager.rightWeapon.oh_hold_RT_Action.PerformAction(player);
                    }
                }
            }
        }

        private void HandleTapRTInput()
        {
            if (tap_rt_Input)
            {
                tap_rt_Input = false;

                if (player.playerInventoryManager.rightWeapon.oh_tap_RT_Action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.oh_tap_RT_Action.PerformAction(player);
                }
            }
        }

        private void HandleTapLTInput()
        {
            if (tap_lt_Input)
            {
                tap_lt_Input = false;

                if (player.isTwoHandingWeapon)
                {
                    //It will be the right handed weapon
                    if (player.playerInventoryManager.rightWeapon.oh_tap_LT_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                        player.playerInventoryManager.rightWeapon.oh_tap_LT_Action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.leftWeapon.oh_tap_LT_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                        player.playerInventoryManager.leftWeapon.oh_tap_LT_Action.PerformAction(player);
                    }
                }
            }
        }

        private void HandleHoldLBInput()
        {
            if (!player.isGrounded ||
                player.isSprinting ||
                player.isFiringSpell)
            {
                lb_Input = false;
                return;
            }

            if (lb_Input)
            {
                if (player.isTwoHandingWeapon)
                {
                    if (player.playerInventoryManager.rightWeapon.oh_hold_LB_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                        player.playerInventoryManager.rightWeapon.oh_hold_LB_Action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.leftWeapon.oh_hold_LB_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                        player.playerInventoryManager.leftWeapon.oh_hold_LB_Action.PerformAction(player);
                    }
                }
            }
            else if (lb_Input == false)
            {
                if (player.isBlocking)
                {
                    player.isBlocking = false;
                    AudioManager.instance.Play("Blocking");
                }
            }
        }

        private void HandleTapLBInput()
        {
            if (tap_lb_Input)
            {
                tap_lb_Input = false;

                if (player.isTwoHandingWeapon)
                {
                    if (player.playerInventoryManager.rightWeapon.oh_tap_LB_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                        player.playerInventoryManager.rightWeapon.oh_tap_LB_Action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.leftWeapon.oh_tap_LB_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                        player.playerInventoryManager.leftWeapon.oh_tap_LB_Action.PerformAction(player);
                    }
                }
            }
        }

        private void HandleQuickSlotsInput()
        {
            if (d_Pad_Right)
            {
                player.playerInventoryManager.ChangeRightWeapon();
            }
            else if (d_Pad_Left)
            {
                player.playerInventoryManager.ChangeLeftWeapon();
            }
            else if (d_Pad_Down)
            {
                player.playerInventoryManager.ChangeConsumableItem();
            }
        }

        private void HandleInventoryInput()
        {
            if (inventoryFlag)
            {
                player.uiManager.UpdateUI();
            }

            if (inventory_Input)
            {
                inventoryFlag = !inventoryFlag;

                if (inventoryFlag)
                {
                    player.uiManager.OpenSelectWindow();
                    player.uiManager.hudWindow.SetActive(false);
                }
                else
                {
                    player.uiManager.CloseSelectWindow();
                    player.uiManager.CloseAllInventoryWindows();
                    player.uiManager.hudWindow.SetActive(true);
                }
            }
        }

        private void HandleLockOnInput()
        {
            if (lockOnInput && lockOnFlag == false)
            {
                lockOnInput = false;
                player.cameraHandler.HandleLockOn();
                if (player.cameraHandler.nearestLockOnTarget != null)
                {
                    player.cameraHandler.currentLockOnTarget = player.cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if (lockOnInput && lockOnFlag)
            {
                lockOnInput = false;
                lockOnFlag = false;
                player.cameraHandler.ClearLockOnTargets();
            }

            if (lockOnFlag && right_Stick_Left_Input)
            {
                right_Stick_Left_Input = false;
                player.cameraHandler.HandleLockOn();

                if (player.cameraHandler.leftLockTarget != null)
                {
                    player.cameraHandler.currentLockOnTarget = player.cameraHandler.leftLockTarget;
                }
            }

            if (lockOnFlag && right_Stick_Right_Input)
            {
                right_Stick_Right_Input = false;
                player.cameraHandler.HandleLockOn();
                if (player.cameraHandler.rightLockTarget != null)
                {
                    player.cameraHandler.currentLockOnTarget = player.cameraHandler.rightLockTarget;
                }
            }

            if (player.cameraHandler != null)
            {
                player.cameraHandler.SetCameraHeight();
            }
        }

        private void HandleTwoHandInput()
        {
            if (y_Input)
            {
                y_Input = false;

                twoHandFlag = !twoHandFlag;

                if (twoHandFlag)
                {
                    player.isTwoHandingWeapon = true;
                    player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.rightWeapon, false);
                    AudioManager.instance.Play("TwoHandingWeapon");
                }
                else
                {
                    player.isTwoHandingWeapon = false;
                    player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.rightWeapon, false);
                    player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.leftWeapon, true);
                    AudioManager.instance.Play("TwoHandingWeapon");
                }
            }
        }

        private void HandleUseConsumableInput()
        {
            if (x_Input)
            {
                x_Input = false;
                player.playerInventoryManager.currentConsumable.AttemptToConsumeItem(player.playerAnimatorManager, player.playerWeaponSlotManager, player.playerEffectsManager, player);
            }
        }

        private void QueInput(ref bool quedInput)
        {
            //  DISABLE ALL OTHER QUED INPUTS
            //Qued_LB_Input = false;
            //Qued_RT_Input = false;

            //  ENABLE THE REFERENCED INPUT BY REFERENCE
            //  If we are interacting, we can que an input, otherwise queing is not needed
            if (player.isInteracting)
            {
                quedInput = true;
                current_Qued_Input_Timer = default_Qued_Input_Time;
                input_Has_Been_Qued = true;
            }
        }

        private void HandleQuedInput()
        {
            if (input_Has_Been_Qued)
            {
                if (current_Qued_Input_Timer > 0)
                {
                    current_Qued_Input_Timer = current_Qued_Input_Timer - Time.deltaTime;
                    ProcessQuedInput();
                }
                else
                {
                    input_Has_Been_Qued = false;
                    current_Qued_Input_Timer = 0;
                }
            }
        }

        private void ProcessQuedInput()
        {
            if (qued_RB_Input)
            {
                tap_rb_Input = true;
            }
            // If Qued Lb Input => Tap LB Input = true
            // If Qued LT Input => Tap LT Input = true
        }
    }
}