using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity { Common, Uncommon, Refined, Unique, Elite, Super};
public class EquippableItem : Item
{
    [Range(0,11)][SerializeField] protected int equipSlotID;
    [SerializeField] protected Rarity itemRarity;
    [SerializeField] protected int strength, intellect, agility, stamina, haste, spirit, criticalStrike;
    [SerializeField] GameObject model;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetEquipSlotID() { return equipSlotID; }
    public int GetItemStrength() { return strength; }
    public int GetItemIntellect() { return intellect; }
    public int GetItemAgility() { return agility; }
    public int GetItemStamina() { return stamina; }
    public int GetItemHaste() { return haste; }
    public int GetItemSpirit() { return spirit; }
    public int GetItemCrticalStrike() { return criticalStrike; }
    public GameObject GetItemModel() { return model; }
    public void SetItemModel(GameObject _model) { model = _model; }

}
