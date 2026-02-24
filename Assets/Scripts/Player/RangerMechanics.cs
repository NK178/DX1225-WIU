using System.Collections;
using UnityEngine;

public class RangerMechanics : BaseClassMechanics
{
    [Header("References")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private LineRenderer laserLine;
    [SerializeField] private TargetingSystem targetingSystem;
    public RangerClassData rangerData;

    [Header("Visuals & Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform characterMesh;
    [SerializeField] private float rollRotationSpeed = 800f;

    private float nextAttackTime;
    private float nextLaserTime;
    private float nextRollTime;
    private float rollCooldown = 3.0f;

    private bool isRolling;
    private bool isFiringLaser;

    private void Start()
    {
        if (laserLine != null) laserLine.enabled = false;
    }

    private void Update()
    {
        if (BattleUIManager.Instance != null && rangerData != null)
        {
            float laserRemaining = Mathf.Max(0, nextLaserTime - Time.time);
            BattleUIManager.Instance.UpdateCooldownUI(BattleUIManager.Instance.laserCooldownImage, laserRemaining, rangerData.laserCooldown);

            float rollRemaining = Mathf.Max(0, nextRollTime - Time.time);
            BattleUIManager.Instance.UpdateCooldownUI(BattleUIManager.Instance.rollCooldownImage, rollRemaining, rollCooldown);
        }
    }

    private void LateUpdate()
    {
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
        if (AudioManager.instance != null) AudioManager.instance.Play("RangerShoot");
    }

    public override void HandleDefense()
    {
        if (Time.time >= nextRollTime && !isRolling && activeData != null && activeData.isMoving && !isFiringLaser && rangerData != null)
        {
            nextRollTime = Time.time + rollCooldown;
            if (AudioManager.instance != null) AudioManager.instance.Play("RangerRoll");
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

            if (characterMesh != null && activeData.isMoving)
            {
                characterMesh.Rotate(Vector3.right, rollRotationSpeed * Time.deltaTime, Space.Self);
            }

            yield return null;
        }

        activeData.currentMoveSpeed = originalSpeed;
        if (animator != null) animator.SetBool("IsRolling", false);

        if (characterMesh != null) characterMesh.localRotation = Quaternion.identity;

        isRolling = false;
    }

    public override void HandleAbility()
    {
        if (Time.time >= nextLaserTime && !isFiringLaser && !isRolling && rangerData != null)
        {
            nextLaserTime = Time.time + rangerData.laserCooldown;
            if (AudioManager.instance != null) AudioManager.instance.Play("RangerLaser");
            StartCoroutine(LaserRoutine());
           
        }
    }

    private IEnumerator LaserRoutine()
    {
        isFiringLaser = true;
        if (laserLine != null) laserLine.enabled = true;

        float timer = 0f;

        while (timer < rangerData.laserDuration)
        {
            timer += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        if (laserLine != null) laserLine.enabled = false;
        isFiringLaser = false;
    }
}