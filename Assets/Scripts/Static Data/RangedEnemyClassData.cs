using UnityEngine;

[CreateAssetMenu(fileName = "RangedEnemyClassData", menuName = "Scriptable Objects/RangedEnemyClassData")]
public class RangedEnemyClassData : BaseClassData
{
    public float attackSpeed;
    public float moveSpeed;
    public float detectionRadius;
    public float stopDistance;
    public float launchForce;
    public ENEMYCLASSTYPE enemyClassType;
}
