//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class EnemyStats : MonoBehaviour
//{
//    [Header("UI Things")]
//    [SerializeField] Sprite enemyPortrait;
//    [SerializeField] Canvas enemyCanvas;
//    [SerializeField] Image healthBarImage;
//    [SerializeField] GameObject floatingTextPrefab;
//    [SerializeField] string enemyName;
//    [SerializeField] int enemyLevel;
//    [Header("Enemy Stats")]
//    [SerializeField] float enemyAttackPower;
//    [SerializeField] float timeBetweenAttacks;
//    [SerializeField] float enemyRunSpeed;
//    [SerializeField] float currentEnemyHealth;
//    [SerializeField] float maxEnemyHealth;
//    [SerializeField] float currentEnemyResource;
//    [SerializeField] float maxEnemyResource;
//    [SerializeField] float resourceRegen;
//    [SerializeField] float healthRegen;
//    [Header("Enemy Killed")]
//    [SerializeField] int goldToDrop;
//    [SerializeField] int experienceOnDeath;
//    [SerializeField] GameObject lootableObject;
//    //PlayerStats playerStats;
//    [SerializeField] bool currentlyTargetted = false;
//    Animator animController;
//    [SerializeField] List<DoT> dots = new List<DoT>();
//    Vector3 startingPos;
//    Quaternion startingRotation;

//    bool doubleDipping = false;
//    bool inCombat = false;
//    bool returnHome = false;
//    bool attackReady = true;
//    int activeDots = 0;
//    //PlayerUI playerUI;
//    void Start()
//    {
//        currentEnemyHealth = maxEnemyHealth;
//        currentEnemyResource = maxEnemyResource / 2;
//        //playerStats = FindObjectOfType<PlayerStats>();
//        animController = GetComponentInChildren<Animator>();
//        floatingTextPrefab.GetComponentInChildren<TextMesh>().transform.localScale = Vector3.one;
//        enemyCanvas.GetComponentInChildren<Text>().text = "[" + enemyLevel + "] " + enemyName;
//        //playerUI = FindObjectOfType<PlayerUI>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        currentlyTargetted = GetCurrentlyTargetted();
//        enemyCanvas.transform.LookAt(Camera.main.transform);
//        if (currentlyTargetted && dots.Count > 0)
//        {
            
//            foreach (DoT dot in dots)
//            {
//                //playerUI.UpdateDebuffImage(dot.timeLeft / dot.GetAbilityDuration());
//            }
//        }
//        if(inCombat)
//        {
//            ChasePlayer();
//        }
//        if(!returnHome)
//            ToFarFromHome();
//        if(returnHome)
//        {
//            ReturnHome();
//        }
//    }
//    void ToFarFromHome()
//    {
//        if(Vector3.Distance(transform.position, startingPos) > 10f)
//        {
//            returnHome = true;
//            inCombat = false;
//        }
//    }
//    void ReturnHome()
//    {
//        transform.LookAt(startingPos);
//        transform.position = Vector3.MoveTowards(transform.position, startingPos, enemyRunSpeed * Time.deltaTime);
//        if (Vector3.Distance(transform.position, startingPos) < .1f)
//        {
//            transform.rotation = startingRotation;
//            currentEnemyHealth = maxEnemyHealth;
//            currentEnemyResource = maxEnemyResource;
//            healthBarImage.fillAmount = 1f;
//            returnHome = false;
//            animController.SetBool("chasing", false);
//            RemoveAllDoTs();
//            activeDots = 0;
//        }
//    }
//    void ChasePlayer()
//    {
//        //transform.LookAt(playerStats.transform);
//        if (Vector3.Distance(transform.position, playerStats.transform.position) > 2f)
//        {
//            transform.position = Vector3.MoveTowards(transform.position, playerStats.transform.position, enemyRunSpeed * Time.deltaTime);
//            animController.SetBool("chasing", true);
//        }
//        else
//        {
//            animController.SetBool("chasing", false);
//            if (attackReady)
//            {
//                attackReady = false;
//                animController.SetBool("attack", true);
//                StartCoroutine(AnimationBoolDelay("attack", false, 0.2f));
//                StartCoroutine(GetAttackReady());
//            }
//        }
//    }
//    public void TakeDamage(float damage, bool criticalStrike)
//    {
//        animController.SetBool("takeDamage", true);
//        StartCoroutine(AnimationBoolDelay("takeDamage", false, 0.2f));
//        GameObject floatingText;
//        if (!returnHome)
//        {
//            if (!inCombat)
//            {
//                inCombat = true;
//                startingPos = transform.position;
//                startingRotation = transform.rotation;
//            }
//            currentEnemyHealth -= damage;
//            healthBarImage.fillAmount = currentEnemyHealth / maxEnemyHealth;
//            transform.LookAt(playerStats.transform);
//            if (criticalStrike) { floatingTextPrefab.GetComponentInChildren<TextMesh>().transform.localScale *= 2; }
//            floatingTextPrefab.GetComponentInChildren<TextMesh>().text = damage.ToString();
//            floatingText = Instantiate(floatingTextPrefab, transform.position, playerStats.transform.rotation);
//            Destroy(floatingText, 2f);
//            if (currentEnemyHealth <= 0 && !doubleDipping)
//            {
//                Death();
//            }
//            floatingTextPrefab.GetComponentInChildren<TextMesh>().transform.localScale = Vector3.one;
//        }
//        else
//        {
//            floatingTextPrefab.GetComponentInChildren<TextMesh>().text = "Immune";
//            floatingText = Instantiate(floatingTextPrefab, transform.position, playerStats.transform.rotation);
//        }
//    }

