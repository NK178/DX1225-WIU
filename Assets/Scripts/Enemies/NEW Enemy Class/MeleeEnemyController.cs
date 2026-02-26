using UnityEngine;

public class MeleeEnemyController : EnemyController
{
    [SerializeField] private MeleeEnemyClassData meleeData;

    protected override void Start()
    {
        base.Start();
        if (activeData == null) return;

        detectionRadius = meleeData.detectionRadius;
        stopDistance = meleeData.stopDistance;

        activeData.enemyClassType = ENEMYCLASSTYPE.MELEE;
        activeData.dataType = DataHolder.DATATYPE.MELEE_ENEMY;
        activeData.currentHealth = meleeData.maxHealth;
        activeData.currentAttack = meleeData.damage;
        activeData.currentMoveSpeed = meleeData.moveSpeed;
    }

    protected override void HandleMove()
    {
        activeData.currentMoveSpeed = meleeData.moveSpeed;

        switch (activeData.currentState)
        {
            case EnemyActiveData.AIState.WANDERING:
                HandleWander(meleeData.moveSpeed);
                break;
            case EnemyActiveData.AIState.CHASING:
                HandleChasing(meleeData.moveSpeed);
                break;
        }
    }

    protected override void HandleAttack()
    {
        if (activeData.targetPlayer == null) return;

        transform.LookAt(new Vector3(
            activeData.targetPlayer.position.x,
            transform.position.y,
            activeData.targetPlayer.position.z));

        if (!isAttacking)
        {
            enemyAnimator.TriggerAttack();
            isAttacking = false;
        }
    }

    protected new void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
    }
}