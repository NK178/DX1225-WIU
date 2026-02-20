using UnityEngine;

//can be reused with player as well
[System.Serializable]
public enum CLASSTYPE
{
    MELEE,
    RANGED,
    NUM_TYPES
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private DataHolder dataHolder;

    private PlayerActiveData activeData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        activeData = (PlayerActiveData)dataHolder.activeData;

        if (activeData == null)
        {
            Debug.Log("PLAYER DATA NOT FOUND");
            return;
        }
    }

    void Update()
    {
        // PlayerController no longer forces the speed variable every frame,
        // It just executes the movement based on whatever the active data currently says
        DebugHandleMove();
    }

    //Testing function since no animation move
    void DebugHandleMove()
    {
        if (activeData == null || !activeData.isMoving)
            return;

        // reads whatever speed the active class (or a dodge roll) has set
        float moveSpeed = activeData.currentMoveSpeed;

        Vector3 moveDirection = new Vector3(activeData.moveDirection.x, 0, activeData.moveDirection.y);

        Vector3 velocity = moveDirection * moveSpeed * Time.deltaTime;

        transform.position = transform.position + velocity;
    }
}