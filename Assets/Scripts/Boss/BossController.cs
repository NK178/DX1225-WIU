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

        if (activeData ==  null)
        {
            Debug.Log("NO BOSS ACTIVE DATA FOUND");
            return;
        }
    }

    private void Update()
    {
        for (int i = 0; i < attackPhaseData[0]._atks.Count; i++)
        {
            Debug.Log(attackPhaseData[0]._atks[i]);
        }
    }

    public void HandleMove()
    {

        //Debug.Log((BossActiveData.BossAnimStates)animator.GetAnimState()); // Check what Anim it is at
    }

    public void HandleAttack(BossActiveData.BossAnimStates atk)
    {
        //switch (atk)
        //{
        //    case BossActiveData.BossAnimStates.KNIFE_ATTACK:
        //        attackColliders[0].SetActive(true);
        //        break;
        //    case BossActiveData.BossAnimStates.HANDSWIPE_ATTACK:
        //        attackColliders[0].SetActive(true);
        //        break;
        //    case BossActiveData.BossAnimStates.TRIPLEKNIFE_ATTACK:
        //        attackColliders[0].SetActive(true);
        //        break;
        //    case BossActiveData.BossAnimStates.HANDSLAM_ATTACK:
        //        attackColliders[0].SetActive(true);
        //        break;
        //    case BossActiveData.BossAnimStates.FLYSWATTER_ATTACK:
        //        attackColliders[0].SetActive(true);
        //        break;
        //    case BossActiveData.BossAnimStates.CLAWGRAB_ATTACK:
        //        attackColliders[0].SetActive(true);
        //        break;
        //    case BossActiveData.BossAnimStates.SUGARCANEMISSILES_ATTACK:
        //        attackColliders[0].SetActive(true);
        //        break;
        //    case BossActiveData.BossAnimStates.FRUITAIRSTRICK_ATTACK:
        //        attackColliders[0].SetActive(true);
        //        break;
        //}
    }
}
