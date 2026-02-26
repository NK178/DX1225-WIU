using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable, VolumeComponentMenu("Post-processing/Custom/OutlineColor")]
public class OutlineColorVolume : VolumeComponent, IPostProcessComponent
{
    public ColorParameter tintColor = new ColorParameter(Color.white);
    public ColorParameter outlineColor = new ColorParameter(Color.black);
    public ClampedFloatParameter threshold = new ClampedFloatParameter(0f, 0f, 10f);

    public bool IsActive() => threshold.value > 0f || tintColor.value != Color.white;
    public bool IsTileCompatible() => false;
}