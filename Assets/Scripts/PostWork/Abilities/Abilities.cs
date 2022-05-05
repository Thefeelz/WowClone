using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Abilities : MonoBehaviour
{
    // Abilty Stats and Features
    [SerializeField] protected string abilityName;
    [SerializeField] protected int abilityID;
    [SerializeField] protected float abilityCooldownTime;
    [SerializeField] protected float currentCooldownTime = 0f;
    [SerializeField] protected Sprite abilityImage;
    [SerializeField] protected int abilityRange;
    [SerializeField] protected float abilityCastTime;
    [SerializeField] Buffs abilityBuff;
    [SerializeField] bool isProc;
    [SerializeField] bool guaranteedProc;
    [Range(0,100)][SerializeField] float percentChanceToProc;
    [SerializeField] bool procOnNormalAbility;
    [SerializeField] bool procOnCriticalAbility;

    // These variables will hold data to reduce the cost or cast time of an ability by a proc/buff
    [SerializeField] int buffAbilityCostReduction;
    [SerializeField] float buffCastTimeReduction;


    float elapsedTime = 0f;
    bool readyToCast = true;
    bool calculateCooldown = false;
    protected BasePlayerStats player;
    protected ProcManager procManager;
    

    protected virtual void Update()
    {

    }

    public void SetCalculateCooldown(bool value) { calculateCooldown = value; }
    public bool GetAlreadyCalculatedCooldown() { return calculateCooldown; }
    public float GetAbilityCooldownFillAmount() { return 1- currentCooldownTime / abilityCooldownTime; }
    public bool IsAbilityOffCooldown() { return readyToCast; }
    public void PutAbilityOnCooldown() { readyToCast = false; }
    public void PutAbilityOffCooldown() { readyToCast = true; }
    public void SetReadyToCast(bool value) { readyToCast = value; }
    public bool GetReadyToCast() { return readyToCast; }
    public void SetAbilityName(string _abilityName) { abilityName = _abilityName; }
    public string GetAbilityName() { return abilityName; }
    public void SetAbilityID(int _abilityID) { abilityID = _abilityID; }
    public int GetAbilityID() { return abilityID; }
    public void SetAbilityCooldownTime(float _cooldownTime) { abilityCooldownTime = _cooldownTime; }
    public float GetAbilityCooldownTime() { return abilityCooldownTime; }
    public void SetAbilityCurrentCooldownTime(float _cooldownTime) { currentCooldownTime = _cooldownTime; }
    public float GetAbilityCurrentCooldownTime() { return currentCooldownTime; }
    public void SetAbilityImage(Sprite _abilityImage) { abilityImage = _abilityImage; }
    public Sprite GetAbilityImage() { return abilityImage; }
    public void SetAbilityRange(int _abilityRange) { abilityRange = _abilityRange; }
    public int GetAbilityRange() { return abilityRange; }
    public void SetAbilityCastTime(float _abilityCastTime) { abilityCastTime = _abilityCastTime; }
    public float GetAbilityCastTime() { return abilityCastTime; }
    public void PutAbilityOnGlobalCoolDown() { FindObjectOfType<CooldownManager>().AddAbilityToCooldownListForGlobalCooldown(this); }
    public void AssignCasterToAbility(BasePlayerStats _player) { player = _player; }
    public BasePlayerStats GetCaster() { return player; }
    //protected void SetToSpellFlash(int spellIDCooldownResetOnCrit ) { FindObjectOfType<SpellFlash>().SetAbilityToSpellFlash(player.GetPlayerSpellbook().GetAbilityFromList(spellIDCooldownResetOnCrit)); }
    public bool GetHasProcs() { return isProc; }
    public bool GetGuaranteedProc() { return guaranteedProc; }
    public float GetPercentChanceToProc() { return percentChanceToProc; }
    public bool GetProcOnNormalAbility() { return procOnNormalAbility; }
    public bool GetProcOnCriticalAbility() { return procOnCriticalAbility; }
    public void SetSuccesfulProc() { player.GetComponent<ProcManager>().AddProcToList(abilityBuff.GetComponent<Procs>()); }
    public Buffs GetBuffFromAbility() { return abilityBuff; }
    public void SetBuffAbilityCostReduction(int amountToReduceAbilityCost) { buffAbilityCostReduction = amountToReduceAbilityCost; }
    public int GetBuffAbilityCostReduction() { return buffAbilityCostReduction; }
    public void SetBuffAbilityCastTimeReduction(float castTimeReduction) { buffCastTimeReduction = castTimeReduction; }
    public float GetBuffAbilityCastTimeReduction() { return buffCastTimeReduction; }
    public void ClearProcsFromList(BasePlayerStats _player) 
    { 
        _player.GetComponent<ProcManager>().RemoveProcFromListOnSpellUse(abilityID);
        buffAbilityCostReduction = 0;
        buffCastTimeReduction = 0;
    }
    public void ProcRemovedFromList()
    {
        buffAbilityCostReduction = 0;
        buffCastTimeReduction = 0;
    }
}
