using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{
    [SerializeField] EquippableItem[] equippedGear = new EquippableItem[13];
    [SerializeField]int stamina, haste, criticalStrike, strength, intellect, agility, spirit;
    [SerializeField] GameObject twoHandedWeaponSlot;
    // Start is called before the first frame update
    void Start()
    {
        UpdateAllResourceValues();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool EquipItem(EquippableItem item)
    {
        if(equippedGear[item.GetEquipSlotID()] == null)
        {
            //Debug.Log("This should be null, getting filled with " + item.GetItemName() + " and returning true");
            equippedGear[item.GetEquipSlotID()] = item;
            AddStats(item);
            if (item.GetEquipSlotID() == 6)
            {
                GameObject newItem = Instantiate(item.GetItemModel());
                newItem.transform.SetParent(twoHandedWeaponSlot.transform);
                newItem.transform.localPosition = Vector3.zero;
                newItem.transform.localRotation = Quaternion.identity;
            }
            return true;
        }
        //Debug.Log("We are returning false");
        return false;
    }
    public EquippableItem SwapAndEquipItem(EquippableItem item)
    {
        //Debug.Log("Swap and Equip: Item getting passed in is " + item.GetItemName() + " and item it is replacing is " + equippedGear[item.GetEquipSlotID()].GetItemName());
        EquippableItem tempHolder;
        tempHolder = equippedGear[item.GetEquipSlotID()];
        RemoveStats(tempHolder);
        equippedGear[item.GetEquipSlotID()] = item;
        AddStats(item);
        GameObject newItem = Instantiate(item.GetItemModel());
        Destroy(twoHandedWeaponSlot.transform.GetChild(0).gameObject);
        newItem.transform.SetParent(twoHandedWeaponSlot.transform);
        newItem.transform.localPosition = Vector3.zero;
        newItem.transform.localRotation = Quaternion.identity;
        return tempHolder;
    }

    void UpdateAllResourceValues()
    {
        for(int i = 0; i < equippedGear.Length; i++)
        {
            if (equippedGear[i] != null)
                AddStats(equippedGear[i]);
        }
    }
    void AddStats(EquippableItem item)
    {
        CalculateAgility(item.GetItemAgility());
        CalculateCriticalStrike(item.GetItemCrticalStrike());
        CalculateHaste(item.GetItemHaste());
        CalculateIntellect(item.GetItemIntellect());
        CalculateSpirit(item.GetItemSpirit());
        CalculateStamina(item.GetItemStamina());
        CalculateStrength(item.GetItemStrength());
        UpdatePlayerAdditionalStats();
    }
    void RemoveStats(EquippableItem item)
    {
        CalculateAgility(-item.GetItemAgility());
        CalculateCriticalStrike(-item.GetItemCrticalStrike());
        CalculateHaste(-item.GetItemHaste());
        CalculateIntellect(-item.GetItemIntellect());
        CalculateSpirit(-item.GetItemSpirit());
        CalculateStamina(-item.GetItemStamina());
        CalculateStrength(-item.GetItemStrength());
        UpdatePlayerAdditionalStats();
    }
    void CalculateStamina(int value) { stamina += value; }
    void CalculateIntellect(int value) { intellect += value; }
    void CalculateAgility(int value) { agility += value; }
    void CalculateStrength(int value) { strength += value; }
    void CalculateHaste(int value) { haste += value; }
    void CalculateCriticalStrike(int value) { criticalStrike += value; }
    void CalculateSpirit(int value) { spirit += value; }

    void UpdatePlayerAdditionalStats()
    {
        if(GetComponent<BasePlayerStats>().GetClassType() == BasePlayerStats.ClassType.Intellect)
        {
            ReturnMagicStats();
        }
    }
    void ReturnMagicStats()
    {
        MagicUser player = GetComponent<MagicUser>();
        player.ApplyAllAdditionalStats(stamina, intellect, spirit, haste, criticalStrike);
    }
}
