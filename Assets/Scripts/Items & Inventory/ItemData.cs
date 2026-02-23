using UnityEngine;

abstract public class ItemData : ScriptableObject
{

    public enum ITEMTYPE{
        APPLE,
        PINEAPPLE, 
        BANANA,
        NUM_ITEMS
    }

    public ITEMTYPE itemType;
    public Sprite sprite;

    abstract public void ItemFunction();
}
