using UnityEngine;

public class OrbProjectile : MonoBehaviour
{

    [Header("Enemy Settings")]
    [SerializeField] private GameObject[] enemies;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnEnemy()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            Debug.Log("ENEMY SPAWN!");
            SpawnEnemy();
        }
    }
}
