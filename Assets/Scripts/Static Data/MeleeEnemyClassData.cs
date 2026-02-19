using UnityEngine;

[CreateAssetMenu(fileName = "MeleeEnemyClassData", menuName = "Scriptable Objects/MeleeEnemyClassData")]
public class MeleeEnemyClassData : BaseClassData
{
    public float attackSpeed;
    public float moveSpeed;
    public ENEMYCLASSTYPE enemyClassType;
}
