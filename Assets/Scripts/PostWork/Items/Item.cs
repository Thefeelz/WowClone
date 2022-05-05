using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Items/Base Item")]
public class Item : ScriptableObject
{
    [SerializeField] protected string itemName;
    [SerializeField] protected Sprite itemImage;
    [SerializeField] protected int itemID;
    [SerializeField] protected int itemValue;
    [SerializeField] protected int itemDropChance;

    public int GetItemDropChance() { return itemDropChance; }
    public string GetItemName() { return itemName; }

    public Sprite GetItemSprite() { return itemImage; }
}
