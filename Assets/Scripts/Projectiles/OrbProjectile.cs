using System.Reflection;
using UnityEngine;

public class OrbProjectile : MonoBehaviour
{

    [Header("Enemy Settings")]
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject spawnEffect;
    [SerializeField] private EnemySpawner spawner;

    private static GameObject sharedEffectInstance;
    private bool hasSpawned = false;

    public void SetSpawner(EnemySpawner spawnerRef)
    {
        spawner = spawnerRef;
    }

    private void SpawnEnemy()
    {
        if (enemies == null || enemies.Length == 0)
        {
            Debug.LogError("NO ENEMIES");
            return;
        }

        int randomIndex = Random.Range(0, enemies.Length);
        GameObject enemyPrefab = enemies[randomIndex];

        GameObject spawnedEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        UnityEngine.AI.NavMeshAgent agent = spawnedEnemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.Warp(transform.position); // Forces the agent onto the NavMesh
        }

        EnemyController enemyController = spawnedEnemy.GetComponent<EnemyController>();
        if (enemyController != null && spawner != null)
            spawner.RegisterEnemy(enemyController);

        //Effect if there is one
        //if (spawnEffect != null)
        //{
        //    Instantiate(spawnEffect, transform.position, Quaternion.identity);
        //}

        HandleEffect();
        Destroy(gameObject);
    }

    private void HandleEffect()
    {
        if (spawnEffect == null) return;

        if (sharedEffectInstance == null)
        {
            sharedEffectInstance = Instantiate(spawnEffect);
            sharedEffectInstance.SetActive(false);
        }

        sharedEffectInstance.transform.position = transform.position;
        sharedEffectInstance.SetActive(true);

        ParticleSystem ps = sharedEffectInstance.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Clear();
            ps.Play();
        }
    }
        
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            hasSpawned = true;
            Debug.Log("ENEMY SPAWN!");
            SpawnEnemy();
        }
    }
}
