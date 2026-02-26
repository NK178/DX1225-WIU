using UnityEngine;

public class GameManager : MonoBehaviour
{


    [SerializeField] private BossController boss;

    [SerializeField] private GameObject endGameColldier; 
    [SerializeField] private GameObject startBossCollider; 

    public static GameManager Instance; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //AudioManager.instance.Play("Test");


        if (Instance == null)  
            Instance = this;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OpenExitMap()
    {
        endGameColldier.SetActive(false);
    }
    

    public void StartBoss()
    {
        startBossCollider.SetActive(false);
        //BossController boss = GameObject.FindWithTag("Boss").GetComponent<BossController>();
        if (boss != null)
        {
            //boss.gameObject.SetActive(true);
            boss.TriggerBossStart();
        }
    }
}
