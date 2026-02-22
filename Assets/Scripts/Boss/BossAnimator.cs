using System.Collections;
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

    //private void OnEnable()
    //{
    //    StartCoroutine(WaitForData());
    //}

    //private IEnumerator WaitForData()
    //{
    //    while (dataHolder.activeData == null)
    //        yield return null;

    //    Debug.Log("FOUDN DATA");
    //    activeData = (BossActiveData)dataHolder.activeData;
    //    activeData.onStateChanged += OnStateChanged;
    //}


    public int GetAnimState()
    {
        return (int)activeData.BAnimState;
    }



    public void OnStateChanged()
    {
        Debug.Log("ANIM STATE CHANGED");
        string targetAnimation = ((BossActiveData.BossAnimStates)GetAnimState()).ToString();
        Debug.Log("ANIMATION: " + targetAnimation);


        if (activeData.BAnimState == BossActiveData.BossAnimStates.IDLE && !activeData.isMoving)
        {
            activeData.isAttacking = false;
            //animator.CrossFadeInFixedTime(targetAnimation, 0.2f);
            //currentAnimation = targetAnimation;
        }
        else if (activeData.isAttacking)
        {
            //if (activeData.BAnimState == BossActiveData.BossAnimStates.KNIFE_ATTACK)
            //{
            //    //playanim

            //}
            switch (activeData.BAnimState)
            {
                case BossActiveData.BossAnimStates.KNIFE_ATTACK:
                    animator.CrossFadeInFixedTime(targetAnimation, 0.2f);
                    break;
                case BossActiveData.BossAnimStates.HANDSWIPE_ATTACK:
                    animator.CrossFadeInFixedTime(targetAnimation, 0.2f);
                    break;
                case BossActiveData.BossAnimStates.TRIPLEKNIFE_ATTACK:
                    animator.CrossFadeInFixedTime(targetAnimation, 0.2f);
                    break;
                case BossActiveData.BossAnimStates.HANDSLAM_ATTACK:
                    animator.CrossFadeInFixedTime(targetAnimation, 0.2f);
                    break;
                case BossActiveData.BossAnimStates.FLYSWATTER_ATTACK:
                    animator.CrossFadeInFixedTime(targetAnimation, 0.2f);
                    break;
                case BossActiveData.BossAnimStates.CLAWGRAB_ATTACK:
                    animator.CrossFadeInFixedTime(targetAnimation, 0.2f);
                    break;
                case BossActiveData.BossAnimStates.SUGARCANEMISSILES_ATTACK:
                    animator.CrossFadeInFixedTime(targetAnimation, 0.2f);
                    break;
                case BossActiveData.BossAnimStates.FRUITAIRSTRICK_ATTACK:
                    animator.CrossFadeInFixedTime(targetAnimation, 0.2f);
                    break;
            }
        }
    }
}
