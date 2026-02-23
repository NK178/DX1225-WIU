using System.Collections.Generic;
using UnityEngine;


//me when unity cant do 2d array rip 

[System.Serializable]
public class CraftingRecipe
{
    //[Header("3x3 GRID FROM TOP LEFT")]
    //[SerializeField] public A_Item outputItem;
    //// [SerializeField] public int outputQty;
    //[SerializeField] public A_Item[] recipe = new A_Item[9];

}

public class CraftingSystem : MonoBehaviour
{

    //[SerializeField] private A_InventoryManager inventory;
    //[SerializeField] private List<A_InventoryGrid> gridList;

    [SerializeField] private List<CraftingRecipe> recipes;

    private bool isTableEmpty, shouldCraft;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isTableEmpty = true;
    }

    // Update is called once per frame
    void Update()
    {

        // Debug.Log("ISTABLEEMPTY " + isTableEmpty);

        //CheckTableState();

        //if (!isTableEmpty)
        //{
        //    int craftIndex = CheckValidCraft();
        //    // if can craft 
        //    if (craftIndex != -1)
        //        shouldCraft = true;


        //    if (shouldCraft)
        //    {
        //        CraftItemAndAddToInventory(craftIndex);
        //        EmptyCraftingTable();
        //    }
        //}


    }

    //void CheckTableState()
    //{
    //    foreach (A_InventoryGrid grid in gridList)
    //    {
    //        if (!grid.isEmpty)
    //        {
    //            isTableEmpty = false;
    //            return;
    //        }
    //    }
    //    isTableEmpty = true;
    //}

    //int CheckValidCraft()
    //{
    //    //compare the grid with the recipes 
    //    int craftIndex = -1;

    //    for (int rep = 0; rep < recipes.Count; rep++)
    //    {
    //        bool isCraftValid = true;

    //        for (int iter = 0; iter < 9; iter++)
    //        {
    //            A_DraggableItem a_Item = gridList[iter].GetComponentInChildren<A_DraggableItem>();
    //            A_Item recipe_Item = recipes[rep].recipe[iter];
    //            //method to handle filled slots 
    //            if (a_Item != null)
    //            {
    //                if (recipe_Item.GetName() != a_Item.GetItemName())
    //                {
    //                    isCraftValid = false;
    //                    break;
    //                }
    //            }
    //            else
    //            {
    //                //if crafting table grid null but recipe not null, fail craft
    //                if (recipe_Item.GetName() != "blank")
    //                {
    //                    isCraftValid = false;
    //                    break;
    //                }
    //            }
    //        }

    //        //only exit if craft is valid 
    //        if (isCraftValid)
    //        {
    //            craftIndex = rep;
    //            break;
    //        }
    //    }

    //    return craftIndex;
    //}


    //void CraftItemAndAddToInventory(int index)
    //{
    //    A_Item output = recipes[index].outputItem;

    //    inventory.AddItemToInventory(output.gameObject);
    //}

    //void EmptyCraftingTable()
    //{
    //    foreach (A_InventoryGrid grid in gridList)
    //    {
    //        grid.RemoveItem();
    //    }
    //    shouldCraft = false;
    //}



}
