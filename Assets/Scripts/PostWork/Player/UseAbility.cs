using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAbility : MonoBehaviour
{
    // ===============Fields to Store Target Data===============
    UPlayerTargetting playerTarget;
    GameObject testTarget;
    BaseEnemyStats newEnemytarget, castedEnemyTarget;
    BaseFriendlyStats newFriendlyTarget, castedFriendlyTarget;
    [SerializeField] Transform spellCastLocation;

    // ===============Variables Utilized in the Code===============
    BasePlayerStats ourPlayer; // Reference to the Base Player Class (Used Primarily at the moment for GCD Control)

    MagicUser magicUser;

    UPlayerUI myUI; // A Reference to our UI, so the UI can display information that is getting fed in, instead of holding the information

    bool castingDamageSpell = false; // This will only allow a damage spell to be cast while active

    bool castingDotMagic = false;

    bool castingBeneficialSpell = false;

    bool castingBuffMagic = false;

    float elapsedTime = 0f; // This is the time since the 'cast' began, after cast is complete it gets reset to 0

    float castTime = 0f; // This is the stored cast time from the ability we want to cast, after a completed cast it gets reset to 0

    Abilities abilityToUse; // This is the generic Ability that we store ANY ability in, to avoid errors we use bools to control
                            // the logic flow of what ability CAN be cast, so when we instantiate or do something, we always are
                            // using the correct 'Ability'
    bool casting = false; // This is used to determine if the character is interupted whie casting

    bool succesfulCast = false;

    Vector3 startingCastPosition; // This will be used to determine if the player has moved at all during the cast

    Animator animController; // Controller attached to the player

    SpellFlash spellFlash; // UI Display for when abilities are proc'd

    [SerializeField] ErrorMessage errorMessage;

    private void Start()
    {
        SetUpStart();
    }

    private void Update()
    {
        if(castingDamageSpell && casting)
        {
            CastingDamageMagicAttack();
        }
        else if (castingDotMagic && casting)
        {
            CastingDoTMagicAttack();
        }
        else if (castingBeneficialSpell && casting)
        {
            CastingFriendlyMagic();
        }
        if(casting)
        {
            CheckForPlayerMovement();
        }
    }
    public void UsePlayerAbility(Abilities ability)
    {
        SetUpPlayerTargets(playerTarget.PT_GetCurrentTarget());
        if (ability.GetComponent<DamageMagic>())
        {
            AttackTarget(ability.GetComponent<DamageMagic>());
        }
        else if (ability.GetComponent<FriendlyMagic>())
        {
            ProcessFriendlyMagic(ability.GetComponent<FriendlyMagic>());
            //MagicBuffTarget(ability.GetBuffFromAbility());
        }
    }
    public void AttackTarget(DamageMagic attackAbility)
    {
        
        if (!casting)
        {
            // Verify that our player is targetting something
            if (testTarget != null)
            {
                // Verify that our player is targetting an ATTACKABLE object
                if (newEnemytarget != null)
                {
                    // Since we are attacking an enemy, assign the test target to an enemy target
                    castedEnemyTarget = newEnemytarget;

                    // Check to see if we are on the GCD and that our ability is off CD
                    if (!ourPlayer.GetOnGCD() && attackAbility.IsAbilityOffCooldown())
                    {                   
                        // Function to determine if the attack is still valid
                        ValidateMagicAttack(attackAbility);      
                    }
                    else
                    {
                        errorMessage.SetMessageToScreen("Cannot cast that ability yet", 3f);
                    }
                }
                else
                {
                    errorMessage.SetMessageToScreen("Invalid Target", 3f);
                }
            }
            else
            {
                errorMessage.SetMessageToScreen("You don't have a target", 3f);
            }
        }
        else
        {
            errorMessage.SetMessageToScreen("Cannot cast that ability yet", 3f);
        }
    }

    void ProcessFriendlyMagic(FriendlyMagic friendlyAbility)
    {
        if (!casting)
        {
            // Verify that our player is targetting a FRIENDLY object
            if (newEnemytarget == null)
            {
                // Check to see if we are on the GCD and that our ability is off CD
                if (!ourPlayer.GetOnGCD() && friendlyAbility.IsAbilityOffCooldown())
                {
                    if (newFriendlyTarget != null)
                    {
                        // Since we are attacking an enemy, assign the test target to an enemy target
                        castedFriendlyTarget = newFriendlyTarget;
                        ValidateFriendlyMagic(friendlyAbility, castedFriendlyTarget);
                    }
                    else
                    {
                        // Function to determine if the attack is still valid
                        ValidateFriendlyMagic(friendlyAbility);
                    }
                    
                }
                else
                {
                    errorMessage.SetMessageToScreen("Cannot cast that ability yet", 3f);
                }
            }
            else
            {
                errorMessage.SetMessageToScreen("Invalid Target", 3f);
            }
        }
        else
        {
            errorMessage.SetMessageToScreen("Cannot cast that ability yet", 3f);
        }
    }
    void ValidateFriendlyMagic(FriendlyMagic friendlyAbility, BaseFriendlyStats friendlyStats)
    {
        // Step 1, ensure that we have enough mana to cast the ability
        if (ourPlayer.GetComponent<MagicUser>().GetPlayerCurrentMana() >= friendlyAbility.GetAbilityManaCost())
        {
            // Step2, ensure that our target is not dead
            if (castedFriendlyTarget.GetCurrentFriendlyHealth() > 0)
            {
                // Step 3, make sure that we are in range of our target
                if (Helper.GetAbilityInRange(transform, castedFriendlyTarget.transform, friendlyAbility.GetAbilityRange()))
                {
                    // Step 4, Make sure we are in LoS of target
                    if (Helper.GetInLineOfSight(transform, castedFriendlyTarget.transform))
                    {
                        if (friendlyAbility.GetComponent<MagicBuffs>())
                        {   // Set the GCD to be true
                            ourPlayer.SetOnGCD(true);

                            // Start casting the Magic Attack
                            PrepareUsingAbility(friendlyAbility.GetComponent<MagicBuffs>());
                        }
                    }
                    else
                    {
                        errorMessage.SetMessageToScreen("Not in Line of Sight", 3f);
                    }
                }
                else
                {
                    errorMessage.SetMessageToScreen("Ability is not in range", 3f);
                }
            }
            else
            {
                errorMessage.SetMessageToScreen("Invalid Target", 3f);
            }
        }
        else
        {
            errorMessage.SetMessageToScreen("Not enough mana", 3f);
        }
    }
    void ValidateFriendlyMagic(FriendlyMagic friendlyAbility)
    {
        // Step 1, ensure that we have enough mana to cast the ability
        if (ourPlayer.GetComponent<MagicUser>().GetPlayerCurrentMana() >= friendlyAbility.GetAbilityManaCost())
        { 
            if (friendlyAbility.GetComponent<MagicBuffs>())
            {   // Set the GCD to be true
                ourPlayer.SetOnGCD(true);

                // Start casting the Magic Attack
                PrepareUsingAbility(friendlyAbility.GetComponent<MagicBuffs>());
            }  
        }
        else
        {
            errorMessage.SetMessageToScreen("Not enough mana", 3f);
        }
    }
    void CastingFriendlyMagic()
    {
        // Incriment Time
        elapsedTime += Time.deltaTime;
        // Update the cast bar so that the user can see that we are casting
        myUI.UpdateCastBar(elapsedTime, castTime);
        // If the time since the start of the cast is greater than or equal to our cast time, we start the casting process
        if (elapsedTime >= castTime)
        {
            // Verify that we are still facing the target at the end of the cast
            if (castedFriendlyTarget == null || Helper.GetInLineOfSight(transform, castedFriendlyTarget.transform))
            {
                // Verify that the target is still alive
                if (castedFriendlyTarget == null || castedFriendlyTarget.GetCurrentFriendlyHealth() > 0)
                {
                    // Take mana away from the player
                    ourPlayer.GetComponent<MagicUser>().SetPlayerCurrentMana(ourPlayer.GetComponent<MagicUser>().GetPlayerCurrentMana() - abilityToUse.GetComponent<FriendlyMagic>().GetAbilityManaCost());

                    if (castingBuffMagic)
                        MagicBuffTarget(abilityToUse.GetBuffFromAbility());
                    // Set the ability cooldown time current to its cooldown time max
                    abilityToUse.SetAbilityCurrentCooldownTime(abilityToUse.GetAbilityCooldownTime());
                    // Set the flag to decriment the ability cooldown in other scripts (this prevents data from being stored when the ability is not even being used)
                    abilityToUse.PutAbilityOnCooldown();

                    // Check against Spell Flash Procs
                    spellFlash.CheckNewCastToUseSpellFlash(abilityToUse);
                    abilityToUse.ClearProcsFromList(ourPlayer);
                    // Finish the casting animation
                    CompleteCastDamageAnimation();
                }
                else
                {
                    errorMessage.SetMessageToScreen("Invalid Target", 3f);

                }
            }
            else
            {
                errorMessage.SetMessageToScreen("You must be facing your target", 3f);

            }
            // At this point, whether or not there was a succesful cast, casting will have stopped
            SetAllCastingToFalse();
        }
    }
    void MagicBuffTarget(Buffs buffAbility)
    {
        if (castedFriendlyTarget && castedFriendlyTarget.GetComponent<BuffManager>())
        {
            castedFriendlyTarget.GetComponent<BuffManager>().AddBuffToList(buffAbility);
        }
        else
        {
            ourPlayer.AddToBuffManager(buffAbility);
        }
    }

    private void ValidateMagicAttack(DamageMagic attackAbility)
    {
        // Step 1, ensure that we have enough mana to cast the ability
        if (ourPlayer.GetComponent<MagicUser>().GetPlayerCurrentMana() >= attackAbility.GetAbilityManaCost())
        {
            // Step2, ensure that our target is not dead
            if (castedEnemyTarget.GetCurrentEnemyHealth() > 0)
            {
                // Step 3, make sure that we are in range of our target
                if (GetAbilityInRange(attackAbility.GetAbilityRange()))
                {
                    // Step 4, make sure that we are facing the target
                    if (IsPlayerFacingTarget())
                    {
                        if (attackAbility.GetComponent<DamageMagicProjectiles>())
                        {   // Set the GCD to be true
                            ourPlayer.SetOnGCD(true);

                            // Start casting the Magic Attack
                            PrepareUsingAbility(attackAbility.GetComponent<DamageMagicProjectiles>());
                        }
                        else if(attackAbility.GetComponent<DamageMagicDoT>())
                        {
                            // Set the GCD to be true
                            ourPlayer.SetOnGCD(true);

                            // Start casting the Magic Attack
                            PrepareUsingAbility(attackAbility.GetComponent<DamageMagicDoT>());
                        }
                    }
                }
                else
                {
                    errorMessage.SetMessageToScreen("Ability is not in range", 3f);
                }
            }
            else
            {
                errorMessage.SetMessageToScreen("Invalid Target", 3f);
            }
        }
        else
        {
            errorMessage.SetMessageToScreen("Not enough mana", 3f);
        }
    }

    private void PrepareUsingAbility(DamageMagicProjectiles attackAbility)
    {
        // This is used to store our location and determine if we moved during the cast
        startingCastPosition = transform.position;
        // Set our cast time to the ability cast time
        castTime = attackAbility.GetAbilityCastTime() - ourPlayer.GetTotalHasteTimeToReduce() - attackAbility.GetBuffAbilityCastTimeReduction();
        if (castTime <= 0)
        {
            castTime = 0;
            animController.SetBool("instantCast", true);
            StartCoroutine(AnimationBoolDelay("instantCast", false, 1f));
        }
        
        // Set our ability to use to the attack ability;
        abilityToUse = attackAbility;
        // Set casting Damage spell to true, this is to ensure that the right ability gets called when we introduce more than attacking abilities
        castingDamageSpell = true;
        // Set casting to true to detect if we are interuppted whilst casting
        casting = true;
        // Start the casting Animation
        StartCastAnimation();
        // Pass in the ability we are about to cast into the UI (IMPORTANT NOTE, the UI is READING data and displaying it, not to be used for holding data for validation)
        myUI.CastBarInitialize(attackAbility);
    }

    private void PrepareUsingAbility(DamageMagicDoT attackAbility)
    {
        // This is used to store our location and determine if we moved during the cast
        startingCastPosition = transform.position;
        // Set our cast time to the ability cast time
        castTime = attackAbility.GetAbilityCastTime() - ourPlayer.GetTotalHasteTimeToReduce();
        if (castTime <= 0)
        {
            castTime = 0;
            animController.SetBool("instantCast", true);
            StartCoroutine(AnimationBoolDelay("instantCast", false, 1f));
        }

        // Set our ability to use to the attack ability;
        abilityToUse = attackAbility;
        // Set casting Damage spell to true, this is to ensure that the right ability gets called when we introduce more than attacking abilities
        castingDotMagic = true;
        // Set casting to true to detect if we are interuppted whilst casting
        casting = true;
        // Start the casting Animation
        StartCastAnimation();
        // Pass in the ability we are about to cast into the UI (IMPORTANT NOTE, the UI is READING data and displaying it, not to be used for holding data for validation)
        myUI.CastBarInitialize(attackAbility);
    }

    private void PrepareUsingAbility(MagicBuffs friendlyAbility)
    {
        // This is used to store our location and determine if we moved during the cast
        startingCastPosition = transform.position;
        // Set our cast time to the ability cast time
        castTime = friendlyAbility.GetAbilityCastTime() - ourPlayer.GetTotalHasteTimeToReduce();
        if (castTime <= 0)
        {
            castTime = 0;
            animController.SetBool("instantCast", true);
            StartCoroutine(AnimationBoolDelay("instantCast", false, 1f));
        }

        // Set our ability to use to the attack ability;
        abilityToUse = friendlyAbility;
        // Set casting Damage spell to true, this is to ensure that the right ability gets called when we introduce more than attacking abilities
        castingBeneficialSpell = true;
        castingBuffMagic = true;
        // Set casting to true to detect if we are interuppted whilst casting
        casting = true;
        // Start the casting Animation
        StartCastAnimation();
        // Pass in the ability we are about to cast into the UI (IMPORTANT NOTE, the UI is READING data and displaying it, not to be used for holding data for validation)
        myUI.CastBarInitialize(friendlyAbility);
    }
    void CastingDamageMagicAttack()
    {
        // Incriment Time
        elapsedTime += Time.deltaTime;
        // Update the cast bar so that the user can see that we are casting
        myUI.UpdateCastBar(elapsedTime, castTime);
        // If the time since the start of the cast is greater than or equal to our cast time, we start the casting process
        if(elapsedTime >= castTime)
        {
            // Verify that we are still facing the target at the end of the cast
            if(IsPlayerFacingTarget())
            {
                // Verify that the target is still alive
                if (castedEnemyTarget.GetCurrentEnemyHealth() > 0)
                {
                    // Take mana away from the player
                    ourPlayer.GetComponent<MagicUser>().SetPlayerCurrentMana(ourPlayer.GetComponent<MagicUser>().GetPlayerCurrentMana() - abilityToUse.GetComponent<DamageMagicProjectiles>().GetAbilityManaCost());
                    // Instantiate the object as a new projectile
                    DamageMagicProjectiles newProjectile = Instantiate(abilityToUse.GetComponent<DamageMagicProjectiles>(), spellCastLocation.position, Quaternion.identity);
                    // Set the ability cooldown time current to its cooldown time max
                    abilityToUse.SetAbilityCurrentCooldownTime(abilityToUse.GetAbilityCooldownTime());
                    // Set the flag to decriment the ability cooldown in other scripts (this prevents data from being stored when the ability is not even being used)
                    abilityToUse.PutAbilityOnCooldown();
                    // ==========IMPORTANT==========
                    // You NEED to set the projectile to a target, it cannot do so on its own (It tries but its dumb, or maybe I am, but Ima say its dumb so I feel better)
                    newProjectile.AssignCasterToAbility(ourPlayer);
                    newProjectile.GetTarget(castedEnemyTarget);
                    newProjectile.CalculateAbilityDamage(magicUser.GetPlayerTotalIntellect(), magicUser.GetPlayerTotalCriticalStrikePercentage(), newEnemytarget);

                    // Check against Spell Flash Procs
                    spellFlash.CheckNewCastToUseSpellFlash(newProjectile);
                    abilityToUse.ClearProcsFromList(ourPlayer);

                    // Finish the casting animation
                    CompleteCastDamageAnimation();
                }
                else
                {
                    errorMessage.SetMessageToScreen("Invalid Target", 3f);
                    
                }
            }
            else
            {
                errorMessage.SetMessageToScreen("You must be facing your target", 3f);
               
            }
            // At this point, whether or not there was a succesful cast, casting will have stopped
            SetAllCastingToFalse();
        }
    }

    void CastingDoTMagicAttack()
    {
        // Incriment Time
        elapsedTime += Time.deltaTime;
        // Update the cast bar so that the user can see that we are casting
        myUI.UpdateCastBar(elapsedTime, castTime);
        // If the time since the start of the cast is greater than or equal to our cast time, we start the casting process
        if (elapsedTime >= castTime)
        {
            // Verify that we are still facing the target at the end of the cast
            if (IsPlayerFacingTarget())
            {
                
                // Verify that the target is still alive
                if (castedEnemyTarget.GetCurrentEnemyHealth() > 0)
                {
                    // Take mana away from the player
                    ourPlayer.GetComponent<MagicUser>().SetPlayerCurrentMana(ourPlayer.GetComponent<MagicUser>().GetPlayerCurrentMana() - abilityToUse.GetComponent<DamageMagicDoT>().GetAbilityManaCost());
                    // Instantiate the object as a new projectile with all of the correct and real time stats
                    DamageMagicDoT newDoT = Instantiate(abilityToUse.GetComponent<DamageMagicDoT>(), spellCastLocation.position, Quaternion.identity);
                    newDoT.AssignCasterToAbility(ourPlayer);
                    newDoT.CalculateAbilityDamage(magicUser.GetPlayerTotalIntellect(), magicUser.GetPlayerTotalCriticalStrikePercentage(), castedEnemyTarget);
                    newDoT.SetTickRate(newDoT.GetTickRate() - magicUser.GetTotalHasteTimeToReduce());
                    // Set the ability cooldown time current to its cooldown time max
                    abilityToUse.SetAbilityCurrentCooldownTime(abilityToUse.GetAbilityCooldownTime());
                    // Set the flag to decriment the ability cooldown in other scripts (this prevents data from being stored when the ability is not even being used)
                    abilityToUse.PutAbilityOnCooldown();
                    // ==========IMPORTANT==========
                    // You NEED to set the projectile to a target, it cannot do so on its own (It tries but its dumb, or maybe I am, but Ima say its dumb so I feel better)
                    castedEnemyTarget.GetComponent<DebuffManager>().AddDotToList(newDoT);

                    // Check against Spell Flash Procs
                    spellFlash.CheckNewCastToUseSpellFlash(newDoT);

                    // Finish the casting animation
                    CompleteCastDamageAnimation();
                }
                else
                {
                    errorMessage.SetMessageToScreen("Invalid Target", 3f);

                }
            }
            else
            {
                errorMessage.SetMessageToScreen("You must be facing your target", 3f);

            }
            // At this point, whether or not there was a succesful cast, casting will have stopped
            SetAllCastingToFalse();
        }
    }

    void StopCasting()
    {
        // Reset everything so we can cast a new ability
        castTime = 0;
        elapsedTime = 0;
        abilityToUse = null;
        // We check for a succesful cast to either stop casting animation entirley, or to animate the "throw a magic ball" animation
        if (!succesfulCast)
            animController.SetBool("castStopped", true);
        animController.SetBool("casting", false);
        // Turn off the cast bar since we are done casting
        myUI.CompletedCast();
        GetComponent<HandGlowToggle>().ToggleHandGlow();
        
    }
    void SetAllCastingToFalse()
    {
        
        // This is incase our player starts a cast for less than 1 second and moves, dont give the full 1 second GCD penalty since they didn't cast at all
        // If they did cast an ability that is longer than 1 second, GCD would be up anyways
        // If they casted an ability that is less than 1 second, internal code in other scripts impliments the GCD remainder onto the ability cooldown
        if(castTime != 0 && !succesfulCast)
            ourPlayer.SetOnGCD(false);
        // Set casting to be false so we arent checking for player movement to interupt casting
        casting = false;
        // Set casting of all types to be false, this is to ensure we dont call the wrong things when casting different abilities
        castingDamageSpell = false;
        castingDotMagic = false;
        castingBeneficialSpell = false;
        castingBuffMagic = false;
        castedEnemyTarget = null;
        castedFriendlyTarget = null;
        // Call the Stop Casting function
        StopCasting();
    }

    // ================================================================================
    // ===============FUNCTIONS BELOW HERE ARE BEING USED FOR ANIMATION===============
    // ================================================================================
    void StartCastAnimation()
    {
        GetComponent<HandGlowToggle>().ToggleHandGlow();
        animController.SetBool("castComplete", false);
        animController.SetBool("castStopped", false);
        animController.SetBool("casting", true);
    }

    void CompleteCastDamageAnimation()
    {
        animController.SetBool("castComplete", true);
        
        // Starts a coroutine that will delay animation calls so that animations flow succesfully
        StartCoroutine(AnimationBoolDelay("castComplete", false, 0.25f));
        // This is to determine whether we follow through with certain animations
        succesfulCast = true;
    }
    void CompleteCastFriendlyAnimation()
    {
        animController.SetBool("castComplete", true);
        // Starts a coroutine that will delay animation calls so that animations flow succesfully
        StartCoroutine(AnimationBoolDelay("castComplete", false, 0.25f));
        // This is to determine whether we follow through with certain animations
        succesfulCast = true;
    }

    // ================================================================================
    // ===============FUNCTIONS BELOW HERE ARE BEING USED FOR VALIDATION===============
    // ================================================================================
    private bool IsPlayerFacingTarget()
    {
        RaycastHit hit;

        Physics.Linecast(transform.position, newEnemytarget.transform.position, out hit);
        if (hit.transform.gameObject.layer != 6)
        {
            if((Vector3.Angle(transform.forward, newEnemytarget.transform.position - transform.position) < 90))
            {
                return true;
            }
            else
            {
                errorMessage.SetMessageToScreen("You must be facing your target", 3f);
            }
        }
        else
        {
            errorMessage.SetMessageToScreen("Target is not in Line of Sight.", 3f);
        }
        return false;
    }

    bool GetAbilityInRange(int range)
    {
        if(Vector3.Distance(transform.position, newEnemytarget.transform.position) < range) { return true; }
        return false;
    }

    void CheckForPlayerMovement()
    {
        if(startingCastPosition != transform.position)
        {
            errorMessage.SetMessageToScreen("Interupted", 3f);

            SetAllCastingToFalse();
        }
    }

    public ErrorMessage GetCurrentErrorMessagePrompt() { return errorMessage; }

    // ================================================================================
    // ===========================COROUTINES BEGIN HERE================================
    // ================================================================================
    IEnumerator AnimationBoolDelay(string setBoolName, bool stateToEnter, float timeToDelay)
    {
        yield return new WaitForSeconds(timeToDelay);
        animController.SetBool(setBoolName, stateToEnter);
    }
    void SetUpStart()
    {
        spellFlash = FindObjectOfType<SpellFlash>();
        playerTarget = GetComponent<UPlayerTargetting>();
        ourPlayer = GetComponent<BasePlayerStats>();
        myUI = FindObjectOfType<UPlayerUI>();
        animController = GetComponentInChildren<Animator>();
        if (ourPlayer.GetClassType() == BasePlayerStats.ClassType.Intellect)
            magicUser = ourPlayer.GetComponent<MagicUser>();
        newEnemytarget = null;
    }
    void SetUpPlayerTargets(GameObject targetCheck)
    {
        newEnemytarget = null;
        newFriendlyTarget = null;
        testTarget = targetCheck;
        if(testTarget)
        {
            if (testTarget.GetComponent<BaseEnemyStats>())
                newEnemytarget = testTarget.GetComponent<BaseEnemyStats>();
            else if (testTarget.GetComponent<BaseFriendlyStats>())
                newFriendlyTarget = testTarget.GetComponent<BaseFriendlyStats>();
        }
    }
    IEnumerator DelayMoveBack()
    {
        yield return new WaitForSeconds(0.1f);
        transform.position = startingCastPosition;
    }
}
