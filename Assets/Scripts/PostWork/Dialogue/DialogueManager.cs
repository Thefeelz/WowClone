using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // This object is literally what this is, I am using the variable for clarity reasons
    GameObject dialogueHandler;
    Image dialoguePortrait;
    Text dialogueNPCName;

    // NORMAL DIALOGUE VARIABLES
    GameObject normalDialogueHandler;
    Text normalBodyText;

    // QUEST DIALOGUE VARIABLES
    GameObject questDialogueHandler;
    Text questBodyText;
    Text questTitleText;
    Text questRewardText;
    GameObject questItemRewardHandler;
    List<Item> questRewardItems = new List<Item>();
    List<Image> questRewardItemsImages = new List<Image>();
    Button acceptButton;
    Button completeButton;

    // QUEST SELECTION VARIABLES
    GameObject questSelectHandler;
    List<Text> selectableQuestNames = new List<Text>();

    // GLOBAL VARIABLES
    Dialogue[] myDialogues;
    List<Quest> validQuests = new List<Quest>();
    int currentPage = 0;
    int activeQuestRewardChoice = 0;
    int activeQuestPosition = 0;
    BasePlayerStats myPlayer;
    BaseFriendlyStats npc;

    enum WindowState {Normal, Quest, Select, Closed};
    WindowState currentState;
    // Start is called before the first frame update
    void Start()
    {
        myPlayer = FindObjectOfType<BasePlayerStats>();
        dialogueHandler = gameObject;
        SetUpDialogueSystem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpDialogueFromNPC(BaseFriendlyStats _npc)
    {
        // Standard NPC used for Dialogue Sessions
        npc = _npc;
        // Grab any Dialogues from the NPC, We already know that the NPC is a Dialogue Manager because of a check in UPlayerTargetting
        myDialogues = npc.GetComponent<NPCDialogueManager>().GetDialogues();

        // Set up the Name and Portrait
        dialogueNPCName.text = npc.GetFriendlyName();
        dialoguePortrait.sprite = npc.GetFriendlyPortrait();

        // Set the initial Dialogue text for the body to the first message
        if(myDialogues.Length > 0)
            normalBodyText.text = myDialogues[0].GetDialogue();

        // Check to see if the NPC is a quest Giver
        if (npc.GetComponent<NPCQuestManager>() && npc.GetComponent<NPCQuestManager>().GetQuestsFromNPC().Count > 0)
            currentState = WindowState.Select;
        else if (npc.GetComponent<NPCDialogueManager>() && npc.GetComponent<NPCDialogueManager>().GetDialogues().Length > 0)
            currentState = WindowState.Normal;

        if (currentState == WindowState.Select)
            SetUpQuestSelectWindow();
        else
            SetUpNormalDialogueWindow();
    }

    void SetUpQuestSelectWindow()
    {
        // Grab Eligible quests from the Quest Giver
        validQuests = npc.GetComponent<NPCQuestManager>().GetQuestsWithRequirementCheck();

        int i = 0;
        foreach (Quest quest in validQuests)
        {
            selectableQuestNames.ToArray()[i].text = quest.GetQuestTitle();
            if (quest.GetQuestActive())
            {
                if (quest.GetQuestComplete())
                    selectableQuestNames.ToArray()[i].color = Color.green;
                else
                    selectableQuestNames.ToArray()[i].color = Color.gray;
            }
            else
                selectableQuestNames.ToArray()[i].color = Color.white;
        }
        MoveToSelectQuestHandler();
    }
    void SetUpQuestDialogueWindow()
    {
        // Check to see if to Display possible item rewards based on the list size
        if(validQuests.ToArray()[activeQuestPosition].GetItemRewardSize() > 0)
        {
            // Initially Set all of the possible image icons to deactive, we will turn them on as we iterate through the list
            questItemRewardHandler.SetActive(true);
            foreach (Image image in questRewardItemsImages)
            {
                image.gameObject.SetActive(false);
            }
            // Store the list of quest items to the dialogue manager
            questRewardItems = validQuests.ToArray()[activeQuestPosition].GetItemRewards();

            // Iterate through the list of items from the quest giver, turning on the images as we go through
            int i = 0;
            foreach (Item item in questRewardItems)
            {
                questRewardItemsImages.ToArray()[i].gameObject.SetActive(true);
                questRewardItemsImages.ToArray()[i].sprite = item.GetItemSprite();
                i++;
            }
        }
        else
        {
            questItemRewardHandler.SetActive(false);
        }

        // Checks to see if the quest is active or not, if its not active turn on the accept quest buttons
        if (!validQuests.ToArray()[activeQuestPosition].GetQuestActive())
        {
            acceptButton.gameObject.SetActive(true);
            completeButton.gameObject.SetActive(false);
        }
        // The quest is active, check to see if it is completed
        else if (validQuests.ToArray()[activeQuestPosition].GetQuestActive())
        {
            // If the quest is not completed, diable the accept and complete buttons
            if(!validQuests.ToArray()[activeQuestPosition].GetQuestComplete())
            {
                acceptButton.gameObject.SetActive(false);
                completeButton.gameObject.SetActive(false);
            }
            // The quest is completed, make sure that the complete button is turned on
            else
            {
                completeButton.gameObject.SetActive(true);
                acceptButton.gameObject.SetActive(false);
            }
        }
    }
    void SetUpNormalDialogueWindow()
    {
        MoveToNormalDialogueHandler();
    }
    // This sets up the dialogue system on start (DONT TOUCH THIS)
    void SetUpDialogueSystem()
    {
        foreach (Transform item in dialogueHandler.transform)
        {
            if (item.CompareTag("UIPortrait"))
                foreach (Transform item1 in item)
                {
                    dialoguePortrait = item1.GetComponent<Image>();
                }
            else if (item.CompareTag("UINameText"))
                dialogueNPCName = item.GetComponent<Text>();
            else if (item.CompareTag("UIDialogueHandler"))
            {
                normalDialogueHandler = item.gameObject;
                normalBodyText = item.GetComponentInChildren<Text>();
            }
            else if (item.CompareTag("UIQuest"))
            {
                questDialogueHandler = item.gameObject;
                foreach (Transform item1 in item)
                {
                    if (item1.CompareTag("UINameText"))
                        questTitleText = item1.GetComponent<Text>();
                    else if (item1.CompareTag("UIQuest"))
                        questBodyText = item1.GetComponent<Text>();
                    else if (item1.CompareTag("UIQuestSelect"))
                        acceptButton = item1.GetComponent<Button>();
                    else if (item1.CompareTag("UICompleteButton"))
                        completeButton = item1.GetComponent<Button>();
                    else if (item1.CompareTag("UIResourceBar"))
                        questRewardText = item1.GetComponent<Text>();
                    else if (item1.CompareTag("UInventoryRow"))
                    {
                        questItemRewardHandler = item1.gameObject;
                        foreach (Transform item2 in item1)
                        {
                            if(item2.CompareTag("UIInventoryHandler"))
                            {
                                questRewardItemsImages.Add(item2.GetComponent<Image>());
                            }
                        }
                    }
                }   
            }
            else if (item.CompareTag("UIQuestSelect"))
            {
                questSelectHandler = item.gameObject;
                int i = 0;
                foreach (Transform transform in item)
                {
                    if (transform.CompareTag("UIQuestSelect"))
                    {
                        selectableQuestNames.Add(transform.GetComponent<Text>());
                        i++;
                    }
                }
            }
            
        }
        dialogueHandler.SetActive(false);
    }

    public void AcceptButton()
    { 
        // Grab the current quest that you are selecting and move it to the current user quest log
        myPlayer.GetComponent<QuestManager>().AddToQuestLog(validQuests.ToArray()[activeQuestPosition]);

        // Check to see if there are more quests that you can accept
        if (validQuests.Count > 0)
        {
            SetUpQuestSelectWindow();
            questSelectHandler.SetActive(true);
            questDialogueHandler.SetActive(false);
        }
        // Check to see if there is any dialogue that can be had if there are no available quests
        else if (myDialogues.Length > 0)
        {
            MoveToNormalDialogueHandler();
        }
        // Essentially Close the Window
        else
        {
            SetAllHandlersActiveToFalse();
        }        
    }
    public void CompleteButton()
    {
        // Complete the quest for the player and add it to the completed quest list
        if (questRewardItems != null && questRewardItems.Count > 0)
        {
            myPlayer.GetComponent<QuestManager>().CompleteQuest(validQuests.ToArray()[activeQuestPosition], activeQuestRewardChoice);
            questRewardItems = null;
        }
        else
            myPlayer.GetComponent<QuestManager>().CompleteQuest(validQuests.ToArray()[activeQuestPosition]);

        // Remove the quest from the npc
        npc.GetComponent<NPCQuestManager>().GetQuestsFromNPC().Remove(validQuests.ToArray()[activeQuestPosition]);

        // Repopulate the valid quests list
        validQuests = npc.GetComponent<NPCQuestManager>().GetQuestsWithRequirementCheck();
        Debug.Log(validQuests.Count);

        // Check to see if there are any new valid quests and move to the quest select page
        if (validQuests.Count > 0)
            SetUpQuestSelectWindow();
        // Check to see if there are any dialogues and if there are move to dialogue page
        else if (myDialogues.Length > 0)
            MoveToNormalDialogueHandler();
        // Close the window
        else
            SetAllHandlersActiveToFalse();
    }
    public void DeclineButton()
    {
        if (currentState == WindowState.Select)
            MoveToSelectQuestHandler();
        else if (currentState == WindowState.Normal)
            MoveToNormalDialogueHandler();

    }
    public void ContinueButton()
    {

    }
    public void GoodbyeButton()
    {
        SetAllHandlersActiveToFalse();
    }
    public void ClickOnQuestSelectText(int questNumber)
    {
        activeQuestPosition = questNumber;
        questSelectHandler.SetActive(false);
        questDialogueHandler.SetActive(true);
        questBodyText.text = validQuests.ToArray()[questNumber].GetQuestDescription();
        questTitleText.text = validQuests.ToArray()[questNumber].GetQuestTitle();
        questRewardText.text = validQuests.ToArray()[questNumber].GetRewardText();
        SetUpQuestDialogueWindow();
    }
    void SetAllHandlersActiveToFalse()
    {
        dialogueHandler.SetActive(false);
        normalDialogueHandler.SetActive(false);
        questDialogueHandler.SetActive(false);
        questSelectHandler.SetActive(false);
        ClearAllData();
    }
    void MoveToNormalDialogueHandler()
    {
        normalDialogueHandler.SetActive(true);
        questDialogueHandler.SetActive(false);
        questSelectHandler.SetActive(false);
    }
    void MoveToSelectQuestHandler()
    {
        normalDialogueHandler.SetActive(false);
        questDialogueHandler.SetActive(false);
        questSelectHandler.SetActive(true);

    }
    void ClearAllData()
    {
        if(validQuests != null)
            validQuests.Clear();
        if(questRewardItems != null)
            questRewardItems.Clear();
        currentPage = 0;
        activeQuestPosition = 0;
        currentState = WindowState.Closed;
    }
    public void SelectReward(int i)
    {
        activeQuestRewardChoice = i;
    }
}
