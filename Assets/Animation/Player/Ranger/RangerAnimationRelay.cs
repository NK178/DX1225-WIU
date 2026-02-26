using UnityEngine;

public class RangerAnimationRelay : MonoBehaviour
{
    private RangerMechanics mechanics;

    private void Start()
    {
        mechanics = GetComponentInParent<RangerMechanics>();
    }
    public void AE_ShootSeedLeft()
    {
        if (mechanics != null) mechanics.AE_ShootSeedLeft();
    }

    public void AE_ShootSeedRight()
    {
        if (mechanics != null) mechanics.AE_ShootSeedRight();
    }

    public void AE_PlayFootstep()
    {
        if (mechanics != null) mechanics.AE_PlayFootstep();
    }

    public void AE_EnableLaser()
    {
        if (mechanics != null) mechanics.AE_EnableLaser();
    }
}