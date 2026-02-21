using UnityEngine;

public class FighterMechanics : BaseClassMechanics
{

    [SerializeField] private FighterClassData fighterClassData;
    [SerializeField] private AttackHandler SwordHandler;

    private float AtkCDTimer;
    private float ParryCDTimer;

    public override void EquipClass()
    {
        if (fighterClassData == null)
        {
            Debug.LogError("FIGHTER CLASS DATA NOT FOUND!");
            return;
        }
        activeData.currentMoveSpeed = fighterClassData.moveSpeed;
        activeData.currentClassType = fighterClassData.classType;
    }

    public override void HandleAbility()
    {
        if (activeData == null)
        {
            Debug.LogError("ACTIVE DATA NOT FOUND!");
            return;
        }
        if (fighterClassData == null)
        {
            Debug.LogError("FIGHTER CLASS DATA NOT FOUND!");
            return;
        }
    }

    public override void HandleAttack()
    {
        if (Time.time < AtkCDTimer)
            return;
        AtkCDTimer = Time.time + fighterClassData.AtkCD;
        SwordHandler.EnableCollider("Sword");
    }

    public override void HandleDefense()
    {
    }
}
