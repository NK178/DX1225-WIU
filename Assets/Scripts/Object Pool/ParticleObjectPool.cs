using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ParticleObjectPool : MonoBehaviour, IObjectPool
{
    [SerializeField] private int defaultCapacity;
    [SerializeField] private int maxCapacity;
    [SerializeField] private float particleLifetime;

    public GameObject particlePrefab;
    private ObjectPool<GameObject> particlePool;

    void Start()
    {
        particlePool = new ObjectPool<GameObject>(
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
        GameObject obj = Instantiate(particlePrefab);
        obj.SetActive(false);
        return obj;
    }

    private void OnTakeFromPool(GameObject effect) => effect.SetActive(true);
    private void OnReturnToPool(GameObject effect) => effect.SetActive(false);
    private void OnDestroyEffect(GameObject effect) => Destroy(effect);

    public void ReleaseObject(GameObject obj)
    {
        particlePool.Release(obj);
    }

    public GameObject Get()
    {
        return particlePool.Get();
    }

    public void SpawnImpactEffect(Vector3 position, Vector3 normal)
    {
        GameObject effect = particlePool.Get();
        effect.transform.position = position;
        effect.transform.rotation = Quaternion.LookRotation(normal);

        // Triggers the pool to take it back after 3 seconds
        StartCoroutine(ReleaseImpactEffect(effect, particleLifetime));
    }


    //This one to get reference pro strats 
    public void SpawnImpactEffect(Vector3 position, Vector3 normal, ref GameObject reference)
    {
        GameObject effect = particlePool.Get();
        effect.transform.position = position;
        effect.transform.rotation = Quaternion.LookRotation(normal);
        reference = effect; 
        // Triggers the pool to take it back after 3 seconds
        StartCoroutine(ReleaseImpactEffect(effect, particleLifetime));
    }


    private IEnumerator ReleaseImpactEffect(GameObject objectRef, float delay)
    {
        yield return new WaitForSeconds(delay);
        // Only release if it's still active to prevent double-release errors
        if (objectRef.activeSelf)
        {
            particlePool.Release(objectRef);
        }
    }
}