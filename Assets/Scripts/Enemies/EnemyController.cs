using UnityEditor.Rendering;
using UnityEngine.AI;
using UnityEngine;

[System.Serializable]
public enum ENEMYCLASSTYPE
{
    MELEE,
    RANGED,
    NUM_TYPES
}

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Data")]
    [SerializeField] private DataHolder dataHolder;
    [SerializeField] private ENEMYCLASSTYPE enemyType;

    [SerializeField] private MeleeEnemyClassData knifeData;
    [SerializeField] private RangedEnemyClassData rangerData;

    private Vector3 currentVelocity = Vector3.zero;

    [Header("Wander Settings")]
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float deceleration = 8f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float arrivalRadius = 1.5f;

    [Header("Detection Settings")]
    [SerializeField] private LayerMask playerLayer;

    [Header("Combat Settings")]
    [SerializeField] private Transform firePoint;
    private float stopDistance;

    private EnemyActiveData activeData;
    private float detectionRadius;

    private void Awake()
    {
        activeData = (EnemyActiveData)dataHolder.activeData;

        if (activeData == null)
            return;

        activeData.enemyClassType = enemyType;
    }

    void Start()
    {
        //Casting it to access wanderDestination/currentState
        activeData = dataHolder.activeData as EnemyActiveData;

        if (activeData != null)
        {
            //Set the initial wander destination based on where the enemy was placed
            activeData.wanderDestination = transform.position;
            activeData.waitTimer = 2f;
            activeData.currentState = EnemyActiveData.AIState.WANDERING;
        }
    }

    void Update()
    {
        HandleMove();
        HandleAttack();
    }

    private void HandleMove()
    {
        activeData.enemyClassType = enemyType;

        if (activeData.enemyClassType == ENEMYCLASSTYPE.MELEE)
        {
            activeData.currentMoveSpeed = knifeData.moveSpeed;
            detectionRadius = knifeData.detectionRadius;
            stopDistance = knifeData.stopDistance;
        }
        else if (activeData.enemyClassType == ENEMYCLASSTYPE.RANGED)
        {
            activeData.currentMoveSpeed = rangerData.moveSpeed;
            detectionRadius = rangerData.detectionRadius;
            stopDistance = rangerData.stopDistance;
        }

        float moveSpeed = activeData.currentMoveSpeed;

        Collider[] player = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        if (player.Length > 0)
        {
            activeData.targetPlayer = player[0].transform;
            activeData.currentState = EnemyActiveData.AIState.CHASING;
        }
        else
        {
            //If player is out of range/gone return back to wandering 
            activeData.currentState = EnemyActiveData.AIState.WANDERING;
        }

        switch (activeData.currentState)
        {
            case EnemyActiveData.AIState.WANDERING:
                HandleWander(moveSpeed);
                break;
            case EnemyActiveData.AIState.CHASING:
                HandleChasing(moveSpeed, detectionRadius);
                break;
            default:
                break;
        }
    }

    #region Wander Functions

    private void HandleWander(float speed)
    {
        Vector3 toTarget = (activeData.wanderDestination - transform.position);
        toTarget.y = Vector3.zero.y;
        float distanceToTarget = toTarget.magnitude;

        if (distanceToTarget < arrivalRadius)
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);

            activeData.waitTimer -= Time.deltaTime;

            if (activeData.waitTimer <= 0)
            {
                PickNewWanderDestination();
                activeData.waitTimer = Random.Range(2f, 5f);
            }
        }
        else
        {
            Vector3 direction = toTarget.normalized;
            Vector3 targetVelocity = direction * speed;

            //Smoothly accelerate towards the target
            currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.deltaTime);

            //Smooth Rotation
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        //Apply movement once at the end
        transform.position += currentVelocity * Time.deltaTime;
    }

    private void PickNewWanderDestination()
    {
        //float minDistance = 2f;
        float maxDistance = 10f;

        #region Old Destination Checker
        //Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Vector3.zero.y, Random.Range(-1f, 1f));
        //float randomDistance = Random.Range(minDistance, maxDistance);
        //activeData.wanderDestination = transform.position + (randomDirection * randomDistance);
        #endregion

        //Generate the raw random point
        Vector3 randomDirection = Random.insideUnitSphere * maxDistance;
        randomDirection.y = 0; // Keep it on horizontal plane
        Vector3 sourcePosition = transform.position + randomDirection;

        //Find the closest valid point on the NavMesh
        NavMeshHit hit;

        //Searching within MaxDistance to find a valid navmesh Floor
        if (NavMesh.SamplePosition(sourcePosition, out hit, maxDistance, NavMesh.AllAreas))
        {
            activeData.wanderDestination = hit.position;
        }
        else
        {
            //If no point found, stay put and try again next frame
            activeData.wanderDestination = transform.position;
        }
    }

    #endregion

    private void HandleChasing(float speed, float detectionRadius)
    {
        if (activeData.targetPlayer == null)
            return;

        Vector3 toPlayer = activeData.targetPlayer.position - transform.position;
        toPlayer.y = Vector3.zero.y;

        float distanceToPlayer = toPlayer.magnitude;
        Vector3 direction = toPlayer.normalized;

        if (distanceToPlayer > stopDistance)
        {
            //Move towards the player
            Vector3 targetVelocity = direction * speed;
            currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.deltaTime);
        }
        else
        {
            //Stop and Prepare to ATTACK
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);

            Debug.Log("ATTACK TRIGGERED");

            if (activeData.enemyClassType == ENEMYCLASSTYPE.RANGED)
            {
                ShootRubberBand();
            }
        }

        //Always rotate to face the player while chasing
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        //Apply movement once at the end
        transform.position += currentVelocity * Time.deltaTime;
    }

    private void HandleAttack()
    {

    }

    private void ShootRubberBand()
    {

    }

    private void TakeDamage(int damage)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    #region Testing Functions
    private void OnDrawGizmosSelected()
    {
        // Detection Radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Attack/Stop Radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }

    #endregion
}