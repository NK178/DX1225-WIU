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
        RANGER_SEED,
        PARTICLE_SUGARCANESPLASH,
        RUBBERBAND_BULLETS,
        NUM_TYPES
    }

    [Header("Particle Spawners")]
    [SerializeField] private ProjectileObjectPool sugarCaneSpawner;
    [SerializeField] private ProjectileObjectPool rangerSeedSpawner;
    [SerializeField] private ProjectileObjectPool rubberBandSpawner;
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
        if (rubberBandSpawner != null) particleMap[SPAWNABLE_TYPES.RUBBERBAND_BULLETS] = rubberBandSpawner;
        //if (sugarcaneSplashEffectSpawner != null) particleMap[SPAWNABLE_TYPES.PARTICLE_SUGARCANESPLASH] = sugarcaneSplashEffectSpawner;

        if (sugarcaneSplashEffectSpawner != null)
        {
            Debug.Log("SPLASH SPAWNER");
            particleMap[SPAWNABLE_TYPES.PARTICLE_SUGARCANESPLASH] = sugarcaneSplashEffectSpawner;
        }


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

            if (particleMap.ContainsKey(baseActiveData.spawnableType))
            {
                if (baseActiveData.spawnableType == SPAWNABLE_TYPES.RANGER_SEED)
                {
                    particleMap[SPAWNABLE_TYPES.RANGER_SEED].SpawnProjectile(spawnPos, spawnNormal, DataHolder.DATATYPE.PLAYER, damage, launchForce);
                }
                else if (baseActiveData.spawnableType == SPAWNABLE_TYPES.SUGARCANE_MISSILES)
                {
                    //particleMap[SPAWNABLE_TYPES.SUGARCANE_MISSILES].SpawnKinematicProjectiles(spawnPos, spawnNormal, DataHolder.DATATYPE.BOSS_ENEMY, damage);
                    particleMap[SPAWNABLE_TYPES.SUGARCANE_MISSILES].SpawnKinematicProjectiles(spawnPos, spawnNormal, baseActiveData, damage);
                }
                else if (baseActiveData.spawnableType == SPAWNABLE_TYPES.RANGER_SEED)
                {
                    //particleMap[SPAWNABLE_TYPES.RUBBERBAND_BULLETS].SpawnProjectile
                }

                //PARTICLES PART
                else if (baseActiveData.spawnableType == SPAWNABLE_TYPES.PARTICLE_SUGARCANESPLASH)
                {
                    Debug.Log("SPAWN EFFECT");
                    particleMap[SPAWNABLE_TYPES.PARTICLE_SUGARCANESPLASH].SpawnImpactEffect(spawnPos, spawnNormal);
                }
            }

            baseActiveData.isObjectPoolTriggered = false;
        }
    }
}