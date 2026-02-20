using System.Collections;
using UnityEngine;

public class RangerMechanics : BaseClassMechanics
{
    [Header("References")]
    [SerializeField] private RangerClassData rangerData;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LineRenderer laserLine;
    [SerializeField] private TargetingSystem targetingSystem;

    private float nextAttackTime;
    private bool isRolling;
    private int currentLaserAmmo;
    private bool isFiringLaser;

    private void Start()
    {
        if (rangerData != null)
        {
            currentLaserAmmo = rangerData.maxLaserAmmo;
        }
        if (laserLine != null) laserLine.enabled = false;
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
        if (Time.time >= nextAttackTime && !isRolling && rangerData != null)
        {
            ShootSeed();
            nextAttackTime = Time.time + rangerData.attackCooldown;
        }
    }

    private void ShootSeed()
    {
        if (rangerData == null || firePoint == null) return;

        // AUTO-AIM LOGIC: Look at the target if we have one
        if (targetingSystem != null && targetingSystem.currentTarget != null)
        {
            Vector3 aimDirection = (targetingSystem.currentTarget.position - firePoint.position).normalized;
            firePoint.rotation = Quaternion.LookRotation(aimDirection);
        }
        else
        {
            // Reset to shoot straight forward based on player rotation
            firePoint.localRotation = Quaternion.identity;
        }

        // Package the data for the Object Pool Manager
        activeData.objectPoolSpawnData = new ObjectPoolSpawnData(
            firePoint.position,
            firePoint.forward,
            rangerData.damage,
            rangerData.seedLaunchForce
        );

        // Tell the manager what to spawn and pull the trigger
        activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.RANGER_SEED;
        activeData.isObjectPoolTriggered = true;
    }

    public override void HandleDefense()
    {
        if (!isRolling && activeData != null && activeData.isMoving && rangerData != null)
        {
            StartCoroutine(RollRoutine());
        }
    }

    private IEnumerator RollRoutine()
    {
        isRolling = true;
        float originalSpeed = rangerData.moveSpeed;

        activeData.currentMoveSpeed = originalSpeed * rangerData.rollSpeedMultiplier;
        yield return new WaitForSeconds(rangerData.rollDuration);

        activeData.currentMoveSpeed = originalSpeed;
        isRolling = false;
    }

    public override void HandleAbility()
    {
        if (currentLaserAmmo > 0 && !isFiringLaser && !isRolling && rangerData != null)
        {
            StartCoroutine(LaserRoutine());
        }
    }

    private IEnumerator LaserRoutine()
    {
        isFiringLaser = true;
        if (laserLine != null) laserLine.enabled = true;

        while (currentLaserAmmo > 0)
        {
            currentLaserAmmo--;
            Vector3 fireDirection = firePoint.forward;
            if (targetingSystem != null && targetingSystem.currentTarget != null)
            {
                // Constantly calculate direction to the target so the laser "bends" to track them
                fireDirection = (targetingSystem.currentTarget.position - firePoint.position).normalized;
            }
            Ray ray = new Ray(firePoint.position, firePoint.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, rangerData.laserRange, rangerData.hitMask))
            {
                if (laserLine != null)
                {
                    laserLine.SetPosition(0, firePoint.position);
                    laserLine.SetPosition(1, hit.point);
                }
            }
            else
            {
                if (laserLine != null)
                {
                    laserLine.SetPosition(0, firePoint.position);
                    laserLine.SetPosition(1, firePoint.position + firePoint.forward * rangerData.laserRange);
                }
            }

            yield return new WaitForSeconds(0.1f);
        }

        if (laserLine != null) laserLine.enabled = false;
        isFiringLaser = false;
    }
}