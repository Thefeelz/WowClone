using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLog : MonoBehaviour
{
    [SerializeField] List<Quest> quests;

    public List<Quest> GetListOfActiveQuests() { return quests; }

    public int GetSizeOfQuestLog() { return quests.Count; }

    public void AddToQuestLog(Quest newQuest)
    {
        quests.Add(newQuest);
    }
    public void RemoveQuestFromList(Quest questToRemove)
    {
        quests.Remove(questToRemove);
    }
}
