using UnityEngine;

[CreateAssetMenu(fileName = "FruitRainAttack", menuName = "Bossing/FruitRainAttack")]
public class FruitRainAttack : BossAttacks
{

    [SerializeField] private float attackDuration;
    [SerializeField] private Vector3 spawnPoint;

    [SerializeField] private float spawnDelayMin;
    [SerializeField] private float spawnDelayMax;

    [SerializeField] private int fireCountMin;
    [SerializeField] private int fireCountMax;

    [SerializeField] private float fireSpread;

    [SerializeField] private float flightTime = 1.5f;

    private float timer = 0f;
    private bool canFire = false;
    private float randomSpawnDelay;

    private GameObject playerRef;

    public override void ExecuteAttack(BossActiveData activeData)
    {
        Debug.Log("Fruit Attack");

        timer = 0f;
        canFire = false;
        randomSpawnDelay = Random.Range(spawnDelayMin, spawnDelayMax);


        playerRef = GameObject.FindWithTag("Player");

        debugTest = true;
    }
    

    private bool debugTest = true;

    public override void UpdateAttack(BossActiveData activeData)
    {
        Debug.Log("Fruit Attack Loop");


        timer += Time.deltaTime;    

        if (!canFire && timer > randomSpawnDelay)
        {
            canFire = true;
        }


        if (canFire && debugTest)
        {
            timer = 0;
            canFire = false;
            //debugTest = false;

            randomSpawnDelay = Random.Range(spawnDelayMin, spawnDelayMax);

            //Since its exculsive 
            int fireCount = Random.Range(fireCountMin, fireCountMax + 1);

            for (int i = 0; i < fireCount; i++)
            {

                float xSpread = Random.Range(-fireSpread, fireSpread);
                float zSpread = Random.Range(-fireSpread, fireSpread);

                Vector3 targetPosition = new Vector3(
                                        playerRef.transform.position.x + xSpread,
                                        playerRef.transform.position.y,
                                        playerRef.transform.position.z + zSpread
                                        );

                Vector3 fireForce = CalculateForce(targetPosition);

                activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.FRUIT_CHUNKS;

                activeData.objectPoolSpawnData = new ObjectPoolSpawnData(spawnPoint, Vector3.up, fireForce, 0);
                activeData.isObjectPoolTriggered = true;
                activeData.isObjectPoolTriggered = false;
            }
        }


        //if (canFire && debugTest)
        //{
        //    timer = 0;
        //    canFire = false;
        //    //debugTest = false;

        //    //randomSpawnDelay = -1;
        //    randomSpawnDelay = Random.Range(spawnDelayMin, spawnDelayMax);

        //    Vector3 fireForce = CalculateForce(playerRef.transform.position);

        //    activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.FRUIT_CHUNKS;


        //    Debug.Log("FIRE FORCE: " + fireForce);
        //    //activeData.objectPoolSpawnData = new ObjectPoolSpawnData(spawnPoint, fireForce.normalized, 0, fireForce.magnitude);

        //    activeData.objectPoolSpawnData = new ObjectPoolSpawnData(spawnPoint, Vector3.up, fireForce, 0);

        //    activeData.isObjectPoolTriggered = true;
        //}



    }


    public Vector3 CalculateForce(Vector3 targetPos)
    {
        Vector3 directionToPlayer = (targetPos - spawnPoint);
        Vector3 normalizeDirection = directionToPlayer.normalized;
        float distanceFromPlayer = directionToPlayer.magnitude;

        //float flightTime = 1.5f;
        float horizontalSpeed = distanceFromPlayer / flightTime;

        //vy = (deltaY - 0.5*g*t^2) / t
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




