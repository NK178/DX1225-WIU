using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{

    [SerializeField] private PlayerInput playerInput;


    [SerializeField] private DataHolder dataHolder;


    private PlayerActiveData activeData;

    private InputAction moveAction; 
    
    void Start()
    {
        moveAction = playerInput.actions.FindAction("Move");
        if (moveAction != null)
            moveAction.Enable();


        activeData = (PlayerActiveData)dataHolder.activeData;

    }

    void Update()
    {
        HandleMove();


    }

    void HandleMove()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        Debug.Log("MOVE: " +  direction.magnitude);
        if (direction.magnitude > 0)
        {
            activeData.moveDirection = direction;
            activeData.isMoving = true; 
        }
        else
        {
            activeData.moveDirection = Vector2.zero;
            activeData.isMoving = false;
        }
    }
}
