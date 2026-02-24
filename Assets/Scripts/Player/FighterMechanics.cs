using UnityEngine;

public class FighterMechanics : BaseClassMechanics
{
    public FighterClassData fighterClassData;
    [SerializeField] private AttackHandler SwordHandler;

    private float AtkCDTimer;
    private float ParryCDTimer;

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
        SwordHandler.EnableCollider("Sword");
        if (AudioManager.instance != null) AudioManager.instance.Play("FighterAttack");
    }

    public override void HandleDefense()
    {
        if (Time.time < ParryCDTimer) return;

        ParryCDTimer = Time.time + fighterClassData.parryCD;
        if (AudioManager.instance != null) AudioManager.instance.Play("FighterParry");
        Debug.Log("Parry Executed!");
    }

    public override void HandleAbility()
    {
        // Slash logic
        if (AudioManager.instance != null) AudioManager.instance.Play("FighterAbility");
    }
}