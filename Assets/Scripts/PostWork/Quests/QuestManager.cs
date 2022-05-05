using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    UPlayerUI myUI;
    QuestLog questLog;
    public List<Quest> completedQuests = new List<Quest>();
    PlayerInventory playerInventory;

    private void Start()
    {
        myUI = FindObjectOfType<UPlayerUI>();
        questLog = GetComponent<QuestLog>();
        playerInventory = questLog.GetComponent<PlayerInventory>();
    }
    public void UpdateQuestProcessOnEnemyDeath(int enemyID)
    {
        List<Quest> currentKillingQuests = questLog.GetListOfActiveQuests();
        foreach (Quest quest in currentKillingQuests)
        {
            quest.UpdateQuest(enemyID);
            UpdateQuestLogUI();
        }
    }
    public void AddToQuestLog(Quest questToAdd)
    {
        questLog.AddToQuestLog(questToAdd);
        questToAdd.SetQuestActive(true);
        UpdateQuestLogUI();
    }

    public void RemoveQuestFromQuestLog(Quest questToRemove)
    {
        questToRemove.SetQuestActive(false);
        questLog.RemoveQuestFromList(questToRemove);
        UpdateQuestLogUI();
    }
    public void UpdateQuestLogUI()
    {
        myUI.UpdateQuestLog();
    }
    public void CompleteQuest(Quest questCompleted)
    {
        completedQuests.Add(questCompleted);
        questLog.RemoveQuestFromList(questCompleted);
        GetComponent<BasePlayerStats>().AddExperienceToPlayer(questCompleted.GetQuestExperience());
        GiveRewardToPlayer(questCompleted);
        UpdateQuestLogUI();
    }

    public void CompleteQuest(Quest questCompleted, int rewardChosen)
    {
        completedQuests.Add(questCompleted);
        questLog.RemoveQuestFromList(questCompleted);
        GetComponent<BasePlayerStats>().AddExperienceToPlayer(questCompleted.GetQuestExperience());
        GiveRewardToPlayer(questCompleted, rewardChosen);
        UpdateQuestLogUI();
    }
    public List<Quest> GetCompletedQuests() { return completedQuests; }
    public bool CheckForCompeltedQuestID(int questID)
    {
        foreach (Quest id in completedQuests)
        {
            if(id.GetQuestID() == questID)
            {
                return true;
            }
        }
        return false;
    }
    void GiveRewardToPlayer(Quest quest)
    {
        playerInventory.AddGoldToInventory(quest.GetGoldReward());
    }
    void GiveRewardToPlayer(Quest quest, int rewardChosen)
    {
        playerInventory.AddGoldToInventory(quest.GetGoldReward());
        playerInventory.AddItemToInventory(quest.GetItemRewards().ToArray()[rewardChosen]);
    }
}
