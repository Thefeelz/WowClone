using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UPlayerUI : MonoBehaviour
{
    // ===================Player Frame GameObjects======================
    GameObject playerFrame;
    Image playerSprite;
    Image playerHealthBar;
    Image playerResourceBar;
    Text playerName;
    Text playerLevel;

    // ===================Target Frame GameObjects======================
    GameObject targetFrame;
    Image targetSprite;
    Image targetHealthBar;
    Image targetResourceBar;
    Text targetName;
    Text targetLevel;

    // ===================Cast Bar GameObjects======================
    GameObject castBar;
    Image castBarSprite;
    Image castBarProgressImage;
    Text castBarName;
    Text castBarProgressText;

    // ===================Hot Keys GameObjects======================
    GameObject hotKeysHandler;
    [SerializeField] List<Image> hotKeyImage = new List<Image>();
    [SerializeField] List<Text> hotKeyCooldownText = new List<Text>();

    // ===================Spellbook GameObjects======================
    GameObject spellBookHandler;
    List<Image> spellBookImage = new List<Image>();

    // ===================Debuff GameObjects======================
    GameObject debuffHandler;
    List<Image> debuffImage = new List<Image>();

    // ==================Hover Display GameObjects===============
    GameObject hoverHandler;
    Text hoverTitle;
    Text hoverDescription;

    // ====================Experience Bar GameObjects====================
    GameObject experienceBarHandler;
    Image experienceBarFillBar;

    // =================Quest Log GameObjects=========================
    GameObject questLogHandler;
    [SerializeField] List<GameObject> currentQuests = new List<GameObject>();

    // =================Dialogue GameObjects=========================
    GameObject dialogueHandler;

    // ================Inventory Reference===============
    InventoryUIManager  inventoryUIManager;
    // ===============Variables Used In Script===============
    bool haveTarget = false;
    bool isHovering = false;
    BasePlayerStats currentPlayer;
    BaseEnemyStats currentEnemyTarget;
    BaseFriendlyStats currentFriendlyTarget;
    MagicUser magicPlayer;
    SpellBook currentSpellbook;
    Abilities[] abilityList;
    QuestLog questLog;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = FindObjectOfType<BasePlayerStats>();
        currentSpellbook = currentPlayer.GetPlayerSpellbook();
        abilityList = currentSpellbook.GetAbilityList();
        magicPlayer = currentPlayer.GetComponent<MagicUser>();
        questLog = currentPlayer.GetComponent<QuestLog>();
        // On Startup, set all the variables to be used
        SetUpUI();
        castBar.SetActive(false);
        spellBookHandler.SetActive(false);
        inventoryUIManager = GetComponent<InventoryUIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerInfo_Magic();
        if(currentFriendlyTarget != null) { UpdateFriendlyBars(); }
        if (currentEnemyTarget != null) { UpdateEnemyBars(); }
        UpdateHotKeysFillAmount();
    }

    void UpdatePlayerInfo_Magic()
    {
        playerHealthBar.fillAmount = magicPlayer.GetPlayerHealthBarFillAmount();
        playerResourceBar.fillAmount = magicPlayer.GetPlayerCurrentManaFillAmount();
    }
    private void UpdateEnemyBars()
    {
        targetHealthBar.fillAmount = currentEnemyTarget.GetEnemyHealthBarFill();
        targetResourceBar.fillAmount = currentEnemyTarget.GetEnemyResourceBarFill();
        if(currentEnemyTarget.GetComponent<DebuffManager>().GetList().Count == 0) { debuffImage[0].fillAmount = 0; return; }
        debuffImage[0].sprite = currentEnemyTarget.GetComponent<DebuffManager>().GetList()[0].GetAbilityImage();
        debuffImage[0].fillAmount = currentEnemyTarget.GetComponent<DebuffManager>().GetList()[0].GetDoTFillProgress();
        
    }

    private void UpdateFriendlyBars()
    {
        targetHealthBar.fillAmount = currentFriendlyTarget.GetFriendlyHealthBarFill();
        targetResourceBar.fillAmount = currentFriendlyTarget.GetFriendlyResourceBarFill();
        if (currentFriendlyTarget.GetComponent<DoTManager>().GetList().Count == 0) { debuffImage[0].fillAmount = 0; return; }
        debuffImage[0].sprite = currentFriendlyTarget.GetComponent<DoTManager>().GetList()[0].GetAbilityImage();
        debuffImage[0].fillAmount = currentFriendlyTarget.GetComponent<DoTManager>().GetList()[0].GetDoTFillProgress();
    }
    public void CastBarInitialize(Abilities abilityToCast)
    {
        castBar.SetActive(true);
        castBarName.text = abilityToCast.GetAbilityName();
        castBarSprite.sprite = abilityToCast.GetAbilityImage();
        castBarProgressImage.fillAmount = 0f;
    }
    public void UpdateCastBar(float currentTime, float totalTime)
    {
        castBarProgressImage.fillAmount = currentTime / totalTime;
        castBarProgressText.text = currentTime.ToString("F1") + "/" + totalTime.ToString("F1");
    }
    public void CompletedCast()
    {
        castBar.SetActive(false);
    }

    // This function will ONLY get called if something is SUCCESFULLY targetted
    public void UI_SetPlayerTarget(GameObject newPlayerTarget)
    {
        // Set the target frame to active
        targetFrame.SetActive(true);
        haveTarget = true;
        // Check to see if the target is an enemy
        if (newPlayerTarget.GetComponent<BaseEnemyStats>()) 
        {
            // Set our current enemy target to the new player target
            currentEnemyTarget = newPlayerTarget.GetComponent<BaseEnemyStats>();

            // Do an initial update to get correct information displayed on the UI
            UpdateEnemyTargetInformation(currentEnemyTarget);
        }
        else if (newPlayerTarget.GetComponent<BaseFriendlyStats>())
        {

            currentFriendlyTarget = newPlayerTarget.GetComponent<BaseFriendlyStats>();
            UpdateFriendlyTargetInformation(currentFriendlyTarget);
        }
        
    }

    // This function should ONLY be called on the initial targetting and updates the target frame
    private void UpdateEnemyTargetInformation(BaseEnemyStats enemyTarget)
    {
        targetSprite.sprite = enemyTarget.GetEnemyPortrait();
        targetName.text = enemyTarget.GetEnemyName();
        targetLevel.text = enemyTarget.GetEnemyLevel().ToString();
        targetHealthBar.fillAmount = enemyTarget.GetEnemyHealthBarFill();
        targetResourceBar.fillAmount = enemyTarget.GetEnemyResourceBarFill();
    }

    // This function should ONLY be called on the initial targetting and updates the target frame
    private void UpdateFriendlyTargetInformation(BaseFriendlyStats friendlyTarget)
    {
        targetSprite.sprite = friendlyTarget.GetFriendlyPortrait();
        targetName.text = friendlyTarget.GetFriendlyName();
        targetLevel.text = friendlyTarget.GetFriendlyLevel().ToString();
        targetHealthBar.fillAmount = friendlyTarget.GetFriendlyHealthBarFill();
        targetResourceBar.fillAmount = friendlyTarget.GetFriendlyResourceBarFill();
    }

    public bool DoesPlayerHaveTarget() { return haveTarget; }
    // This function should ONLY be called when the player loses focus of a target
    public void UI_ClearTargetFrame()
    {
        haveTarget = false;
        currentEnemyTarget = null;
        targetFrame.SetActive(false);
    }

    // ==========HOT KEYS STUFF IS HERE==========
    public void UpdateGlobalCooldown(float fillAmount)
    {
        int i = 0;
        int maxSize = abilityList.Length;
        foreach (Image image in hotKeyImage)
        {
            if (currentSpellbook.GetAbilityFromList(image.GetComponentInParent<SpellID>().GetSpellID()).GetAbilityCurrentCooldownTime() <= fillAmount)
            {
                image.fillAmount = fillAmount;
                
            }
        }
    }

    public void UpdateHotKeysFillAmount()
    {
        foreach (Image image in hotKeyImage)
        {
            image.fillAmount = currentSpellbook.GetAbilityFromList(image.GetComponentInParent<SpellID>().GetSpellID()).GetAbilityCooldownFillAmount();
            if (currentEnemyTarget)
                ChangeHotKeyTextColorBasedOnRangeEnemy(image);
            else
                ChangeHotKeyColorsOnNoTarget(image);
        }
        foreach (Text text in hotKeyCooldownText)
        {
            if (currentSpellbook.GetAbilityFromList(text.GetComponentInParent<SpellID>().GetSpellID()).GetAbilityCurrentCooldownTime() > 0)
            {
                if (text.gameObject.activeSelf)
                    text.text = Mathf.CeilToInt(currentSpellbook.GetAbilityFromList(text.GetComponentInParent<SpellID>().GetSpellID()).GetAbilityCurrentCooldownTime()).ToString();
                else
                    text.gameObject.SetActive(true);
            }
            else
            {
                text.gameObject.SetActive(false);
            }
        }
    }

    private void ChangeHotKeyColorsOnNoTarget(Image image)
    {
        image.GetComponentInChildren<Text>().color = Color.white;
    }
    private void ChangeHotKeyTextColorBasedOnRangeEnemy(Image image)
    {
        if (currentSpellbook.GetAbilityFromList(image.GetComponentInParent<SpellID>().GetSpellID()).GetAbilityRange() < Vector3.Distance(currentPlayer.transform.position, currentEnemyTarget.transform.position))
        {
            image.GetComponentInChildren<Text>().color = Color.red;
        }
        else
        {
            image.GetComponentInChildren<Text>().color = Color.white;
        }
    }

    public int GetAbilityIDFromKeyPress(int keyPress)
    {
        return hotKeyImage.ToArray()[keyPress - 1].GetComponentInParent<SpellID>().GetSpellID();
    }
    public void ToggleSpellBookDisplay() { spellBookHandler.SetActive(!spellBookHandler.activeSelf); }
    public void ToggleDialogueDisplay() { dialogueHandler.SetActive(!dialogueHandler.activeSelf); }
    public void ToggleInventoryDisplay() { inventoryUIManager.ToggleInventoryHandler(); }

    // ==========HOVER DISPLAY FUNCTIONS==========
    public void ActivateHover(UI_HoverInformation hoverTarget)
    {
        hoverHandler.SetActive(true);
        hoverTitle.text = hoverTarget.GetTitle();
        hoverDescription.text = hoverTarget.GetDescription();
    }
    public void ClearHover()
    {
        hoverTitle.text = "";
        hoverDescription.text = "";
        hoverHandler.SetActive(false);
    }

    // ==========EXPERIENCE BAR FUNCTIONS==========
    public void UpdateExperienceBar()
    {
        experienceBarFillBar.fillAmount = currentPlayer.GetPlayerExperienceFillAmount();
        playerLevel.text = currentPlayer.GetPlayerLevel().ToString();
    }

    // ==========QUEST LOG FUNCTIONS==========
    public void UpdateQuestLog()
    {
        int i = 0;
        int j = 0;
        if(questLog.GetSizeOfQuestLog() > 0)
        {
            foreach (GameObject objects in currentQuests)
            {
                if (i < questLog.GetSizeOfQuestLog())
                {
                    if (!objects.activeSelf)
                        objects.SetActive(true);
                    foreach (Transform text in objects.transform)
                    {
                        if (j == 0)
                        {
                            text.GetComponent<Text>().text = questLog.GetListOfActiveQuests().ToArray()[i].GetQuestTitle();
                            j++;
                        }
                        else
                        {
                            text.GetComponent<Text>().text = questLog.GetListOfActiveQuests().ToArray()[i].GetRequirementsForUI();
                            if (questLog.GetListOfActiveQuests().ToArray()[i].CheckIfAllRequirementsAreMet())
                                text.GetComponent<Text>().color = Color.green;
                            else
                                text.GetComponent<Text>().color = Color.red;
                            j = 0;
                            i++;
                        }
                    }
                }
                else
                {
                    objects.SetActive(false);
                }
            }
        }
        else
        {
            foreach (GameObject objects in currentQuests)
            {
                objects.SetActive(false);
            }
        }
    }

    // ==========DIALOGUE FUNCTIONS==========
    public GameObject GetDialogueObjectFromUI() { return dialogueHandler; }

    public bool GetDialogueToggle() { return dialogueHandler.activeSelf; }
    // ============================================================================
    // Function calls only used on Start Up (DO NOT ALTER ANY CODE BELOW THIS LINE)
    // ============================================================================
    void SetUpUI()
    {
        foreach (Transform gameObject in transform)
        {
            if (gameObject.CompareTag("UITargetHandler")) { SetUpTargetHandler(gameObject); }
            else if (gameObject.CompareTag("UIPlayerHandler")) { SetUpPlayerHandler(gameObject); }
            else if (gameObject.CompareTag("UICastBarHandler")) { SetUpCastBarHandler(gameObject); }
            else if (gameObject.CompareTag("UIHotKeysHandler")) { SetUpHotKeyHandler(gameObject); }
            else if (gameObject.CompareTag("UISpellBookHandler")) { SetUpSpellBookHandler(gameObject); }
            else if (gameObject.CompareTag("UIDebuffs")) { SetUpDebuffHandler(gameObject); }
            else if (gameObject.CompareTag("UIHoverDisplay")) { SetUpHoverHandler(gameObject); }
            else if (gameObject.CompareTag("UIExperienceBar")) { SetUpExperienceBarHandler(gameObject); }
            else if (gameObject.CompareTag("UIQuestLogHandler")) { SetUpQuestLogHandler(gameObject); }
            else if (gameObject.CompareTag("UIDialogueHandler")) { SetUpDialogueHandler(gameObject); }
            else if (gameObject.CompareTag("UIInventoryHandler")) { SetUpInventoryHandler(gameObject); }
        }
    }
    void SetUpTargetHandler(Transform targetHandler)
    {
        targetFrame = targetHandler.gameObject;
        foreach (Transform child in targetHandler.transform)
        {
            if(child.CompareTag("UIPortrait")) { targetSprite = child.GetComponent<Image>(); }
            else if (child.CompareTag("UIHealthBar")) { targetHealthBar = child.GetComponent<Image>(); }
            else if (child.CompareTag("UIResourceBar")) { targetResourceBar = child.GetComponent<Image>(); }
            else if (child.CompareTag("UINameText")) { targetName = child.GetComponent<Text>(); }
            else if (child.CompareTag("UILevelText")) { targetLevel = child.GetComponentInChildren<Text>(); }
            else if (child.CompareTag("UIDebuffs")) { debuffImage.Add(child.GetComponentInChildren<Image>()); }
        }
        
        targetHandler.gameObject.SetActive(false);
    }
    void SetUpPlayerHandler(Transform playerHandler)
    {
        playerFrame = playerHandler.gameObject;
        foreach (Transform child in playerHandler.transform)
        {
            if (child.CompareTag("UIPortrait")) { playerSprite = child.GetComponent<Image>(); }
            else if (child.CompareTag("UIHealthBar")) { playerHealthBar = child.GetComponent<Image>(); }
            else if (child.CompareTag("UIResourceBar")) { playerResourceBar = child.GetComponent<Image>(); }
            else if (child.CompareTag("UINameText")) { playerName = child.GetComponent<Text>(); }
            else if (child.CompareTag("UILevelText")) { playerLevel = child.GetComponentInChildren<Text>(); }
            else if (child.CompareTag("UIBuffHandler")) { SetUpBuffHandler(child); }
        }
    }
    void SetUpCastBarHandler(Transform castBarHandler)
    {
        castBar = castBarHandler.gameObject;
        foreach (Transform child in castBarHandler.transform)
        {
            if (child.CompareTag("UICastBarImage")) { castBarSprite = child.GetComponent<Image>(); }
            else if(child.CompareTag("UICastBarFill")) { castBarProgressImage = child.GetComponent<Image>(); }
            else if(child.CompareTag("UICastBarName")) { castBarName = child.GetComponent<Text>(); }
            else if(child.CompareTag("UICastBarTimeText")) { castBarProgressText = child.GetComponent<Text>(); }
        }
    }
    void SetUpHotKeyHandler(Transform hotKeyHandler)
    {
        hotKeysHandler = hotKeyHandler.gameObject;
        foreach (Transform child in hotKeyHandler.transform)
        {
            if (child.GetComponent<SpellID>())
            {
                foreach (Transform child1 in child)
                {
                    if(child1.CompareTag("UIHotkeyButton"))
                    {
                        child1.GetComponent<Image>().sprite = currentSpellbook.GetAbilityFromList(child.GetComponent<SpellID>().GetSpellID()).GetAbilityImage();
                        hotKeyImage.Add(child1.GetComponent<Image>());
                    }
                    else if(child1.CompareTag("UIHotkeySaturatedBackground"))
                    {
                        child1.GetComponent<Image>().sprite = currentSpellbook.GetAbilityFromList(child.GetComponent<SpellID>().GetSpellID()).GetAbilityImage();
                        child1.GetComponent<Image>().color = Color.gray;
                    }
                    else if(child1.CompareTag("UIHotkeyCooldownText"))
                    {
                        hotKeyCooldownText.Add(child1.GetComponent<Text>());
                    }
                }
                /*
                child.GetComponentInChildren<Button>().GetComponent<Image>().sprite = currentSpellbook.GetAbilityFromList(child.GetComponentInChildren<SpellID>().GetSpellID()).GetAbilityImage();
                hotKeyImage.Add(child.GetComponentInChildren<Button>().GetComponent<Image>());
                child.GetComponentInChildren<Image>().sprite = child.GetComponentInChildren<Button>().GetComponent<Image>().sprite;
                child.GetComponentInChildren<Image>().color = Color.gray;*/
                
            }
        }
    }
    void SetUpSpellBookHandler(Transform _spellBookHandler)
    {
        int i = 0;
        int maxSize = abilityList.Length;
        spellBookHandler = _spellBookHandler.gameObject;
        foreach (Transform child in _spellBookHandler.transform)
        {
            foreach (Transform children in child.transform)
            {
                foreach (Transform tinyChildren in children.transform)
                {
                    if (i < maxSize)
                    {
                        if(tinyChildren.CompareTag("UIAbilityImageDrag"))
                        {
                            tinyChildren.GetComponent<Image>().sprite = abilityList[i].GetAbilityImage();
                            tinyChildren.GetComponent<SpellID>().SetSpellID(abilityList[i].GetAbilityID());
                        }
                        else
                            tinyChildren.GetComponent<Image>().sprite = abilityList[i].GetAbilityImage();
                    }
                }
                i++;
            }
        }
    }
    void SetUpDebuffHandler(Transform _debuffHandler)
    {
        debuffHandler = _debuffHandler.gameObject;
        foreach (GameObject item in _debuffHandler.transform)
        {
            debuffImage.Add(item.GetComponent<Image>());
            Debug.Log(item.name);
        }
    }
    void SetUpHoverHandler(Transform _hoverHandler)
    {
        hoverHandler = _hoverHandler.gameObject;
        int i = 0;
        // One thing to note, when using GameObject item compared to Transform, it gave me an invalid cast, I dont know why since in SetUpDebuffHandler it worked fine.
        // This is an ugly function and it is totally based on the current set up in the UI with the title and description being the 2nd and 3rd element.
        // Possible TODO will be give them tags and if compare tag and check to see if they are what they are.
        foreach (Transform item in hoverHandler.transform)
        {
            if (i == 0)
                i++;
            else if (i == 1)
            {
                hoverTitle = item.GetComponent<Text>();
                i++;
            }
            else
                hoverDescription = item.GetComponent<Text>();
        }
        hoverHandler.SetActive(false);
    }
    void SetUpExperienceBarHandler(Transform _experienceBarHandler)
    {
        experienceBarHandler = _experienceBarHandler.gameObject;
        int i = 0;
        foreach (Transform item in experienceBarHandler.transform)
        {
            if (i == 1)
                experienceBarFillBar = item.GetComponent<Image>();
            else
                i++;
        }
        experienceBarFillBar.fillAmount = currentPlayer.GetPlayerExperienceFillAmount();
    }
    void SetUpQuestLogHandler(Transform _questLogHandler)
    {
        questLogHandler = _questLogHandler.gameObject;
        foreach (Transform item in questLogHandler.transform)
        {
            foreach (Transform childItem in item)
            {
                if (childItem.CompareTag("UIQuest"))
                {
                    currentQuests.Add(childItem.gameObject);
                }
            }
        }
        UpdateQuestLog();
    }
    void SetUpDialogueHandler(Transform _dialogueHandler)
    {
        dialogueHandler = _dialogueHandler.gameObject;
    }
    void SetUpInventoryHandler(Transform _inventoryHandler)
    {
        GetComponent<InventoryUIManager>().SetUpInventoryUI(_inventoryHandler);
    }
    void SetUpBuffHandler(Transform _buffHandler)
    {
        currentPlayer.GetComponent<BuffManager>().SetUpBuffManager(_buffHandler);
    }
}
