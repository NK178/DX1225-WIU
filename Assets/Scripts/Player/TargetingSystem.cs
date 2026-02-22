using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    [Header("Settings")]
    public float targetingRadius = 15f;
    public LayerMask enemyLayer;
    public Transform playerModel;

    public Transform currentTarget { get; private set; }

    private List<Transform> availableTargets = new List<Transform>();
    private int currentTargetIndex = -1;

    public void ToggleLockOn()
    {
        Debug.Log("--> BUTTON PRESSED: ToggleLockOn() triggered!");

        if (currentTarget != null)
        {
            Debug.Log("ACTION: Target already exists. Clearing current target.");
            ClearTarget();
        }
        else
        {
            Debug.Log("ACTION: No target currently locked. Scanning for enemies...");
            FindAvailableTargets();

            if (availableTargets.Count > 0)
            {
                currentTargetIndex = 0;
                currentTarget = availableTargets[currentTargetIndex];
                Debug.Log($"SUCCESS: Locked onto: {currentTarget.name}");
            }
            else
            {
                Debug.LogWarning("FAILED: Search finished, but 0 valid targets were added to the list.");
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

        Debug.Log($"PHYSICS CHECK: Casting OverlapSphere at {transform.position} with radius {targetingRadius}. LayerMask value: {enemyLayer.value}");

        Collider[] hits = Physics.OverlapSphere(transform.position, targetingRadius, enemyLayer);

        Debug.Log($"PHYSICS RESULT: OverlapSphere physically touched {hits.Length} colliders on that layer.");

        foreach (Collider hit in hits)
        {
            Debug.Log($"FOUND: Added {hit.gameObject.name} to available targets.");
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

        // Handle Player Rotation
        if (currentTarget != null && playerModel != null)
        {
            Vector3 direction = (currentTarget.position - playerModel.position).normalized;

            // Keep the rotation strictly horizontal so the player doesn't tilt up/down
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                playerModel.rotation = Quaternion.Slerp(playerModel.rotation, lookRotation, Time.deltaTime * 15f);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, targetingRadius);
    }
}