using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIItemHolder : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Item item;
    CharacterEquipment characterEquipment;
    InventoryUIManager inventoryUIManager;
    PlayerInventory playerInventory;
    int row;
    int column;
    private void Start()
    {
        characterEquipment = FindObjectOfType<CharacterEquipment>();
        inventoryUIManager = FindObjectOfType<InventoryUIManager>();
        playerInventory = FindObjectOfType<PlayerInventory>();
    }
    public void AddItem(Item _item) 
    { 
        item = _item;
        GetComponent<Image>().sprite = item.GetItemSprite();
    }
    public Item GetItemBeingHeld() { return item; }

    // Unity Event Reference
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData != null && item)
        {
            // Check for Right Click
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                // Cast to see if the item is equippable
                if ((EquippableItem)item)
                {
                    if(characterEquipment.EquipItem((EquippableItem)item))
                    {
                        playerInventory.RemoveItemFromInventoryByRowAndColumn(row, column);
                        item = null;
                    }
                    else
                    {
                        item = (characterEquipment.SwapAndEquipItem((EquippableItem)item));
                        playerInventory.SwapItemFromInventoryByRowAndColumn(row, column, item);
                    }
                    inventoryUIManager.UpdateItemsInInventory();
                }
            }
        }
    }

    public int GetItemHolderRow() { return row; }
    public int GetItemHolderColumn() { return column; }
    public void SetItemHolderRowAndColumnReference(int _row, int _column)
    {
        row = _row;
        column = _column;
    }
}
