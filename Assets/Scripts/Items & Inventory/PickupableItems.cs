using UnityEngine;

public class PickupableItem : MonoBehaviour
{

    [SerializeField] private ItemData itemData;
    [SerializeField] private MeshFilter meshFilter; 

    public void SetMesh(Mesh mesh)
    {
        meshFilter.mesh = mesh;   
    }

    public ItemData GetItemData()
    {
        return itemData;    
    }
}
