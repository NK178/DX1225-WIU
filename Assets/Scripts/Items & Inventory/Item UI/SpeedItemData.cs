using UnityEngine;

[CreateAssetMenu(fileName = "SpeedItemData", menuName = "Item/SpeedItemData")]
public class SpeedItemData : ItemData
{

    [SerializeField] private float speedMultiplier;

    public override void ItemFunction()
    {
        Debug.Log("GAS GAS GAS");

        //might as well hard code for the player 
        PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (player != null)
        {
            //float newHealth = player.GetCurrentHealth() + addHealth;
            //player.SetCurrentHealth(newHealth);
        }
    }

}
