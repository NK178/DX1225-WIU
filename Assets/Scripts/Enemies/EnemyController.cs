using UnityEngine;

[System.Serializable]
public enum ENEMYCLASSTYPE
{
    MELEE,
    RANGED,
    NUM_TYPES
}

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Data")]
    [SerializeField] private DataHolder dataHolder;
    [SerializeField] private ENEMYCLASSTYPE enemyType;

    [SerializeField] private MeleeEnemyClassData knifeData;
    [SerializeField] private RangedEnemyClassData rangerData;

    private EnemyActiveData activeData;

    private void Awake()
    {
        activeData = (EnemyActiveData)dataHolder.activeData;

        if (activeData == null)
            return;

        activeData.enemyClassType = enemyType;
    }

    void Update()
    {
        HandleMove();
        HandleAttack();
    }

    private void HandleMove()
    {
        activeData.enemyClassType = enemyType;

        if (activeData.enemyClassType == ENEMYCLASSTYPE.MELEE)
            activeData.currentMoveSpeed = knifeData.moveSpeed;
        else if (activeData.enemyClassType == ENEMYCLASSTYPE.RANGED)
            activeData.currentMoveSpeed = rangerData.moveSpeed;

        float moveSpeed = activeData.currentMoveSpeed;
    }

    private void HandleAttack()
    {

    }

    private void TakeDamage(int damage)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
