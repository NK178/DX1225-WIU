using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnableItemList;
    [SerializeField] private int maxItemCount;

    [SerializeField] private BoxCollider boxCollider;


    [SerializeField] private float minSpawnTime; 
    [SerializeField] private float maxSpawnTime;

    public bool isActive = true;
    private bool spawnFruit = false;

    private int totalItemCount = 0; 

    private IEnumerator spawnerCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnFruit = false;
    }

    // Update is called once per frame
    void Update()
    {


        if (!isActive)
            return;


        if (spawnerCoroutine == null && !spawnFruit)
        {
            //dont start spawning if cant spawn anymore 
            if (totalItemCount != maxItemCount)
            {
                spawnerCoroutine = SpawnTimerCoroutine();
                StartCoroutine(spawnerCoroutine);
            }
        }

        
        if (spawnFruit)
        {
            SpawnFruit();
            spawnFruit = false;
            StopCoroutine(spawnerCoroutine);
            spawnerCoroutine = null;
        }
    }


    private void SpawnFruit()
    {
        Vector3 extents = boxCollider.size / 2f;

        Vector3 localPoint = new Vector3(
            Random.Range(-extents.x, extents.x),
            boxCollider.gameObject.transform.localPosition.y,
            Random.Range(-extents.z, extents.z)
        ) + boxCollider.center;

        Vector3 spawnPosition = boxCollider.transform.TransformPoint(localPoint);


        int randomFruitIndex = Random.Range(0, spawnableItemList.Count);

        GameObject fruit = Instantiate(spawnableItemList[randomFruitIndex], spawnPosition, Quaternion.identity);
        PickupableItem item = fruit.GetComponent<PickupableItem>();
        if (item != null)
        {
            item.onPickedUpEvent += OnItemPickedUp;
            totalItemCount++;
        }

    }

    private IEnumerator SpawnTimerCoroutine()
    {

        float spawnTime = Random.Range(minSpawnTime, maxSpawnTime);

        yield return new WaitForSeconds(spawnTime);

        spawnFruit = true; 
    }


    private void OnItemPickedUp()
    {
        totalItemCount--;
    }

}
