using UnityEngine;

[CreateAssetMenu(fileName = "HandSlamAttack", menuName = "Bossing/HandSlamAttack")]
public class HandSlamAttack : BossAttacks
{
    public override void ExecuteAttack(BossActiveData activeData)
    {
        Debug.Log("Hand Slam");


    }
}
