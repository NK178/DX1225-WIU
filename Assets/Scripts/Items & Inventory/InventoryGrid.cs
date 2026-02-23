using UnityEngine;
using UnityEngine.EventSystems;


public class InventoryGrid : MonoBehaviour, IDropHandler
{
    [HideInInspector] public bool isEmpty;
    public GameObject heldItem;
    private int row, col;

    InventoryGrid()
    {

    }

    public InventoryGrid(int row, int col)
    {
        this.row = row;
        this.col = col;
        heldItem = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!isEmpty)
            return; 

        GameObject dropped = eventData.pointerDrag;
        DraggableItemUI draggableItem = dropped.GetComponent<DraggableItemUI>();
        HandleItemDrop(draggableItem);
    }


    public void HandleItemDrop(DraggableItemUI dragItem)
    {
        dragItem.parentAfterDrag = transform;
        dragItem.transform.parent = dragItem.parentAfterDrag;   

        Debug.Log("DROPPING BOMBS");
        //store the gameobject 
        heldItem = gameObject;
        isEmpty = false;
    }
    public void EditGridPos(int row, int col)
    {
        this.row = row;
        this.col = col;
    }

    public void AddItem(DraggableItemUI dragItem)
    {
        //add item aka make it a child of this game object
        if (dragItem != null)
        {
            //need to set item name for crafting to work
            heldItem = dragItem.gameObject;
            dragItem.AssignToParent(this.gameObject);
            isEmpty = false;
        }

    }


    public bool IsGridEmpty()
    {
        return isEmpty;
    }

    public void RemoveItem()
    {
        //remove item 
        if (!isEmpty)
        {
            isEmpty = true;
            //delete child item if have 
            DraggableItemUI dragItem = GetComponentInChildren<DraggableItemUI>();
            if (dragItem != null)
                Destroy(dragItem.gameObject);
            heldItem = null;
        }
    }

    public void SetImageVisible(bool condition)
    {
        if (heldItem != null)
        {
            DraggableItemUI dragItem = heldItem.GetComponent<DraggableItemUI>();
            if (dragItem != null)
                dragItem.SetImageVisible(condition);
        }
    }



    void UpdateGridStatus()
    {
        if (!isEmpty)
        {
            //clear item 
            DraggableItemUI dragItem = GetComponentInChildren<DraggableItemUI>();
            if (dragItem == null)
            {
                RemoveItem();
                isEmpty = true;
            }
        }
    }


    void Start()
    {
        heldItem = null;
        isEmpty = true;

    }

    void Update()
    {
        ////needed to clear the grid when item is removed 
        //UpdateGridStatus();
    }

}

