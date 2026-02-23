using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private List<InventoryGrid> inventoryGridList;

    [SerializeField] private GameObject itemPrefab; 

 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdateItemUI(ItemData itemData)
    {
        //check for empty space
        foreach (InventoryGrid grid in inventoryGridList)
        {

            if (grid.isEmpty)
            {
                DraggableItemUI itemUI = CreateDraggableItem(itemData);
                grid.HandleItemDrop(itemUI);
                break;
            }


        }
    }

    public DraggableItemUI CreateDraggableItem(ItemData itemData)
    {
        GameObject refObj = Instantiate(itemPrefab);
        refObj.transform.SetParent(inventoryPanel.gameObject.transform);
        DraggableItemUI itemUI = refObj.GetComponent<DraggableItemUI>();

        itemUI.usedItemEvent += HandleItemUsed;
        itemUI.itemData = itemData;

        itemUI.image.sprite = itemData.sprite;

        Debug.Log("HELLO SPAWN UI");

        return itemUI;
    }
    
    private void HandleItemUsed(ItemData itemData, DraggableItemUI itemUI)
    {
        Debug.Log("ITEM USED: " + itemData.sprite.name);

        InventoryManager.Instance.UseItemFunction(itemData);

        Destroy(itemUI.gameObject);

    }


}


