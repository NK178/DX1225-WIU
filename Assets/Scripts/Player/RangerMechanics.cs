using System.Collections;
using UnityEngine;

public class RangerMechanics : BaseClassMechanics
{
    [Header("References")]
    [SerializeField] private RangerClassData rangerData;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LineRenderer laserLine; // For Laser Eyes visual

    private float nextAttackTime;
    private bool isRolling;
    private int currentLaserAmmo;
    private bool isFiringLaser;

    private void Start()
    {
        currentLaserAmmo = rangerData.maxLaserAmmo;
        if (laserLine != null) laserLine.enabled = false;
    }

    public override void HandleAttack()
    {
        // Semi-automatic seed gun
        if (Time.time >= nextAttackTime && !isRolling)
        {
            ShootSeed();
            nextAttackTime = Time.time + rangerData.attackCooldown;
        }
    }

    private void ShootSeed()
    {
        if (rangerData.seedProjectilePrefab == null || firePoint == null) return;

        GameObject seed = Instantiate(rangerData.seedProjectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = seed.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.useGravity = true;
            rb.AddForce(firePoint.forward * rangerData.seedLaunchForce, ForceMode.Impulse);
        }
    }

    public override void HandleDefense()
    {
        // Dodge / Evade Roll
        if (!isRolling && activeData != null && activeData.isMoving)
        {
            StartCoroutine(RollRoutine());
        }
    }

    private IEnumerator RollRoutine()
    {
        isRolling = true;
        float originalSpeed = activeData.currentMoveSpeed;

        // Temporarily boost speed for the dodge
        activeData.currentMoveSpeed = originalSpeed * rangerData.rollSpeedMultiplier;

        yield return new WaitForSeconds(rangerData.rollDuration);

        // Reset speed
        activeData.currentMoveSpeed = originalSpeed;
        isRolling = false;
    }

    public override void HandleAbility()
    {
        // Laser Eyes - Superman pose beam
        if (currentLaserAmmo > 0 && !isFiringLaser && !isRolling)
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

            Ray ray = new Ray(firePoint.position, firePoint.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, rangerData.laserRange, rangerData.hitMask))
            {
                if (laserLine != null)
                {
                    laserLine.SetPosition(0, firePoint.position);
                    laserLine.SetPosition(1, hit.point);
                }
                // Deal damage to hit.collider.gameObject here
            }
            else
            {
                if (laserLine != null)
                {
                    laserLine.SetPosition(0, firePoint.position);
                    laserLine.SetPosition(1, firePoint.position + firePoint.forward * rangerData.laserRange);
                }
            }

            yield return new WaitForSeconds(0.1f); // Ammo drain rate
        }

        if (laserLine != null) laserLine.enabled = false;
        isFiringLaser = false;
    }
}