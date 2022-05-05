//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerStats : MonoBehaviour
//{
//    [SerializeField] string playerName;
//    [SerializeField] int playerLevel;
//    [SerializeField] int playerCurrentExperience;
//    [SerializeField] int playerNeededExperience;
//    [SerializeField] float currentPlayerHealth;
//    [SerializeField] float maxPlayerHealth;
//    [SerializeField] float currentPlayerResource;
//    [SerializeField] float maxPlayerResource;
//    [SerializeField] float resourceRegen;
//    [SerializeField] float healthRegen;
//    [SerializeField] float intellect;
//    [SerializeField] float stamina;
//    [SerializeField] float strength;
//    [SerializeField] float agility;
//    [SerializeField] float spirit;
//    [SerializeField] float criticalStrike;

//    int experienceToAdd = 0;
//    LevelUpgrade levelUpgrade;
//    PlayerUI myUI;

//    private void Awake()
//    {
//        currentPlayerHealth = maxPlayerHealth;
//        currentPlayerResource = maxPlayerResource;
//        levelUpgrade = GetComponent<LevelUpgrade>();
//        myUI = GetComponent<PlayerUI>();
        
//    }
//    private void Start()
//    {
//        if (GameManager.Instance.GetPlayerNameFromMenu() != "")
//            playerName = GameManager.Instance.GetPlayerNameFromMenu();
//    }

//    private void Update()
//    {
//        PassiveHealthIncrease();
//        PassiveResourceIncrease();
//    }

//    private void PassiveResourceIncrease()
//    {
//        if (currentPlayerResource < maxPlayerResource)
//            currentPlayerResource += resourceRegen * Time.deltaTime;
//        else
//            currentPlayerResource = maxPlayerResource;
//    }
//    private void PassiveHealthIncrease()
//    {
//        if (currentPlayerHealth < maxPlayerHealth)
//            currentPlayerHealth += healthRegen * Time.deltaTime;
//        else
//            currentPlayerHealth = maxPlayerHealth;
//    }
//    public string GetPlayerName()
//    {
//        return playerName;
//    }
//    public void SetPlayerName(string newName)
//    {
//        playerName = newName;
//    }
//    public float GetCurrentPlayerPassiveHealthRegen()
//    {
//        return healthRegen;
//    }
//    public void SetCurrentPlayerPassiveHealthRegen(float value)
//    {
//        healthRegen = value;
//    }
//    public float GetCurrentPlayerHealth()
//    {
//        return currentPlayerHealth;
//    }
//    public void SetCurrentPlayerHealth(float newValue)
//    {
//        currentPlayerHealth = newValue;
//    }
//    public float GetMaxPlayerHealth()
//    {
//        return maxPlayerHealth;
//    }
//    public void SetMaxPlayerHealth(float newValue)
//    {
//        maxPlayerHealth = newValue;
//    }
//    public float GetCurrentPlayerResource()
//    {
//        return currentPlayerResource;
//    }
//    public void SetCurrentPlayerResource(float newValue)
//    {
//        currentPlayerResource = newValue;
//    }
//    public float GetMaxPlayerResource()
//    {
//        return maxPlayerResource;
//    }
//    public void SetMaxPlayerResource(float newValue)
//    {
//        maxPlayerResource = newValue;
//    }
//    public float GetPlayerIntellect()
//    {
//        return intellect;
//    }
//    public float GetPlayerStrength()
//    {
//        return strength;
//    }
//    public float GetPlayerAgility()
//    {
//        return agility;
//    }
//    public float GetPlayerStamina()
//    {
//        return stamina;
//    }
//    public float GetPlayerSpirit()
//    {
//        return spirit;
//    }
//    public float GetPlayerCriticalStrike()
//    {
//        return criticalStrike;
//    }
//    public int GetPlayerLevel()
//    {
//        return playerLevel;
//    }
//    public void SetPlayerLevel(int newLevel)
//    {
//        playerLevel = newLevel;
//    }
//    public int GetPlayerNeededExperience()
//    {
//        return playerNeededExperience;
//    }
//    public void SetPlayerNeededExperience(int newExperience)
//    {
//        playerNeededExperience = newExperience;
//    }
//    public int GetPlayerCurrentExperience()
//    {
//        return playerCurrentExperience;
//    }
//    public void IncreaseExperience(int gainedExperience)
//    {
//        if(gainedExperience + playerCurrentExperience >= playerNeededExperience)
//        {
//            experienceToAdd = gainedExperience + playerCurrentExperience - playerNeededExperience;
//            levelUpgrade.UpdatePlayerStats();
//            playerCurrentExperience = experienceToAdd;
//            myUI.UpdateLevelUI(false);
//            return;
//        }
//        playerCurrentExperience += gainedExperience;
//        myUI.UpdateExperienceUI((float)playerCurrentExperience / playerNeededExperience);

//    }
//    public void TakeDamage(float damage)
//    {
//        currentPlayerHealth -= damage;
//    }
//}
