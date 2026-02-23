using UnityEngine;

[CreateAssetMenu(fileName = "DamageItemData", menuName = "Item/DamageItemData")]
public class DamageItemData : ItemData
{

    [SerializeField] private float damageMultiplier;

    public override void ItemFunction()
    {
        Debug.Log("Damage Multiplier");

        //might as well hard code for the player 
        PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (player != null)
        {
            player.SetDamageMultiplier(damageMultiplier);
        }
    }
}
