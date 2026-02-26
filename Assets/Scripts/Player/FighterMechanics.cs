using System.Collections.Generic;
using System;
using UnityEngine;

public enum FighterState
{
    IDLE = 0,
    WALK,
    ATTACK1,
    ATTACK2, 
    ATTACK3,
    ABILITY,
    DEFENSIVE,
}

[System.Serializable]
public struct FighterAvatar
{
    public FighterState State;
    public Avatar avatar;
}

public class FighterMechanics : BaseClassMechanics
{
    public FighterClassData fighterClassData;
    [SerializeField] private AttackHandler SwordHandler;

    private float AtkCDTimer;
    private float ParryCDTimer;
    private int combo;

    [SerializeField] private Animator animator;
    [SerializeField] private List<FighterAvatar> FightAvas;
    [HideInInspector] public FighterAvatar[] FighterAvatars;
    public Avatar currentAvatar;
    private AnimatorStateInfo stateInfo;

    private void Start()
    {
        FighterAvatars = new FighterAvatar[Enum.GetValues(typeof(FighterState)).Length];
        for (int i = 0; i < FightAvas.Count; i++)
        {
            FighterAvatars[(int)FightAvas[i].State].avatar = FightAvas[i].avatar;
        }
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

        if (animator != null)
        {
            switch(combo)
            {
                case 0:
                    currentAvatar = FighterAvatars[(int)FighterState.ATTACK1].avatar;
                    animator.avatar = currentAvatar;
                    animator.CrossFadeInFixedTime("Fighter_RtL_Slash", 0.2f);
                    combo++;
                    break;
                case 1:
                    currentAvatar = FighterAvatars[(int)FighterState.IDLE].avatar;
                    animator.avatar = currentAvatar;
                    animator.CrossFadeInFixedTime("Fighter_LtR_Slash", 0.2f);
                    combo++;
                    break;
                case 2:
                    //currentAvatar = FighterAvatars[(int)FighterState.IDLE].avatar;
                    //animator.avatar = currentAvatar;
                    animator.CrossFadeInFixedTime("Fighter_Thrust", 0.2f);
                    combo = 0;
                    break;
            }
        }
            SwordHandler.EnableCollider("Sword");
        if (AudioManager.instance != null) AudioManager.instance.Play("FighterAttack");
    }

    public override void HandleDefense()
    {
        if (Time.time < ParryCDTimer) return;
        activeData.isDefensive = true;
        if (animator != null) animator.CrossFadeInFixedTime("Fighter_Block", 0.2f);
        ParryCDTimer = Time.time + fighterClassData.parryCD;
        if (AudioManager.instance != null) AudioManager.instance.Play("FighterParry");
        Debug.Log("Parry Executed!");
    }

    public override void HandleAbility()
    {
        // Slash logic
        if (animator != null) animator.CrossFadeInFixedTime("Fighter_Ability", 0.2f);
        if (AudioManager.instance != null) AudioManager.instance.Play("FighterAbility");
    }
}