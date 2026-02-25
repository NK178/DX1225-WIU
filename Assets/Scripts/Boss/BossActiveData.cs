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

    //public bool isAttacking;

    public BossAnimStates BAnimState;
    public int BossPhase;
    public BossActiveData()
    {
        isMoving = true;
        isAttacking = false;
        BossPhase = 0;
        Debug.Log("Boss Initalizing");
        BAnimState = BossAnimStates.IDLE;
        dataType = DataHolder.DATATYPE.BOSS_ENEMY;
    }
}
