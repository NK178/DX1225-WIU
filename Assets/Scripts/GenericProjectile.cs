using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GenericProjectile : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float lifetime = 5f;

    private DataHolder.DATATYPE spawnerType;
    private float projectileDamage;
    private ProjectileObjectPool myPool;

    public void SetPool(ProjectileObjectPool pool)
    {
        myPool = pool;
    }

    public void Initialize(DataHolder.DATATYPE spawner, float damageAmount)
    {
        spawnerType = spawner;
        projectileDamage = damageAmount;

        // Ensures the projectile doesn't last forever if shot into the void
        StartCoroutine(LifetimeRoutine());
    }

    private IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(lifetime);
        ReturnToPool();
    }

    private void ReturnToPool()
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
        bool hitEnemy = collision.gameObject.CompareTag("Enemy");
        bool hitPlayer = collision.gameObject.CompareTag("Player");
        bool hitEnvironment = collision.gameObject.CompareTag("Environment");

        if (spawnerType == DataHolder.DATATYPE.PLAYER && hitEnemy)
        {
            Debug.Log($"Hit Enemy for {projectileDamage} damage!");
            ReturnToPool();
        }
        else if ((spawnerType == DataHolder.DATATYPE.RANGED_ENEMY || spawnerType == DataHolder.DATATYPE.BOSS_ENEMY) && hitPlayer)
        {
            Debug.Log($"Hit Player for {projectileDamage} damage!");
            ReturnToPool();
        }

        if (hitEnvironment)
        {
            ReturnToPool();
        }
    }
}