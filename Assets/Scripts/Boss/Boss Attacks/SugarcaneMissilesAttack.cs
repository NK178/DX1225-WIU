using UnityEngine;

[CreateAssetMenu(fileName = "SugarcaneMissilesAttack", menuName = "Scriptable Objects/SugarcaneMissilesAttack")]
public class SugarcaneMissilesAttack : BossAttacks
{

    [SerializeField] private float debugStartHeight = 3f; 
    [SerializeField] private float spawnDelay = 2f;
    [SerializeField] private float ringRadius = 3f;

    GameObject playerRef;

    private float timer = 0f;
    public override void ExecuteAttack(BossActiveData activeData)
    {
        //Get reference to player position, bad method but it ok 
        playerRef = GameObject.FindWithTag("Player");

        if (playerRef == null)
        {
            Debug.Log("PLAYER NULL");
            return; 
        }

        activeData.BAnimState = _attack;
        activeData.isAttacking = true;

        timer = 0f;
    }

    public override void UpdateAttack(BossActiveData activeData)
    {
        if (timer > spawnDelay)
        {
            timer = 0f;
            float ringAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(ringAngle) * ringRadius, debugStartHeight, Mathf.Sin(ringAngle) * ringRadius);
            Vector3 spawnPos = playerRef.transform.position + offset;

            Vector3 directionToPlayer = (playerRef.transform.position - spawnPos).normalized;

            ////New way that will work with spawned enemies and what not 
            //activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.SUGARCANE_MISSILES;
            //activeData.objectPoolSpawnData = new ObjectPoolSpawnData(spawnPos, -directionToPlayer);
            //activeData.isObjectPoolTriggered = true;
            //ObjectPoolManager.Instance.HandleSpawnRequest(activeData);

            //Current way 
            activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.SUGARCANE_MISSILES;
            activeData.objectPoolSpawnData = new ObjectPoolSpawnData(spawnPos, -directionToPlayer);
            activeData.isObjectPoolTriggered = true;
        }

        timer += Time.deltaTime;
    }



}
