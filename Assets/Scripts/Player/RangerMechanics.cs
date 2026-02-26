using System.Collections;
using UnityEngine;

public class RangerMechanics : BaseClassMechanics
{
    [Header("References")]
    [SerializeField] private Transform leftGunFirePoint; 
    [SerializeField] private Transform rightGunFirePoint;
    [SerializeField] private Transform laserFirePoint;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private TargetingSystem targetingSystem;
    public RangerClassData rangerData;

    [Header("Avatars & Animators")]
    [SerializeField] private GameObject armedAvatar;
    [SerializeField] private Animator armedAnimator;

    [SerializeField] private GameObject unarmedAvatar;
    [SerializeField] private Animator unarmedAnimator;

    private Animator currentAnimator;

    [Header("Visuals")]
    [SerializeField] private Transform characterMesh;

    private float nextAttackTime;
    private float nextLaserTime;
    private float nextRollTime;
    private float rollCooldown = 3.0f;

    private bool isRolling;
    private bool isFiringLaser;
    private bool shootLeftHandNext = true;
    private float nextLaserDamageTime;
    public float laserDamageTickRate = 0.2f;

    private void Start()
    {
        if (laserPrefab != null) laserPrefab.SetActive(false);
        SwapToArmed(false); // Start the game with guns out
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

        // Continuously send the movement speed to whichever Animator is active
        if (currentAnimator != null && activeData != null)
        {
            currentAnimator.SetFloat("Speed", activeData.moveDirection.magnitude);
        }
    }

    private void LateUpdate()
    {
       
        if (isFiringLaser && laserPrefab != null && laserFirePoint != null && rangerData != null)
        {
            
            Vector3 fireDirection = laserFirePoint.forward;

            if (targetingSystem != null && targetingSystem.currentTarget != null)
            {
               
                fireDirection = (targetingSystem.currentTarget.position - laserFirePoint.position).normalized;
            }

          
            laserPrefab.transform.position = laserFirePoint.position;
            laserPrefab.transform.rotation = Quaternion.LookRotation(fireDirection);

            float hitDistance = rangerData.laserRange;
           
            Ray ray = new Ray(laserFirePoint.position, fireDirection);

            if (Physics.Raycast(ray, out RaycastHit hit, rangerData.laserRange, rangerData.hitMask))
            {
                hitDistance = hit.distance;

                if (Time.time >= nextLaserDamageTime)
                {
                    Collider other = hit.collider;

                   
                    BossController boss = other.GetComponentInParent<BossController>();
                    EnemyController enemy = other.GetComponentInParent<EnemyController>();

                    float damageToDeal = rangerData.damage * activeData.currentDamageMultiplier;
                    bool dealtDamage = false;

                    if (boss != null)
                    {
                        boss.TakeDamage(damageToDeal);
                        dealtDamage = true;
                    }
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damageToDeal);
                        dealtDamage = true;
                    }

                    if (dealtDamage && BattleUIManager.Instance != null && activeData is PlayerActiveData playerData)
                    {
                        BattleUIManager.Instance.AddDamage(playerData.currentClassType, damageToDeal);
                    }

  
                    nextLaserDamageTime = Time.time + laserDamageTickRate;
                }
            }

            // Stretch the prefab to the hit distance
            laserPrefab.transform.localScale = new Vector3(
                laserPrefab.transform.localScale.x,
                laserPrefab.transform.localScale.y,
                hitDistance
            );
        }
    }

    public override void EquipClass()
    {
        if (activeData != null && rangerData != null)
        {
            activeData.currentMoveSpeed = rangerData.moveSpeed;
            activeData.currentClassType = rangerData.classType;

            SwapToArmed(true);
        }
    }

    // ==========================================
    // AVATAR SWAPPING 
    // ==========================================

    private void SwapToUnarmed(string animationToPlay)
    {
        if (armedAvatar != null) armedAvatar.SetActive(false);
        if (unarmedAvatar != null) unarmedAvatar.SetActive(true);

        currentAnimator = unarmedAnimator;

        if (currentAnimator != null) currentAnimator.Play(animationToPlay, 0, 0f);
    }

    private void SwapToArmed(bool playDrawAnimation)
    {
        if (unarmedAvatar != null) unarmedAvatar.SetActive(false);
        if (armedAvatar != null) armedAvatar.SetActive(true);

        currentAnimator = armedAnimator;

        if (playDrawAnimation && currentAnimator != null)
        {
            currentAnimator.Play("BringUpGun", 0, 0f);
        }
    }

    // ==========================================
    // COMBAT & ABILITIES
    // ==========================================

    public override void HandleAttack()
    {
        if (Time.time >= nextAttackTime && !isRolling && !isFiringLaser && rangerData != null)
        {
            if (currentAnimator != null)
            {
                if (shootLeftHandNext)
                {
                    currentAnimator.Play("AkimboShootingLeftOnly", 0, 0f);
                }
                else
                {
                    currentAnimator.Play("AkimboShootingRightOnly", 0, 0f);
                }
            }

            shootLeftHandNext = !shootLeftHandNext;
            nextAttackTime = Time.time + rangerData.attackCooldown;
        }
    }

    public void AE_ShootSeedLeft()
    {
        ShootSeed(leftGunFirePoint);
    }

    public void AE_ShootSeedRight()
    {
        ShootSeed(rightGunFirePoint);
    }

    public void AE_PlayFootstep()
    {
        if (AudioManager.instance != null) AudioManager.instance.Play("Footstep");
    }

    public void AE_EnableLaser()
    {
        if (laserPrefab != null) laserPrefab.SetActive(true);
    }

    private void ShootSeed(Transform activeBarrel)
    {
        if (rangerData == null || activeBarrel == null) return;

        if (targetingSystem != null && targetingSystem.currentTarget != null)
        {
            Vector3 aimDirection = (targetingSystem.currentTarget.position - activeBarrel.position).normalized;
            activeBarrel.rotation = Quaternion.LookRotation(aimDirection);
        }
        else
        {
            activeBarrel.localRotation = Quaternion.identity;
        }

        if (activeData is PlayerActiveData playerData)
        {
            playerData.objectPoolSpawnData = new ObjectPoolSpawnData(
                activeBarrel.position,
                activeBarrel.forward,
                rangerData.damage * playerData.currentDamageMultiplier,
                rangerData.seedLaunchForce
            );

            playerData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.RANGER_SEED;
            playerData.isObjectPoolTriggered = true;
        }

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

        if (activeData is PlayerActiveData playerData)
        {
            playerData.isInvincible = true;
            playerData.isRolling = true;

            Vector2 rollDir = playerData.moveDirection;
            if (rollDir.magnitude < 0.1f)
            {
                rollDir = new Vector2(0, 1);
            }

            playerData.moveDirection = rollDir.normalized;
            playerData.isMoving = true;
        }

        SwapToUnarmed("stand to roll");

        activeData.currentMoveSpeed = originalSpeed * rangerData.rollSpeedMultiplier;

        yield return new WaitForSeconds(rangerData.rollDuration);

        activeData.currentMoveSpeed = originalSpeed;
        if (characterMesh != null) characterMesh.localRotation = Quaternion.identity;

        SwapToArmed(true);

        if (activeData is PlayerActiveData pData)
        {
            pData.isInvincible = false;
            pData.isRolling = false;
        }

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

        SwapToUnarmed("LaserState");

        float timer = 0f;
        while (timer < rangerData.laserDuration)
        {
            timer += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        if (laserPrefab != null) laserPrefab.SetActive(false);

        SwapToArmed(true);
        isFiringLaser = false;
    }
}