using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightInstance : ScriptableObject
{
    public float timeInFight = 0;
    public int totalDamageDealt = 0;
    public int id = 0;
    public List<AbilityInstance> abilitiesUsed = new List<AbilityInstance>();
    public AbilityInstance[] printoutAbilityArray;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateAbilitiesUsedList(int damage, bool crit, int spellID)
    {
        if (!AbiltyInList(spellID))
            AddAndUpdate(damage, crit, spellID);
        else
            UpdateOnly(damage, crit, spellID);
    }
    bool AbiltyInList(int spellID)
    {
        foreach (AbilityInstance ability in abilitiesUsed)
        {
            if (ability.abilityID == spellID)
                return true;
        }
        return false;
    }
    void AddAndUpdate(int damage, bool crit, int spellID)
    {
        Debug.Log("Add and Update");
        abilitiesUsed.Add(new AbilityInstance(spellID, damage, crit));
    }
    void UpdateOnly(int damage, bool crit, int spellID)
    {
        Debug.Log("Update Only");
        int i = AbilityArrayLocation(spellID);
        if (i >= 0)
            abilitiesUsed.ToArray()[i].UpdateInstance(damage, crit);
    }
    int AbilityArrayLocation(int spellID)
    {
        int i = 0;
        foreach (AbilityInstance abilityInstance in abilitiesUsed)
        {
            if (abilityInstance.abilityID == spellID)
                return i;
            i++;
        }
        return -1;
    }
    public string GeneratePrintoutForFightInstance(SpellBook spellBook)
    {
        ReorderList();
        string message = "Fight ID: " + id +"\n";
        foreach (AbilityInstance instance in printoutAbilityArray)
        {
            message += "" + spellBook.GetAbilityFromList(instance.abilityID).GetAbilityName() + instance.GeneratePrintout();
        }
        return message;
    }
    public AbilityInstance[] GetPrintOutArray() 
    {
        ReorderList();
        return printoutAbilityArray; 
    }
        
    void ReorderList()
    {
        printoutAbilityArray = new AbilityInstance[abilitiesUsed.Count - 1];
        printoutAbilityArray = abilitiesUsed.ToArray();
        AbilityInstance tempInstance;   
        for(int i = 0; i < abilitiesUsed.Count - 1; i++)
        {
            for(int j = 0; j < abilitiesUsed.Count - i - 1; j++)
            {
                if(printoutAbilityArray[j].abilityTotalDamage < printoutAbilityArray[j + 1].abilityTotalDamage)
                {
                    tempInstance = printoutAbilityArray[j + 1];
                    printoutAbilityArray[j + 1] = printoutAbilityArray[j];
                    printoutAbilityArray[j] = tempInstance;
                }
            }
        }

    }
}
