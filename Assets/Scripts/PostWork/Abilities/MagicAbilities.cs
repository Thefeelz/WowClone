using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAbilities : Abilities
{
    [SerializeField] protected int abilityManaCost;

    protected override void Update()
    {
        base.Update();
    }
    public void SetAbilityManaCost(int _abilityManaCost) { abilityManaCost = _abilityManaCost; }
    public int GetAbilityManaCost() { return abilityManaCost; }
}
