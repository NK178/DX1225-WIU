using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

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

    private PlayerActiveData activeData;
    public PlayerActiveData ActiveData
    {
        get { return activeData; }
        private set { activeData = value; }
    }

    [Header("OnHitVFX")]
    [SerializeField] private Renderer objectRenderer;
    [SerializeField] private Color damageColor;
    [SerializeField] private float damageEffectDuration;
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
        if (activeData == null || !activeData.isMoving)
            return;

        // reads whatever speed the active class (or a dodge roll) has set
        float moveSpeed = activeData.currentMoveSpeed;

        // Movement changed a bit to fit
        Vector3 moveDirection = activeData.moveDirection.y * transform.forward + activeData.moveDirection.x * transform.right;
        //Vector3 moveDirection = new Vector3(activeData.moveDirection.x, 0, activeData.moveDirection.y);

        Vector3 velocity = moveDirection.normalized * moveSpeed * Time.deltaTime;

        characterController.Move(velocity);
    }

    public void SetCurrentHealth(float newHealth)
    {

    }

    //public float GetCurrentHealth()
    //{
    //    //return 
    //}

    public void TakeDamage(float Damage)
    {
        activeData.currentHealth -= Damage;
        StartCoroutine(TakeDamageEffect());
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