using UnityEngine;

public class PlayerActiveData : BaseActiveData
{

    public Vector2 moveDirection;



    public PlayerActiveData()
    {
        Debug.Log("INITALIZED PLAYER DATA");
        moveDirection = Vector2.zero;   
    }

}
