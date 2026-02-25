using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Rendering;

public class PlayerInputController : MonoBehaviour
{
    [Header("Character Mechanics")]
    [SerializeField] private FighterMechanics fighterMechanics;
    [SerializeField] private RangerMechanics rangerMechanics; 

    private BaseClassMechanics currentMechanics;

    [Header("Core Data")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private DataHolder dataHolder;

    [Header("Targeting")]
    [SerializeField] private TargetingSystem targetingSystem;

    private PlayerActiveData activeData;

    // Movement & Combat Actions
    private InputAction moveAction;
    private InputAction attackAction;
    private InputAction defenseAction;
    private InputAction abilityAction;
    private InputAction jumpAction;
    [SerializeField] private float JumpVelocity;

    // Switching Actions
    private InputAction switchFighterAction;
    private InputAction switchRangerAction;

    //Inventory (Ains) 
    private InputAction inventoryAction; 
    /// <summary>
    /// Camera (Klaus)
    /// playerTransform set to the Player Controller
    /// cameraTransform set to the cameraHolder / playerInputManager 
    /// (Basically whatever CineMachine Third Person is targetting)
    /// upCamLimit (0 - 90), Looking downwards, Bird's Eye View
    /// downCamLimit (360 - 270) Looking upwards, Worm's Eye View
    /// </summary>
    [Header("Camera")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CinemachineThirdPersonFollow cinemachineCamera;
    [SerializeField, Range(0,89)] private float upCamLimit;
    [SerializeField, Range(271,360)] private float downCamLimit;
    private InputAction cameraAction;
    private float tempCameraY;
    private InputAction camZoomAction;
    private InputAction camChange;
    private enum CAMTYPE
    {
        THIRD_PERSON = 0,
        FREE_LOOK,
        SPLINE_DOLLY,
    }
    private CAMTYPE camtype;
    [SerializeField] private GameObject[] cams = new GameObject[2];

    // Targeting Actions
    private InputAction lockOnAction;
    private InputAction switchTargetRightAction;
    private InputAction switchTargetLeftAction;

    [Header("Health")]
    // Health Issues
    [SerializeField] private float fighterHP;
    [SerializeField] private float rangerHP;

    void Start()
    {
        activeData = (PlayerActiveData)dataHolder.activeData;

        if (fighterMechanics != null) 
            fighterMechanics.Initialize(activeData);
        if (rangerMechanics != null) rangerMechanics.Initialize(activeData);

        fighterHP = fighterMechanics.fighterClassData.maxHealth;
        rangerHP = rangerMechanics.rangerData.maxHealth;
        activeData.currentHealth = rangerHP;

        // default to melee class
        SwitchCharacter(CLASSTYPE.MELEE);

        SetupInputs();
    }

    private void SetupInputs()
    {
        moveAction = playerInput.actions.FindAction("Move");
        moveAction?.Enable();

        jumpAction = playerInput.actions.FindAction("Jump");
        jumpAction?.Enable();

        attackAction = playerInput.actions.FindAction("Attack");
        attackAction?.Enable();

        defenseAction = playerInput.actions.FindAction("Defense");
        defenseAction?.Enable();

        abilityAction = playerInput.actions.FindAction("Ability");
        abilityAction?.Enable();

        switchFighterAction = playerInput.actions.FindAction("SwitchFighter");
        switchFighterAction?.Enable();

        switchRangerAction = playerInput.actions.FindAction("SwitchRanger");
        switchRangerAction?.Enable();

        lockOnAction = playerInput.actions.FindAction("LockOn");
        lockOnAction?.Enable();

        switchTargetRightAction = playerInput.actions.FindAction("SwitchRight");
        switchTargetRightAction?.Enable();

        switchTargetLeftAction = playerInput.actions.FindAction("SwitchLeft");
        switchTargetLeftAction?.Enable();

        cameraAction = playerInput.actions.FindAction("Look");
        cameraAction?.Enable();

        camZoomAction = playerInput.actions.FindAction("CameraZoom");
        camZoomAction?.Enable();

        inventoryAction = playerInput.actions.FindAction("Inventory");
        inventoryAction?.Enable();

        camChange = playerInput.actions.FindAction("CamSwap");
        camChange?.Enable();
    }

    void Update()
    {
        HandleCharacterSwitching();
        HandleMove();
        HandleCameraMovement();
        HandleCombat();

        if (inventoryAction.WasPressedThisFrame())
        {
            activeData.isInventoryOpen = !activeData.isInventoryOpen;

            if (BattleUIManager.Instance != null)
            {
                BattleUIManager.Instance.ToggleUI(!activeData.isInventoryOpen);
            }
        }

        if (camChange.WasPressedThisFrame())
        {
            if (camtype == CAMTYPE.THIRD_PERSON)
            {
                camtype = CAMTYPE.FREE_LOOK;
                cams[0].SetActive(false);
                cams[1].SetActive(true);
            }
            else if (camtype == CAMTYPE.FREE_LOOK)
            {
                camtype = CAMTYPE.THIRD_PERSON;
                cams[0].SetActive(true);
                cams[1].SetActive(false);
            }
        }
    }

    void HandleCharacterSwitching()
    {
        if (switchFighterAction != null && switchFighterAction.WasPressedThisFrame())
        {
            SwitchCharacter(CLASSTYPE.MELEE);
        }

        if (switchRangerAction != null && switchRangerAction.WasPressedThisFrame())
        {
            SwitchCharacter(CLASSTYPE.RANGED);
        }
    }

    private void SwitchCharacter(CLASSTYPE newClass)
    {
        if (activeData != null && activeData.currentClassType == newClass && currentMechanics != null) return;

        if (activeData != null)
        {
            activeData.currentClassType = newClass;
        }

        if (newClass == CLASSTYPE.MELEE)
        {
            rangerHP = activeData.currentHealth;
            currentMechanics = fighterMechanics;
            currentMechanics.EquipClass();
            activeData.currentHealth = fighterHP;
            activeData.maxHealth = fighterMechanics.fighterClassData.maxHealth;
            activeData.currentAttack = fighterMechanics.fighterClassData.damage;
            Debug.Log("Swapped to DragonFruit (Fighter)!");
            if (AudioManager.instance != null) AudioManager.instance.Play("SwapFighter");
            //Debug.Log("currentHealth: " + activeData.currentHealth);
            //Debug.Log("fighterHP: " + fighterHP);
            //Debug.Log("rangerHP: " + rangerHP);
        }
        else if (newClass == CLASSTYPE.RANGED)
        {
            fighterHP = activeData.currentHealth;
            currentMechanics = rangerMechanics;
            currentMechanics.EquipClass();
            activeData.currentHealth = rangerHP;
            activeData.maxHealth = rangerMechanics.rangerData.maxHealth;
            activeData.currentAttack = rangerMechanics.rangerData.damage;
            Debug.Log("Swapped to Mandarin (Ranger)!");
            if (AudioManager.instance != null) AudioManager.instance.Play("SwapRanger");
            //Debug.Log("currentHealth: " + activeData.currentHealth);
            //Debug.Log("fighterHP: " + fighterHP);
            //Debug.Log("rangerHP: " + rangerHP);
        }
        if (BattleUIManager.Instance != null)
        {
            BattleUIManager.Instance.SwapActivePlayerUI(newClass);
        }
    }

    void HandleMove()
    {
        if (moveAction == null) return;

        Vector2 direction = moveAction.ReadValue<Vector2>();

        if (direction.magnitude > 0)
        {
            if (camtype == CAMTYPE.FREE_LOOK)
            {
                // Modify the move direction according to where the camera is facing
                direction = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up) * direction;
                // Rotate the character facing towards the move direction
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.parent.localRotation = Quaternion.RotateTowards(transform.parent.localRotation, targetRotation, 2f);
            }
            activeData.moveDirection = direction;
            activeData.isMoving = true;
        }
        else
        {   
            activeData.moveDirection = Vector2.zero;
            activeData.isMoving = activeData.isJumping;
        }

        if (jumpAction == null) return;

        if (jumpAction.IsPressed() && !activeData.isJumping)
        {
            activeData.jumpVel.y = JumpVelocity;
            activeData.isMoving = true;
            activeData.isJumping = true;
        }
    }
    
    // Klaus
    /// <summary>
    /// Player can turn the character left and right but not up and down.
    /// (You can just set the playerTransform and cameraTransform to the same GO to achieve a left right up down)
    /// Rotates the X-Axis of the input manager GO, it is the one being tracked by Cinemachine
    /// Rotate the Y-Axis of the player.
    /// </summary>
    private void HandleCameraMovement()
    {
        if (cameraAction == null && cameraTransform == null && playerTransform == null) return;
        if (activeData.isInventoryOpen) return;
        HandleCamZoom();
        if (camtype == CAMTYPE.THIRD_PERSON)
        {
            Vector2 dir = cameraAction.ReadValue<Vector2>();
            if (dir.magnitude <= 0) return;
            tempCameraY = cameraTransform.localEulerAngles.x;
            tempCameraY += -dir.y * 0.1f;
            // My own version of clamp
            // Clamp can't work since I need to stay in the 1st and 4th quadrants, 0-89 degress and 360-270 degrees
            if (!(tempCameraY < upCamLimit || tempCameraY > downCamLimit))
            {
                if (tempCameraY < 180f)
                    tempCameraY = upCamLimit;
                else
                    tempCameraY = downCamLimit;
            }
            cameraTransform.localEulerAngles = new Vector3(tempCameraY,cameraTransform.localEulerAngles.y,cameraTransform.localEulerAngles.z);
            playerTransform.eulerAngles += new Vector3(0, dir.x) * 0.1f;
        }
    }

    // Klaus
    /// <summary>
    /// Player Can Zoom in and out with scroll wheel
    /// Can also be used for Aiming
    /// Something akin to roblox (Maybe add on swap to First Person after a certain Zoom in?)
    /// </summary>
    private void HandleCamZoom(float camZoom = 0f)
    {
        if (camZoomAction == null) return;
        float dir = -camZoomAction.ReadValue<Vector2>().y;
        if (dir == 0) return;
        if (camZoom == 0f)
        {
            cinemachineCamera.CameraDistance += dir;
            cinemachineCamera.CameraDistance = Mathf.Clamp(cinemachineCamera.CameraDistance, 1f, 6f);
        }
        else
            cinemachineCamera.CameraDistance = Mathf.Clamp(camZoom, 1f, 6f);
    }

    void HandleCombat()
    {
        if (currentMechanics == null || activeData.isInventoryOpen) return;

        if (attackAction != null && attackAction.WasPressedThisFrame())
        {
            currentMechanics.HandleAttack();
        }

        if (defenseAction != null && defenseAction.WasPressedThisFrame())
        {
            currentMechanics.HandleDefense();
        }

        if (abilityAction != null && abilityAction.WasPressedThisFrame())
        {
            currentMechanics.HandleAbility();
        }

        if (targetingSystem != null)
        {
            if (lockOnAction != null && lockOnAction.WasPressedThisFrame())
            {
                targetingSystem.ToggleLockOn();
            }

            if (switchTargetRightAction != null && switchTargetRightAction.WasPressedThisFrame())
            {
                targetingSystem.SwitchTarget(1);
            }

            if (switchTargetLeftAction != null && switchTargetLeftAction.WasPressedThisFrame())
            {
                targetingSystem.SwitchTarget(-1);
            }
        }
    }
}