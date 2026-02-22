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

    override public void Initialize(BaseActiveData activeData, float damageAmount)
    {

        isActive = false;
        referenceData = activeData;
        spawnerType = activeData.dataType;
        projectileDamage = damageAmount;

        bossActive = (BossActiveData)activeData;

        rb.isKinematic = false;

    }


    void Update()
    {
        if (isActive)
        {
            float velocity = disintergateSpeed * Time.deltaTime;
            transform.position += -Vector3.up * velocity;
        }

        Vector3 raycastPosition = transform.position + raycastOffset;
        Debug.DrawLine(raycastPosition, raycastPosition + -Vector3.up * raycastDistance, Color.red);

        bool hittingFloor = Physics.Raycast(raycastPosition, -Vector3.up, out RaycastHit hit, raycastDistance, raycastTargetLayer);

        if (isActive)
        {
            float velocity = disintergateSpeed * Time.deltaTime;
            transform.position += -Vector3.up * velocity;

            if (!hittingFloor)
                ReturnToPool();
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
            rb.isKinematic = true;

            isActive = true;
            //spawn particles
            bossActive.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_FRUITSPLASH;
            bossActive.objectPoolSpawnData = new ObjectPoolSpawnData(hitPoint, Vector3.up);
            bossActive.isObjectPoolTriggered = true;
        }
    }
}
