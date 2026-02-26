using UnityEngine;
using System.Collections;

public class DummyController : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Visual Feedback")]
    [SerializeField] private Renderer objectRenderer;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.15f;
    private Color originalColor;
    private Coroutine flashCoroutine;

    void Start()
    {
        // We set health to a high value, but we won't let it decrease
        health = 100f;

        if (objectRenderer != null)
        {
            // Store the original color so we can swap back
            originalColor = objectRenderer.material.color;
        }
    }

    public void TakeDamage(float dmg)
    {
        // 1. Infinite Health Logic: 
        // We show the "hit" but don't actually let health drop to 0
        Debug.Log($"Dummy hit for {dmg} damage!");

        // 2. Visual Feedback
        if (flashCoroutine != null) StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(HitFlash());
    }

    private IEnumerator HitFlash()
    {
        // Set to red
        objectRenderer.material.color = damageColor;

        // Wait for a split second
        yield return new WaitForSeconds(flashDuration);

        // Return to normal
        objectRenderer.material.color = originalColor;
    }

    public bool IsAlive()
    {
        return true; // Always returns true for your EnemyController's detection
    }
}