//    private void Death()
//    {
//        GameObject loot = Instantiate(lootableObject, transform.position, Quaternion.identity);
//        loot.transform.SetParent(this.transform);
//        doubleDipping = true;
//        playerStats.IncreaseExperience(experienceOnDeath);
//        animController.SetBool("dead", true);
//        inCombat = false;
//        Destroy(gameObject, 10f);
//    }

//    private void PassiveResourceIncrease()
//    {
//        if (currentEnemyResource < maxEnemyResource)
//            currentEnemyResource += resourceRegen * Time.deltaTime;
//        else
//            currentEnemyResource = maxEnemyResource;
//    }

//    private void PassiveHealthIncrease()
//    {
//        if (currentEnemyHealth < maxEnemyHealth)
//            currentEnemyHealth += healthRegen * Time.deltaTime;
//        else
//            currentEnemyHealth = maxEnemyHealth;
//    }

//    public float GetCurrentEnemyHealth()
//    {
//        return currentEnemyHealth;
//    }
//    public void SetCurrentEnemyHealth(float newValue)
//    {
//        currentEnemyHealth = newValue;
//    }
//    public float GetMaxEnemyHealth()
//    {
//        return maxEnemyHealth;
//    }
//    public void SetMaxEnemyHealth(float newValue)
//    {
//        maxEnemyHealth = newValue;
//    }
//    public float GetCurrentEnemyResource()
//    {
//        return currentEnemyResource;
//    }
//    public void SetCurrentEnemyResource(float newValue)
//    {
//        currentEnemyResource = newValue;
//    }
//    public float GetMaxEnemyResource()
//    {
//        return maxEnemyResource;
//    }
//    public void SetMaxEnemyResource(float newValue)
//    {
//        maxEnemyResource = newValue;
//    }
//    public void SetCurrentlyTargetted(bool newValue)
//    {
//        currentlyTargetted = newValue;
//    }
//    public bool GetCurrentlyTargetted()
//    {
//        return currentlyTargetted;
//    }
//    public int GetEnemyLevel()
//    {
//        return enemyLevel;
//    }
//    public string GetEnemyName()
//    {
//        return enemyName;
//    }
//    public Sprite GetEnemyPortrait()
//    {
//        return enemyPortrait;
//    }
//    public void RemoveDoTFromList(DoT dotToRemove)
//    {
//        dots.Remove(dotToRemove);
//        Destroy(dotToRemove.gameObject);
//        activeDots--;
//    }
//    public void RemoveAllDoTs()
//    {
//        for(int i = 0; i < dots.Count; i++)
//        {
//            RemoveDoTFromList(dots.ToArray()[i]);
//        }
//    }
//    public bool AddDoTToList(DoT newDot)
//    {
//        if (!inCombat)
//        {
//            inCombat = true;
//            startingPos = transform.position;
//            startingRotation = transform.rotation;
//        }
//        foreach (DoT doT in dots)
//        {
//            if (doT.GetAbilityName() == newDot.GetAbilityName())
//                return false;
//        }
//        dots.Add(newDot);
//        activeDots++;
//        playerUI.SetDebuffImage(newDot.GetAbilityImage());
//        return true;
//    }
//    public int GetActiveDoTs()
//    {
//        return activeDots;
//    }
//    IEnumerator AnimationBoolDelay(string setBoolName, bool stateToEnter, float timeToDelay)
//    {
//        yield return new WaitForSeconds(timeToDelay);
//        animController.SetBool(setBoolName, stateToEnter);
//    }
//    IEnumerator GetAttackReady()
//    {
//        yield return new WaitForSeconds(timeBetweenAttacks);
//        attackReady = true;
//    }
//    public void DoDamageToCharacter()
//    {
//        playerStats.TakeDamage(Mathf.Floor(Random.Range((enemyAttackPower * .8f), (enemyAttackPower * 1.2f))));
//    }
//    public int GoldToDrop()
//    {
//        return Random.Range(0, goldToDrop);
//    }
//}
