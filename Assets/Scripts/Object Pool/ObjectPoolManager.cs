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
        NUM_TYPES
    }

    [Header("Particle Spawners")]
    [SerializeField] private ProjectileObjectPool sugarCaneSpawner;
    [SerializeField] private ProjectileObjectPool rangerSeedSpawner;
    [SerializeField] private ProjectileObjectPool fruitChunkSpawner;
    [SerializeField] private ParticleObjectPool sugarcaneSplashEffectSpawner;

    [SerializeField] private DataHolder[] dataHolders;
    private List<BaseActiveData> entityDataList;

    Dictionary<SPAWNABLE_TYPES, IObjectPool> particleMap;

    private void Start()
    {
        entityDataList = new List<BaseActiveData>();
        particleMap = new Dictionary<SPAWNABLE_TYPES, IObjectPool>();

        if (sugarCaneSpawner != null) particleMap[SPAWNABLE_TYPES.SUGARCANE_MISSILES] = sugarCaneSpawner;
        if (rangerSeedSpawner != null) particleMap[SPAWNABLE_TYPES.RANGER_SEED] = rangerSeedSpawner;
        if (fruitChunkSpawner != null) particleMap[SPAWNABLE_TYPES.FRUIT_CHUNKS] = fruitChunkSpawner;
        if (sugarcaneSplashEffectSpawner != null) particleMap[SPAWNABLE_TYPES.PARTICLE_SUGARCANESPLASH] = sugarcaneSplashEffectSpawner;


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
        foreach (BaseActiveData data in entityDataList)
        {
            data.onObjectPoolTriggered -= HandleSpawnRequest;
        }
    }

    private void HandleSpawnRequest(BaseActiveData baseActiveData)
    {
        if (baseActiveData.isObjectPoolTriggered)
        {
            Vector3 spawnPos = baseActiveData.objectPoolSpawnData.spawnPos;
            Vector3 spawnNormal = baseActiveData.objectPoolSpawnData.spawnNormal;
            float damage = baseActiveData.objectPoolSpawnData.damage;
            float launchForce = baseActiveData.objectPoolSpawnData.launchForce;


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

                    particleMap[SPAWNABLE_TYPES.FRUIT_CHUNKS].SpawnProjectile(spawnPos, spawnNormal, DataHolder.DATATYPE.BOSS_ENEMY, damage, baseActiveData.objectPoolSpawnData.impluseForce);

                    //particleMap[SPAWNABLE_TYPES.FRUIT_CHUNKS].SpawnProjectile(spawnPos, spawnNormal, DataHolder.DATATYPE.BOSS_ENEMY, damage, launchForce);
                    break;
                case SPAWNABLE_TYPES.SUGARCANE_MISSILES:
                    particleMap[SPAWNABLE_TYPES.SUGARCANE_MISSILES].SpawnKinematicProjectiles(spawnPos, spawnNormal, baseActiveData, damage);
                    break;
                case SPAWNABLE_TYPES.PARTICLE_SUGARCANESPLASH:
                    particleMap[SPAWNABLE_TYPES.PARTICLE_SUGARCANESPLASH].SpawnImpactEffect(spawnPos, spawnNormal);
                    break;

            }
            baseActiveData.isObjectPoolTriggered = false;
        }
    }
}