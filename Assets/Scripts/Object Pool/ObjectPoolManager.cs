using System.Collections.Generic;
using UnityEngine;

public interface IObjectPool
{
    GameObject Get();

    //Testing to make the method not required to implement
    public void SpawnImpactEffect(Vector3 position, Vector3 normal) {
        Debug.Log("NO IMPACT EFECT FUNCTION");
        return; 
    }

    public void SpawnImpactEffect(Vector3 position, Vector3 normal, ref GameObject reference)
    {
        Debug.Log("NO IMPACT EFECT FUNCTION");
        return;
    }

    public void SpawnProjectile(Vector3 position, Vector3 forward, DataHolder.DATATYPE spawner, float damage, float launchForce)
    {
        Debug.Log("NO SPAWN PROJECTILE FUNCTION");
        return;
    }

    //TESTING 
    public void SpawnProjectile(Vector3 position, Vector3 forward, DataHolder.DATATYPE spawner, float damage, Vector3 impluse)
    {
        Debug.Log("NO SPAWN PROJECTILE WITH FORCE IMPLUSE FUNCTION");
        return;
    }

    //PROBABLY THE BETTER VERSION
    public void SpawnProjectile(Vector3 position, Vector3 forward, BaseActiveData baseData, float damage, Vector3 impluse)
    {
        Debug.Log("NO SPAWN PROJECTILE BASEDATA WITH FORCE IMPLUSE FUNCTION");
        return;
    }

    public void SpawnKinematicProjectiles(Vector3 position, Vector3 forward, DataHolder.DATATYPE spawner, float damage)
    {
        Debug.Log("NO KINEMACTIC SPAWN PROJECTILE FUNCTION");
        return;
    }

    public void SpawnKinematicProjectiles(Vector3 position, Vector3 forward, BaseActiveData refData, float damage)
    {
        Debug.Log("NO KINEMACTIC SPAWN PROJECTILE FUNCTION");
        return;
    }
}

public class ObjectPoolManager : MonoBehaviour
{
    public enum SPAWNABLE_TYPES
    {
        SUGARCANE_MISSILES,
        FRUIT_CHUNKS,
        RANGER_SEED,
        PARTICLE_SUGARCANESPLASH,
        PARTICLE_FRUITSPLASH,
        PARTICLE_DUSTSPLASH,
        PARTICLE_WOODSPLINTER,
        PARTICLE_ELECTRICSPARK,
        PARTICLE_HEALINGEFFECT,
        PARTICLE_DAMAGEEFFECT,
        PARTICLE_SMOKESOURCEEFFECT,
        RUBBERBAND_BULLETS,
        ROLLINGPIN,
        NUM_TYPES
    }

    [Header("Particle Spawners")]
    [SerializeField] private ProjectileObjectPool sugarCaneSpawner;
    [SerializeField] private ProjectileObjectPool rangerSeedSpawner;
    [SerializeField] private ProjectileObjectPool fruitChunkSpawner;
    [SerializeField] private ProjectileObjectPool rubberBandSpawner;
    [SerializeField] private ProjectileObjectPool rollingPinSpawner;
    [SerializeField] private ParticleObjectPool sugarcaneSplashEffectSpawner;
    [SerializeField] private ParticleObjectPool fruitSplashEffectSpawner;
    [SerializeField] private ParticleObjectPool woodSplinterEffectSpawner;
    [SerializeField] private ParticleObjectPool dustSplashEffectSpawner;
    [SerializeField] private ParticleObjectPool electricSparkEffectSpawner;
    [SerializeField] private ParticleObjectPool healingEffectSpawner;
    [SerializeField] private ParticleObjectPool damageEffectSpawner;
    [SerializeField] private ParticleObjectPool smokeSourceEffectSpawner;

    [SerializeField] private DataHolder[] dataHolders;
    private List<BaseActiveData> entityDataList;

    Dictionary<SPAWNABLE_TYPES, IObjectPool> particleMap;

    public static ObjectPoolManager Instance; 

