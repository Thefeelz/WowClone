using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] Item[,] itemsInInventory;
    [SerializeField] UIItemHolder[,] itemHolderArray;
    GameObject inventoryUIHandler;
    Text goldText;

    int numOfColumns = 5;
    int numOfRows = 7;
    void Awake()
    {
        itemsInInventory = new Item[numOfRows, numOfColumns];
        itemHolderArray = new UIItemHolder[numOfRows, numOfColumns];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetUpInventoryUI(Transform handler)
    {
        inventoryUIHandler = handler.gameObject;
        int i = 0;
        int j = 0;
        foreach (Transform based in inventoryUIHandler.transform)
        {
            
            foreach (Transform item in based)
            {
                
                if (item.CompareTag("UInventoryRow"))
                {
                    foreach (Transform row in item)
                    {
                        foreach (Transform column in row)
                        {
                            itemHolderArray[i, j] = column.GetComponent<UIItemHolder>();
                            itemHolderArray[i, j].GetComponent<Image>().color = NewColor(217f, 217f, 217f, 1f);
                            itemHolderArray[i, j].SetItemHolderRowAndColumnReference(i, j);
                            j++;
                        }
                        j = 0;
                        i++;
                    }
                }
                else if (item.CompareTag("UINameText"))
                {
                    goldText = item.GetComponentInChildren<Text>();
                    goldText.text = FindObjectOfType<PlayerInventory>().GetTotalGold().ToString();
                }
            }
        }
        UpdateItemsInInventory(); 
    }
    public void ToggleInventoryHandler()
    {
        inventoryUIHandler.SetActive(!inventoryUIHandler.activeSelf);
    }
    Color NewColor(float r, float g, float b, float a)
    {
        return new Color(r / 255, g / 255, b / 255, a);
    }
    public void UpdateItemsInInventory()
    {
        Item[,] tempArray = FindObjectOfType<PlayerInventory>().GetPlayerInventory();
        for(int i = 0; i < numOfRows; i++)
        {
            for(int j = 0; j < numOfColumns; j++)
            {
                if (tempArray[i, j])
                    itemHolderArray[i, j].AddItem(tempArray[i, j]);
                else
                    itemHolderArray[i, j].GetComponent<Image>().sprite = null;
            }
        }
    }
    public void UpdateGoldAmount(int goldAmount) { goldText.text = goldAmount.ToString(); }
}
