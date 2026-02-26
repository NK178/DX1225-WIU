using UnityEngine;

// Klaus & Ainsley

public class BossActiveData : BaseActiveData
{
    public enum BossAnimStates
    {
        IDLE = 0,
        KNIFE_ATTACK,
        HANDSWIPE_ATTACK,
        TRIPLEKNIFE_ATTACK,
        HANDSLAM_ATTACK,
        FLYSWATTER_ATTACK,
        CLAWGRAB_ATTACK,
        GRABBING_SUGARCANE,
        SUGARCANEMISSILES_ATTACK,
        ROLLINGPIN_ATTACK,
        FRUITRAIN_ATTACK,
        FRUITAIRSTRICK_ATTACK,
        THROWING_SUGARCANE_ATTACK,
    }

    public enum IKTYPE
    {
        IK_KNIFE = 0, 
        IK_HANDSLAMS
    }

    //public bool isAttacking;

    public Vector3 knifeHitPosition;
    public Vector3 leftHitPosition;
    public Vector3 rightHitPosition;

    public bool isBossActive = false;

    public IKTYPE activeIKType = IKTYPE.IK_KNIFE;
    public BossAnimStates BAnimState;
    public int BossPhase;
    public BossActiveData()
    {
        isBossActive = false;
        isMoving = true;
        isAttacking = false;
        BossPhase = 0;
        Debug.Log("Boss Initalizing");
        BAnimState = BossAnimStates.IDLE;
        dataType = DataHolder.DATATYPE.BOSS_ENEMY;
        activeIKType = IKTYPE.IK_KNIFE;
        knifeHitPosition = Vector3.zero;
    }
}
