using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCQuestManager : MonoBehaviour
{
    BasePlayerStats myPlayer;
    [SerializeField] List<Quest> npcQuests = new List<Quest>();
    QuestManager playerQuestManager;
    // Start is called before the first frame update
    void Start()
    {
        myPlayer = FindObjectOfType<BasePlayerStats>();
        playerQuestManager = myPlayer.GetComponent<QuestManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Quest> GetQuestsFromNPC() { return npcQuests; }
    public List<Quest> GetQuestsWithRequirementCheck()
    {
        List<Quest> questsWithinRequirements = new List<Quest>();
        Debug.Log(npcQuests.Count);
        foreach (Quest quest in npcQuests)
        {
            if(quest.GetLevelRequirement() <= myPlayer.GetPlayerLevel())
            {
                if (quest.GetCompletedQuestsRequirements().Count != 0)
                {
                    foreach (int id in quest.GetCompletedQuestsRequirements())
                    {
                        if (playerQuestManager.CheckForCompeltedQuestID(id))
                            questsWithinRequirements.Add(quest);
                    }
                }
                else
                    questsWithinRequirements.Add(quest);
            }
        }
        return questsWithinRequirements;
    }
}
