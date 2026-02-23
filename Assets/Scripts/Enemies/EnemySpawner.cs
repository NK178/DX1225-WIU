using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{

    [Header("Spawner Settings")]
    [SerializeField] private GameObject spawnerOrb;
    [SerializeField] private GameObject spawner;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float spawnRadius = 15f;

    [SerializeField] private float minSpawnTime = 2f;
    [SerializeField] private float maxSpawnTime = 7f;
    [SerializeField] private int currentSpawnCount;    

    [Header("Sine Wave Settings")]
    [SerializeField] private float amplitude = 0.5f;
    [SerializeField] private float frequency = 2f;
    [SerializeField] private bool useLocalSpace = true;

    [Header("Projectile Settings")]
    [SerializeField] private float flightTime = 1.5f;

    private Vector3 startPos;
    private int maxSpawnCount = 7;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (spawner != null)
        {
            startPos = useLocalSpace ? spawner.transform.localPosition : spawner.transform.position;
        }

        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (spawner != null)
            ApplySineWave();
    }

    private void ApplySineWave()
    {
        float newY = Mathf.Sin(Time.time * frequency) * amplitude;

        if (useLocalSpace)
            spawner.transform.localPosition = startPos + new Vector3(0, newY, 0);
        else
            spawner.transform.position = startPos + new Vector3(0, newY, 0);
    }

    private IEnumerator SpawnRoutine()
    {
        while (currentSpawnCount < maxSpawnCount)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);
            LaunchOrb();
        }
    }
    private void LaunchOrb()
    {
        if (spawnerOrb == null || firePoint == null) return;

        Vector3 targetPos;
        if (GetRandomNavMeshPoint(out targetPos))
        {
            GameObject orb = Instantiate(spawnerOrb, firePoint.position, Quaternion.identity);
            Rigidbody orbRb = orb.GetComponent<Rigidbody>();

            if (orbRb != null)
            {
                Vector3 force = CalculateForce(targetPos);
                orbRb.AddForce(force, ForceMode.VelocityChange);
            }

            currentSpawnCount++;
        }
    }

    private bool GetRandomNavMeshPoint(out Vector3 result)
    {
        Vector3 center = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 randomPoint = center + Random.insideUnitSphere * spawnRadius;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 5f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private Vector3 CalculateForce(Vector3 targetPos)
    {
        Vector3 spawnPoint = firePoint.position;
        Vector3 directionToPlayer = (targetPos - spawnPoint);
        Vector3 normalizeDirection = directionToPlayer.normalized;
        float distanceFromPlayer = directionToPlayer.magnitude;

        //float flightTime = 1.5f;
        float horizontalSpeed = distanceFromPlayer / flightTime;

        //vy = (deltaY - 0.5gt^2) / t
        float deltaY = targetPos.y - spawnPoint.y;

        float gravity = Mathf.Abs(Physics.gravity.y);
        float verticalSpeed = (deltaY + 0.5f * gravity * flightTime * flightTime) / flightTime;

        //float verticalSpeed = (deltaY - 0.5f * 1000f * flightTime * flightTime) / flightTime;

        Vector3 firingForce = new Vector3(
            normalizeDirection.x * horizontalSpeed,
            verticalSpeed,
            normalizeDirection.z * horizontalSpeed
        );

        return firingForce;
    }
}
