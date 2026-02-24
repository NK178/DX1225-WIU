using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GenericProjectile : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected float lifetime = 5f;

    protected DataHolder.DATATYPE spawnerType;
    protected float projectileDamage;
    protected ProjectileObjectPool myPool;


    ///////TESTING 
    protected BaseActiveData referenceData;

    public void SetPool(ProjectileObjectPool pool)
    {
        myPool = pool;
    }

    virtual public void Initialize(DataHolder.DATATYPE spawner, float damageAmount)
    {
        spawnerType = spawner;
        projectileDamage = damageAmount;

        // Ensures the projectile doesn't last forever if shot into the void
        StartCoroutine(LifetimeRoutine());
    }


    virtual public void Initialize(BaseActiveData activeData, float damageAmount)
    {
        referenceData = activeData;
        spawnerType = activeData.dataType;
        projectileDamage = damageAmount;

        // Ensures the projectile doesn't last forever if shot into the void
        StartCoroutine(LifetimeRoutine());
    }

    private IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(lifetime);
        ReturnToPool();
    }

    protected void ReturnToPool()
    {
        if (myPool != null && gameObject.activeSelf)
        {
            myPool.ReleaseObject(gameObject);
        }
    }

    private void OnDisable()
    {
        // Stop the lifetime timer immediately when it gets deactivated by the pool
        StopAllCoroutines();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //bool hitEnemy = collision.gameObject.CompareTag("Enemy");
        //bool hitPlayer = collision.gameObject.CompareTag("Player");
        //bool hitEnvironment = collision.gameObject.CompareTag("Environment");

        //if (spawnerType == DataHolder.DATATYPE.PLAYER && hitEnemy)
        //{
        //    Debug.Log($"Hit Enemy for {projectileDamage} damage!");
        //    ReturnToPool();
        //}
        //else if ((spawnerType == DataHolder.DATATYPE.RANGED_ENEMY || spawnerType == DataHolder.DATATYPE.BOSS_ENEMY) && hitPlayer)
        //{
        //    Debug.Log($"Hit Player for {projectileDamage} damage!");
        //    ReturnToPool();
        //}

        //if (hitEnvironment)
        //{
        //    ReturnToPool();
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        //bool hitEnemy = other.CompareTag("Enemy");
        //bool hitEnvironment = other.CompareTag("Environment");


        bool hitPlayer = other.CompareTag("Player");

        bool hitDummyTag = other.CompareTag("Dummy");
        bool hitDummyLayer = other.gameObject.layer == LayerMask.NameToLayer("Dummy");
        bool hitEnemy = other.CompareTag("Enemy");
        bool hitFloor = other.CompareTag("Floor");

        // Enemy/Boss shoots the Player
        if ((spawnerType == DataHolder.DATATYPE.BOSS_ENEMY || spawnerType == DataHolder.DATATYPE.RANGED_ENEMY) && hitPlayer)
        {
            PlayerController player = other.GetComponentInParent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(projectileDamage);
            }
            ReturnToPool();
        }

        // Player shoots the Boss or Enemy
        if (spawnerType == DataHolder.DATATYPE.PLAYER && (hitEnemy || other.CompareTag("Boss")))
        {
            // Try to find what we hit and damage it
            BossController boss = other.GetComponentInParent<BossController>();
            EnemyController enemy = other.GetComponentInParent<EnemyController>();

            if (boss != null) boss.TakeDamage(projectileDamage);
            if (enemy != null) enemy.TakeDamage(projectileDamage);

            // Tell the UI
            if (BattleUIManager.Instance != null && referenceData is PlayerActiveData playerData)
            {
                BattleUIManager.Instance.AddDamage(playerData.currentClassType, projectileDamage);
            }
            ReturnToPool();
        }

        // Dummy logic
        if (spawnerType == DataHolder.DATATYPE.RANGED_ENEMY && (hitDummyTag || hitDummyLayer))
        {
            DummyController dummy = other.GetComponentInParent<DummyController>();
            if (dummy != null) dummy.TakeDamage(10);
            ReturnToPool();
        }

        //if (spawnerType == DataHolder.DATATYPE.RANGED_ENEMY && hitDummy)
        //{

        //    DummyController dummy = other.GetComponentInParent<DummyController>();

        //    if (dummy != null)
        //    {
        //        Debug.LogWarning("DUMMY HIT!");
        //        dummy.TakeDamage(10);
        //    }
        //    else
        //    {
        //        Debug.LogWarning("DUMMY NULL!");
        //    }

        //    ReturnToPool();
        //}
        //if (hitEnvironment)
        //{
        //    ReturnToPool();
        //}
    }

}