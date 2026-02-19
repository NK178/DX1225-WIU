using UnityEngine;

public class BossActiveData : BaseActiveData
{
    public enum BossAnimStates
    {
        IDLE = 0,
        KNIFE_ATTACK,
        HANDSWIPE_ATTACK,
    }

    public BossAnimStates BAnimState;
    public bool isAttacking;
    public BossActiveData()
    {
        Debug.Log("Boss Initalizing");
        BAnimState = BossAnimStates.IDLE;
    }
}
