using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyController : MonoBehaviour
{
    [Header("Enemy Data")]
    [SerializeField] protected DataHolder dataHolder;
    [SerializeField] protected EnemyAnimator enemyAnimator;

    [Header("Wander Settings")]
    [SerializeField] protected float acceleration = 5f;
    [SerializeField] protected float rotationSpeed = 5f;
    [SerializeField] protected float arrivalRadius = 1.5f;

    [Header("Detection Settings")]
    [SerializeField] protected LayerMask playerLayer;

    [Header("Edge Avoidance")]
    [SerializeField] protected float edgeDetectionRadius = 2f;
    [SerializeField] protected float edgeSlowdownRadius = 4f;

    [Header("OnHitVFX")]
    [SerializeField] protected Renderer objectRenderer;
    [SerializeField] protected Color damageColor;
    [SerializeField] protected float damageEffectDuration;

    [Header("Death Settings")]
    [SerializeField] protected string dissolvePropertyName = "DissolveValue";
    [SerializeField] protected float dissolveDuration = 2f;
    [SerializeField] protected float dissolveStartValue = -1f;
    [SerializeField] protected float dissolveEndValue = 0.7f;

    protected NavMeshAgent agent;
    protected EnemyActiveData activeData;
    protected Transform validTarget;
    protected float stopDistance;
    protected float detectionRadius;
    protected Color originalColor;
    protected Material targetMaterial;
    protected bool isDead = false;
    protected bool isAttacking = false;

    private Coroutine flashCoroutine;
    public event System.Action onEnemyDied;

    public EnemyActiveData ActiveData
    {
        get { return activeData; }
        private set { activeData = value; }
    }

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = arrivalRadius;
        agent.acceleration = acceleration;
        agent.angularSpeed = rotationSpeed * 100f;
        agent.avoidancePriority = Random.Range(30, 70);

        activeData = dataHolder.activeData as EnemyActiveData;
        if (activeData == null) return;

        activeData.wanderDestination = transform.position;
        activeData.waitTimer = 2f;
        activeData.currentState = EnemyActiveData.AIState.WANDERING;

        if (objectRenderer != null)
        {
            targetMaterial = objectRenderer.material;
            originalColor = targetMaterial.color;
        }
    }

    protected virtual void Update()
    {
        if (isDead) return;
        DetectTarget();
        HandleMove();
        UpdateAnimator();
    }

    private void DetectTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        validTarget = null;

        foreach (Collider col in hits)
        {
            PlayerController player = col.GetComponentInParent<PlayerController>();

            if (player != null)
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
            if (activeData.currentState == EnemyActiveData.AIState.CHASING)
            {
                agent.ResetPath();
                PickNewWanderDestination(Vector3.zero);
            }

            activeData.targetPlayer = null;
            activeData.currentState = EnemyActiveData.AIState.WANDERING;
        }
    }

    private void UpdateAnimator()
    {
        if (enemyAnimator != null && agent != null)
        {
            bool moving = agent.hasPath && agent.velocity.magnitude > 0.1f && !agent.isStopped;
            enemyAnimator.SetMoving(moving);
        }
    }

    // Subclasses define their own move logic
    protected abstract void HandleMove();
    protected abstract void HandleAttack();

    protected abstract string[] GetDamageSounds();

    protected void HandleWander(float speed)
    {
        agent.stoppingDistance = arrivalRadius;
        agent.speed = speed;

        Vector3 awayFromEdge;
        if (CheckForEdge(out awayFromEdge))
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            activeData.waitTimer = Random.Range(1f, 2f);
            PickNewWanderDestination(awayFromEdge);
            return;
        }

        bool arrived = !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
        if (arrived)
        {
            activeData.waitTimer -= Time.deltaTime;
            if (activeData.waitTimer <= 0)
            {
                PickNewWanderDestination(Vector3.zero);
                activeData.waitTimer = Random.Range(2f, 5f);
            }
        }
    }

    protected void HandleChasing(float speed)
    {
        if (activeData.targetPlayer == null) return;

        agent.speed = speed;
        agent.isStopped = false;
        agent.stoppingDistance = stopDistance;
        agent.SetDestination(activeData.targetPlayer.position);

        float realDistance = Vector3.Distance(transform.position, activeData.targetPlayer.position);

        Vector3 directionToPlayer = (activeData.targetPlayer.position - transform.position).normalized;
        directionToPlayer.y = 0f;

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        bool inAttackRange = !agent.pathPending && agent.remainingDistance <= stopDistance;

        if (inAttackRange)
        {
            if (realDistance > stopDistance + 0.2f)
            {
                agent.isStopped = false;
                return;
            }

            agent.isStopped = true;

            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer < 15f)
                HandleAttack();
        }
    }

    protected void PickNewWanderDestination(Vector3 biasDirection)
    {
        Vector3 randomDirection = biasDirection != Vector3.zero
            ? Vector3.Slerp(biasDirection, Random.insideUnitSphere, 0.5f) * 10f
            : Random.insideUnitSphere * 10f;

        randomDirection.y = 0f;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position + randomDirection, out hit, 10f, NavMesh.AllAreas))
        {
            activeData.wanderDestination = hit.position;
            agent.SetDestination(hit.position);
            agent.isStopped = false;
        }
        else
        {
            activeData.waitTimer = 0f;
        }
    }

    protected bool CheckForEdge(out Vector3 awayFromEdge)
    {
        NavMeshHit hit;
        awayFromEdge = Vector3.zero;

        if (NavMesh.FindClosestEdge(transform.position, out hit, NavMesh.AllAreas))
        {
            if (hit.distance < edgeDetectionRadius)
            {
                awayFromEdge = (transform.position - hit.position).normalized;
                awayFromEdge.y = 0f;
                return true;
            }
        }
        return false;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        activeData.currentHealth -= damage;

        string[] sounds = GetDamageSounds();
        if (sounds != null && sounds.Length > 0)
        {
            string randomHit = sounds[Random.Range(0, sounds.Length)];
            AudioManager.instance.Play(randomHit);
        }

        StartCoroutine(TakeDamageEffect());

        if (activeData.currentHealth <= 0)
            Destroy(gameObject);
    }

    public void SetAttacking(bool value) => isAttacking = value;

    private IEnumerator TakeDamageEffect()
    {
        targetMaterial.color = damageColor;

        float elapsed = 0f;
        while (elapsed < damageEffectDuration)
        {
            elapsed += Time.deltaTime;
            targetMaterial.color = Color.Lerp(damageColor, originalColor, elapsed / damageEffectDuration);
            yield return null;
        }

        targetMaterial.color = originalColor;
        flashCoroutine = null;
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}