    private void Start()
    {
        entityDataList = new List<BaseActiveData>();
        particleMap = new Dictionary<SPAWNABLE_TYPES, IObjectPool>();

        if (sugarCaneSpawner != null) particleMap[SPAWNABLE_TYPES.SUGARCANE_MISSILES] = sugarCaneSpawner;
        if (rangerSeedSpawner != null) particleMap[SPAWNABLE_TYPES.RANGER_SEED] = rangerSeedSpawner;
        if (fruitChunkSpawner != null) particleMap[SPAWNABLE_TYPES.FRUIT_CHUNKS] = fruitChunkSpawner;
        if (rubberBandSpawner != null) particleMap[SPAWNABLE_TYPES.RUBBERBAND_BULLETS] = rubberBandSpawner;
        if (rollingPinSpawner != null) particleMap[SPAWNABLE_TYPES.ROLLINGPIN] = rollingPinSpawner;
        if (sugarcaneSplashEffectSpawner != null) particleMap[SPAWNABLE_TYPES.PARTICLE_SUGARCANESPLASH] = sugarcaneSplashEffectSpawner;
        if (fruitSplashEffectSpawner != null) particleMap[SPAWNABLE_TYPES.PARTICLE_FRUITSPLASH] = fruitSplashEffectSpawner;
        if (woodSplinterEffectSpawner != null) particleMap[SPAWNABLE_TYPES.PARTICLE_WOODSPLINTER] = woodSplinterEffectSpawner;
        if (dustSplashEffectSpawner != null) particleMap[SPAWNABLE_TYPES.PARTICLE_DUSTSPLASH] = dustSplashEffectSpawner;
        if (electricSparkEffectSpawner != null) particleMap[SPAWNABLE_TYPES.PARTICLE_ELECTRICSPARK] = electricSparkEffectSpawner;
        if (healingEffectSpawner != null) particleMap[SPAWNABLE_TYPES.PARTICLE_HEALINGEFFECT] = healingEffectSpawner;
        if (damageEffectSpawner != null) particleMap[SPAWNABLE_TYPES.PARTICLE_DAMAGEEFFECT] = damageEffectSpawner;
        if (smokeSourceEffectSpawner != null) particleMap[SPAWNABLE_TYPES.PARTICLE_SMOKESOURCEEFFECT] = smokeSourceEffectSpawner;


        if (Instance == null)
        {
            Instance = this;
        }

        //Insurance for using bad method
        if (dataHolders == null)
            return; 

        foreach (DataHolder dataHolder in dataHolders)
        {
            entityDataList.Add(dataHolder.activeData);
        }

        foreach (BaseActiveData data in entityDataList)
        {
            data.onObjectPoolTriggered += HandleSpawnRequest;
        }
    }

    private void OnDisable()
    {

        if (entityDataList == null)
            return;

        foreach (BaseActiveData data in entityDataList)
        {
            data.onObjectPoolTriggered -= HandleSpawnRequest;
        }
    }

