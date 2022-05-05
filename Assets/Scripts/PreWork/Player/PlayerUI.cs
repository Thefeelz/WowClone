//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class PlayerUI : MonoBehaviour
//{
//    [Header("Player Frame")]
//    [SerializeField] Image healthBarImage;
//    [SerializeField] Image resourceBarImage;
//    [SerializeField] Text playerLevelText;
//    [SerializeField] Text playerNameText;
//    [Header ("Cast Bar")]
//    [SerializeField] Image castBarFill_Image;
//    [SerializeField] Image castBarIcon;
//    [SerializeField] Text castBarText;
//    [SerializeField] Text castBarTimerText;
//    [SerializeField] GameObject castBarHandler;
//    [Header("Target Frame")]
//    [SerializeField] Image targetHealthBarImage;
//    [SerializeField] Image targetResourceBarImage;
//    [SerializeField] GameObject targetFrameHandler;
//    [SerializeField] Text targetLevelText;
//    [SerializeField] Text targetNameText;
//    [SerializeField] Image targetPortrait;
//    [SerializeField] Image debuff1Image;
//    [SerializeField] GameObject debuffHandler;
//    [Header("Hot Keys")]
//    [SerializeField] GameObject hotKeyButtonParent;
//    Sprite hotKeyButton1Image;
//    Text hotKeyButton1Text;
//    [Header("ExperienceBar")]
//    [SerializeField] Image experienceBarImage;
//    [Header("Level Up")]
//    [SerializeField] GameObject levelUpObject;

//    [SerializeField] PlayerProjectile[] hotKeyButtonArray;
//    [SerializeField] List<Text> hotkeyButtonText = new List<Text>();

//    float castTime = 0f;
//    float elapsedTime = 0f;

//    PlayerStats stats;
//    PlayerTarget playerTarget;
    
//    GameObject target = null;
//    EnemyStats enemyTarget;

//    bool targetIsEnemy = false;
//    bool castStopImplimented = false;
//    void Start()
//    {
//        stats = GetComponent<PlayerStats>();
//        playerTarget = GetComponent<PlayerTarget>();
        
//        healthBarImage.fillAmount = stats.GetCurrentPlayerHealth() / stats.GetMaxPlayerHealth();
//        resourceBarImage.fillAmount = stats.GetCurrentPlayerResource() / stats.GetMaxPlayerResource();
//        targetFrameHandler.SetActive(false);
//        castBarHandler.SetActive(false);
//        UpdateHotKeys(hotKeyButtonParent);
//        UpdateLevelUI(true);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        // Since the player is constantly restoring health and resource, we need a constant update of those numbers
//        healthBarImage.fillAmount = stats.GetCurrentPlayerHealth() / stats.GetMaxPlayerHealth();
//        resourceBarImage.fillAmount = stats.GetCurrentPlayerResource() / stats.GetMaxPlayerResource();

//        // If the target is not null and the enemy target is not null
//        // TODO, make sure that friendlies and enemies can be added as targets
//        if(targetIsEnemy && enemyTarget)
//        {
//            // Function to display the Target Frame
//            DisplayTargetFrameEnemy();
//            UpdateInRangeTextColors();
//        }
//        else ///The target is not a valid target
//        {
//            // Turn off the enemy frame handler parent object
//            targetFrameHandler.SetActive(false);
//        }
//        // Check to see if the player is casting
//        if(playerAttack.GetCasting())
//        {
//            //Start filling the cast bar
//            CastBarFill();
//        }// so we dont constantly run through and assign values, we have a if statement to stop the logic train while the player isnt casting 
//        else if (!castStopImplimented)
//        {
//            // A stop filling the cast bar function
//            CastStop();
//            UpdateInRangeTextColors();
//        }

//        UpdateCooldowns();
//        UpdateInRangeTextColors();
//    }
//    // This is called only if the current target is an enemy
//    void DisplayTargetFrameEnemy()
//    {
//        // Show health and resource values
//        targetHealthBarImage.fillAmount = enemyTarget.GetCurrentEnemyHealth() / enemyTarget.GetMaxEnemyHealth();
//        targetResourceBarImage.fillAmount = enemyTarget.GetCurrentEnemyResource() / enemyTarget.GetMaxEnemyResource();
//        if (enemyTarget.GetActiveDoTs() > 0)
//        {
//            debuffHandler.SetActive(true);

