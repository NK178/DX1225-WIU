using UnityEngine;

[CreateAssetMenu(fileName = "FlySwatterAttack", menuName = "Bossing/FlySwatterAttack")]
public class FlySwatterAttack : BossAttacks
{

    [SerializeField] private float attackDelayMin;
    [SerializeField] private float attackDelayMax;

    private float attackDelayTime;
    private float timer;
    private bool shouldAttack = false;
    private bool hasAttacked = false;

    public override void ExecuteAttack(BossActiveData activeData)
    {
        attackDelayTime = Random.Range(attackDelayMin, attackDelayMax);
        timer = 0f;
        shouldAttack = hasAttacked = false;
    }

    public override void UpdateAttack(BossActiveData activeData)
    {

        timer += Time.deltaTime;

        if (timer > attackDelayTime)
        {
            shouldAttack = true;
        }

        if (shouldAttack && !hasAttacked)
        {
            Debug.Log("FLY SWAT ATTACK");
            hasAttacked = true;



        }

    }

}