    public void HandleSpawnRequest(BaseActiveData baseActiveData)
    {

        if (baseActiveData.isObjectPoolTriggered)
        {
            Vector3 spawnPos = baseActiveData.objectPoolSpawnData.spawnPos;
            Vector3 spawnNormal = baseActiveData.objectPoolSpawnData.spawnNormal;
            float damage = baseActiveData.objectPoolSpawnData.damage;
            float launchForce = baseActiveData.objectPoolSpawnData.launchForce;

            GameObject referenceGameobject = null;
            PlayerActiveData playerActiveData = null;   
            playerActiveData = baseActiveData as PlayerActiveData;

            if (!particleMap.ContainsKey(baseActiveData.spawnableType))
            {
                return; 
            }

            switch(baseActiveData.spawnableType)
            {
                case SPAWNABLE_TYPES.RANGER_SEED:
                    particleMap[SPAWNABLE_TYPES.RANGER_SEED].SpawnProjectile(spawnPos, spawnNormal, DataHolder.DATATYPE.PLAYER, damage, launchForce);
                    break;
                case SPAWNABLE_TYPES.FRUIT_CHUNKS:
                    particleMap[SPAWNABLE_TYPES.FRUIT_CHUNKS].SpawnProjectile(spawnPos, spawnNormal, baseActiveData, damage, baseActiveData.objectPoolSpawnData.impluseForce);
                    break;
                case SPAWNABLE_TYPES.SUGARCANE_MISSILES:
                    particleMap[SPAWNABLE_TYPES.SUGARCANE_MISSILES].SpawnKinematicProjectiles(spawnPos, spawnNormal, baseActiveData, damage);
                    break;
                case SPAWNABLE_TYPES.ROLLINGPIN:
                    particleMap[SPAWNABLE_TYPES.ROLLINGPIN].SpawnKinematicProjectiles(spawnPos, spawnNormal, baseActiveData, damage);
                    break;
                case SPAWNABLE_TYPES.PARTICLE_SUGARCANESPLASH:
                    particleMap[SPAWNABLE_TYPES.PARTICLE_SUGARCANESPLASH].SpawnImpactEffect(spawnPos, spawnNormal);
                    break;
                case SPAWNABLE_TYPES.PARTICLE_FRUITSPLASH:
                    particleMap[SPAWNABLE_TYPES.PARTICLE_FRUITSPLASH].SpawnImpactEffect(spawnPos, spawnNormal);
                    break;
                case SPAWNABLE_TYPES.PARTICLE_WOODSPLINTER:
                    particleMap[SPAWNABLE_TYPES.PARTICLE_WOODSPLINTER].SpawnImpactEffect(spawnPos, spawnNormal);
                    break;
                case SPAWNABLE_TYPES.PARTICLE_DUSTSPLASH:
                    particleMap[SPAWNABLE_TYPES.PARTICLE_DUSTSPLASH].SpawnImpactEffect(spawnPos, spawnNormal);
                    break;
                case SPAWNABLE_TYPES.PARTICLE_ELECTRICSPARK:
                    particleMap[SPAWNABLE_TYPES.PARTICLE_ELECTRICSPARK].SpawnImpactEffect(spawnPos, spawnNormal);
                    break;
                case SPAWNABLE_TYPES.PARTICLE_HEALINGEFFECT:
                    particleMap[SPAWNABLE_TYPES.PARTICLE_HEALINGEFFECT].SpawnImpactEffect(spawnPos, spawnNormal,ref referenceGameobject);
                    if (playerActiveData != null)
                        playerActiveData.AddActiveParticle(referenceGameobject);
                    //baseActiveData.referenceParticle = referenceGameobject;
                    break;
                case SPAWNABLE_TYPES.PARTICLE_DAMAGEEFFECT:
                    particleMap[SPAWNABLE_TYPES.PARTICLE_DAMAGEEFFECT].SpawnImpactEffect(spawnPos, spawnNormal, ref referenceGameobject);
                    if (playerActiveData != null)
                        playerActiveData.AddActiveParticle(referenceGameobject);                    
                    //baseActiveData.referenceParticle = referenceGameobject;
                    break;
                case SPAWNABLE_TYPES.PARTICLE_SMOKESOURCEEFFECT:
                    particleMap[SPAWNABLE_TYPES.PARTICLE_SMOKESOURCEEFFECT].SpawnImpactEffect(spawnPos, spawnNormal, ref referenceGameobject);
                    if (playerActiveData != null)
                        playerActiveData.AddActiveParticle(referenceGameobject);
                    //baseActiveData.referenceParticle = referenceGameobject;
                    break;
                case SPAWNABLE_TYPES.RUBBERBAND_BULLETS:
                    //particleMap[SPAWNABLE_TYPES.RUBBERBAND_BULLETS].SpawnProjectile(spawnPos, spawnNormal, DataHolder.DATATYPE.RANGED_ENEMY, damage, baseActiveData.objectPoolSpawnData.impluseForce);
                    particleMap[SPAWNABLE_TYPES.RUBBERBAND_BULLETS].SpawnProjectile(spawnPos, spawnNormal, baseActiveData, damage, baseActiveData.objectPoolSpawnData.impluseForce);
                    break; 
               
            }
            baseActiveData.isObjectPoolTriggered = false;
        }
    }
}