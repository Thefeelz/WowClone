using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityInstance
{
    public int abilityID;
    public int abilityTotalDamage;
    public int abilityBiggestHit;
    public int abilityNumberOfHits;
    public int abilityNumberOfCrits;
    public AbilityInstance(int _abilityID, int _damage, bool _crit)
    {
        abilityID = _abilityID;
        abilityBiggestHit = _damage;
        if (_crit)
            abilityNumberOfCrits = 1;
        else
            abilityNumberOfCrits = 0;
        abilityTotalDamage = _damage;
        abilityNumberOfHits++;
    }
    public void UpdateInstance(int _damage, bool _crit)
    {
        abilityNumberOfHits++;
        abilityTotalDamage += _damage;
        if (_damage > abilityBiggestHit)
            abilityBiggestHit = _damage;
        if (_crit)
            abilityNumberOfCrits++;
    }
    public string GeneratePrintout()
    {
        string message = " = Total Damage: " + abilityTotalDamage + " = " + " Crit %: " + (((float)abilityNumberOfCrits / abilityNumberOfHits) * 100).ToString("F1") + " = Biggest Hit: " + abilityBiggestHit + "\n";
        return message; 
    }
}
