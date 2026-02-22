using UnityEngine;

[CreateAssetMenu(fileName = "BossAttacks", menuName = "Bossing/BossAttacks")]

public class BossAttacks : BAttacks
{
    [SerializeField] protected BossActiveData.BossAnimStates _attack;
    public override void ExecuteAttack(BossActiveData activeData)
    {
        activeData.BAnimState = _attack;
        activeData.isAttacking = true;
    }

    public override void UpdateAttack(BossActiveData activeData)
    {
        //activeData.BAnimState = _attack;
        //activeData.isAttacking = true;
    }
}
