using UnityEngine;

// 

abstract public class BAttacks : ScriptableObject
{
    abstract public void ExecuteAttack(BossActiveData activeData);

    abstract public void UpdateAttack(BossActiveData activeData);

}
