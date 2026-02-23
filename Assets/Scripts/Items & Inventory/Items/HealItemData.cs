using UnityEngine;

[CreateAssetMenu(fileName = "HealItemData", menuName = "Item/HealItemData")]
public class HealItemData : ItemData
{
    public override void ItemFunction()
    {
        Debug.Log("Heal Me");
    }
}
