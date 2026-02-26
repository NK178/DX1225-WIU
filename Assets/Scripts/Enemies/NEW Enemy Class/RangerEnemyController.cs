using UnityEngine;

public class RangedEnemyController : EnemyController
{
    [SerializeField] private RangedEnemyClassData rangerData;
    [SerializeField] private Transform firePoint;

    [Header("Ranger Settings")]
    [SerializeField] private float minAttackCooldown = 0.5f;
    [SerializeField] private float maxAttackCooldown = 5f;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 15f;

    private float lastAttackTime;

    protected override void Start()
    {
        base.Start();
        if (activeData == null) return;

        detectionRadius = rangerData.detectionRadius;
        stopDistance = rangerData.stopDistance;

        activeData.enemyClassType = ENEMYCLASSTYPE.RANGED;
        activeData.dataType = DataHolder.DATATYPE.RANGED_ENEMY;
        activeData.currentHealth = rangerData.maxHealth;
        activeData.currentAttack = rangerData.damage;
        activeData.currentMoveSpeed = rangerData.moveSpeed;
    }

    protected override void HandleMove()
    {
        activeData.currentMoveSpeed = rangerData.moveSpeed;

        switch (activeData.currentState)
        {
            case EnemyActiveData.AIState.WANDERING:
                HandleWander(rangerData.moveSpeed);
                break;
            case EnemyActiveData.AIState.CHASING:
                HandleChasing(rangerData.moveSpeed);
                break;
        }
    }

    protected override void HandleAttack()
    {
        if (firePoint == null || activeData.targetPlayer == null) return;

        float distance = Vector3.Distance(transform.position, activeData.targetPlayer.position);
        float factor = Mathf.InverseLerp(minDistance, maxDistance, distance);
        float cooldown = Mathf.Lerp(minAttackCooldown, maxAttackCooldown, factor);

        if (Time.time < lastAttackTime + cooldown) return;

        Vector3 aimDirection = (activeData.targetPlayer.position - firePoint.position).normalized;
        firePoint.rotation = Quaternion.LookRotation(aimDirection);

        activeData.objectPoolSpawnData = new ObjectPoolSpawnData(
            firePoint.position,
            firePoint.forward,
            firePoint.forward * rangerData.launchForce,
            rangerData.damage
        );

        activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.RUBBERBAND_BULLETS;
        activeData.isObjectPoolTriggered = true;

        if (ObjectPoolManager.Instance != null)
        {
            Debug.Log("RANGER ATTACK!");
            ObjectPoolManager.Instance.HandleSpawnRequest(activeData);
        }

        lastAttackTime = Time.time;
    }

    protected override string[] GetDamageSounds()
    {
        return new string[] { "RangerHurt" };
    }

    protected new void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, minDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxDistance);

        if (activeData != null && activeData.targetPlayer != null)
        {
            float dist = Vector3.Distance(transform.position, activeData.targetPlayer.position);
            float factor = Mathf.InverseLerp(minDistance, maxDistance, dist);
            float currentCD = Mathf.Lerp(minAttackCooldown, maxAttackCooldown, factor);

            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, activeData.targetPlayer.position);

            #if UNITY_EDITOR
            UnityEditor.Handles.Label(transform.position + Vector3.up * 2f,
                $"Distance: {dist:F1}m\nNext CD: {currentCD:F2}s");
            #endif
        }
    }
}