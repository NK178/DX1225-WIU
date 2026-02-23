using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DraggableItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    private string itemName;

    void Start()
    {
        //screw u start
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;

        //update the grid parent also 
        InventoryGrid grid = parentAfterDrag.GetComponent<InventoryGrid>();
        if (grid != null)
        {
            grid.isEmpty = true;
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;

    }


    public void AssignToParent(GameObject parent)
    {
        transform.SetParent(parent.transform);
        if (image == null)
        {
            // Debug.Log("IMAGE NULL");
        }
        image.raycastTarget = true;

    }


    public void SetImageVisible(bool condition)
    {
        image.enabled = condition;
    }

    public void SetItemName(string newName)
    {
        itemName = newName;
    }

    public string GetItemName()
    {
        return itemName;
    }


}
