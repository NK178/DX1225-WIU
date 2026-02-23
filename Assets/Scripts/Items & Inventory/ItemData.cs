using UnityEngine;

abstract public class ItemData : ScriptableObject
{

    public Sprite sprite;

    abstract public void ItemFunction();
}
