using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName ="New Quest", menuName ="Quests")]
public class Quest : ScriptableObject
{
    [SerializeField] string questTitle = "";
    [TextAreaAttribute(5, 10)]
    [SerializeField] string questDescription = "";
    [Header("Requirements")]
    [SerializeField] List<EnemyRequirements> requirements;
    [SerializeField] int questID;
    [SerializeField] bool activeQuest;
    [SerializeField] int experienceReward;
    [SerializeField] int goldReward;
    [SerializeField] int levelRequirement;
    [SerializeField] List<Item> itemRewards = new List<Item>();
    [SerializeField] List<int> requiredQuests = new List<int>();

    bool allRequirementsMet = false;

    public void UpdateQuest(int enemyKilledID)
    {
        foreach (EnemyRequirements enemyRequirements in requirements)
        {
            if (enemyKilledID == enemyRequirements.GetEnemyTypeID())
            {
                enemyRequirements.IncrimentQuestKillCount();
            }
        }

        foreach(EnemyRequirements enemyRequirements in requirements)
        {
            if(!enemyRequirements.AreRequirementsMet())
            {
                allRequirementsMet = false;
                break;
            }
        }
        allRequirementsMet = true;
    }

    public bool CheckIfAllRequirementsAreMet() 
    {
        foreach (EnemyRequirements enemy in requirements)
        {
            if (!enemy.AreRequirementsMet())
                return false;
        }
        return true; 
    }

    public int GetQuestID() { return questID;}

    public string GetQuestTitle() { return questTitle; }
    public string GetQuestDescription() { return questDescription; }

    public string GetRequirementsForUI() { return CreateEnemyRequirementsString(); }

    string CreateEnemyRequirementsString()
    {
        string stringToCreate = "";
        foreach (EnemyRequirements enemy in requirements)
        {
            stringToCreate += enemy.GetEnemyForEnemyRequirements().GetEnemyName() + ": " + enemy.currentKillCount + " / " + enemy.GetTotalAmountToFulfillRequirements() + "\n";
        }

        return stringToCreate;
    }
    public void SetQuestActive(bool value) { activeQuest = value; }
    public bool GetQuestActive() { return activeQuest; }

    public bool GetQuestComplete() { return CheckIfAllRequirementsAreMet(); }
    public int GetQuestExperience() { return experienceReward; }
    public int GetGoldReward() { return goldReward; }
    public string GetRewardText() { return "Gold: " + goldReward + ", Experience: " + experienceReward; }

    public int GetLevelRequirement() { return levelRequirement; }
    public List<int> GetCompletedQuestsRequirements() { return requiredQuests; }
    public List<Item> GetItemRewards() { return itemRewards; }
    public int GetItemRewardSize() { return itemRewards.Count; }
}
