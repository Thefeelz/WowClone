using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLootTable : MonoBehaviour
{
    [SerializeField] List<Item> enemyPossibleLootTable;
    [SerializeField] int maxEnemyGoldDrop;
    [SerializeField] int maxIterations = 5;
    [SerializeField] int maxNumberOfItemsDropped = 1;

    // TODO Unserialize these fields after testing
    [SerializeField] List<Item> enemyActualLootTable;
    [SerializeField] int actualGoldDrop;


    bool containsLoot;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateLoot()
    {
        int i = 0;
        while(enemyActualLootTable.Count < maxNumberOfItemsDropped)
        {
            foreach (Item item in enemyPossibleLootTable)
            {
                if(rollTheDiceOnLoot(item))
                {
                    enemyActualLootTable.Add(item);
                    if (enemyActualLootTable.Count >= maxNumberOfItemsDropped)
                        break;
                }
            }
            i++;
            if (i >= maxIterations)
                break;
        }
        actualGoldDrop = Random.Range(0, maxEnemyGoldDrop);
        if (enemyActualLootTable.Count > 0 || actualGoldDrop > 0)
            containsLoot = true;
        else
            containsLoot = false;
    }
    public void OpenLootWindow()
    {
        PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
        for(int i = 0; i< enemyActualLootTable.Count; i++)
        {
            if(inventory.AddItemToInventory(enemyActualLootTable.ToArray()[i]))
            {
                enemyActualLootTable.Remove(enemyActualLootTable.ToArray()[i]);
            }
            else
            {
                Debug.Log("Failed");
            }
        }
        inventory.AddGoldToInventory(actualGoldDrop);
        actualGoldDrop = 0;
        Destroy(gameObject, 10f);
    }
    bool rollTheDiceOnLoot(Item itemToChance)
    {
        int i = Random.Range(0, 100);
        if (itemToChance.GetItemDropChance() >= i)
            return true;
        return false;
    }
    public bool GetContainsLoot() { return containsLoot; }
}
