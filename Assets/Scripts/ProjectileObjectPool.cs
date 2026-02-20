using System.Collections;
using UnityEngine;
using UnityEngine.Pool;


//THIS VERSION NO NEED DESPANW TIME 
public class ProjectileObjectPool : MonoBehaviour, IObjectPool
{
    [SerializeField] private int defaultCapacity;
    [SerializeField] private int maxCapacity;

    
    public GameObject projectilePrefab;

    private ObjectPool<GameObject> projectilePool;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        projectilePool = new ObjectPool<GameObject>(
        CreateImpactEffect,
        OnTakeFromPool,
        OnReturnToPool,
        OnDestroyEffect,
        true,
        defaultCapacity,
        maxCapacity
        );
    }
    // Update is called once per frame
    void Update()
    {

    }

    private GameObject CreateImpactEffect()
    {
        GameObject impactEffect = Instantiate(projectilePrefab);
        impactEffect.SetActive(false);
        return impactEffect;
    }


    private void OnTakeFromPool(GameObject effect)
    {
        effect.SetActive(true);
    }

    private void OnReturnToPool(GameObject effect)
    {
        effect.SetActive(false);
    }

    private void OnDestroyEffect(GameObject effect)
    {
        Destroy(effect);
    }


    public void SpawnImpactEffect(Vector3 position, Vector3 normal)
    {
        GameObject effect = projectilePool.Get();
        effect.transform.position = position;
        effect.transform.rotation = Quaternion.LookRotation(normal);
        //StartCoroutine(ReleaseImpactEffect(effect, 3));
    }

    private IEnumerator ReleaseImpactEffect(GameObject objectRef, float delay)
    {
        yield return new WaitForSeconds(delay);
        projectilePool.Release(objectRef);
    }

    public GameObject Get()
    {
        return this.gameObject;
    }

}

