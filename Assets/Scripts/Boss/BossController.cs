using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Klaus Phase 1: Mechanical Knife Attack & Hand Swipe Attack
// Ainsley Phase 2: Hand Slam, Fly Swatter Attack, Claw Grab, Sugarcane Missiles and Fruit Air Strike

[System.Serializable]
struct AttackPhaseData
{
    // Phase 1 2 3
    // Attack Scriptable Object
    public int phaseNo;
    public List<BossAttacks> _atks;
    //public BossAttacks _atks;
}

public class BossController : MonoBehaviour
{
    [SerializeField] private DataHolder dataHolder;
    [SerializeField] private BossAnimator animator;

    [SerializeField] private List<AttackPhaseData> attackPhaseData;
    private BossActiveData activeData;

    [Header("Debugging")]
    [SerializeField] private BossAttacks DEBUGAttackData;
    [SerializeField] private AttackHandler DEBUGAttackHandler;

    public float HP;
    public float ATK;


    public bool debugRunning = false;


    private void Start()
    {
        if (dataHolder.activeData == null)
        {
            Debug.LogError("NO ACTIVE DATA HELD");
            return;
        }

        activeData = (BossActiveData)dataHolder.activeData;

        if (activeData == null)
        {
            Debug.LogError("NO BOSS ACTIVE DATA FOUND");
            return;
        }

        //Set to idle 
        activeData.BAnimState = BossActiveData.BossAnimStates.IDLE;
        activeData.isMoving = false;


        //Debug 
        DEBUGAttackData.ExecuteAttack(activeData);
        debugRunning = true;
    }

    private void Update()
    {
        //for (int i = 0; i < attackPhaseData[0]._atks.Count; i++)
        //{
        //    //Debug.Log(attackPhaseData[0]._atks[i]);
        //}

        if (debugRunning) {

            DEBUGAttackData.UpdateAttack(activeData);
        }
    }


    public void HandleMove()
    {

        //Debug.Log((BossActiveData.BossAnimStates)animator.GetAnimState()); // Check what Anim it is at
    }

    public void HandleAttack()
    {
        
    }


    public void HandleTriggerParticles(Vector3 hitPoint)
    {
        switch (activeData.BAnimState)
        {
            case BossActiveData.BossAnimStates.FLYSWATTER_ATTACK:
                activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_ELECTRICSPARK;
                activeData.objectPoolSpawnData = new ObjectPoolSpawnData(hitPoint, Vector3.up);
                activeData.isObjectPoolTriggered = true;
                ObjectPoolManager.Instance.HandleSpawnRequest(activeData);
                break;
            case BossActiveData.BossAnimStates.HANDSLAM_ATTACK:
                Debug.Log("HAND SLAM ATTACK");
                activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_WOODSPLINTER;
                activeData.objectPoolSpawnData = new ObjectPoolSpawnData(hitPoint, Vector3.up);
                activeData.isObjectPoolTriggered = true;
                activeData.isObjectPoolTriggered = false;

                activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_DUSTSPLASH;
                activeData.objectPoolSpawnData = new ObjectPoolSpawnData(hitPoint, Vector3.up);
                activeData.isObjectPoolTriggered = true;
                ObjectPoolManager.Instance.HandleSpawnRequest(activeData);
                break;

        }
    }

}
