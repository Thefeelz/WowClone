using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    [SerializeField] Abilities[] abilityList;
    public Abilities[] GetAbilityList()
    {
        return abilityList;
    }

    public Abilities GetAbilityFromList(int abilityID) 
    {
        foreach (Abilities abilities in abilityList)
        {
            if(abilities.GetAbilityID() == abilityID)
            {
                return abilities;
            }
        }
        Debug.LogError("In SpellBook.cs, could not find any ability with the ID " + abilityID);
        return null;
    }

    // Get Ability from list and put it on cool down
    // Constantly go through the list and iterate its cooldown

}
