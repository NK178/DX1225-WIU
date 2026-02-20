using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "FruitRainAttack", menuName = "Bossing/FruitRainAttack")]
public class FruitRainAttack : BossAttacks
{

    [SerializeField] private float attackDuration;
    [SerializeField] private Vector3 spawnPoint;

    [SerializeField] private float spawnDelayMax;
    [SerializeField] private float spawnDelayMin;


    [SerializeField] private float launchForce = 50f;

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

            //randomSpawnDelay = -1;
            randomSpawnDelay = Random.Range(spawnDelayMin, spawnDelayMax);

            Vector3 fireForce = CalculateForce(playerRef.transform.position);

            activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.FRUIT_CHUNKS;


            Debug.Log("FIRE FORCE: " + fireForce);
            //activeData.objectPoolSpawnData = new ObjectPoolSpawnData(spawnPoint, fireForce.normalized, 0, fireForce.magnitude);

            activeData.objectPoolSpawnData = new ObjectPoolSpawnData(spawnPoint, Vector3.up, fireForce, 0);

            activeData.isObjectPoolTriggered = true;
        }


        //GameObject playerRef = GameObject.FindWithTag("Player");

        //if (playerRef != null)
        //{
        //    activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.FRUIT_CHUNCKS;
        //    activeData.objectPoolSpawnData = new ObjectPoolSpawnData(spawnPoint, Vector3.up);
        //    activeData.isObjectPoolTriggered = true;
        //}



    }


    public Vector3 CalculateForce(Vector3 targetPos)
    {
        Vector3 directionToPlayer = (targetPos - spawnPoint);
        Vector3 normalizeDirection = directionToPlayer.normalized;
        float distanceFromPlayer = directionToPlayer.magnitude;

        float flightTime = 1.5f;
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




