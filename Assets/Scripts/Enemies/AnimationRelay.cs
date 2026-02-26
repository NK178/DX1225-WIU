using UnityEngine;

public class AnimationRelay : MonoBehaviour
{
    private OldEnemyController controller;

    void Start()
    {
        controller = GetComponentInParent<OldEnemyController>();

        if (controller != null)
            Debug.Log("CONTROLLER ADDED");
        else
            Debug.LogError("CONTROLLER NULL");
    }

    public void SetAttackTrue() => controller.SetAttack(true);
    public void SetAttackFalse() => controller.SetAttack(false);
}
