using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DraggableItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{

    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    private string itemName;

    private float clickCountDelay = 0.5f;
    private bool resetClickCount = false;
    private int numClicks = 0;

    public ItemData itemData;
    public event Action<ItemData, DraggableItemUI> usedItemEvent;

    void Start()
    {
        resetClickCount = false;
        numClicks = 0;
    }

    private void Update()
    {
        if (resetClickCount)
        {
            numClicks = 0;
            resetClickCount = false;
        }
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (numClicks != 2)
        {
            if (numClicks != 1)
                StartCoroutine(ClickCountDelay());
            numClicks++;
        }

        //used up items
        if (numClicks == 2)
        {
            Debug.Log("USING ITEM");
            numClicks = 0;
            StopAllCoroutines();
            usedItemEvent?.Invoke(itemData, this);
        }
    }


    private IEnumerator ClickCountDelay()
    {
        yield return new WaitForSeconds(clickCountDelay);
        Debug.Log("RESETTING CLICKS");
        numClicks = 0;
        resetClickCount = true;
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
