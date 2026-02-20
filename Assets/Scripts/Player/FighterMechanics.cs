using UnityEngine;

public class FighterMechanics : BaseClassMechanics
{

    [SerializeField] private FighterClassData fighterClassData;

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
    }

    public override void HandleDefense()
    {
    }
}
