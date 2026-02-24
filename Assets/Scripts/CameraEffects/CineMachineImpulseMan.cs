using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

/// Klaus
/// <summary>
/// Ainsley specifically requested this be something similar to objectPoolManager, able to be called from anywhere
/// So I'll be storing all the possible needed camera effects here
/// </summary>
/// 

// TO:DO CREATE ENUM? (AINSLEY WANTS TO DO SO)
// SCRIPTABLE OBJ

[System.Serializable]
public struct camEffect
{
    public CinemachineImpulseSource test;
}

public class CineMachineImpulseMan : MonoBehaviour
{
    [SerializeField] private List<camEffect> camEffects;

    [Header("Debugging")]
    [SerializeField] private PlayerInput playerInput;
    private InputAction shakeAction;


    public static CineMachineImpulseMan Instance; 

    private void Start()
    {
        shakeAction = playerInput.actions["Debugging"];
        shakeAction?.Enable();

        if (Instance == null)
        {
            Instance = new CineMachineImpulseMan(camEffects, playerInput, shakeAction);
        }
    }

    public CineMachineImpulseMan(List<camEffect> camEffects, PlayerInput playerInput, InputAction shakeAction)
    {
        this.camEffects = camEffects;
        this.playerInput = playerInput;
        this.shakeAction = shakeAction;
    }

    private void Update()
    {
        if (shakeAction.WasPressedThisFrame())
        {
            testFunc();
        }
    }

    public void testFunc()
    {
        camEffects[0].test.GenerateImpulse();
        Debug.Log("SHAKE SCREEN!");
    }
}
