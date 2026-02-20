using UnityEngine;

[CreateAssetMenu(fileName = "RangerClassData", menuName = "Scriptable Objects/RangerClassData")]
public class RangerClassData : BaseClassData
{
    public CLASSTYPE classType;
    public float moveSpeed;

    [Header("Main Attack: Seed Gun")]
    public GameObject seedProjectilePrefab;
    public float seedLaunchForce;
    public float attackCooldown;

    [Header("Defense: Roll")]
    public float rollSpeedMultiplier = 2.5f;
    public float rollDuration = 0.4f;

    [Header("Ability: Laser Eyes")]
    public int maxLaserAmmo = 50;
    public float laserRange = 50f;
    public LayerMask hitMask;
}