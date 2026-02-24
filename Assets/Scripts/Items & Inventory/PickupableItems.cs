using System;
using Unity.VisualScripting;
using UnityEngine;

public class PickupableItem : MonoBehaviour
{

    [SerializeField] private ItemData itemData;
    [SerializeField] private GameObject spriteObject;
    [SerializeField] private float rotationSpeed;

    private float currentAngle; 

    public event Action onPickedUpEvent;

    private void Start()
    {
        currentAngle = spriteObject.transform.rotation.eulerAngles.y; 
    }

    private void Update()
    {

        currentAngle += rotationSpeed * Time.deltaTime;

        spriteObject.transform.rotation = Quaternion.Euler(
                    spriteObject.transform.rotation.x,
                    currentAngle,
                    spriteObject.transform.rotation.z);

    }

    public ItemData GetItemData()
    {
        return itemData;    
    }

    public void InvokePickUpEvent()
    {
        onPickedUpEvent?.Invoke();
    }

    public void AddPullForce(Vector3 velocity)
    {
        transform.position += velocity;
    }
}
