using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private DataHolder dataHolder;


    [Header("Movement")]
    [SerializeField] private float moveSpeed; 

    private PlayerActiveData activeData;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        activeData = (PlayerActiveData)dataHolder.activeData;

    }


    void Update()
    {
        DebugHandleMove();
    }


    //Testing function since no animation move
    void DebugHandleMove()
    {

        if (!activeData.isMoving)
            return;


        Vector3 moveDirection = new Vector3(activeData.moveDirection.x, 0, activeData.moveDirection.y);

        Vector3 velocity = moveDirection * moveSpeed * Time.deltaTime;

        transform.position = transform.position + velocity;
    }
}
