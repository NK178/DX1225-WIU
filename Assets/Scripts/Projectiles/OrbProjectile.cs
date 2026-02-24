using UnityEngine;

public class OrbProjectile : MonoBehaviour
{

    [Header("Enemy Settings")]
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject spawnEffect;

    private bool hasSpawned = false;

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

        //Effect if there is one
        if (spawnEffect != null)
        {
            Instantiate(spawnEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
        
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            hasSpawned = true;
            Debug.Log("ENEMY SPAWN!");
            SpawnEnemy();
            Destroy(gameObject);
        }
    }
}
