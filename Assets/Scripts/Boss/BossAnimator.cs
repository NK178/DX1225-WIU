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

    [SerializeField] private AttackHandler DEBUGAttackHandler;

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

    public string GetAnimStateName()
    {
        string animState = "BossIdle";
        if (activeData.BAnimState == BossActiveData.BossAnimStates.IDLE && !activeData.isAttacking)
        {
            animState = "BossIdle";
        }
        else if (activeData.isAttacking)
        {
            if (activeData.BAnimState == BossActiveData.BossAnimStates.KNIFE_ATTACK)
                animState = "BossKnifeAttack";
        }
        return animState;
    }

    //the gooder version
    public void OnStateChanged()
    {
        string targetAnimation = GetAnimStateName();
        Debug.Log("CHANGE ANIMATION: " + targetAnimation);

        animator.CrossFadeInFixedTime(targetAnimation, 0.2f);
    }

    //public void OnStateChanged()
    //{
    //    Debug.Log("ANIM STATE CHANGED");
    //    string targetAnimation = ((BossActiveData.BossAnimStates)GetAnimState()).ToString();
    //    Debug.Log("ANIMATION: " + targetAnimation); 


    //    if (activeData.BAnimState == BossActiveData.BossAnimStates.IDLE && !activeData.isMoving)
    //    {
    //        activeData.isAttacking = false;
    //        //animator.CrossFadeInFixedTime(targetAnimation, 0.2f);
    //        //currentAnimation = targetAnimation;
    //    }
    //    else if (activeData.isAttacking)
    //    {
    //        //if (activeData.BAnimState == BossActiveData.BossAnimStates.KNIFE_ATTACK)
    //        //{
    //        //    //playanim

    //        //}
    //        //if (animator == null)
    //        //{
    //        //    Debug.Log("No animator!");
    //        //    return;
    //        //}
    //        switch (activeData.BAnimState)
    //        {
    //            case BossActiveData.BossAnimStates.KNIFE_ATTACK:
    //                DEBUGAttackHandler.EnableCollider("KnifeCollider");
    //                //activeData.isAttacking = false;
    //                //animator.CrossFadeInFixedTime(targetAnimation, 0.2f);
    //                break;
    //            case BossActiveData.BossAnimStates.HANDSWIPE_ATTACK:
    //                DEBUGAttackHandler.EnableCollider("HandCollider");
    //                //activeData.isAttacking = false;
    //                //animator.CrossFadeInFixedTime(targetAnimation, 0.2f);
    //                break;
    //            case BossActiveData.BossAnimStates.TRIPLEKNIFE_ATTACK:
    //                DEBUGAttackHandler.EnableCollider(targetAnimation);
    //                //animator.CrossFadeInFixedTime(targetAnimation, 0.2f);
    //                break;
    //            case BossActiveData.BossAnimStates.HANDSLAM_ATTACK:
    //                DEBUGAttackHandler.EnableCollider("HandCollider");
    //                //activeData.isAttacking = false;
    //                //animator.CrossFadeInFixedTime(targetAnimation, 0.2f);
    //                break;
    //            case BossActiveData.BossAnimStates.FLYSWATTER_ATTACK:
    //                DEBUGAttackHandler.EnableCollider(targetAnimation);
    //                //animator.CrossFadeInFixedTime(targetAnimation, 0.2f);
    //                break;
    //            case BossActiveData.BossAnimStates.CLAWGRAB_ATTACK:
    //                DEBUGAttackHandler.EnableCollider("ClawCollider");
    //                //activeData.isAttacking = false;
    //                //animator.CrossFadeInFixedTime(targetAnimation, 0.2f);
    //                break;
    //            case BossActiveData.BossAnimStates.SUGARCANEMISSILES_ATTACK:
    //                DEBUGAttackHandler.EnableCollider(targetAnimation);
    //                //animator.CrossFadeInFixedTime(targetAnimation, 0.2f);
    //                break;
    //            case BossActiveData.BossAnimStates.FRUITAIRSTRICK_ATTACK:
    //                DEBUGAttackHandler.EnableCollider(targetAnimation);
    //                //animator.CrossFadeInFixedTime(targetAnimation, 0.2f);
    //                break;
    //        }
    //    }
    //}
}
