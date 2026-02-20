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

    public override void ExecuteAttack(BossActiveData activeData)
    {
        Debug.Log("Fruit Attack");

        timer = 0f;
        canFire = false;
        randomSpawnDelay = Random.Range(spawnDelayMin, spawnDelayMax);
    }

    public override void UpdateAttack(BossActiveData activeData)
    {
        Debug.Log("Fruit Attack Loop");


        timer += Time.deltaTime;    

        if (!canFire && timer > randomSpawnDelay)
        {
            canFire = true;
        }

        if (canFire)
        {
            timer = 0;
            canFire = false;
            randomSpawnDelay = Random.Range(spawnDelayMin, spawnDelayMax);

            activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.FRUIT_CHUNKS;
            activeData.objectPoolSpawnData = new ObjectPoolSpawnData(spawnPoint, Vector3.up, launchForce);
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


}

