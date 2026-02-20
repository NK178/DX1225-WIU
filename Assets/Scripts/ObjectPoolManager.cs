using System.Collections.Generic;
using UnityEngine;

public interface IObjectPool
{
    GameObject Get();
    public void SpawnImpactEffect(Vector3 position, Vector3 normal);
    public void SpawnProjectile(Vector3 position, Vector3 forward, DataHolder.DATATYPE spawner, float damage, float launchForce);
}

public class ObjectPoolManager : MonoBehaviour
{
    public enum SPAWNABLE_TYPES
    {
        SUGARCANE_MISSILES,
        RANGER_SEED,
        NUM_TYPES
    }

    [Header("Particle Spawners")]
    [SerializeField] private ProjectileObjectPool sugarCaneSpawner;
    [SerializeField] private ProjectileObjectPool rangerSeedSpawner;

    [SerializeField] private DataHolder[] dataHolders;
    private List<BaseActiveData> entityDataList;

    Dictionary<SPAWNABLE_TYPES, IObjectPool> particleMap;

    private void Start()
    {
        entityDataList = new List<BaseActiveData>();
        particleMap = new Dictionary<SPAWNABLE_TYPES, IObjectPool>();

        if (sugarCaneSpawner != null) particleMap[SPAWNABLE_TYPES.SUGARCANE_MISSILES] = sugarCaneSpawner;
        if (rangerSeedSpawner != null) particleMap[SPAWNABLE_TYPES.RANGER_SEED] = rangerSeedSpawner;

        foreach (DataHolder dataHolder in dataHolders)
        {
            entityDataList.Add(dataHolder.activeData);
        }

        foreach (BaseActiveData data in entityDataList)
        {
            data.onObjectPoolTriggered += HandleParticleRequests;
        }
    }

    private void OnDisable()
    {
        foreach (BaseActiveData data in entityDataList)
        {
            data.onObjectPoolTriggered -= HandleParticleRequests;
        }
    }

    private void HandleParticleRequests(BaseActiveData baseActiveData)
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
                    particleMap[SPAWNABLE_TYPES.SUGARCANE_MISSILES].SpawnImpactEffect(spawnPos, spawnNormal);
                }
            }

            baseActiveData.isObjectPoolTriggered = false;
        }
    }
}