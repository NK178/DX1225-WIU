using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;

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

    // Switching Actions
    private InputAction switchFighterAction;
    private InputAction switchRangerAction;

    //Inventory (Ains) 
    private InputAction inventoryAction; 

    [Header("Camera")]
    // Camera (Klaus)
    // playerTransform set to the Player Controller
    // cameraTransform set to the cameraHolder / playerInputManager 
    // (Basically whatever CineMachine Third Person is targetting)
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform cameraTransform;
    private InputAction cameraAction;

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

        inventoryAction = playerInput.actions.FindAction("Inventory");
        inventoryAction?.Enable();
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
            activeData.moveDirection = direction;
            activeData.isMoving = true;
        }
        else
        {   
            activeData.moveDirection = Vector2.zero;
            activeData.isMoving = false;
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
        Vector2 dir = cameraAction.ReadValue<Vector2>();
        if (dir.magnitude <= 0) return;
        //Debug.Log("Moving camera");
        cameraTransform.eulerAngles += new Vector3(-dir.y, 0) * 0.1f;
        playerTransform.eulerAngles += new Vector3(0, dir.x) * 0.1f;
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