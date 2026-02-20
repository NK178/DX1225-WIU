using System;
using System.ComponentModel;
using UnityEngine;
using static DataHolder;

public class ObjectPoolSpawnData
{
    public Vector3 spawnPos;
    public Vector3 spawnNormal;
    public float damage;
    public float launchForce;

    public ObjectPoolSpawnData()
    {

    }

    public ObjectPoolSpawnData(Vector3 pos, Vector3 normal, float dmg = 0f, float force = 0f)
    {
        spawnPos = pos;
        spawnNormal = normal;
        damage = dmg;
        launchForce = force;
    }
}

public class BaseActiveData
{
    public event Action onStateChanged;
    public float currentMoveSpeed;

   
    //New stuff 
    public DATATYPE dataType;

    public ObjectPoolManager.SPAWNABLE_TYPES spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.NUM_TYPES;

    public ObjectPoolSpawnData objectPoolSpawnData;
    public event Action<BaseActiveData> onObjectPoolTriggered;

    private bool _isObjectPoolTriggered;
    public bool isObjectPoolTriggered
    {
        get => _isObjectPoolTriggered;
        set
        {
            if (_isObjectPoolTriggered != value)
            {
                _isObjectPoolTriggered = value;
                onObjectPoolTriggered?.Invoke(this);
            }
        }
    }

    //need to do this for derrived classes so just use this one
    protected void TriggerStateChanged()
    {
        onStateChanged?.Invoke();
    }

    //Using the power of action events
    private bool _isMoving = false;
    public bool isMoving
    {
        get => _isMoving;
        set
        {
            if (_isMoving != value)
            {
                _isMoving = value;
                TriggerStateChanged();
            }
        }
    }

    public BaseActiveData()
    {
        isMoving = false;
        isObjectPoolTriggered = false;
        spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.NUM_TYPES;
    }
}