using UnityEngine;

[CreateAssetMenu(fileName = "RollingPinAttack", menuName = "Bossing/RollingPinAttack")]
public class RollingPinAttack : BossAttacks
{

    [SerializeField] private float attackDelayMin;
    [SerializeField] private float attackDelayMax;

    private float attackDelayTime;
    private float timer;
    private bool shouldAttack = false;

    public override void ExecuteAttack(BossActiveData activeData)
    {
        attackDelayTime = Random.Range(attackDelayMin, attackDelayMax);
        timer = 0f;
        shouldAttack = false;
        activeData.BAnimState = _attack;
        activeData.isAttacking = true;
    }

    //This fuction is useless 
    public override void UpdateAttack(BossActiveData activeData)
    {

        timer += Time.deltaTime;

        if (timer > attackDelayTime)
        {
            shouldAttack = true;
        }

        if (shouldAttack)
        {
            Debug.Log("ROLLING PIN");



            activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.FRUIT_CHUNKS;
            //activeData.objectPoolSpawnData = new ObjectPoolSpawnData(spawnPoint, Vector3.up, fireForce, 0);
            activeData.isObjectPoolTriggered = true;

            ObjectPoolManager.Instance.HandleSpawnRequest(activeData);

            shouldAttack = false;
        }

    }


}