//        }
//        else
//            debuffHandler.SetActive(false);
//    }
//    public void SetUITarget(GameObject newTarget)
//    {
//        if (target && newTarget.GetInstanceID() != target.GetInstanceID() && target.GetComponent<EnemyStats>())
//            target.GetComponent<EnemyStats>().SetCurrentlyTargetted(false);
//        // Pass in the new target into the UI so it is not null
//        target = newTarget;
//        // Check to see if the target is an enemy
//        if (target.GetComponent<EnemyStats>())
//        {
//            // Cache the new enemy so we dont constantly have to call GetComponent during the update to retrieve enemy health and resource amounts
//            enemyTarget = target.GetComponent<EnemyStats>();
//            enemyTarget.SetCurrentlyTargetted(true);
//            //Turn on the Target Frame
//            targetFrameHandler.SetActive(true);
//            //Set target is enemy to true so that we can update the enemies health and resource amount
//            targetIsEnemy = true;
//            targetLevelText.text = "Level: " + enemyTarget.GetEnemyLevel().ToString();
//            targetNameText.text = enemyTarget.GetEnemyName();
//            targetPortrait.sprite = enemyTarget.GetEnemyPortrait();
//        }
//    }
//    // A public reference to let us know that the player has no target
//    public void ClearTargetFrame()
//    {
//        // Turn off the target frame, set target to null, enemy target to null, and the check for target is an enemy to null
//        if(enemyTarget)
//            enemyTarget.SetCurrentlyTargetted(false);
//        targetFrameHandler.SetActive(false);
//        target = null;
//        enemyTarget = null;
//        targetIsEnemy = false;
//    }
//    // A public function to let the UI know which ability is being passed in and the duration of the cast
//    public void StartCast(int hotkeyNumber)
//    {
//        // Set the class cast time to the passed in ability cast time
//        castTime = hotKeyButtonArray[hotkeyNumber - 1].GetAbilityCastTime();
//        castBarText.text = hotKeyButtonArray[hotkeyNumber - 1].GetAbilityName();
//        castBarIcon.sprite = hotKeyButtonArray[hotkeyNumber - 1].GetAbilityImage();
//        // Set the cast bar to active
//        castBarHandler.SetActive(true);
//        // Cast stop implimented is used to call CastStop, while it is false, if the player moves or interupts their cast at all
//        // CastStop() will be called and set castStopImplimented to true so we dont continuously call CastStop()
//        castStopImplimented = false;
//    }
//    // Simple function to fill the cast bar fill amount
//    void CastBarFill()
//    {
//        elapsedTime += Time.deltaTime;
//        castBarFill_Image.fillAmount = elapsedTime / castTime;
//        castBarTimerText.text = elapsedTime.ToString("F1") + "/" + castTime.ToString("F1");
//        if(elapsedTime > castTime)
//        {
//            CastStop();
//        }
//    }
//    // Function that is called to stop the cast, clear everything and set castStopImplimented to true so we dont continously call this function for no reason
//    void CastStop()
//    {
//        elapsedTime = 0;
//        castTime = 0;
//        castBarHandler.SetActive(false);
//        castStopImplimented = true;
//    }
    
//    void UpdateHotKeys(GameObject hotkeyButtonParent)
//    {
//        int iteration = 0;
//        foreach (Transform hotkeyButton in hotkeyButtonParent.transform)
//        {
//            foreach (Transform child in hotkeyButton.transform)
//            {
//                if (child.CompareTag("HotkeyUI") || child.CompareTag("HotkeyCDUI"))
//                {
//                    child.GetComponent<Image>().sprite = hotKeyButtonArray[iteration].GetAbilityImage();
//                }
//                if(child.CompareTag("HotkeyText"))
//                {
//                    hotkeyButtonText.Add(child.GetComponent<Text>());
//                }
//            }
//            iteration++;
//        }
//    }
//    public float GetAbilityCost (int hotkeyNumber)
//    {
//        return hotKeyButtonArray[hotkeyNumber - 1].GetAbilityCost();
//    }
//    public PlayerProjectile GetAbility(int hotkeyNumber)
//    {
//        return hotKeyButtonArray[hotkeyNumber - 1];
//    }
//    public float GetAbilityCastTime(int hotkeyNumber)
//    {
//        return hotKeyButtonArray[hotkeyNumber - 1].GetAbilityCastTime();
//    }
//    public float GetAbilityRange(int hotkeyNumber)
//    {
//        return hotKeyButtonArray[hotkeyNumber - 1].GetAbilityRange();
//    }
//    public bool GetAbilityInRange(int hotkeyNumber)
//    {
//        if (hotkeyButtonText.ToArray()[hotkeyNumber - 1].color == Color.white)
//        {
//            return true;
//        }
//        else
//            return false;
//    }
//    public bool IsAbilityDoT(int hotkeyNumber)
//    {
//        if (hotKeyButtonArray[hotkeyNumber - 1].GetComponent<DoT>())
//            return true;
//        else
//            return false;
//    }
//    void UpdateInRangeTextColors()
//    {
//        int iteration = 0;
//        foreach (Text text  in hotkeyButtonText)
//        {
//            if (enemyTarget != null)
//            {
//                if (Vector3.Distance(enemyTarget.transform.position, transform.position) < hotKeyButtonArray[iteration].GetAbilityRange()
//                                                        && stats.GetCurrentPlayerResource() > hotKeyButtonArray[iteration].GetAbilityCost())
//                {
//                    text.color = Color.white;
//                }
//                else
//                {
//                    text.color = Color.red;
//                }
//            }
//            else if (stats.GetCurrentPlayerResource() > hotKeyButtonArray[iteration].GetAbilityCost())
//            {
//                text.color = Color.white;
//            }
//            else
//            {
//                text.color = Color.red;
//            }
            
//            iteration++;
//        }
//    }
//    void UpdateCooldowns()
//    {
//        // Spellbook TODO
//    }
//    public void UpdateLevelUI(bool onStart)
//    {
//        playerNameText.text = stats.GetPlayerName();
//        playerLevelText.text = "Level: " + stats.GetPlayerLevel().ToString();
//        experienceBarImage.fillAmount = (float)stats.GetPlayerCurrentExperience() / stats.GetPlayerNeededExperience();
//        if (onStart) { return; }
//        levelUpObject.SetActive(true);
//        levelUpObject.GetComponent<LevelUpText>().StartLevelUpText();
//    }
//    public void UpdateExperienceUI(float fillAmount)
//    {
//        experienceBarImage.fillAmount = fillAmount;
//    }
//    public void UpdateDebuffImage(float fillAmount)
//    {
//        debuff1Image.fillAmount = fillAmount;
//    }
//    public void SetDebuffImage(Sprite debuffSprite)
//    {
//        debuffHandler.SetActive(true);
//        debuff1Image.sprite = debuffSprite;
//    }
//}
