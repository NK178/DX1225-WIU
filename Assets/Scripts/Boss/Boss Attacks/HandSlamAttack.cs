using UnityEngine;

[CreateAssetMenu(fileName = "HandSlamAttack", menuName = "Bossing/HandSlamAttack")]
public class HandSlamAttack : BossAttacks
{
    [SerializeField] private float attackDelayMin;
    [SerializeField] private float attackDelayMax;

    private float attackDelayTime;
    private float timer;
    private bool shouldAttack = false;
    private bool hasAttacked = false;

    GameObject player = null;

    public override void ExecuteAttack(BossActiveData activeData)
    {

        player = GameObject.FindWithTag("Player");

        attackDelayTime = Random.Range(attackDelayMin, attackDelayMax);
        timer = 0f;
        shouldAttack = hasAttacked = false;

        activeDuration = defaultDuration + attackDelayTime;
    }


    //This fuction is useless 
    public override void UpdateAttack(BossActiveData activeData)
    {

        timer += Time.deltaTime;

        if (timer > attackDelayTime)
        {
            shouldAttack = true;
        }

        if (shouldAttack && !hasAttacked)
        {
            hasAttacked = true;


            Vector3 referencePlayer = player.transform.position;

            activeData.activeIKType = BossActiveData.IKTYPE.IK_HANDSLAMS;
            activeData.leftHitPosition = referencePlayer;
            activeData.BAnimState = _attack;
            activeData.isAttacking = true;


            //float zOffset = 10f;

            //Vector3 knifeHitPosition = player.transform.position
            //                        + Vector3.right * zOffset;

            //activeData.knifeHitPosition = knifeHitPosition;
            //activeData.BAnimState = _attack;
            //activeData.isAttacking = true;

        }

    }

    //public override void ExecuteAttack(BossActiveData activeData)
    //{


    //    //get the references    
    //    //AttackHandler atkHandler = GameObject.FindWithTag("Boss").GetComponent<AttackHandler>();

    //    //if (atkHandler != null)
    //    //{
    //    //    Debug.Log("Hand Slam");
    //    //    atkHandler.EnableCollider("LeftHandCollider");
    //    //    atkHandler.EnableCollider("RightHandCollider");
    //    //}

    //    activeData.BAnimState = _attack;
    //    activeData.isAttacking = true;

    //    timer = 0f;
    //    playParticle1 = true;
    //    playParticle2 = true;

    //}

    //public override void UpdateAttack(BossActiveData activeData)
    //{
    //    //erm cant really do animaion rnow so rip but oh well

    //    //all for debug rnow 
    //    if (timer > particleDelay && playParticle1)
    //    {
    //        playParticle1 = false;

    //        AttackHandler atkHandler = GameObject.FindWithTag("Boss").GetComponent<AttackHandler>();

    //        if (atkHandler != null)
    //        {
    //            Debug.Log("Hand Slam");
    //            atkHandler.EnableCollider("LeftHandCollider");
    //            atkHandler.EnableCollider("RightHandCollider");
    //        }
    //    }

    //    //if (timer > particleDelay && playParticle2)
    //    //{
    //    //    playParticle2 = false;


    //    //    activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_DUSTSPLASH;
    //    //    activeData.objectPoolSpawnData = new ObjectPoolSpawnData(Vector3.zero, Vector3.up);
    //    //    activeData.isObjectPoolTriggered = true;

    //    //}


    //    timer += Time.deltaTime;
    //}


}
