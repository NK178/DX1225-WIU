using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] private DataHolder dataHolder; 
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private SphereCollider sphereColldier;

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
