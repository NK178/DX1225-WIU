using UnityEngine;

// 

abstract public class BAttacks : ScriptableObject
{
    abstract public void ExecuteAttack(BossActiveData activeData);
}
