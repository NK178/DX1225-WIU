using UnityEngine;

// Klaus Phase 1: Mechanical Knife Attack & Hand Swipe Attack
// Ainsley Phase 2: Hand Slam, Fly Swatter Attack, Claw Grab, Sugarcane Missiles and Fruit Air Strike

// Klaus

public class BossAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private DataHolder dataHolder;

    private BossActiveData activeData;


    private void Start()
    {
        if (animator == null)
            Debug.Log("ANIMATOR NOT FOUND!");

        activeData = (BossActiveData)dataHolder.activeData;

        activeData.onStateChanged += OnStateChanged;
    }


    public int GetAnimState()
    {
        return (int)activeData.BAnimState;
    }

    public void OnStateChanged()
    {
        string targetAnimation = ((BossActiveData.BossAnimStates)GetAnimState()).ToString();
        if (activeData.isAttacking)
        {
            if (activeData.BAnimState == BossActiveData.BossAnimStates.KNIFE_ATTACK)
            {
                //playanim

            }
        }
    }
}
