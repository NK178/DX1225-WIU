using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SugarcaneMissilesAttack", menuName = "Scriptable Objects/SugarcaneMissilesAttack")]
public class SugarcaneMissilesAttack : BossAttacks
{

    [SerializeField] private float delayTimeBeforeFiring = 2f;

    public override void ExecuteAttack(BossActiveData activeData)
    {
        Debug.Log("SUGARCANE ATTACK");


        activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.SUGARCANE_MISSILES;
        activeData.objectPoolSpawnData = new ObjectPoolSpawnData(Vector3.zero, Vector3.zero);
        activeData.isObjectPoolTriggered = true;



        //SpawnProjectile(); 

        //need run projectile here 
        //while (true)
        //{
        //    if (timer > delayTimeBeforeFiring)
        //    {
        //        SpawnProjectile();
        //        break;
        //    }

        //    //Debug.Log("TIME: " + timer);
        //    timer += Time.deltaTime;
        //}



        //workaround nvm didnt work
        //sugarcanePrefab.GetComponent<MonoBehaviour>().StartCoroutine(HandleFiring());


    }

    //private void SpawnProjectile()
    //{
    //    Debug.Log("SPAWNED SUGARCANE");
    //    //GameObject newProjectile = Instantiate(sugarcanePrefab);
    //}


    //private IEnumerator HandleFiring()
    //{
    //    yield return new WaitForSeconds(delayTimeBeforeFiring);

    //    GameObject newProjectile = Instantiate(sugarcanePrefab);

    //    Debug.Log("SPAWNED SUGARCANE");
    //}




}
