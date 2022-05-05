using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseEnemyStats : MonoBehaviour
{
    public enum EnemyState { Idle, Chasing, Attacking, Dead, ReturnHome, Fleeing};
    [SerializeField] protected EnemyState enemyState, previousState;
    [Header("UI Things")]
    [SerializeField] Sprite enemyPortrait;
    [SerializeField] Canvas enemyCanvas;
    [SerializeField] Image healthBarImage;
    [SerializeField] GameObject floatingTextPrefab;
    [SerializeField] string enemyName;
    [SerializeField] int enemyLevel;
    [Header("Enemy Stats")]
    [SerializeField] int currentEnemyHealth;
    [SerializeField] int maxEnemyHealth;
    [SerializeField] int currentEnemyResource;
    [SerializeField] int maxEnemyResource;
    [Header("Death Things")]
    [SerializeField] int experienceToAwardPlayer;
    [SerializeField] int enemyID;
    [SerializeField] bool currentlyTargetted = false;
    [Header("Attack Sections")]
    [SerializeField] protected int maxDetectionRange;
    [SerializeField] protected int runSpeed, walkSpeed;

    // Values used in the Script
    bool deadAndLootDropped = false;

    protected BasePlayerStats player;
    UPlayerUI newUI;
    protected Animator enemyAnim;
    
    CombatLog combatLog;
    PlayerInCombatTracker combatTracker;
    bool inCombatTracker = false;
    // Start is called before the first frame update
    protected void Start()
    {
        
        player = FindObjectOfType<BasePlayerStats>();
        combatTracker = FindObjectOfType<PlayerInCombatTracker>();
        combatLog = FindObjectOfType<CombatLog>();
        currentEnemyHealth = maxEnemyHealth;
        currentEnemyResource = maxEnemyResource;
        enemyCanvas.GetComponentInChildren<Text>().text = "[" + enemyLevel + "] " + enemyName;
        newUI = FindObjectOfType<UPlayerUI>();
        enemyAnim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    protected void Update()
    {
        //GetPotentialTargetInRange();
        enemyCanvas.transform.LookAt(Camera.main.transform);
    }
    void GetPotentialTargetInRange()
    {
        //BasePlayerStats 
        //if(FindObjectsOfType<BasePlayerStats>())
    }
    public void TakeDamage(int damage, float criticalStrike, int spellID, Abilities ability)
    {
        player = ability.GetCaster();
        ProcessStateOnDamage();
        if (!deadAndLootDropped)
        {
            bool crit = false;
            if (CalculateCriticalStrike(criticalStrike))
            {
                damage = Mathf.CeilToInt(damage * 1.5f);
                crit = true;
            }
            currentEnemyHealth -= damage;

            // ================HUGE FUCKING TODO, HOPEFULLY SOONER THAN LATER, HANDLE BOTH PHYSICAL AND MAGIC INSTEAD OF JUST MAGIC=====================
            HandleAbility(ability.GetComponent<DamageMagic>(), crit);
            CombatLogStuff(damage, crit, spellID);
            healthBarImage.fillAmount = (float)currentEnemyHealth / maxEnemyHealth;
            transform.LookAt(player.transform);
            FloatingTextDisplay(damage, crit);
            StartCoroutine(AnimationBoolSwitchWithDelay("takeDamage", 1f, false));
            if (currentEnemyHealth <= 0)
            {
                KillEnemy();
            }
        }
    }
    void HandleAbility(DamageMagic ability, bool crit)
    {
        /*
        if(ability.HasCriticalStrikeExtraEffects() && crit)
        {
            if(ability.HasResetCooldownOnCriticalStrike())
            {
                ability.HandleResetCooldownOnCrticalStrike();
            }
        }
        */
        if(ability.GetHasProcs())
        {
            HandleProcs(ability, crit);
        }
    }
    void HandleProcs(DamageMagic ability, bool crit)
    {
        if(ability.GetGuaranteedProc() || ability.GetPercentChanceToProc() >= Random.Range(0,100))
        {
            if(ability.GetProcOnCriticalAbility() && crit)
            {
                HandleSuccesfulProc(ability);
                return;
            }
            if(ability.GetProcOnNormalAbility())
            {
                HandleSuccesfulProc(ability);
            }
            
        }
    }
    void HandleSuccesfulProc(DamageMagic ability)
    {
        ability.SetSuccesfulProc();
    }
    void CombatLogStuff(int damage, bool criticalStrike, int spellID)
    {  
        if(!inCombatTracker)
        {
            combatTracker.AddEnemyToList(this);
            inCombatTracker = true;
        }
        combatLog.AddCurrentDamage(damage, criticalStrike, spellID);
    }

    public int CalculateExperienceToGive()
    {
        if (Mathf.Abs(player.GetPlayerLevel() - enemyLevel) < 5)
            return experienceToAwardPlayer;
        else
            return 0;
    }

    // ==================================================================
    //           GETTERS AND SETTERS FOR THE ENEMY CLASS
    // ==================================================================
    public float GetCurrentEnemyHealth(){ return currentEnemyHealth; }
    public void SetCurrentEnemyHealth(int newValue){currentEnemyHealth = newValue;}
    public float GetMaxEnemyHealth(){return maxEnemyHealth;}
    public void SetMaxEnemyHealth(int newValue){maxEnemyHealth = newValue;}
    public float GetCurrentEnemyResource(){return currentEnemyResource;}
    public void SetCurrentEnemyResource(int newValue){currentEnemyResource = newValue;}
    public float GetMaxEnemyResource(){ return maxEnemyResource; }
    public void SetMaxEnemyResource(int newValue) { maxEnemyResource = newValue;}
    public void SetCurrentlyTargetted(bool newValue)
    {
        if (newValue)
            enemyCanvas.GetComponentInChildren<Text>().color = Color.red;
        else
            enemyCanvas.GetComponentInChildren<Text>().color = Color.black;
        currentlyTargetted = newValue;
    }
    public bool GetCurrentlyTargetted(){return currentlyTargetted;}
    public int GetEnemyLevel(){return enemyLevel;}
    public string GetEnemyName(){return enemyName;}
    public Sprite GetEnemyPortrait(){return enemyPortrait;}
    public float GetEnemyHealthBarFill() { return (float)currentEnemyHealth / maxEnemyHealth; }
    public float GetEnemyResourceBarFill() { return (float)currentEnemyResource / maxEnemyResource; }

    public int GetEnemyID() { return enemyID; }

    void CheckIfForQuestProgress() { player.GetComponent<QuestManager>().UpdateQuestProcessOnEnemyDeath(enemyID); }

    IEnumerator AnimationBoolSwitchWithDelay(string animationName, float delayTime, bool desiredValue)
    {
        enemyAnim.SetBool(animationName, !desiredValue);
        yield return new WaitForSeconds(delayTime);
        enemyAnim.SetBool(animationName, desiredValue);
    }

    void FloatingTextDisplay(int damage, bool crit)
    {
        GameObject floatingText;
        floatingTextPrefab.GetComponentInChildren<TextMesh>().text = damage.ToString();
        floatingText = Instantiate(floatingTextPrefab, transform.position, player.transform.rotation);
        if (crit)
        { 
            floatingText.transform.localScale *= 2f;
            floatingText.GetComponentInChildren<TextMesh>().color = Color.red;
        }
        
        Destroy(floatingText, 2f);
    }
    private void KillEnemy()
    {
        SetCurrentStateToDead();
        combatTracker.RemoveEnemyFromList(this);
        player.AddExperienceToPlayer(CalculateExperienceToGive());
        GetComponent<EnemyLootTable>().GenerateLoot();
        CheckIfForQuestProgress();
        deadAndLootDropped = true;
        enemyAnim.SetBool("dead", true);
        if (GetComponent<EnemyLootTable>().GetContainsLoot())
            Destroy(gameObject, 60f);
        else
            Destroy(gameObject, 20f);
    }

    bool CalculateCriticalStrike(float critChance)
    {
        if(Random.Range(0, 100) < critChance)
        {
            return true;
        }
        return false;
    }
    protected void ProcessStateOnDamage()
    {
        if (enemyState == EnemyState.Idle)
            enemyState = EnemyState.Chasing;
    }
    protected void SetCurrentStateToDead()
    {
        enemyAnim.SetBool("dead", true);
        enemyState = EnemyState.Dead;
    }
}
