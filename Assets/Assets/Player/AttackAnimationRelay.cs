using UnityEngine;

public class AttackAnimationRelay : MonoBehaviour
{
    private AttackHandler handler;

    private void Start()
    {
        handler = GetComponentInParent<AttackHandler>();
    }

    public void EnableCollider(string atkName)
    {
        if (handler != null) handler.EnableCollider(atkName);
    }

    public void DisableCollider(string atkName)
    {
        if (handler != null) handler.DisableCollider(atkName);
    }
}
