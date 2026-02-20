using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileObjectPool : MonoBehaviour, IObjectPool
{
    [SerializeField] private int defaultCapacity;
    [SerializeField] private int maxCapacity;

    public GameObject projectilePrefab;
    private ObjectPool<GameObject> projectilePool;

    void Start()
    {
        projectilePool = new ObjectPool<GameObject>(
            CreatePooledItem,
            OnTakeFromPool,
            OnReturnToPool,
            OnDestroyEffect,
            true,
            defaultCapacity,
            maxCapacity
        );
    }

    private GameObject CreatePooledItem()
    {
        GameObject obj = Instantiate(projectilePrefab);
        GenericProjectile proj = obj.GetComponent<GenericProjectile>();
        if (proj != null)
        {
            proj.SetPool(this);
        }

        obj.SetActive(false);
        return obj;
    }

    private void OnTakeFromPool(GameObject effect) => effect.SetActive(true);
    private void OnReturnToPool(GameObject effect) => effect.SetActive(false);
    private void OnDestroyEffect(GameObject effect) => Destroy(effect);

    public void ReleaseObject(GameObject obj)
    {
        projectilePool.Release(obj);
    }

    public GameObject Get()
    {
        return projectilePool.Get();
    }
    public void SpawnImpactEffect(Vector3 position, Vector3 normal)
    {
        GameObject effect = projectilePool.Get();
        effect.transform.position = position;
        effect.transform.rotation = Quaternion.LookRotation(normal);

        // Triggers the pool to take it back after 3 seconds
        StartCoroutine(ReleaseImpactEffect(effect, 3f));
    }

    private IEnumerator ReleaseImpactEffect(GameObject objectRef, float delay)
    {
        yield return new WaitForSeconds(delay);
        // Only release if it's still active to prevent double-release errors
        if (objectRef.activeSelf)
        {
            projectilePool.Release(objectRef);
        }
    }

    // for seeds and rubber bands
    public void SpawnProjectile(Vector3 position, Vector3 forward, DataHolder.DATATYPE spawner, float damage, float launchForce)
    {
        GameObject obj = projectilePool.Get();
        obj.transform.position = position;
        obj.transform.rotation = Quaternion.LookRotation(forward);

        GenericProjectile proj = obj.GetComponent<GenericProjectile>();
        if (proj != null)
        {
            proj.Initialize(spawner, damage);
        }

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = true;
            rb.AddForce(forward * launchForce, ForceMode.Impulse);
        }
    }
}