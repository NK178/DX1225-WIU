using UnityEngine;

public class PlayerActiveData : BaseActiveData
{

    public Vector2 moveDirection;


    public CLASSTYPE currentClassType;

    private bool _isInventoryOpen = false;
    public bool isInventoryOpen
    {
        get => _isInventoryOpen;
        set
        {
            if (_isInventoryOpen != value)
            {
                _isInventoryOpen = value;
                TriggerStateChanged();
            }
        }
    }

    public PlayerActiveData()
    {
        Debug.Log("INITALIZED PLAYER DATA");
        moveDirection = Vector2.zero;
        currentClassType = CLASSTYPE.MELEE;
        dataType = DataHolder.DATATYPE.PLAYER;
        isInventoryOpen = false;
    }

}
