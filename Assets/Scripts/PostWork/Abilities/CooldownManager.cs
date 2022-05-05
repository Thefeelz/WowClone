using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownManager : MonoBehaviour
{
    List<Abilities> abilitiesOnCooldown = new List<Abilities>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(abilitiesOnCooldown.Count > 0)
        {
            UpdateAbilitiesOnCooldownList();
        }
    }

    void UpdateAbilitiesOnCooldownList()
    {
        for(int i = 0; i < abilitiesOnCooldown.Count; i++)
        {
            //abilitiesOnCooldown.ToArray()[i].
        }
    }
    public void AddAbilityToCooldownList(Abilities ability) { abilitiesOnCooldown.Add(ability); }
    public void AddAbilityToCooldownListForGlobalCooldown(Abilities ability) 
    {
        ability.SetAbilityCurrentCooldownTime(1f);
        abilitiesOnCooldown.Add(ability); 
    }
}
