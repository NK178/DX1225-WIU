using System.Collections.Generic;
using UnityEngine;

// Klaus
// Design taken from Rayyan
[CreateAssetMenu(fileName = "FighterClassData", menuName = "Scriptable Objects/FighterClassData")]
public class FighterClassData : BaseClassData
{
    public CLASSTYPE classType;
    public float moveSpeed;

    [Header("Main Attack: Sword")]
    public float AtkCD;

    [Header("Defensive: Riposte")]
    public float parryTiming; // If I ever do it
    public float blockDamageReduction;
    public float parryCD;
    public bool isBlocking;

    [Header("Special Ability: Dragon Flaming Slash")]
    public float damageMultiplier;
    public float abilityCD;
}
