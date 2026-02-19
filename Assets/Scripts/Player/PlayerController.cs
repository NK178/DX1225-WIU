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

    //for now, i leave it like this , might be bad implentation if need switch between data, see how 
    [SerializeField] private FighterClassData fighterData; 
    [SerializeField] private RangerClassData rangerData;



    [SerializeField] private CLASSTYPE startClass;


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


        activeData.currentClassType = startClass;

    }


    void Update()
    {

        //example code 

        activeData.currentClassType = startClass;

        if (activeData.currentClassType == CLASSTYPE.MELEE)
        {
            activeData.currentMoveSpeed = fighterData.moveSpeed;
        }
        else if (activeData.currentClassType == CLASSTYPE.RANGED)
        {
            activeData.currentMoveSpeed = rangerData.moveSpeed;
        }

        DebugHandleMove();
    }
        

    //Testing function since no animation move
    void DebugHandleMove()
    {

        if (!activeData.isMoving)
            return;

        float moveSpeed = activeData.currentMoveSpeed;

        Vector3 moveDirection = new Vector3(activeData.moveDirection.x, 0, activeData.moveDirection.y);

        Vector3 velocity = moveDirection * moveSpeed * Time.deltaTime;

        transform.position = transform.position + velocity;
    }
}
