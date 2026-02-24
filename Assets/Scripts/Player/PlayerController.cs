using System.Collections;
using UnityEngine;

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
    [SerializeField] private float speedEffectDuration; 


    private PlayerActiveData activeData;
    public PlayerActiveData ActiveData
    {
        get { return activeData; }
        private set { activeData = value; }
    }

    [Header("OnHitVFX")]
    [SerializeField] private Renderer objectRenderer;
    [SerializeField] private Color damageColor;

    private Color originalColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        activeData = (PlayerActiveData)dataHolder.activeData;

        if (activeData == null)
        {
            Debug.Log("PLAYER DATA NOT FOUND");
            return;
        }

        originalColor = objectRenderer.material.color;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // PlayerController no longer forces the speed variable every frame,
        // It just executes the movement based on whatever the active data currently says
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


        Vector3 velocity = (moveDirection.normalized * moveSpeed + activeData.jumpVel) * Time.deltaTime;

        characterController.Move(velocity);
        //Debug.Log(velocity);
    }

    public void SetCurrentHealth(float newHealth)
    {
        activeData.currentHealth = newHealth;
        Debug.Log("PLAYER CURRENT HEALTH: " + activeData.currentHealth);
    }

    public float GetCurrentHealth()
    {
        return activeData.currentHealth;
    }

    public void SetDamageMultiplier(float damageMulti)
    {
        activeData.currentDamageMultiplier = damageMulti;
        StartCoroutine(DamageEffectCoroutine());
        Debug.Log("PLAYER CURRENT MULTIPLIER: " + activeData.currentDamageMultiplier);
    }

    public void SetSpeedMultiplier(float speedMulti)
    {
        activeData.currentMoveSpeed = speedMulti;
        StartCoroutine(SpeedEffectCoroutine());

        Debug.Log("PLAYER SPEED MULTI: " + activeData.currentMoveSpeed);
    }

    private IEnumerator DamageEffectCoroutine()
    {
        yield return new WaitForSeconds(damageEffectDuration);
        activeData.currentDamageMultiplier = 1f;
    }

    private IEnumerator SpeedEffectCoroutine()
    {
        yield return new WaitForSeconds(speedEffectDuration);
        activeData.currentMoveSpeed = 1f;
    }
    //public float GetCurrentHealth()
    //{
    //    //return 
    //}

    public void TakeDamage(float Damage)
    {
        activeData.currentHealth -= Damage;
        StartCoroutine(TakeDamageEffect());
        if (BattleUIManager.Instance != null)
        {
            BattleUIManager.Instance.UpdatePlayerHealthUI(activeData.currentHealth, activeData.maxHealth, activeData.currentClassType);
        }
    }
    private IEnumerator TakeDamageEffect()
    {
        // Set to damage color instantly
        objectRenderer.material.color = damageColor;
        // Gradually transition back to the original color over time
        float elapsedTime = 0f;
        while (elapsedTime < damageEffectDuration)
        {
            objectRenderer.material.color = Color.Lerp(damageColor,
            originalColor, elapsedTime / damageEffectDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Ensure the final color is reset to the original
        objectRenderer.material.color = originalColor;
    }

}