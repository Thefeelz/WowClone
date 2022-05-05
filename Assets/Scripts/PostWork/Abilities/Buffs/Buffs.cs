using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffs : MonoBehaviour
{
    [SerializeField] protected string buffName;
    [SerializeField] protected Sprite buffImage;
    [SerializeField] protected int buffID;
    [SerializeField] protected int buffMaxDuration;
    [SerializeField] protected float currentBuffDuration;
    [SerializeField] protected bool stackable;
    [Range(1, 10)] [SerializeField] protected int maxNumberOfStacks;
    [SerializeField] protected int currentNumberOfStacks;
    [Header("Magic Buff Attributes")]
    [Header("Stamina")]
    [SerializeField] protected bool staminaBuff;
    [Range(0, 100)] [SerializeField] protected float staminaPercentageBuff;
    [Header("Intellect")]
    [SerializeField] protected bool intellectBuff;
    [Range(0, 100)] [SerializeField] protected float intellectPercentageBuff;
    [Header("Spirit")]
    [SerializeField] protected bool spiritBuff;
    [Range(0, 100)] [SerializeField] protected float spiritPercentageBuff;
    [Header("Haste")]
    [SerializeField] protected bool hasteBuff;
    [Range(0, 100)] [SerializeField] protected float hastePercentageBuff;
    [Header("Crtical Strike")]
    [SerializeField] protected bool criticalStrikeBuff;
    [Range(0, 100)] [SerializeField] protected float criticalStrikePercentageBuff;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ResetBuff() 
    { 
        currentNumberOfStacks = 0;
    }
    public void ResetBuff(int abilityIDToClearEffectsFrom, BasePlayerStats player)
    {
        currentNumberOfStacks = 0;
        player.GetPlayerSpellbook().GetAbilityFromList(abilityIDToClearEffectsFrom).ProcRemovedFromList();
    }
    public string GetBuffName() { return buffName; }
    public Sprite GetBuffImage() { return buffImage; }
    public int GetBuffID() { return buffID; }
    public int GetCurrentStacks() { return currentNumberOfStacks; }
    public void StartBuffTimerOnAdd() { currentBuffDuration = buffMaxDuration; }
    public float GetBuffFillAmount() { return currentBuffDuration / buffMaxDuration; }
    public float GetBuffCurrentDuration() { return currentBuffDuration; }
    public void DecrementBuffDuration() { currentBuffDuration -= Time.deltaTime; }
    public bool GetStackable() { return stackable; }
    public int GetMaxNumberOfStacks() { return maxNumberOfStacks; }
    public void AddAStackToTheBuff()
    {
        if ((currentNumberOfStacks + 1) <= maxNumberOfStacks)
            currentNumberOfStacks++;
        currentBuffDuration = buffMaxDuration;
    }
    public bool GetStaminaBuff() { return staminaBuff; }
    public float GetStaminaPercentage()
    {
        if (staminaBuff)
            return (staminaPercentageBuff / 100) + 1;
        return 1;
    }
    public bool GetIntellectBuff() { return intellectBuff; }
    public float GetIntellectPercentage()
    {
        if (intellectBuff)
            return (intellectPercentageBuff / 100) + 1;
        return 1;
    }
    public bool GetSpiritBuff() { return spiritBuff; }
    public float GetSpiritPercentage()
    {
        if (spiritBuff)
            return (spiritPercentageBuff / 100) + 1;
        return 1;
    }
    public bool GetHasteBuff() { return hasteBuff; }
    public float GetHastePercentage()
    {
        if (hasteBuff)
            return (hastePercentageBuff / 100) + 1;
        return 1;
    }
    public bool GetCriticalStrikeBuff() { return criticalStrikeBuff; }
    public float GetCriticalStrikePercentage()
    {
        if (criticalStrikeBuff)
            return (criticalStrikePercentageBuff / 100) + 1;
        return 1;
    }
}
