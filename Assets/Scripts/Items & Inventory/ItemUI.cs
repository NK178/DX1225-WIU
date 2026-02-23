using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//public class ItemUI : MonoBehaviour
//{
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//}




public class ItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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
