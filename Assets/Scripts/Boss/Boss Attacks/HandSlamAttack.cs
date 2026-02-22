using UnityEngine;

[CreateAssetMenu(fileName = "HandSlamAttack", menuName = "Bossing/HandSlamAttack")]
public class HandSlamAttack : BossAttacks
{
    private float particleDelay = 2f;
    private float timer = 0f;
    private bool playParticle1 = true; 
    private bool playParticle2 = true; 

    public override void ExecuteAttack(BossActiveData activeData)
    {


        //get the references    
        AttackHandler atkHandler = GameObject.FindWithTag("Boss").GetComponent<AttackHandler>();

        if (atkHandler != null)
        {
            Debug.Log("Hand Slam");
            atkHandler.EnableCollider("LeftHandCollider");
            atkHandler.EnableCollider("RightHandCollider");
        }


        timer = 0f;
        playParticle1 = true;
        playParticle2 = true;

    }

    public override void UpdateAttack(BossActiveData activeData)
    {
        //erm cant really do animaion rnow so rip but oh well

        //all for debug rnow 
        if (timer > particleDelay && playParticle1)
        {
            playParticle1 = false;
            timer = 0f;

            //spawn particles
            activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_WOODSPLINTER;
            activeData.objectPoolSpawnData = new ObjectPoolSpawnData(Vector3.zero, Vector3.up);
            activeData.isObjectPoolTriggered = true;
            activeData.isObjectPoolTriggered = false;

            activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_DUSTSPLASH;
            activeData.objectPoolSpawnData = new ObjectPoolSpawnData(Vector3.zero, Vector3.up);
            activeData.isObjectPoolTriggered = true;
        }

        if (timer > particleDelay && playParticle2)
        {
            playParticle2 = false;


            activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_DUSTSPLASH;
            activeData.objectPoolSpawnData = new ObjectPoolSpawnData(Vector3.zero, Vector3.up);
            activeData.isObjectPoolTriggered = true;

        }



        timer += Time.deltaTime;
    }


}
