using UnityEditor.Rendering;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.Rendering;

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

    [Header("Ranger Settings")]
    //[SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float minAttackCooldown = 0.5f;
    [SerializeField] private float maxAttackCooldown = 5f;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 15f;
    [SerializeField] private Transform firePoint;

    private float lastAttackTime;
    private float stopDistance;

    private EnemyActiveData activeData;
    private float detectionRadius;

    private Transform validTarget;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = arrivalRadius;
        agent.acceleration = acceleration;
        agent.angularSpeed = rotationSpeed * 100f;

        activeData = (EnemyActiveData)dataHolder.activeData;

        if (activeData == null) return;
        if (firePoint == null) return;

        activeData.enemyClassType = enemyType;

        //Casting it to access wanderDestination/currentState
        activeData = dataHolder.activeData as EnemyActiveData;

        if (activeData != null)
        {
            //Set the initial wander destination based on where the enemy was placed
            activeData.wanderDestination = transform.position;
            activeData.waitTimer = 2f;
            activeData.currentState = EnemyActiveData.AIState.WANDERING;

            if (activeData.enemyClassType == ENEMYCLASSTYPE.MELEE)
                activeData.currentHealth = knifeData.maxHealth;
            else if (activeData.enemyClassType == ENEMYCLASSTYPE.RANGED)
                activeData.currentHealth = rangerData.maxHealth;
        }
    }

    void Update()
    {
        HandleMove();
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
        validTarget = null;

        foreach (Collider col in player)
        {
            DummyController dummy = col.GetComponentInParent<DummyController>();
            PlayerController playercontroller = col.GetComponentInParent<PlayerController>();

            if (dummy != null)
            {
                if (dummy.IsAlive())
                {
                    validTarget = col.transform;
                    break;
                }

                continue;
            }

            else if (playercontroller != null)
            {
                validTarget = col.transform;
                break;

            }
        }

        if (validTarget != null)
        {
            activeData.targetPlayer = validTarget;
            activeData.currentState = EnemyActiveData.AIState.CHASING;
        }
        else
        {
            //If player is out of range/gone return back to wandering 
            activeData.targetPlayer = null;
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

    #region Old Manual Functions

    //#region Wander Functions

    //private void HandleWander(float speed)
    //{
    //    Vector3 toTarget = (activeData.wanderDestination - transform.position);
    //    toTarget.y = Vector3.zero.y;
    //    float distanceToTarget = toTarget.magnitude;

    //    if (distanceToTarget < arrivalRadius)
    //    {
    //        currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);

    //        activeData.waitTimer -= Time.deltaTime;

    //        if (activeData.waitTimer <= 0)
    //        {
    //            PickNewWanderDestination();
    //            activeData.waitTimer = Random.Range(2f, 5f);
    //        }
    //    }
    //    else
    //    {
    //        Vector3 direction = toTarget.normalized;
    //        Vector3 targetVelocity = direction * speed;

    //        //Smoothly accelerate towards the target
    //        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.deltaTime);

    //        //Smooth Rotation
    //        Quaternion targetRotation = Quaternion.LookRotation(direction);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    //    }

    //    if (validTarget == null && activeData.currentState == EnemyActiveData.AIState.CHASING)
    //    {
    //        activeData.currentState = EnemyActiveData.AIState.WANDERING;
    //        PickNewWanderDestination();
    //    }

    //    //Apply movement once at the end
    //    transform.position += currentVelocity * Time.deltaTime;
    //}

    //private void PickNewWanderDestination()
    //{
    //    //float minDistance = 2f;
    //    float maxDistance = 10f;

    //    #region Old Destination Checker
    //    //Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Vector3.zero.y, Random.Range(-1f, 1f));
    //    //float randomDistance = Random.Range(minDistance, maxDistance);
    //    //activeData.wanderDestination = transform.position + (randomDirection * randomDistance);
    //    #endregion

    //    //Generate the raw random point
    //    Vector3 randomDirection = Random.insideUnitSphere * maxDistance;
    //    randomDirection.y = 0; // Keep it on horizontal plane
    //    Vector3 sourcePosition = transform.position + randomDirection;

    //    //Find the closest valid point on the NavMesh
    //    NavMeshHit hit;

    //    //Searching within MaxDistance to find a valid navmesh Floor
    //    if (NavMesh.SamplePosition(sourcePosition, out hit, maxDistance, NavMesh.AllAreas))
    //    {
    //        activeData.wanderDestination = hit.position;
    //    }
    //    else
    //    {
    //        //If no point found, stay put and try again next frame
    //        activeData.wanderDestination = transform.position;
    //    }
    //}

    //#endregion

    //private void HandleChasing(float speed, float detectionRadius)
    //{
    //    if (activeData.targetPlayer == null)
    //        return;

    //    DummyController dummy = activeData.targetPlayer.GetComponentInParent<DummyController>();
    //    if (dummy != null && !dummy.IsAlive())
    //    {
    //        activeData.targetPlayer = null;
    //        activeData.currentState = EnemyActiveData.AIState.WANDERING;
    //        return;
    //    }

    //    Vector3 toPlayer = activeData.targetPlayer.position - transform.position;
    //    toPlayer.y = Vector3.zero.y;

    //    float distanceToPlayer = toPlayer.magnitude;
    //    Vector3 direction = toPlayer.normalized;

    //    if (distanceToPlayer > stopDistance)
    //    {
    //        //Move towards the player
    //        Vector3 targetVelocity = direction * speed;
    //        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.deltaTime);
    //    }
    //    else
    //    {
    //        //Stop and Prepare to ATTACK
    //        //currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);

    //        currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
    //        if (currentVelocity.magnitude < 0.1f) currentVelocity = Vector3.zero;

    //        Debug.Log("ATTACK TRIGGERED");
    //        HandleAttack();
    //    }

    //    //Always rotate to face the player while chasing
    //    if (direction != Vector3.zero)
    //    {
    //        Quaternion targetRotation = Quaternion.LookRotation(direction);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    //    }

    //    //Apply movement once at the end
    //    transform.position += currentVelocity * Time.deltaTime;
    //}

    #endregion

    #region Wandering Function
    private void HandleWander(float speed)
    {
        agent.stoppingDistance = arrivalRadius;
        agent.speed = speed;
        agent.isStopped = false;

        bool arrived = !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;

        if (arrived)
        {
            agent.isStopped = true;
            activeData.waitTimer -= Time.deltaTime;

            if (activeData.waitTimer <= 0)
            {
                PickNewWanderDestination();
                activeData.waitTimer = Random.Range(2f, 5f);
            }
        }
    }

    private void PickNewWanderDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection.y = 0f;
        Vector3 sourcePosition = transform.position + randomDirection;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(sourcePosition, out hit, 10f, NavMesh.AllAreas))
        {
            activeData.wanderDestination = hit.position;
            agent.SetDestination(hit.position); // critical — actually send agent there
        }
        else
        {
            activeData.waitTimer = 0f; // retry next cycle
        }
    }
    #endregion

    private void HandleChasing(float speed, float detectionRadius)
    {
        if (activeData.targetPlayer == null) return;

        agent.speed = speed;
        agent.isStopped = false;
        agent.stoppingDistance = stopDistance;
        agent.SetDestination(activeData.targetPlayer.position);

        bool inAttackRange = !agent.pathPending && agent.remainingDistance <= stopDistance;

        if (inAttackRange)
        {
            agent.isStopped = true;
            HandleAttack();
        }

        // Rotation is handled by the agent automatically
    }

    private void HandleAttack()
    {
        if (activeData.enemyClassType == ENEMYCLASSTYPE.RANGED)
        {
            Debug.Log("RANGER ATTACK!");
            ShootRubberBand();
        }
        else if (activeData.enemyClassType == ENEMYCLASSTYPE.MELEE)
        {
            //MELEE ATTACK
            Debug.Log("MELEE ATTACK!");
        }
    }

    private void ShootRubberBand()
    {
        if (firePoint == null || activeData.targetPlayer == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, activeData.targetPlayer.position);
        float distanceFactor = Mathf.InverseLerp(minDistance, maxDistance, distance);
        float dynamicCooldown = Mathf.Lerp(minAttackCooldown, maxAttackCooldown, distanceFactor);

        //Check cooldown
        if (Time.time < lastAttackTime + dynamicCooldown) return;

        Vector3 aimDirection = (activeData.targetPlayer.position - firePoint.position).normalized;
        firePoint.rotation = Quaternion.LookRotation(aimDirection);

        Vector3 forceVector = firePoint.forward * rangerData.launchForce;

        activeData.objectPoolSpawnData = new ObjectPoolSpawnData(
            firePoint.position,
            firePoint.forward,
            forceVector,
            rangerData.damage
        );

        //Calling the type of object it will spawn
        activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.RUBBERBAND_BULLETS;
        activeData.isObjectPoolTriggered = true;

        if (ObjectPoolManager.Instance != null)
        {
            ObjectPoolManager.Instance.HandleSpawnRequest(activeData);
        }

        lastAttackTime = Time.time;
    }

    private void TakeDamage(int damage)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    #region Testing Functions
    //private void OnDrawGizmosSelected()
    //{
    //    // Detection Radius
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, detectionRadius);

    //    // Attack/Stop Radius
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, stopDistance);
    //}

    private void OnDrawGizmosSelected()
    {
        // 1. Detection Radius (Yellow)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // 2. Stop/Attack Radius (Red)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);

        // --- New Distance-Based Combat Visualization ---

        if (activeData.enemyClassType == ENEMYCLASSTYPE.RANGED)
        {
            // 3. Min Distance (Cyan) - Fastest Fire Rate starts here
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, minDistance);

            // 4. Max Distance (Blue) - Slowest Fire Rate starts here
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, maxDistance);

            // 5. Visual line to Player + Dynamic Data Label
            if (activeData != null && activeData.targetPlayer != null)
            {
                float dist = Vector3.Distance(transform.position, activeData.targetPlayer.position);

                // Calculate what the cooldown WOULD be right now
                float factor = Mathf.InverseLerp(minDistance, maxDistance, dist);
                float currentCD = Mathf.Lerp(minAttackCooldown, maxAttackCooldown, factor);

                // Draw a line to the player
                Gizmos.color = Color.white;
                Gizmos.DrawLine(transform.position, activeData.targetPlayer.position);

                // Draw a label in the Scene View (Requires UnityEditor namespace)
                #if UNITY_EDITOR
                string info = $"Distance: {dist:F1}m\nNext CD: {currentCD:F2}s";
                UnityEditor.Handles.Label(transform.position + Vector3.up * 2f, info);
                #endif
            }
        }

    }

    #endregion
}