using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] private DataHolder dataHolder; 
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private InventoryUI inventoryUI;

    private List<ItemData> itemDataList; 

    private PlayerActiveData activeData;


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

        inventoryUI.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        inventoryUI.gameObject.SetActive(activeData.isInventoryOpen);

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

                Destroy(other.gameObject);
            }
        }
    }
}
