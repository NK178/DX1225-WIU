using System.Collections;
using UnityEngine;
using static PlayerActiveData;

//can be reused with player as well
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




    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

        //activeParticleList = new List<ParticleData>();    

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // PlayerController no longer forces the speed variable every frame,
        // It just executes the movement based on whatever the active data currently says
        DebugHandleMove();


        Debug.Log("CURRENT HP: " + activeData.currentHealth);

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

        Debug.Log("Inventory: " + activeData.isInventoryOpen);

        HandlePlayerParticles();
        if (UnityEngine.InputSystem.Keyboard.current.tKey.wasPressedThisFrame)
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

            switch(particleType)
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

    //Testing function since no animation move
    void DebugHandleMove()
    {
        if (activeData == null)
            return;
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

        if (!activeData.isMoving)
            return;

        // reads whatever speed the active class (or a dodge roll) has set
        float moveSpeed = activeData.currentMoveSpeed * activeData.currentSpeedMultiplier;

        // Movement changed a bit to fit
        Vector3 moveDirection = activeData.moveDirection.y * transform.forward + activeData.moveDirection.x * transform.right;
        //Vector3 moveDirection = new Vector3(activeData.moveDirection.x, 0, activeData.moveDirection.y);
        //if (characterController.isGrounded)

        if (activeData.currentPlayerState >= PlayersAnimStates.FIGHTER_RTL_SLASH)
        {
            return;
        }
        else if (moveDirection != new Vector3(0,0,0))
        {
            activeData.currentPlayerState = PlayersAnimStates.WALK;
        }

        Vector3 velocity = (moveDirection.normalized * moveSpeed + activeData.jumpVel) * Time.deltaTime;

        characterController.Move(velocity);
        //Debug.Log(velocity);
    }

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
        Debug.Log("PLAYER SPEED MULTI: " + activeData.currentMoveSpeed);
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

    public void TakeDamage(float Damage)
    {
        if (activeData.isDefensive)
        {
            activeData.isDefensive = false;
            return;
        }
       
        if (activeData.isDead || activeData.isInvincible) return;

        activeData.currentHealth -= Damage;

      
        if (activeData.currentHealth <= 0)
        {
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

        StartCoroutine(TakeDamageEffect());

        if (AudioManager.instance != null) AudioManager.instance.Play("PlayerTakeDamage");

        if (BattleUIManager.Instance != null)
        {
            BattleUIManager.Instance.UpdatePlayerHealthUI(activeData.currentHealth, activeData.maxHealth, activeData.currentClassType);
        }
    }
    private IEnumerator TakeDamageEffect()
    {
        // Set to damage color instantly
        objectRenderer[MorR].SetColor("_EmissionColor", damageColor);
        // Gradually transition back to the original color over time
        float elapsedTime = 0f;
        while (elapsedTime < damageEffectDuration)
        {
            objectRenderer[MorR].SetColor("_EmissionColor", Color.Lerp(damageColor,
            originalColor, elapsedTime / damageEffectDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Ensure the final color is reset to the original
        objectRenderer[MorR].SetColor("_EmissionColor", originalColor);
    }

}