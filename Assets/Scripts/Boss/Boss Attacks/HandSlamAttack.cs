using UnityEngine;

[CreateAssetMenu(fileName = "HandSlamAttack", menuName = "Bossing/HandSlamAttack")]
public class HandSlamAttack : BossAttacks
{

    GameObject leftHand;
    GameObject rightHand; 

    public override void ExecuteAttack(BossActiveData activeData)
    {
        Debug.Log("Hand Slam");

        
    }

    public override void UpdateAttack(BossActiveData activeData)
    {
     
    }
}
