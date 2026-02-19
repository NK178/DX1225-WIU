using UnityEngine;

public abstract class BaseClassMechanics : MonoBehaviour
{
    protected PlayerActiveData activeData;

    // called by PlayerController to pass in the active data
    public virtual void Initialize(PlayerActiveData data)
    {
        activeData = data;
    }

    // override based on player classes
    public abstract void HandleAttack();
    public abstract void HandleDefense();
    public abstract void HandleAbility();
}