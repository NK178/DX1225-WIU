using System.Collections;
using UnityEngine;
using static PlayerActiveData;

[System.Serializable]
public enum CLASSTYPE
{
    MELEE,
    RANGED,
    NUM_TYPES
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private DataHolder dataHolder;
    [SerializeField] private CharacterController characterController;

    [SerializeField] private float damageEffectDuration;
    [SerializeField] private float damageAbilityDuration;
    [SerializeField] private float speedEffectDuration;

    private PlayerActiveData activeData;
    public PlayerActiveData ActiveData
    {
        get { return activeData; }
        private set { activeData = value; }
    }

    [Header("OnHitVFX")]
    [SerializeField] private Material[] objectRenderer = new Material[2];
    [SerializeField] private Color damageColor;
    private int MorR; // Melee or Ranger
    private Color originalColor;

    //For healing 
    private bool enablePlayerEffect = false;
    private float particleOffset = 1f;
    private GameObject activePlayerParticle = null;

    void Start()
    {
        activeData = (PlayerActiveData)dataHolder.activeData;

        if (activeData == null)
        {
            Debug.Log("PLAYER DATA NOT FOUND");
            return;
        }

        MorR = 0;
        objectRenderer[0].SetColor("_EmissionColor", new Color(0, 0, 0));
        objectRenderer[1].SetColor("_EmissionColor", new Color(0, 0, 0));

        originalColor = objectRenderer[0].GetColor("_EmissionColor");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        DebugHandleMove();

        if (activeData.isInventoryOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (!activeData.isInventoryOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        HandlePlayerParticles();

        // --- THE DEBUG KEYBIND ---
        if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.tKey.wasPressedThisFrame)
        {
            Debug.LogWarning("DEBUG: Ouch! Taking 25 damage!");
            TakeDamage(25f);
        }

        if (activeData.currentClassType == CLASSTYPE.MELEE)
            MorR = 0;
        else
            MorR = 1;
    }

    private void HandlePlayerParticles()
    {
        if (activeData.activeParticleList.Count == 0)
            enablePlayerEffect = false;
        else
            enablePlayerEffect = true;

        foreach (ParticleData data in activeData.activeParticleList)
        {
            ObjectPoolManager.SPAWNABLE_TYPES particleType = data.particleType;

            switch (particleType)
            {
                case ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_HEALINGEFFECT:
                    data.activeParticle.transform.position = transform.position + Vector3.up * -particleOffset;
                    break;
                case ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_DAMAGEEFFECT:
                    data.activeParticle.transform.position = transform.position;
                    break;
                case ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_SMOKESOURCEEFFECT:
                    data.activeParticle.transform.position = transform.position;
                    data.activeParticle.transform.rotation = Quaternion.LookRotation(transform.forward);
                    break;
            }
        }
    }

    void DebugHandleMove()
    {
        if (activeData == null) return;

        if (!characterController.isGrounded)
        {
            activeData.isMoving = true;
            activeData.jumpVel.y -= 9.81f * Time.deltaTime * 3f;
        }
        else if (!activeData.isJumping)
        {
            activeData.jumpVel.y = 0;
        }

        activeData.isJumping = !characterController.isGrounded;

        if (!activeData.isMoving) return;

        float moveSpeed = activeData.currentMoveSpeed * activeData.currentSpeedMultiplier;
        Vector3 moveDirection = activeData.moveDirection.y * transform.forward + activeData.moveDirection.x * transform.right;

        if (activeData.currentPlayerState >= PlayersAnimStates.FIGHTER_RTL_SLASH)
        {
            return;
        }
        else if (moveDirection != new Vector3(0, 0, 0))
        {
            activeData.currentPlayerState = PlayersAnimStates.WALK;
        }

        Vector3 velocity = (moveDirection.normalized * moveSpeed + activeData.jumpVel) * Time.deltaTime;
        characterController.Move(velocity);
    }

    // --- HEALING UI FIX ---
    public void SetCurrentHealth(float newHealth)
    {
        activeData.currentHealth = Mathf.Clamp(newHealth, 0, activeData.maxHealth);
        Debug.Log("PLAYER CURRENT HEALTH: " + activeData.currentHealth);

        if (BattleUIManager.Instance != null)
        {
            BattleUIManager.Instance.UpdatePlayerHealthUI(activeData.currentHealth, activeData.maxHealth, activeData.currentClassType);
        }
    }

    public float GetCurrentHealth()
    {
        return activeData.currentHealth;
    }

    public void TriggerHealingEffect()
    {
        float offset = 1f;
        activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_HEALINGEFFECT;
        activeData.objectPoolSpawnData = new ObjectPoolSpawnData(transform.position + Vector3.up * -offset, Vector3.forward);
        activeData.isObjectPoolTriggered = true;
        enablePlayerEffect = true;
        ObjectPoolManager.Instance.HandleSpawnRequest(activeData);
    }

    public void SetDamageMultiplier(float damageMulti)
    {
        activeData.currentDamageMultiplier = damageMulti;
        StartCoroutine(DamageEffectCoroutine());
        activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_DAMAGEEFFECT;
        activeData.objectPoolSpawnData = new ObjectPoolSpawnData(transform.position, Vector3.up);
        activeData.isObjectPoolTriggered = true;
        enablePlayerEffect = true;
        ObjectPoolManager.Instance.HandleSpawnRequest(activeData);
    }

    public void SetSpeedMultiplier(float speedMulti)
    {
        activeData.currentSpeedMultiplier = speedMulti;
        StartCoroutine(SpeedEffectCoroutine());
        activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_SMOKESOURCEEFFECT;
        activeData.objectPoolSpawnData = new ObjectPoolSpawnData(transform.position, transform.forward);
        activeData.isObjectPoolTriggered = true;
        enablePlayerEffect = true;
        ObjectPoolManager.Instance.HandleSpawnRequest(activeData);
    }

    private IEnumerator DamageEffectCoroutine()
    {
        yield return new WaitForSeconds(damageAbilityDuration);
        activeData.currentDamageMultiplier = 1f;
    }

    private IEnumerator SpeedEffectCoroutine()
    {
        yield return new WaitForSeconds(speedEffectDuration);
        activeData.currentSpeedMultiplier = 1f;
    }

    // --- DAMAGE & DEATH SEQUENCE FIX ---
    public void TakeDamage(float Damage)
    {
        // 1. Check for defensive states first!
        if (activeData.isDefensive)
        {
            activeData.isDefensive = false;
            return;
        }

        // 2. Check for I-Frames or if already dead
        if (activeData.isDead || activeData.isInvincible) return;

        activeData.currentHealth -= Damage;

        // 3. The Death Check!
        if (activeData.currentHealth <= 0)
        {
            activeData.currentHealth = 0; // Clamped to 0 so the UI doesn't break!

            PlayerInputController inputController = GetComponentInParent<PlayerInputController>();

            if (inputController == null)
            {
                inputController = FindFirstObjectByType<PlayerInputController>();
            }

            if (inputController != null)
            {
                Debug.LogWarning("DEATH SIGNAL SENT!");
                inputController.HandleCharacterDeath();
            }
            else
            {
                Debug.LogError("CRITICAL ERROR: Could not find PlayerInputController!");
            }
        }

        // 4. Update the visual effects & UI
        StartCoroutine(TakeDamageEffect());

        if (AudioManager.instance != null) AudioManager.instance.Play("PlayerTakeDamage");

        if (BattleUIManager.Instance != null)
        {
            BattleUIManager.Instance.UpdatePlayerHealthUI(activeData.currentHealth, activeData.maxHealth, activeData.currentClassType);
        }
    }

    private IEnumerator TakeDamageEffect()
    {
        objectRenderer[MorR].SetColor("_EmissionColor", damageColor);
        float elapsedTime = 0f;
        while (elapsedTime < damageEffectDuration)
        {
            objectRenderer[MorR].SetColor("_EmissionColor", Color.Lerp(damageColor, originalColor, elapsedTime / damageEffectDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        objectRenderer[MorR].SetColor("_EmissionColor", originalColor);
    }
}