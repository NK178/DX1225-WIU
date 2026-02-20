using UnityEngine;

public class EnemyActiveData : BaseActiveData
{
    public ENEMYCLASSTYPE enemyClassType;

    //Enum for specific management
    public enum AIState { IDLE, WANDERING, CHASING, ATTACKING}
    public AIState currentState;

    //Wandering State
    public Vector3 wanderDestination;
    public float waitTimer;
    public float minWaitTime = 2f;
    public float maxWaitTime = 5f;
    public float wanderRadius = 10f;

    //Movement / Targeting State
    public Transform currentTarget;
    public Vector3 lastTargetPosition;
    public float distanceToTarget;

    //Combat State
    public bool canAttack;
    public float attackCooldownTimer;

    public EnemyActiveData() : base()
    {
        Debug.Log("ENEMY DATA INITIALIZED!");
        currentState = AIState.IDLE;
        waitTimer = 0f;
        canAttack = true;
        attackCooldownTimer = 0f;
    }
}
