using UnityEngine;

[CreateAssetMenu(fileName = "RollingPinAttack", menuName = "Bossing/RollingPinAttack")]
public class RollingPinAttack : BossAttacks
{

    [SerializeField] private float attackDelayMin;
    [SerializeField] private float attackDelayMax;

    private float attackDelayTime;
    private float timer;
    private bool shouldAttack = false;
    private bool hasAttack = false;

    public override void ExecuteAttack(BossActiveData activeData)
    {
        attackDelayTime = Random.Range(attackDelayMin, attackDelayMax);
        timer = 0f;
        shouldAttack = false;
        hasAttack = false;
        activeData.BAnimState = _attack;
        activeData.isAttacking = true;
    }

    //This fuction is useless 
    public override void UpdateAttack(BossActiveData activeData)
    {
        Debug.Log("ROLLING PIN UPDATE ATTACK: " + timer);


        timer += Time.deltaTime;

        if (timer > attackDelayTime)    
        {
            shouldAttack = true;
        }

        if (shouldAttack && !hasAttack)
        {
            Debug.Log("ROLLING PIN");



            activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.ROLLINGPIN;
            //calc the position 

            Vector3 spawnPoint = Vector3.zero;

            activeData.objectPoolSpawnData = new ObjectPoolSpawnData(spawnPoint, Vector3.up, Vector3.zero, 0);
            activeData.isObjectPoolTriggered = true;

            ObjectPoolManager.Instance.HandleSpawnRequest(activeData);

            hasAttack = true;
        }

    }


}

