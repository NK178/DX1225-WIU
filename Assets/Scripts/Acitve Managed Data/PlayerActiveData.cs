using System.Collections.Generic;
using UnityEngine;

public class PlayerActiveData : BaseActiveData
{

    public enum PlayersAnimStates
    {
        IDLE = 0,
        WALK,
        FIGHTER_SHEATH,
        FIGHTER_RTL_SLASH,
        FIGHTER_LTR_SLASH,
        FIGHTER_THRUST,
        FIGHTER_ABILITY,
        FIGHTER_DEFENSIVE,
    }

    public PlayersAnimStates currentPlayerState;

    public Vector2 moveDirection;
    public Vector3 jumpVel;
    public bool isJumping;

    public bool isDefensive;
    public bool isInvincible;
    public bool isRolling;
    public bool isDead;

    // Using this via animation events
    public void setIsAttackStatus(bool isIt)
    {
        isAttacking = isIt;
    }

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


    //lmao only way 
    public class ParticleData
    {
        public ObjectPoolManager.SPAWNABLE_TYPES particleType;
        public GameObject activeParticle;
        public ParticleData()
        {

        }
        public ParticleData(GameObject p, ObjectPoolManager.SPAWNABLE_TYPES type)
        {
            activeParticle = p;
            particleType = type;
        }
    }

    public List<ParticleData> activeParticleList;

    public void AddActiveParticle(GameObject activeParticle)
    {
        foreach (ParticleData data in activeParticleList)
        {
            if (data.activeParticle == activeParticle)
                return;
        }

        //insurance check against other stuff 
        if ((spawnableType != ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_HEALINGEFFECT) &&
            (spawnableType != ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_DAMAGEEFFECT) &&
            (spawnableType != ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_SMOKESOURCEEFFECT))
            return;
        activeParticleList.Add(new ParticleData(activeParticle, spawnableType));
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
        activeParticleList =  new List<ParticleData>();
    }

}
