using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Procs : Buffs
{
    [SerializeField] Sprite procImage;
    [Header("Proc Effects Begin Here")]
    [SerializeField] int abilityIdToEffect;
    [Header("Cast Reduction Effects")]
    [SerializeField] bool spellCastReduction;
    [SerializeField] bool instantCast;
    [SerializeField] bool incrimentedReducedCast;
    [SerializeField] float amountToReduceCastPerStack;
    [Header("Cooldown Reduction Effects")]
    [SerializeField] bool cooldownReduction;
    [SerializeField] bool totalReset;
    [SerializeField] bool incrimentedCooldownReduction;
    [SerializeField] float amountToReduceCooldownPerStack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public Sprite GetProcImage() { return procImage; }
    public int GetAbilityIdToEffect() { return abilityIdToEffect; }
    public bool GetIsSpellCastReduction() { return spellCastReduction; }
    public bool GetIsInstantCast() { return instantCast; }
    public bool GetIsIncrimentedReducedCast() { return incrimentedReducedCast; }
    public float GetAmountToReduceCastPerStack() { return amountToReduceCastPerStack; }
    public bool GetIsCooldownReduction() { return cooldownReduction; }
    public bool GetIsTotalReset() { return totalReset; }
    public bool GetIsIncrimentedCooldownReduction() { return incrimentedCooldownReduction; }
    public float GetAmountToReduceCooldownPerStack() { return amountToReduceCooldownPerStack; }
}
