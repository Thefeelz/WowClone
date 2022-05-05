using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatLog : MonoBehaviour
{
    [SerializeField] Text totalDamageText, currentDPSText, printoutText;
    [SerializeField] int totalDamage, currentDamage;
    [SerializeField] List<FightInstance> fightDamage = new List<FightInstance>();
    [SerializeField] bool currentlyLogging;
    [SerializeField] float elapsedTimeInCombat = 0f;


    [SerializeField] GameObject prettyBars;
    List<Image> prettyBarsFill = new List<Image>();
    List<Text> prettyBarName = new List<Text>();
    List<Text> prettyBarTotal = new List<Text>();
    List<Text> prettyBarPercent = new List<Text>();


    int iterations = 0;
    SpellBook spellBook;
    AbilityInstance[] viewableDamage = new AbilityInstance[4];
    // Start is called before the first frame update
    void Start()
    {
        spellBook = FindObjectOfType<BasePlayerStats>().GetPlayerSpellbook();
        totalDamageText.text = "0";
        currentDPSText.text = "0";
        SetUpPrettyBars();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentlyLogging)
        {
            elapsedTimeInCombat += Time.deltaTime;
            UpdateBars();
        }
    }
    void UpdateBars()
    {
        totalDamageText.text = currentDamage.ToString();
        currentDPSText.text = (currentDamage / elapsedTimeInCombat).ToString("F1");
    }
    public void AddCurrentDamage(int damageTaken, bool criticalStrike, int spellID)
    {
        fightDamage.ToArray()[iterations].UpdateAbilitiesUsedList(damageTaken, criticalStrike, spellID);
        currentDamage += damageTaken;     
    }
    public void CombatEnded()
    {
        FightInstance newFight = ScriptableObject.CreateInstance<FightInstance>();

        fightDamage.ToArray()[iterations].timeInFight = elapsedTimeInCombat;
        fightDamage.ToArray()[iterations].totalDamageDealt = currentDamage;
        fightDamage.ToArray()[iterations].id = iterations;
        
        currentlyLogging = false;
        elapsedTimeInCombat = 0f;
        currentDamage = 0;
        //PrintOutData();
        GetTotalDataFromFight();
        iterations++;
    }
    public void StartCombat()
    {
        FightInstance newFight = ScriptableObject.CreateInstance<FightInstance>();
        fightDamage.Add(newFight);
        currentlyLogging = true;
    }
    public bool GetCurrentlyLogging() { return currentlyLogging; }
    void PrintOutData()
    {
        printoutText.text = fightDamage.ToArray()[iterations].GeneratePrintoutForFightInstance(FindObjectOfType<BasePlayerStats>().GetPlayerSpellbook());
    }
    void GetTotalDataFromFight()
    {
        viewableDamage = fightDamage.ToArray()[iterations].GetPrintOutArray();
        Debug.Log(viewableDamage.Length);
        PrettyBarsPrintout();
    }
    void PrettyBarsPrintout()
    {
        // int i = 0;
        for(int i = 0; i < prettyBarsFill.Count; i++)
        {
            if(i < viewableDamage.Length) 
            {
                prettyBarsFill[i].transform.parent.gameObject.SetActive(true);
                prettyBarsFill.ToArray()[i].fillAmount = ((float)viewableDamage[i].abilityTotalDamage / viewableDamage[0].abilityTotalDamage);
                prettyBarName.ToArray()[i].text = spellBook.GetAbilityFromList(viewableDamage[i].abilityID).GetAbilityName();
                prettyBarPercent.ToArray()[i].text = (((float)viewableDamage[i].abilityTotalDamage / fightDamage.ToArray()[iterations].totalDamageDealt) * 100).ToString("F1") + "%";
                prettyBarTotal.ToArray()[i].text = viewableDamage[i].abilityTotalDamage.ToString();
            }
            else
            {
                prettyBarsFill[i].transform.parent.gameObject.SetActive(false);
            }
            
        }
    }
    void SetUpPrettyBars()
    {
        foreach (Transform transform in prettyBars.transform)
        {
            foreach (Transform transform1 in transform)
            {
                if(transform1.name == "Fill Bar")
                {
                    //Debug.Log("Added Fill Bar");
                    prettyBarsFill.Add(transform1.GetComponent<Image>());
                }
                else if (transform1.name == "Spell Name")
                {
                    //Debug.Log("Added Spell Name");
                    prettyBarName.Add(transform1.GetComponent<Text>());
                }
                else if (transform1.name == "Spell Total Damage")
                {
                    //Debug.Log("Added Total Damage");
                    prettyBarTotal.Add(transform1.GetComponent<Text>());
                }
                else if (transform1.name == "Spell Percent")
                {
                    //Debug.Log("Added Spell Percent");
                    prettyBarPercent.Add(transform1.GetComponent<Text>());
                }
            }
        }
    }
}
