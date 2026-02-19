using UnityEngine;

public class DataHolder : MonoBehaviour
{

    public enum DATATYPE
    {
        PLAYER,
        MELEE_ENEMY,
        RANGED_ENEMY,
        BOSS_ENEMY,
        NUM_TYPE
    }

    [SerializeField] private DATATYPE dataType;

    public BaseActiveData activeData;

    private void Awake()
    {
        if (dataType == DATATYPE.PLAYER)
        {
            activeData = new PlayerActiveData();
        }
        else if (dataType == DATATYPE.MELEE_ENEMY)
        {
            //Debug.Log("ENEMY DATA");
            //activeData = new GroundEnemyActiveData();
        }
        else if (dataType == DATATYPE.RANGED_ENEMY)
        {
            //Debug.Log("ENEMY DATA");
            //activeData = new GroundEnemyActiveData();
        }
        else if (dataType == DATATYPE.BOSS_ENEMY)
        {
            Debug.Log("BOSS DATA");
            activeData = new BossActiveData();
        }
    }
}
