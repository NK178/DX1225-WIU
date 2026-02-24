using System;
using UnityEngine;
using static DataHolder;

public class ObjectPoolSpawnData
{
    public Vector3 spawnPos;
    public Vector3 spawnNormal;
    public float damage;
    public float launchForce;


    //TESING 
    public Vector3 impluseForce; 

    public ObjectPoolSpawnData(Vector3 pos, Vector3 normal, Vector3 force, float dmg = 0f)
    {
        spawnPos = pos;
        spawnNormal = normal;
        impluseForce = force;
        damage = dmg;

    }

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
    public float currentHealth;
    public float maxHealth;

    //New stuff 
    public DATATYPE dataType;

    public float currentAttack;

    //Stupid workaround but it works
    public GameObject referenceParticle; 

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

    private bool _isAttacking = false;
    public bool isAttacking
    {
        get => _isAttacking;
        set
        {
            if (_isAttacking != value)
            {
                _isAttacking = value;
                TriggerStateChanged();
            }
        }
    }

    public BaseActiveData()
    {
        isMoving = false;
        isAttacking = false; 
        isObjectPoolTriggered = false;
        referenceParticle = null;
        spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.NUM_TYPES;
        objectPoolSpawnData = new ObjectPoolSpawnData();
    }
}