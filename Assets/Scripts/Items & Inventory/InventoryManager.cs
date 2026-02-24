using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] private DataHolder dataHolder; 
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private SphereCollider sphereColldier;


    [SerializeField] private float sphereRadius;
    [SerializeField] private float gravityPullStrength;

    [SerializeField] private GameObject inventoryPanel;  
    [SerializeField] private InventoryUI inventoryUI;

    private List<ItemData> itemDataList; 

    private PlayerActiveData activeData;

    public static InventoryManager Instance; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemDataList = new List<ItemData>();
        activeData = (PlayerActiveData)dataHolder.activeData;

        if (activeData == null)
        {
            Debug.Log("PLAYER DATA NOT FOUND");
            return;
        }

        activeData.isInventoryOpen = false;

        inventoryPanel.gameObject.SetActive(false);

        if (Instance == null)
        {
            Instance = this;
        }

    }

    // Update is called once per frame
    void Update()
    {
        inventoryPanel.gameObject.SetActive(activeData.isInventoryOpen);
        HandleGravityPull();
    }


    public void HandleGravityPull()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sphereRadius);
        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Item"))
            {
                Debug.Log("Item Found");

                //add a force to pull said object to me
                float distanceFromPlayer = (collider.transform.position - transform.position).magnitude;
                Vector3 directionToPlayer = (collider.transform.position - transform.position).normalized;
                float percentage = 1 - (distanceFromPlayer / sphereRadius);
                percentage = Mathf.Max(0, percentage);
                float currentStrength = percentage * gravityPullStrength;
                Vector3 velocity = -directionToPlayer * currentStrength  * Time.deltaTime;

                collider.gameObject.GetComponentInParent<PickupableItem>().AddPullForce(velocity);
            }
        }
    }

    public void UseItemFunction(ItemData item)
    {
        item.ItemFunction();
        //remove from the list 
        foreach (ItemData itemData in itemDataList)
        {
            if (itemData.itemType == item.itemType)
            {
                itemDataList.Remove(itemData);
                break; 
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            PickupableItem pickUpItem = other.GetComponentInParent<PickupableItem>();
            if (pickUpItem != null)
            {
                Debug.Log("PICKED UP ITEM");

                itemDataList.Add(pickUpItem.GetItemData());
                
                inventoryUI.UpdateItemUI(pickUpItem.GetItemData());

                pickUpItem.InvokePickUpEvent();

                Destroy(other.transform.parent.gameObject);
            }
        }
    }
}
