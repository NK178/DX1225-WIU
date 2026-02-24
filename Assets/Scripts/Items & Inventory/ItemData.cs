using UnityEngine;

abstract public class ItemData : ScriptableObject
{

    public enum ITEMTYPE{
        APPLE,
        MANGO, 
        WATERMELON,
        NUM_ITEMS
    }

    public ITEMTYPE itemType;
    public Sprite sprite;

    abstract public void ItemFunction();
}
