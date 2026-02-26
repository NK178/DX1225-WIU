using System.Collections;
using UnityEngine;

public class SugarcaneProjectile : GenericProjectile
{


    //For now I will leave it like this , will take from the base class later 

    [SerializeField] private float delayTimeBeforeFiring = 2f;
    [SerializeField] private float projectileSpeed = 10f;

    private BossActiveData bossActive; 


    bool isActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    override public void Initialize(DataHolder.DATATYPE spawner, float damageAmount)
    {
        isActive = false;
        spawnerType = spawner;
        projectileDamage = damageAmount;

        StopAllCoroutines();
        StartCoroutine(ActivateProjectileCoroutine());

    }


    override public void Initialize(BaseActiveData activeData, float damageAmount)
    {

        isActive = false;
        referenceData = activeData;
        spawnerType = activeData.dataType;
        projectileDamage = damageAmount;

        bossActive = (BossActiveData)activeData;

        StopAllCoroutines();
        StartCoroutine(ActivateProjectileCoroutine());
    }


    // Update is called once per frame
    void Update()
    {

        if (isActive)
        {

            float velocity = projectileSpeed * Time.deltaTime;
            transform.position += -transform.forward * velocity; 
        }
        
    }


    private IEnumerator ActivateProjectileCoroutine()
    {
        yield return new WaitForSeconds(delayTimeBeforeFiring);
        isActive = true;
    }


    public void ActivateProjectile()
    {   
        isActive = true;

        //StartCoroutine(ActivateProjectileCoroutine());
    }


    private void OnTriggerEnter(Collider other)
    {
        //bool hitEnemy = other.CompareTag("Enemy");
        bool hitEnvironment = other.CompareTag("Environment");
        bool hitPlayer = other.CompareTag("Player");

        Vector3 hitPoint = transform.position;
        Vector3 hitNormal = Vector3.up;
        if (Physics.Raycast(transform.position, -transform.forward, out RaycastHit hit, 3f))
        {
            hitPoint = hit.point;
            hitNormal = hit.normal;
        }

        if (spawnerType == DataHolder.DATATYPE.BOSS_ENEMY && hitPlayer)
        {
            Debug.Log($"Hit player for {projectileDamage} damage!");

            CineMachineImpulseMan.Instance.GenerateEffect(EFFECT.EARTHQUAKESHAKE);
            ReturnToPool();
        }


        if (hitEnvironment)
        {
            CineMachineImpulseMan.Instance.GenerateEffect(EFFECT.CAMSHAKE);
            bossActive.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_SUGARCANESPLASH;
            bossActive.objectPoolSpawnData = new ObjectPoolSpawnData(hitPoint, Vector3.up);
            bossActive.isObjectPoolTriggered = true;
            ReturnToPool();
        }
    }
}
