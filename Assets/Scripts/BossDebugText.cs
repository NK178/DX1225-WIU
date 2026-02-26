using TMPro;
using UnityEngine;

public class BossDebugText : MonoBehaviour
{

    [SerializeField] private DataHolder dataHolder; 
    [SerializeField] private TMP_Text text;

    private BossActiveData activeData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (dataHolder.activeData == null)
        {
            Debug.LogError("NO ACTIVE DATA HELD");
            return;
        }

        activeData = (BossActiveData)dataHolder.activeData;

        if (activeData == null)
        {
            Debug.LogError("NO BOSS ACTIVE DATA FOUND");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        text.gameObject.SetActive(true);
        string bossText = "Boss HP: " + activeData.currentHealth + "\nBoss Phase: " + activeData.BossPhase + "\n" + "Boss Attack: " + activeData.BAnimState.ToString();
        text.text = bossText;

        //if (activeData.isAttacking)
        //{

        //}
        //else
        //{
        //    text.gameObject.SetActive(false);
        //}

    }
}
