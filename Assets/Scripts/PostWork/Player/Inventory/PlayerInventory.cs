using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] Item[,] inventory = new Item[7,5];
    [SerializeField] Item[] initialInventory;
    [SerializeField] int maxInventorySize;
    [SerializeField] int gold;
    InventoryUIManager inventoryUIManager;
    int numOfRows = 7;
    int numOfColumns = 5;
    bool startUp = true;
    
    private void Awake()
    {
        inventory = new Item[numOfRows,numOfColumns];
        inventoryUIManager = FindObjectOfType<InventoryUIManager>();
        InitializeItemsInInventory();
    }

    void InitializeItemsInInventory()
    {
        for(int i = 0; i < initialInventory.Length; i++)
        {
            AddItemToInventory(initialInventory[i]);
        }
        startUp = false;
    }
    public bool AddItemToInventory(Item item)
    {
        for(int i = 0; i < numOfRows; i++)
        {
            for(int j = 0; j < numOfColumns; j++)
            {
                if(inventory[i,j] == null)
                {
                    inventory[i, j] = item;

                    // This is here because we call the update items in inventory, during start up it would be getting called a bazillion times if we dont have this bool protecting us on start up
                    if(!startUp)
                        inventoryUIManager.UpdateItemsInInventory();
                    return true;
                }
            }
        }
        return false;
        
    }
    public bool AddItemToInventory(int row, int column, Item item)
    {
        if (inventory[row, column] == null)
        {
            inventory[row, column] = item;
            return true;
        }
        else
            return AddItemToInventory(item);
    }
    public void AddGoldToInventory(int _gold)
    {
        gold += _gold;
        inventoryUIManager.UpdateGoldAmount(gold);
    }

    public Item[,] GetPlayerInventory() 
    {
        return inventory; 
    }
    public int GetTotalGold() { return gold; }
    public void RemoveItemFromInventoryByRowAndColumn(int row, int column)
    {
        inventory[row, column] = null;
        inventoryUIManager.UpdateItemsInInventory();
    }
    public Item SwapItemFromInventoryByRowAndColumn(int row, int column, Item item)
    {
        Item returnedItem = inventory[row, column];
        inventory[row, column] = item;
        return returnedItem;

    }
}
