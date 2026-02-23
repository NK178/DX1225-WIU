using UnityEngine;

[CreateAssetMenu(fileName = "HealItemData", menuName = "Item/HealItemData")]
public class HealItemData : ItemData
{

    [SerializeField] private float addHealth;

    public override void ItemFunction()
    {
        Debug.Log("WHY IS HANYU NOT HEALING ME PLS");

        //might as well hard code for the player 
        PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (player != null)
        {
            float newHealth = player.GetCurrentHealth() + addHealth;
            player.SetCurrentHealth(newHealth);
        }
    }
}
