using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Klaus Phase 1: Mechanical Knife Attack & Hand Swipe Attack

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


    [SerializeField] private BossAttacks DEBUGattackData; 

    public float HP;
    public float ATK;

    private void Start()
    {
        if (dataHolder.activeData == null)
        {
            Debug.Log("NO ACTIVE DATA HELD");
            return;
        }

        activeData = (BossActiveData)dataHolder.activeData;

        if (activeData == null)
        {
            Debug.Log("NO BOSS ACTIVE DATA FOUND");
            return;
        }


        StartCoroutine(TestAttackFunction());
    }


    //FOR MY TESTING - AINS: 
    public IEnumerator TestAttackFunction()
    {
        yield return new WaitForSeconds(2f);

        DEBUGattackData.ExecuteAttack(activeData);
    }


    public void HandleMove()
    {

        //Debug.Log((BossActiveData.BossAnimStates)animator.GetAnimState()); // Check what Anim it is at
    }

    public void HandleAttack()
    {

    }



}
