using System.Collections;
using UnityEngine;

public class SugarcaneProjectile : GenericProjectile
{


    //For now I will leave it like this , will take from the base class later 

    [SerializeField] private float delayTimeBeforeFiring = 2f;
    [SerializeField] private float projectileSpeed = 10f;



    bool isActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    override public void Initialize(DataHolder.DATATYPE spawner, float damageAmount)
    {
        spawnerType = spawner;
        projectileDamage = damageAmount;

    }

    // Update is called once per frame
    void Update()
    {

        if (isActive)
        {
            float velocity = projectileSpeed * Time.deltaTime;
            transform.position += -transform.up * velocity; 
        }
        
    }


    private IEnumerator ActivateProjectileCoroutine()
    {
        yield return new WaitForSeconds(delayTimeBeforeFiring);
        isActive = true;
    }


    public void ActivateProjectile()
    {
        StartCoroutine(ActivateProjectileCoroutine());
    }



    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("HIT GROUND");
    }
}
