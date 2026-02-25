using UnityEngine;

public class RangerAnimationRelay : MonoBehaviour
{
    private RangerMechanics mechanics;

    private void Start()
    {
        mechanics = GetComponentInParent<RangerMechanics>();
    }
    public void AE_ShootSeed()
    {
        if (mechanics != null) mechanics.AE_ShootSeed();
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