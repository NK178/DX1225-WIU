using UnityEngine;

[CreateAssetMenu(fileName = "KnifeChopAttack", menuName = "Bossing/KnifeChopAttack")]
public class KnifeChopAttack : BossAttacks
{

    [SerializeField] private float attackDelayMin;
    [SerializeField] private float attackDelayMax;

    private float attackDelayTime;
    private float timer;
    private bool shouldAttack = false;
    private bool hasAttacked = false;

    GameObject player = null;

    public override void ExecuteAttack(BossActiveData activeData)
    {

        //AttackHandler atkHandler = GameObject.FindWithTag("Boss").GetComponent<AttackHandler>();

        //if (atkHandler != null)
        //{
        //    atkHandler.EnableCollider("FlySwatterCollider");
        //}

        player = GameObject.FindWithTag("Player");

        attackDelayTime = Random.Range(attackDelayMin, attackDelayMax);
        timer = 0f;
        shouldAttack = hasAttacked = false;


        activeDuration = defaultDuration + attackDelayTime;

    }

    //This fuction is useless 
    public override void UpdateAttack(BossActiveData activeData)
    {

        timer += Time.deltaTime;

        if (timer > attackDelayTime)
        {
            shouldAttack = true;
        }

        if (shouldAttack && !hasAttacked)
        {
            hasAttacked = true;
            float zOffset = 10f;

            Vector3 referencePosition = new Vector3(player.transform.position.x,
                                                    22.2f,
                                                    player.transform.position.z);

            Vector3 knifeHitPosition = referencePosition + Vector3.right * zOffset;

            activeData.activeIKType = BossActiveData.IKTYPE.IK_KNIFE;
            activeData.knifeHitPosition = knifeHitPosition; 
            activeData.BAnimState = _attack;
            activeData.isAttacking = true;

        }

    }
}


