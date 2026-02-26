using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private static readonly int isMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int attackHash = Animator.StringToHash("Attacking");

    public void SetMoving(bool moving)
    {
        if (animator != null)
            animator.SetBool(isMovingHash, moving);
    }

    public void TriggerAttack()
    { 
        if (animator != null)
            animator.SetTrigger(attackHash);
    }
}
