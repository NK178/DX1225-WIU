using UnityEngine;

public class DummyController : MonoBehaviour
{
    private float maxHealth = 100f;
    [SerializeField] private float health;
    [SerializeField] private Animator animator;

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
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyBullet") || other.CompareTag("EnemyBullet"))
        {
            Debug.LogWarning("DUMMY HIT!");
            TakeDamage(10);
            other.gameObject.SetActive(false);
        }
    }
}
