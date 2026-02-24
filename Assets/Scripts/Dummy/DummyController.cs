using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DummyController : MonoBehaviour
{
    private float maxHealth = 10f;
    [SerializeField] private float health;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask enemyLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        ActivateRagdoll(false);
    }

    private void ActivateRagdoll(bool active)
    {
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = !active;
        }
    }

    private void EnterRagdoll()
    {
        animator.enabled = false;
        ActivateRagdoll(true);
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            EnterRagdoll();
            OnDeath();
            return;
        }
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    private void OnDeath()
    {
        Collider[] allColliders = GetComponentsInChildren<Collider>();

        foreach (Collider col in allColliders)
        {
            col.excludeLayers |= enemyLayer;
        }
    }
}
