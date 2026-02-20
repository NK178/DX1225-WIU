using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SugarcaneMissilesAttack", menuName = "Scriptable Objects/SugarcaneMissilesAttack")]
public class SugarcaneMissilesAttack : BossAttacks
{

    [SerializeField] private float debugStartHeight = 3f; 
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

            //Vector3 directionToPlayer = (spawnPos - playerRef.transform.position).normalized;

            //Vector3 directionToPlayer = (playerRef.transform.position - spawnPos).normalized;
            //float angle = Mathf.Atan2(directionToPlayer.z, directionToPlayer.y) * Mathf.Rad2Deg;
            //Vector3 rotation = new Vector3(angle, 0, 0);

            ////float angle = Mathf.Atan2(directionToPlayer.x, directionToPlayer.y) * Mathf.Rad2Deg;
            ////Vector3 rotation = new Vector3(0, 0, angle);
            ///

            Vector3 directionToPlayer = (playerRef.transform.position - spawnPos).normalized;
            //Vector3 offsetDirection = Vector3.RotateTowards()


            activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.SUGARCANE_MISSILES;
            activeData.objectPoolSpawnData = new ObjectPoolSpawnData(spawnPos, -directionToPlayer);
            activeData.isObjectPoolTriggered = true;
        }


        //Vector3 offset = Vector3.zero;
        //Vector3 spawnPos = Vector3.zero;


        //for (int i = 0; i < count; i++)
        //{
        //}


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
