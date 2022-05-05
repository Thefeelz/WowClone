using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMagic : MagicAbilities
{
    [Header("Bonus Ability Effects")]
    [SerializeField] protected bool criticalStrikeExtraEffects;
    [SerializeField] protected bool spellCooldownResetOnCrit;
    [SerializeField] protected int spellIDCooldownResetOnCrit;
    [SerializeField] protected bool damageBoostOnHealthPercentage;
    [SerializeField] protected bool abovePercentage;
    [Range(0, 1)] [SerializeField] protected float percentDamageIncreaseOnHealthPercentage, enemyHealthThresholdForDamageBoost;
    [SerializeField] protected int abilityDamage;
    [SerializeField] protected float abilityMovementSpeed;
    
    protected float playerCrit;

    protected override void Update()
    {
        base.Update();
    }
    public void SetAbilityDamage(int _abilityDamage) { abilityDamage = _abilityDamage; }
    public int GetAbilityDamage() { return abilityDamage; }
    public float GetSnapShottedCrit() { return playerCrit; }
    public void SetAbilityMovementSpeed(float _abilityMovementSpeed) { abilityMovementSpeed = _abilityMovementSpeed; }
    public float GetAbilityMovementSpeed() { return abilityMovementSpeed; }

    public void CalculateAbilityDamage(int intellect, float _playerCrit, BaseEnemyStats target)
    {
        abilityDamage = Mathf.RoundToInt(abilityDamage * .1f  * intellect  + (abilityDamage * 0.5f)) ;
        abilityDamage = Mathf.RoundToInt(Random.Range(abilityDamage * 0.8f, abilityDamage * 1.2f));
        if (damageBoostOnHealthPercentage && !abovePercentage && target.GetEnemyHealthBarFill() <= enemyHealthThresholdForDamageBoost)
            abilityDamage = Mathf.RoundToInt(abilityDamage + (abilityDamage * percentDamageIncreaseOnHealthPercentage));
        else if (damageBoostOnHealthPercentage && abovePercentage && target.GetEnemyHealthBarFill() >= enemyHealthThresholdForDamageBoost)
            abilityDamage = Mathf.RoundToInt(abilityDamage + (abilityDamage * percentDamageIncreaseOnHealthPercentage));
        playerCrit = _playerCrit;
    }
    public bool HasCriticalStrikeExtraEffects() { return criticalStrikeExtraEffects; }
    public bool HasResetCooldownOnCriticalStrike() { return spellCooldownResetOnCrit; }
    public void HandleResetCooldownOnCrticalStrike()
    {
        player.GetPlayerSpellbook().GetAbilityFromList(spellIDCooldownResetOnCrit).SetAbilityCurrentCooldownTime(0f);
        //SetToSpellFlash(spellIDCooldownResetOnCrit);
    }
}
