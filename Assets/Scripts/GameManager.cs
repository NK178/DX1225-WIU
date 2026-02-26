using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject endGameColldier; 

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
    
}
