using UnityEngine;

public class PlayerActiveData : BaseActiveData
{

    public Vector2 moveDirection;


    public CLASSTYPE currentClassType;

    public float currentDamageMultiplier = 1f;
    public float currentSpeedMultiplier = 1f;   

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
        currentDamageMultiplier = 1f;
        currentSpeedMultiplier = 1f;
    }

}
