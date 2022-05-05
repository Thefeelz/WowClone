using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageClass : MagicUser
{
    protected override void Start()
    {
        base.Start();
    }

    public void GetAbilityFromSpellBook(int spellID)
    {
        if (playerSpellBook.GetAbilityFromList(spellID))
        {
            useAbility.UsePlayerAbility(playerSpellBook.GetAbilityFromList(spellID));
            /*if (playerSpellBook.GetAbilityFromList(spellID).GetComponent<DamageMagic>())
            {
                useAbility.UsePlayerAbility(playerSpellBook.GetAbilityFromList(spellID));
            }
            else if (playerSpellBook.GetAbilityFromList(spellID).GetComponent<MagicBuffs>())
            {
                useAbility.UsePlayerAbility(playerSpellBook.GetAbilityFromList(spellID));
            }*/
        }
      
    }
    // TODO Make a Foreach loop that runs through the spell book and updates the cooldowns within
}
