using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SugarcaneMissilesAttack", menuName = "Scriptable Objects/SugarcaneMissilesAttack")]
public class SugarcaneMissilesAttack : BossAttacks
{

    [SerializeField] private float debugStartHeight = 3f; 
    //Rnow not workign
    [SerializeField] private float delayTimeBeforeFiring = 2f;
    [SerializeField] private float ringRadius = 3f; 


    public override void ExecuteAttack(BossActiveData activeData)
    {
        Debug.Log("SUGARCANE ATTACK");

        //Get reference to player position, bad method but it ok 
        GameObject playerRef = GameObject.FindWithTag("Player");

        if (playerRef != null)
        {
            float ringAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(ringAngle) * ringRadius, debugStartHeight, Mathf.Sin(ringAngle) * ringRadius);
            Vector3 spawnPos = playerRef.transform.position + offset;

            Vector3 directionToPlayer = (playerRef.transform.position - spawnPos).normalized;

            activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.SUGARCANE_MISSILES;
            activeData.objectPoolSpawnData = new ObjectPoolSpawnData(spawnPos, -directionToPlayer);
            activeData.isObjectPoolTriggered = true;
        }



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
