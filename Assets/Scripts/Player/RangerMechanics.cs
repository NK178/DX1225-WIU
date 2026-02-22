using System.Collections;
using UnityEngine;

public class RangerMechanics : BaseClassMechanics
{
    [Header("References")]
    [SerializeField] private RangerClassData rangerData;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LineRenderer laserLine;
    [SerializeField] private TargetingSystem targetingSystem;

    [Header("Visuals & Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform characterMesh;
    [SerializeField] private float rollRotationSpeed = 800f;

    private float nextAttackTime;
    private float nextLaserTime;
    private bool isRolling;
    private bool isFiringLaser;

    private void Start()
    {
        if (laserLine != null) laserLine.enabled = false;
    }

    private void LateUpdate()
    {
        // Handle the visual drawing of the laser here so it perfectly matches the player's movement
        if (isFiringLaser && laserLine != null && firePoint != null && rangerData != null)
        {
            Vector3 fireDirection = firePoint.forward;

            if (targetingSystem != null && targetingSystem.currentTarget != null)
            {
                fireDirection = (targetingSystem.currentTarget.position - firePoint.position).normalized;
            }

            laserLine.SetPosition(0, firePoint.position);

            Ray ray = new Ray(firePoint.position, fireDirection);
            if (Physics.Raycast(ray, out RaycastHit hit, rangerData.laserRange, rangerData.hitMask))
            {
                laserLine.SetPosition(1, hit.point);
            }
            else
            {
                laserLine.SetPosition(1, firePoint.position + fireDirection * rangerData.laserRange);
            }
        }
    }

    public override void EquipClass()
    {
        if (activeData != null && rangerData != null)
        {
            activeData.currentMoveSpeed = rangerData.moveSpeed;
            activeData.currentClassType = rangerData.classType;
        }
    }

    public override void HandleAttack()
    {
        if (Time.time >= nextAttackTime && !isRolling && !isFiringLaser && rangerData != null)
        {
            ShootSeed();
            nextAttackTime = Time.time + rangerData.attackCooldown;
        }
    }

    private void ShootSeed()
    {
        if (rangerData == null || firePoint == null) return;

        if (targetingSystem != null && targetingSystem.currentTarget != null)
        {
            Vector3 aimDirection = (targetingSystem.currentTarget.position - firePoint.position).normalized;
            firePoint.rotation = Quaternion.LookRotation(aimDirection);
        }
        else
        {
            firePoint.localRotation = Quaternion.identity;
        }

        activeData.objectPoolSpawnData = new ObjectPoolSpawnData(
            firePoint.position,
            firePoint.forward,
            rangerData.damage,
            rangerData.seedLaunchForce
        );

        activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.RANGER_SEED;
        activeData.isObjectPoolTriggered = true;
    }

    public override void HandleDefense()
    {
        if (!isRolling && activeData != null && activeData.isMoving && !isFiringLaser && rangerData != null)
        {
            StartCoroutine(RollRoutine());
        }
    }

    private IEnumerator RollRoutine()
    {
        isRolling = true;
        float originalSpeed = rangerData.moveSpeed;

        if (animator != null) animator.SetBool("IsRolling", true);

        activeData.currentMoveSpeed = originalSpeed * rangerData.rollSpeedMultiplier;

        float timer = 0f;
        while (timer < rangerData.rollDuration)
        {
            timer += Time.deltaTime;

            // Spin the mesh to look like a rolling fruit
            if (characterMesh != null && activeData.isMoving)
            {
                characterMesh.Rotate(Vector3.right, rollRotationSpeed * Time.deltaTime, Space.Self);
            }

            yield return null;
        }

        activeData.currentMoveSpeed = originalSpeed;
        if (animator != null) animator.SetBool("IsRolling", false);

        // Snap the mesh upright when finished
        if (characterMesh != null) characterMesh.localRotation = Quaternion.identity;

        isRolling = false;
    }

    public override void HandleAbility()
    {
        // Check if the cooldown time has passed!
        if (Time.time >= nextLaserTime && !isFiringLaser && !isRolling && rangerData != null)
        {
            // Set the cooldown timer for the next use
            nextLaserTime = Time.time + rangerData.laserCooldown;
            StartCoroutine(LaserRoutine());
        }
    }

    private IEnumerator LaserRoutine()
    {
        isFiringLaser = true;
        if (laserLine != null) laserLine.enabled = true;

        float timer = 0f;

        // Loop continuously until the duration runs out
        while (timer < rangerData.laserDuration)
        {
            timer += 0.1f; // add 0.1 because yield for 0.1 seconds below

            // Example Damage Logic:
            /*
            Vector3 fireDirection = firePoint.forward;
            if (targetingSystem != null && targetingSystem.currentTarget != null)
                fireDirection = (targetingSystem.currentTarget.position - firePoint.position).normalized;
                
            Ray ray = new Ray(firePoint.position, fireDirection);
            if (Physics.Raycast(ray, out RaycastHit hit, rangerData.laserRange, rangerData.hitMask))
            {
                // hit.collider.GetComponent<EnemyHealth>().TakeDamage(tickDamage);
            }
            */

            yield return new WaitForSeconds(0.1f);
        }

        if (laserLine != null) laserLine.enabled = false;
        isFiringLaser = false;
    }
}