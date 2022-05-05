using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerStats : MonoBehaviour
{
    public enum ClassType { Agility, Intellect, Strength };
    [SerializeField] ClassType classType;
    [SerializeField] protected SpellBook playerSpellBook;
    [SerializeField] protected string playerName;
    [SerializeField] protected int playerLevel, currentPlayerExperience, maxPlayerExperience, maxPlayerHealth, currentPlayerHealth, playerCriticalStrike, playerHaste, playerStamina;
    [SerializeField] Sprite playerPortrait;
    [SerializeField] protected int additionalCriticalStrike, additionalHaste, additionalStamina, totalCriticalStrike, totalHaste, totalStamina;
    int baseHealth = 40;

    protected float globalCoolDown = 1f;
    protected float timeSinceCast = 0;
    protected bool onGCD = false;
    protected UPlayerUI myUI;
    protected UseAbility useAbility;
    protected Abilities[] playerAbilityList;
    protected BuffManager buffManager;

    float elapsedTime;

    bool inCombat;

    protected virtual void Start()
    {
        baseHealth += playerLevel * 10;
        // playerName = GameManager.Instance.GetPlayerNameFromMenu();
        maxPlayerHealth = baseHealth + playerStamina * 5;
        currentPlayerHealth = maxPlayerHealth;


        myUI = FindObjectOfType<UPlayerUI>();
        useAbility = GetComponent<UseAbility>();
        buffManager = GetComponent<BuffManager>();
        playerAbilityList = playerSpellBook.GetAbilityList();
    }
    private void Update()
    {
        if (onGCD) { UpdateGCD(); }
        UpdateAbilityList();
        PassiveHealthRegeneration();
    }

    void UpdateAbilityList()
    {
        for(int i = 0; i < playerAbilityList.Length; i++)
        {
            if(!playerAbilityList[i].IsAbilityOffCooldown() && !playerAbilityList[i].GetAlreadyCalculatedCooldown())
            {
                playerAbilityList[i].SetAbilityCurrentCooldownTime(playerAbilityList[i].GetAbilityCurrentCooldownTime() - Time.deltaTime);
                playerAbilityList[i].SetCalculateCooldown(true);
            }
            if(playerAbilityList[i].GetAbilityCurrentCooldownTime() <= 0 && !playerAbilityList[i].IsAbilityOffCooldown())
            {
                playerAbilityList[i].SetAbilityCurrentCooldownTime(0);
                playerAbilityList[i].PutAbilityOffCooldown();
            }
        }
        for(int i = 0; i < playerAbilityList.Length; i++)
        {
            playerAbilityList[i].SetCalculateCooldown(false);
        }
    }
    private void UpdateGCD()
    {
        timeSinceCast += Time.deltaTime;
        myUI.UpdateGlobalCooldown(timeSinceCast / globalCoolDown);
        if(timeSinceCast >= globalCoolDown)
        {
            onGCD = false;
            timeSinceCast = 0;
        }
    }
    public void SetOnGCD(bool value) 
    { 
        if(value == false)
        {
            timeSinceCast = 0;
            myUI.UpdateGlobalCooldown(1);
        }
        onGCD = value; 
    }
    public bool GetOnGCD() { return onGCD; }

    public float GetGCDFillAmount() { return timeSinceCast / globalCoolDown; }

    // Getters and Setters for Possible use in the future
    public SpellBook GetPlayerSpellbook() { return playerSpellBook; }
    public string GetPlayerName() { return playerName; }
    public void SetPlayerName(string _playerName) { playerName = _playerName; }
    public int GetPlayerLevel() { return playerLevel; }
    public void SetPlayerLevel(int _playerLevel) { playerLevel = _playerLevel; }
    public int GetPlayerMaxHealth() { return maxPlayerHealth; }
    public void SetPlayerMaxHealth(int _maxHealth) { maxPlayerHealth = _maxHealth; }
    public int GetPlayerCurrentHealth() { return currentPlayerHealth; }
    public void SetPlayerCurrentHealth(int _currentHealth) { currentPlayerHealth = _currentHealth; }
    public float GetPlayerHealthBarFillAmount() { return (float)currentPlayerHealth / maxPlayerHealth; }
    public int GetPlayerCriticalStrike() { return playerCriticalStrike; }
    public void SetPlayerCriticalStrike(int _criticalStrike) { playerCriticalStrike = _criticalStrike; }
    public int GetAdditionalPlayerCriticalStrike() { return additionalCriticalStrike; }
    public void SetAdditionalPlayerCriticalStrike(int _criticalStrike) { additionalCriticalStrike = _criticalStrike; }
    public float GetPlayerTotalCriticalStrikePercentage() { return totalCriticalStrike * 0.25f; }
    public int GetPlayerHaste() { return playerHaste; }
    public void SetPlayerHaste(int _haste) { playerHaste = _haste; }
    public int GetAdditionalPlayerHaste() { return additionalHaste; }
    public void SetAdditionalPlayerHaste(int _haste) { additionalHaste = _haste; }
    public int GetTotalPlayerHaste() { return totalHaste; }
    public void SetTotalPlayerHaste(int _playerHaste) { totalHaste = _playerHaste; }
    public float GetTotalHasteTimeToReduce() { return totalHaste * .01f; }
    public int GetPlayerStamina() { return playerStamina; }
    public void SetPlayerStamina(int _stamina) { playerStamina = _stamina; }
    public int GetAdditionalPlayerStamina() { return additionalStamina; }
    public void SetAdditionalPlayerStamina(int _stamina) { additionalStamina = _stamina; }
    public void SetPlayerPortrait(Sprite _playerPortrait) { playerPortrait = _playerPortrait; }
    public Sprite GetPlayerPortrait() { return playerPortrait; }
    public int GetPlayerExperience() { return currentPlayerExperience; }
    public float GetPlayerExperienceFillAmount() { return (float)currentPlayerExperience / maxPlayerExperience; }
    public BuffManager GetBuffManager() { return buffManager; }
    public void AddToBuffManager(Buffs buff) { buffManager.AddBuffToList(buff); }

    public void AddExperienceToPlayer(int experienceToAdd)
    {
        if(experienceToAdd + currentPlayerExperience >= maxPlayerExperience)
        {
            LevelUpPlayer(maxPlayerExperience - (experienceToAdd + currentPlayerExperience));
        }
        else
        {
            currentPlayerExperience += experienceToAdd;
        }
        myUI.UpdateExperienceBar();
    }

    protected virtual void LevelUpPlayer(int experienceToAdd)
    {
        maxPlayerExperience = Mathf.FloorToInt(maxPlayerExperience * 1.2f);
        currentPlayerExperience = experienceToAdd;
        playerLevel++;
        baseHealth += playerLevel * 10;
        currentPlayerHealth = maxPlayerHealth;
    }

    public ClassType GetClassType() { return classType; }

    protected void StaminaToHealthCalculator()
    {
        maxPlayerHealth = baseHealth + (totalStamina * 5);
    }
    public void TakeDamage(int damage) { currentPlayerHealth -= damage; }
    void PassiveHealthRegeneration()
    {
        if(currentPlayerHealth >= maxPlayerHealth) 
        {
            currentPlayerHealth = maxPlayerHealth;
            elapsedTime = 0;
            return;
        }
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= 5)
        {
            currentPlayerHealth += Mathf.RoundToInt(maxPlayerHealth * (float).10);
            elapsedTime = 0;
        }
    }
}
