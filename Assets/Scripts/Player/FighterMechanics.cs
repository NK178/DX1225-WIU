using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterMechanics : BaseClassMechanics
{
    public FighterClassData fighterClassData;
    //[SerializeField] private AttackHandler SwordHandler;

    private float AtkCDTimer;
    private float ParryCDTimer;
    private int combo;

    [SerializeField] private Animator animator;

    private bool isWalkLooping;

    private List<IEnumerator> _attackQueue = new List<IEnumerator>();
    private bool isSheathing;

    [SerializeField] private PlayerActiveData.PlayersAnimStates StateChecker;

    private void Start()
    {
        //FighterAvatars = new FighterAvatar[Enum.GetValues(typeof(FighterState)).Length];
        //for (int i = 0; i < FightAvas.Count; i++)
        //{
        //    FighterAvatars[(int)FightAvas[i].State].avatar = FightAvas[i].avatar;
        //}
        isWalkLooping = false;
        isSheathing = false;
    }

    private void Update()
    {
        if (BattleUIManager.Instance != null && fighterClassData != null)
        {
            // Animate Sword UI
            float atkRemaining = Mathf.Max(0, AtkCDTimer - Time.time);
            BattleUIManager.Instance.UpdateCooldownUI(BattleUIManager.Instance.swordCooldownImage, atkRemaining, fighterClassData.AtkCD);

            // Animate Parry Shield UI
            float parryRemaining = Mathf.Max(0, ParryCDTimer - Time.time);
            BattleUIManager.Instance.UpdateCooldownUI(BattleUIManager.Instance.parryCooldownImage, parryRemaining, fighterClassData.parryCD);
        }

        // Weird way of doing things (Prevent a constant call of moving)
        if (activeData.currentPlayerState == PlayerActiveData.PlayersAnimStates.WALK || activeData.currentPlayerState == PlayerActiveData.PlayersAnimStates.IDLE)
        {
            activeData.isAttacking = false;
            activeData.isDefensive = false;
        }

        if (activeData.currentPlayerState == PlayerActiveData.PlayersAnimStates.WALK && !isWalkLooping)
        {
            animator.CrossFadeInFixedTime(activeData.currentPlayerState.ToString(), 0.2f);
            activeData.isAttacking = false;
            activeData.isDefensive = false;
            isWalkLooping = true;
            isSheathing = false;
        }
        else if (activeData.currentPlayerState == PlayerActiveData.PlayersAnimStates.IDLE && isWalkLooping)
        {
            animator.CrossFadeInFixedTime(activeData.currentPlayerState.ToString(), 0.2f);
            activeData.isAttacking = false;
            activeData.isDefensive = false;
            isWalkLooping = false;
            isSheathing = false;
        }

        if (activeData.currentPlayerState == PlayerActiveData.PlayersAnimStates.FIGHTER_SHEATH && !isSheathing)
        {
            animator.CrossFadeInFixedTime(activeData.currentPlayerState.ToString(), 0.2f);
            activeData.isAttacking = false;
            activeData.isDefensive = false;
            isSheathing = true;
        }

        StateChecker = activeData.currentPlayerState;
    }

    public override void EquipClass()
    {
        if (fighterClassData == null) return;

        activeData.currentMoveSpeed = fighterClassData.moveSpeed;
        activeData.currentClassType = fighterClassData.classType;
    }

    public override void HandleAttack()
    {
        if (Time.time < AtkCDTimer) return;

        AtkCDTimer = Time.time + fighterClassData.AtkCD;

        if (animator != null) StartCoroutine(PerformAttack());
        if (AudioManager.instance != null) AudioManager.instance.Play("FighterAttack");
    }

    public override void HandleDefense()
    {
        if (Time.time < ParryCDTimer) return;
        ParryCDTimer = Time.time + fighterClassData.parryCD;
        Debug.Log("Parry Executed!");
        StartCoroutine(DefenseRoutine());
    }

    private IEnumerator DefenseRoutine()
    {
        activeData.currentPlayerState = PlayerActiveData.PlayersAnimStates.FIGHTER_DEFENSIVE;
        activeData.isDefensive = true;

        if (animator != null) animator.CrossFadeInFixedTime(activeData.currentPlayerState.ToString(), 0.2f);
        if (AudioManager.instance != null) AudioManager.instance.Play("FighterParry");

        // Freezes the player for exactly 0.5 seconds for the parry, then unlocks movement
        yield return new WaitForSeconds(0.5f);

        activeData.isDefensive = false;
        activeData.currentPlayerState = PlayerActiveData.PlayersAnimStates.IDLE;
    }

    public override void HandleAbility()
    {
        StartCoroutine(AbilityRoutine());
    }

    private IEnumerator AbilityRoutine()
    {
        activeData.currentPlayerState = PlayerActiveData.PlayersAnimStates.FIGHTER_ABILITY;

        if (animator != null) animator.CrossFadeInFixedTime(activeData.currentPlayerState.ToString(), 0.2f);
        if (AudioManager.instance != null) AudioManager.instance.Play("FighterAbility");

        yield return new WaitForSeconds(0.2f); // Give animator time to transition into the state

        // Wait until the ability animation finishes, then unlock movement
        while (!IsCurrentAnimationReadyForNextStep((activeData.currentPlayerState).ToString()))
        {
            yield return null;
        }

        activeData.currentPlayerState = PlayerActiveData.PlayersAnimStates.IDLE;
    }

    private IEnumerator PerformAttack()
    {
        isSheathing = false;
        isWalkLooping = true;
        switch (combo)
        {
            case 0:
                activeData.currentPlayerState = PlayerActiveData.PlayersAnimStates.FIGHTER_RTL_SLASH;
                animator.CrossFadeInFixedTime(activeData.currentPlayerState.ToString(), 0.2f);
                break;
            case 1:
                activeData.currentPlayerState = PlayerActiveData.PlayersAnimStates.FIGHTER_LTR_SLASH;
                animator.CrossFadeInFixedTime(activeData.currentPlayerState.ToString(), 0.2f);
                break;
            case 2:
                activeData.currentPlayerState = PlayerActiveData.PlayersAnimStates.FIGHTER_THRUST;
                animator.CrossFadeInFixedTime(activeData.currentPlayerState.ToString(), 0.2f);
                break;
        }

        activeData.isAttacking = true;
        combo++;

        while (!IsCurrentAnimationReadyForNextStep((activeData.currentPlayerState).ToString()))
        {
            yield return null;
        }

        if (combo >= _attackQueue.Count || combo >= 3)
        {
            ResetCombo();
        }
        else
        {
            StartCoroutine(_attackQueue[combo]);
        }
    }

    private bool IsCurrentAnimationReadyForNextStep(string name)
    {
        // Check if the current animation has played enough to transition
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime >= 1f && stateInfo.IsName(name);
    }

    private void ResetCombo()
    {
        _attackQueue.Clear();
        activeData.currentPlayerState = PlayerActiveData.PlayersAnimStates.FIGHTER_SHEATH;
        combo = 0;
        activeData.isAttacking = false;
    }
}