using System;
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

// make a enum variable everytime you make a new effect
public enum EFFECT
{
    HEAVYCAMSHAKE = 0,
    CAMSHAKE,
}

[System.Serializable]
struct camEffect
{
    public EFFECT Effect;
    public CinemachineImpulseSource impulseSource;
}

public class CineMachineImpulseMan : MonoBehaviour
{
    // Trying this method, Inspired by actions[string] way of calling (Calling the index as an enum)
    private CinemachineImpulseSource[] impulseSources;
    [SerializeField] private List<camEffect> camEffects;

    [Header("Debugging")]
    [SerializeField] private PlayerInput playerInput;
    private InputAction shakeAction;


    public static CineMachineImpulseMan Instance; 

    private void Start()
    {
        if (shakeAction == null)
        {
            shakeAction = playerInput.actions["Debugging"];
            shakeAction?.Enable();
        }

        if (Instance == null)
        {
            impulseSources = new CinemachineImpulseSource[Enum.GetValues(typeof(EFFECT)).Length];
            for (int i = 0; i < camEffects.Count; i++)
            {
                impulseSources[(int)camEffects[i].Effect] = camEffects[i].impulseSource;
            }
            Instance = new CineMachineImpulseMan(impulseSources, shakeAction);
        }
    }

    public CineMachineImpulseMan(CinemachineImpulseSource[] camEffects, InputAction shakeAction)
    {
        this.impulseSources = camEffects;
        this.shakeAction = shakeAction;
    }

    private void Update()
    {
        if (shakeAction.WasPressedThisFrame())
        {
            GenerateEffect(EFFECT.HEAVYCAMSHAKE);
        }
    }

    public void GenerateEffect(EFFECT effect)
    {
        impulseSources[(int)effect].GenerateImpulse();
        Debug.Log("SHAKE SCREEN!");
    }
}
