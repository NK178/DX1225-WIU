using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.instance.Play("Test");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
