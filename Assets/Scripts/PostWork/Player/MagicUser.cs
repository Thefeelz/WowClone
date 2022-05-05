using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicUser : BasePlayerStats
{
    [SerializeField] protected int maxPlayerMana;
    [SerializeField] protected int currentPlayerMana;
    [SerializeField] protected int playerSpirit;
    [SerializeField] protected int playerIntellect;
    [SerializeField] protected int playerMP5;
    protected int additionalIntellect, additionalSpirit, totalIntellect, totalSpirit;
    int baseMana = 20;
    int baseMP5 = 5;

    protected override void Start()
    {
        baseMana += (playerLevel * 10);
        maxPlayerMana = baseMana + (playerIntellect * 5);
        currentPlayerMana = maxPlayerMana;

        baseMP5 += (playerLevel * 5);
        playerMP5 = baseMP5 + (playerSpirit * 2);
        StartCoroutine(playerMP5Function());

        base.Start();
    }
    public void SetPlayerMaxMana(int mana) { maxPlayerMana = mana; }
    public int GetPlayerMaxMana() { return maxPlayerMana; }
    public void SetPlayerCurrentMana(int mana) { currentPlayerMana = mana; }
    public int GetPlayerCurrentMana() { return currentPlayerMana; }
    public float GetPlayerCurrentManaFillAmount() { return (float)currentPlayerMana / maxPlayerMana; }
    public void SetPlayerSpirit(int spirit) { playerSpirit = spirit; }
    public int GetPlayerSpirit() { return playerSpirit; }
    public void SetPlayerIntellect(int intellect) { playerIntellect = intellect; }
    public int GetPlayerIntellect() { return playerIntellect; }
    public void SetPlayerAdditionalIntellect(int _additionalIntellect) { additionalIntellect = _additionalIntellect; }
    public int GetPlayerAdditionalIntellect() { return additionalIntellect; }
    public void SetPlayerTotalIntellect(int _totalIntellect) { totalIntellect = _totalIntellect; }
    public int GetPlayerTotalIntellect() { return totalIntellect; }
    public void SetPlayerAdditionalSpirit(int _additionalSpirit) { additionalSpirit = _additionalSpirit; }
    public int GetPlayerAdditionalSpirit() { return additionalSpirit; }

    public void ApplyAllAdditionalStats (int _stamina, int _intellect, int _spirit, int _haste, int _criticalStrike)
    {
        additionalCriticalStrike = _criticalStrike;
        additionalHaste = _haste;
        additionalIntellect = _intellect;
        additionalSpirit = _spirit;
        additionalStamina = _stamina;
        UpdateTotalValues();
    }
    void UpdateTotalValues()
    {
        totalCriticalStrike = playerCriticalStrike + additionalCriticalStrike;
        totalHaste = playerHaste + additionalHaste;
        totalIntellect = playerIntellect + additionalIntellect;
        totalSpirit = playerSpirit + additionalSpirit;
        totalStamina = playerStamina + additionalStamina;
        ApplyTotalValues();
    }
    void ApplyTotalValues()
    {
        StaminaToHealthCalculator();
        IntellectToManaCalculator();
        SpiritToMP5Calculator();
    }
    void IntellectToManaCalculator()
    {
        maxPlayerMana = baseMana + (totalIntellect * 5);
    }
    void SpiritToMP5Calculator()
    {
        playerMP5 = baseMP5 + (totalSpirit * 2);
    }
    override protected void LevelUpPlayer(int experienceToAdd)
    {
        // Called so we actually level up
        base.LevelUpPlayer(experienceToAdd);
        baseMana += (playerLevel * 10);
        baseMP5 += (playerLevel * 5);
        ApplyTotalValues();
        currentPlayerMana = maxPlayerMana;
    }
    IEnumerator playerMP5Function()
    {
        while (true)
        {
            int difference = 0;
            yield return new WaitForSeconds(1f);
            if (currentPlayerMana < maxPlayerMana)
            {
                if (((currentPlayerMana + playerMP5) * 0.2f) > maxPlayerMana)
                {
                    difference = maxPlayerMana - (currentPlayerMana + playerMP5);
                    currentPlayerMana += Mathf.RoundToInt((difference) * 0.2f);
                }
                else
                    currentPlayerMana += Mathf.RoundToInt(playerMP5 * 0.2f);
            }
            else
                currentPlayerMana = maxPlayerMana;
        }
    }
    public void HandleMagicBuffs(Buffs newBuff, bool addingToBuffs)
    {
        if (addingToBuffs)
        {          
            totalCriticalStrike = Mathf.RoundToInt(totalCriticalStrike * newBuff.GetCriticalStrikePercentage());          
            totalHaste = Mathf.RoundToInt(totalHaste * newBuff.GetHastePercentage());         
            totalIntellect = Mathf.RoundToInt(totalIntellect * newBuff.GetIntellectPercentage());           
            totalSpirit = Mathf.RoundToInt(totalSpirit * newBuff.GetSpiritPercentage());
            totalStamina = Mathf.RoundToInt(totalStamina * newBuff.GetStaminaPercentage());
        }
        else
        {           
            totalCriticalStrike = Mathf.RoundToInt(totalCriticalStrike / newBuff.GetCriticalStrikePercentage());          
            totalHaste = Mathf.RoundToInt(totalHaste / newBuff.GetHastePercentage());           
            totalIntellect = Mathf.RoundToInt(totalIntellect / newBuff.GetIntellectPercentage());
            totalSpirit = Mathf.RoundToInt(totalSpirit / newBuff.GetSpiritPercentage());
            totalStamina = Mathf.RoundToInt(totalStamina / newBuff.GetStaminaPercentage());
        }
        ApplyTotalValues();
    }
    public void HandleMagicProcs(Procs newProc)
    {
        Abilities newAbility = playerSpellBook.GetAbilityFromList(newProc.GetAbilityIdToEffect());
        if(newProc.GetIsSpellCastReduction())
        {
            if(newProc.GetIsInstantCast())
            {
                newAbility.SetBuffAbilityCastTimeReduction(newAbility.GetAbilityCastTime());
            }
            else if(newProc.GetIsIncrimentedReducedCast())
            {              
                newAbility.SetBuffAbilityCastTimeReduction(newProc.GetAmountToReduceCastPerStack() * newProc.GetCurrentStacks());
            }
        }
        if (newProc.GetIsCooldownReduction())
        {
            if(newProc.GetIsInstantCast())
            {
                newAbility.SetAbilityCurrentCooldownTime(0);
            }
            else if (newProc.GetIsIncrimentedCooldownReduction())
            {
                if (newAbility.GetAbilityCurrentCooldownTime() - newProc.GetAmountToReduceCooldownPerStack() >= 0)
                    newAbility.SetAbilityCurrentCooldownTime(newAbility.GetAbilityCurrentCooldownTime() - newProc.GetAmountToReduceCooldownPerStack());
                else
                    newAbility.SetAbilityCurrentCooldownTime(0);
            }
        }
    }
}
