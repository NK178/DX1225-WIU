using UnityEngine;

public class RollingPinProjectile : GenericProjectile
{

    [SerializeField] private float rollingPinMoveSpeed;
    [SerializeField] private float rollingPinRotateSpeed;
    [SerializeField] private GameObject model;

    private float modelRotX;
    private float modelRotY;
    private float modelRotZ;

    void Start()
    {
        modelRotX = model.transform.localEulerAngles.x;
        modelRotY = model.transform.localEulerAngles.y;
        modelRotZ = model.transform.localEulerAngles.z;
    }

    void Update()
    {
        transform.position += -transform.up * rollingPinMoveSpeed * Time.deltaTime;

        modelRotX += rollingPinRotateSpeed * Time.deltaTime;
        model.transform.localRotation = Quaternion.Euler(modelRotX, modelRotY, modelRotZ);
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

        if (hitPlayer)
        {
            //IMpluse
            //bossActive.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_SUGARCANESPLASH;
            //bossActive.objectPoolSpawnData = new ObjectPoolSpawnData(hitPoint, Vector3.up);
            //bossActive.isObjectPoolTriggered = true;

            CineMachineImpulseMan.Instance.GenerateEffect(EFFECT.CAMSHAKE); 
            ReturnToPool();
        }

        //if (spawnerType == DataHolder.DATATYPE.BOSS_ENEMY && hitPlayer)
        //{
        //    Debug.Log($"Hit player for {projectileDamage} damage!");
        //    ReturnToPool();
        //}


        //if (hitEnvironment)
        //{
        //    //spawn particles
        //    bossActive.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_SUGARCANESPLASH;
        //    bossActive.objectPoolSpawnData = new ObjectPoolSpawnData(hitPoint, Vector3.up);
        //    bossActive.isObjectPoolTriggered = true;
        //    ReturnToPool();
        //}
    }
}
