using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [Header("Character Mechanics")]
    [SerializeField] private BaseClassMechanics fighterMechanics;
    [SerializeField] private BaseClassMechanics rangerMechanics; 

    private BaseClassMechanics currentMechanics;

    [Header("Core Data")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private DataHolder dataHolder;

    private PlayerActiveData activeData;

    // Movement & Combat Actions
    private InputAction moveAction;
    private InputAction attackAction;
    private InputAction defenseAction;
    private InputAction abilityAction;

    // Switching Actions
    private InputAction switchFighterAction;
    private InputAction switchRangerAction;

    void Start()
    {
        activeData = (PlayerActiveData)dataHolder.activeData;

        if (fighterMechanics != null) fighterMechanics.Initialize(activeData);
        if (rangerMechanics != null) rangerMechanics.Initialize(activeData);

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
    }

    void Update()
    {
        HandleCharacterSwitching();
        HandleMove();
        HandleCombat();
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
        // Don't switch if we are already that class
        if (activeData.currentClassType == newClass && currentMechanics != null) return;

        activeData.currentClassType = newClass;

        if (newClass == CLASSTYPE.MELEE)
        {
            currentMechanics = fighterMechanics;
            Debug.Log("Swapped to DragonFruit (Fighter)!");
            // Toggle visual models (fighterModel.SetActive(true))
        }
        else if (newClass == CLASSTYPE.RANGED)
        {
            currentMechanics = rangerMechanics;
            Debug.Log("Swapped to Mandarin (Ranger)!");
            // Toggle visual models (rangerModel.SetActive(true))
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

    void HandleCombat()
    {
        if (currentMechanics == null) return;

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
    }
}