using UnityEngine;

public class RollingPinProjectile : MonoBehaviour
{

    [SerializeField] private float rollingPinMoveSpeed;
    [SerializeField] private float rollingPinRotateSpeed;
    [SerializeField] private GameObject model;

    private float modelRotX;
    private float modelRotY;
    private float modelRotZ;

    void Start()
    {
        modelRotX = model.transform.localEulerAngles.x;
        modelRotY = model.transform.localEulerAngles.y;
        modelRotZ = model.transform.localEulerAngles.z;
    }

    void Update()
    {
        transform.position += transform.forward * rollingPinMoveSpeed * Time.deltaTime;

        modelRotX += rollingPinRotateSpeed * Time.deltaTime;
        model.transform.localRotation = Quaternion.Euler(modelRotX, modelRotY, modelRotZ);
    }
}
