using UnityEngine;


public class FruitChunksProjectile : GenericProjectile
{

    [SerializeField] private float disintergateSpeed = 2f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider thisCollider;
    [SerializeField] private LayerMask ignoreLayerAfterCollision; 
    [SerializeField] private LayerMask raycastTargetLayer;

    [SerializeField] private float raycastDistance;
    [SerializeField] private Vector3 raycastOffset;

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

        //StopAllCoroutines();
        //StartCoroutine(ActivateProjectileCoroutine());

    }


    override public void Initialize(BaseActiveData activeData, float damageAmount)
    {

        isActive = false;
        referenceData = activeData;
        spawnerType = activeData.dataType;
        projectileDamage = damageAmount;

        bossActive = (BossActiveData)activeData;

    }


    void Update()
    {

        //Vector3 raycastPosition = transform.position + raycastOffset;
        //Debug.DrawLine(raycastPosition, raycastPosition + Vector3.up * raycastDistance, Color.red);
        //if (Physics.Raycast(raycastPosition, Vector3.up, out RaycastHit hit, raycastDistance, raycastTargetLayer))
        //{
        //    Debug.Log("HIT NAME: " + hit.collider.gameObject.name);
        //}
        //else if (isActive)
        //{
        //    ReturnToPool();
        //}


        //BROKEN 
        Vector3 raycastPosition = transform.position + raycastOffset;
        Debug.DrawLine(raycastPosition, raycastPosition + Vector3.up * raycastDistance, Color.red);

        bool hittingFloor = Physics.Raycast(raycastPosition, Vector3.up, out RaycastHit hit, raycastDistance);


        if (hittingFloor)
            Debug.Log(hit.collider?.gameObject.name + " layer: " + hit.collider?.gameObject.layer);

        if (isActive && !hittingFloor)
        {
            ReturnToPool();
        }


        ////check if have succesfully exited ground 
        //if (isActive)
        //{
        //    //Vector3 hitPoint = transform.position;
        //    //Vector3 hitNormal = Vector3.up;

        //    Vector3 raycastPosition = transform.position + raycastOffset;
        //    Debug.DrawLine(raycastPosition, raycastPosition + Vector3.up * raycastDistance, Color.red);
        //    if (Physics.Raycast(raycastPosition, Vector3.up, out RaycastHit hit, raycastDistance, raycastIgnoreLayer))
        //    {
        //        //hitPoint = hit.point;
        //        //hitNormal = hit.normal;
        //    }
        //    else
        //    {
        //        //Debug.Log("HIT NAME: " + hit.collider.gameObject.name);
        //        ReturnToPool();
        //    }


        //    //if (hit.collider != null)
        //    //{

        //    //}

        //    //if (hit.collider != null && hit.collider.gameObject.CompareTag("Environment"))
        //    //{
        //    //    ReturnToPool();
        //    //}
        //    ////float velocity = disintergateSpeed * Time.deltaTime;
        //    ////transform.position += -Vector3.up * velocity;
        //    //Vector3 newPosition = transform.position + -Vector3.up * disintergateSpeed * Time.deltaTime;
        //    //rb.MovePosition(newPosition);
        //}

    }



    void FixedUpdate()
    {
        if (isActive)
        {
            rb.MovePosition(rb.position + Vector3.down * disintergateSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        bool hitPlayer = collision.gameObject.CompareTag("Player");
        bool hitEnvironment = collision.gameObject.CompareTag("Environment");

        Vector3 hitPoint = transform.position;
        Vector3 hitNormal = Vector3.up;
        if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit, 1.5f))
        {
            hitPoint = hit.point;
            hitNormal = hit.normal;
        }

        if (spawnerType == DataHolder.DATATYPE.BOSS_ENEMY && hitPlayer)
        {
            Debug.Log($"Hit player for {projectileDamage} damage!");
        }

        if (hitEnvironment)
        {
            rb.linearVelocity = Vector3.zero;
            rb.useGravity = false;
            rb.freezeRotation = true;
            thisCollider.excludeLayers = ignoreLayerAfterCollision;

            isActive = true;
            //spawn particles
            bossActive.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_FRUITSPLASH;
            bossActive.objectPoolSpawnData = new ObjectPoolSpawnData(hitPoint, Vector3.up);
            bossActive.isObjectPoolTriggered = true;
        }
    }

    //private void OnCollisionExit(Collision collision)
    //{
    //    bool hitEnvironment = collision.gameObject.CompareTag("Environment");

    //    if (hitEnvironment)
    //    {
    //        isActive = false;
    //        ReturnToPool();
    //    }
    //}



    //private void OnTriggerEnter(Collider other)
    //{
    //    //bool hitEnemy = other.CompareTag("Enemy");
    //    bool hitEnvironment = other.CompareTag("Environment");
    //    bool hitPlayer = other.CompareTag("Player");

    //    Vector3 hitPoint = transform.position;
    //    Vector3 hitNormal = Vector3.up;
    //    if (Physics.Raycast(transform.position, -transform.forward, out RaycastHit hit, 3f))
    //    {
    //        hitPoint = hit.point;
    //        hitNormal = hit.normal;
    //    }

    //    if (spawnerType == DataHolder.DATATYPE.BOSS_ENEMY && hitPlayer)
    //    {
    //        Debug.Log($"Hit player for {projectileDamage} damage!");
    //        ReturnToPool();
    //    }


    //    //if (hitEnvironment)
    //    //{
    //    //    //spawn particles
    //    //    bossActive.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_SUGARCANESPLASH;
    //    //    bossActive.objectPoolSpawnData = new ObjectPoolSpawnData(hitPoint, Vector3.up);
    //    //    bossActive.isObjectPoolTriggered = true;
    //    //    ReturnToPool();
    //    //}
    //}
}
