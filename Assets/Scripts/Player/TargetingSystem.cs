using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    [Header("Settings")]
    public float targetingRadius = 15f;
    public LayerMask enemyLayer;

    public Transform currentTarget { get; private set; }

    private List<Transform> availableTargets = new List<Transform>();
    private int currentTargetIndex = -1;
    public void ToggleLockOn()
    {
        if (currentTarget != null)
        {
            ClearTarget();
        }
        else
        {
            FindAvailableTargets();
            if (availableTargets.Count > 0)
            {
                currentTargetIndex = 0;
                currentTarget = availableTargets[currentTargetIndex];
                Debug.Log($"Locked onto: {currentTarget.name}");
            }
        }
    }

    // Cycles left (-1) or right (+1)
    public void SwitchTarget(int direction)
    {
        if (availableTargets.Count <= 1) return;

        // Clean up dead/destroyed enemies before switching
        availableTargets.RemoveAll(t => t == null || !t.gameObject.activeInHierarchy);

        if (availableTargets.Count == 0)
        {
            ClearTarget();
            return;
        }

        // Loop through the list
        currentTargetIndex += direction;
        if (currentTargetIndex >= availableTargets.Count) currentTargetIndex = 0;
        else if (currentTargetIndex < 0) currentTargetIndex = availableTargets.Count - 1;

        currentTarget = availableTargets[currentTargetIndex];
        Debug.Log($"Switched target to: {currentTarget.name}");
    }

    private void FindAvailableTargets()
    {
        availableTargets.Clear();
        Collider[] hits = Physics.OverlapSphere(transform.position, targetingRadius, enemyLayer);

        foreach (Collider hit in hits)
        {
            availableTargets.Add(hit.transform);
        }

        // Sort enemies by distance so you always lock onto the closest one first
        availableTargets.Sort((a, b) =>
            Vector3.Distance(transform.position, a.position).CompareTo(Vector3.Distance(transform.position, b.position))
        );
    }

    public void ClearTarget()
    {
        currentTarget = null;
        currentTargetIndex = -1;
        availableTargets.Clear();
        Debug.Log("Lock-on cleared.");
    }

    private void Update()
    {
        // Auto-clear the target if the enemy dies or is disabled
        if (currentTarget != null && !currentTarget.gameObject.activeInHierarchy)
        {
            ClearTarget();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, targetingRadius);
    }
}