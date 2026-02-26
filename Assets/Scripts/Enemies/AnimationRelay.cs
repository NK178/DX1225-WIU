using UnityEngine;

public class AnimationRelay : MonoBehaviour
{
    private EnemyController controller;

    void Start()
    {
        controller = GetComponentInParent<EnemyController>();

        if (controller != null)
            Debug.Log("CONTROLLER ADDED");
        else
            Debug.LogError("CONTROLLER NULL");
    }

    public void SetAttackTrue() => controller.SetAttack(true);
    public void SetAttackFalse() => controller.SetAttack(false);
}
