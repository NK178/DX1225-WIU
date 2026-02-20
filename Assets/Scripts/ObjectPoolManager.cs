using System.Collections.Generic;
using UnityEngine;

interface IObjectPool
{
    GameObject Get();
    public void SpawnImpactEffect(Vector3 position, Vector3 normal);
}

public class ObjectPoolManager : MonoBehaviour
{

    //to store all particle types 

    public enum SPAWNABLE_TYPES
    {
        SUGARCANE_MISSILES, 
        NUM_TYPES
    }

    [Header("Particle Spawners")]
    [SerializeField] private ProjectileObjectPool sugarCaneSpawner;


    [SerializeField] private DataHolder[] dataHolders;
    private List<BaseActiveData> entityDataList;

    Dictionary<SPAWNABLE_TYPES, IObjectPool> particleMap;



    private void Start()
    {

        entityDataList = new List<BaseActiveData>();
        particleMap = new Dictionary<SPAWNABLE_TYPES, IObjectPool>();
        particleMap[SPAWNABLE_TYPES.SUGARCANE_MISSILES] = sugarCaneSpawner;

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
            Debug.Log("SPAWN SOMETHING");

            Vector3 spawnPos = baseActiveData.objectPoolSpawnData.spawnPos;
            Vector3 spawnNormal = baseActiveData.objectPoolSpawnData.spawnNormal;


            if (baseActiveData.spawnableType == SPAWNABLE_TYPES.SUGARCANE_MISSILES && particleMap.ContainsKey(SPAWNABLE_TYPES.SUGARCANE_MISSILES))
            {
                Debug.Log("SUGARCANE");
                particleMap[SPAWNABLE_TYPES.SUGARCANE_MISSILES].SpawnImpactEffect(spawnPos, spawnNormal);
            }

            baseActiveData.isObjectPoolTriggered = false;
        }


    }

}
