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
    public Collider swordCollider;
    public float AtkCD;

    [Header("Defensive: Riposte")]
    public float parryTiming;
    public float blockDamageReduction;
    public float parryCD;
    public bool isBlocking;

    [Header("Special Ability: Dragon Flaming Slash")]
    public float damageMultiplier;
}
