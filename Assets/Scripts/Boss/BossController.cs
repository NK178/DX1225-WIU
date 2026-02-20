using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    [SerializeField] private BossAttacks DEBUGattackData;
    [SerializeField] private AttackHandler DEBUGAttackHandler;

    public float HP;
    public float ATK;


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


        //StartCoroutine(TestAttackFunction());
    }

    private void Update()
    {
        //for (int i = 0; i < attackPhaseData[0]._atks.Count; i++)
        //{
        //    //Debug.Log(attackPhaseData[0]._atks[i]);
        //}
    }


    //FOR MY TESTING - AINS: 
    public IEnumerator TestAttackFunction()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            DEBUGattackData.ExecuteAttack(activeData);
        }
    }


    public void HandleMove()
    {

        //Debug.Log((BossActiveData.BossAnimStates)animator.GetAnimState()); // Check what Anim it is at
    }

    public void HandleAttack()
    {
        
    }
}
