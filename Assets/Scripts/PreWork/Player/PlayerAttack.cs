//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerAttack : MonoBehaviour
//{
//    [SerializeField] ParticleSystem[] glowyHands;
//    [SerializeField] Transform spellCastLocation;
//    PlayerStats playerStats;
//    PlayerTarget playerTarget;
//    EnemyStats enemyTarget;
//    PlayerMovement playerMovement;
//    PlayerUI playerUI;
//    float timeElapsed = 0f;
//    float castTime = 0f;
//    PlayerProjectile spellToCast;
//    [SerializeField] ErrorMessage errorMessage;
//    Animator animController;

//    bool casting = false;
//    bool succesfulCast = false;

//    // Start is called before the first frame update
//    void Start()
//    {
//        playerStats = GetComponent<PlayerStats>();
//        playerTarget = GetComponent<PlayerTarget>();
//        playerMovement = GetComponent<PlayerMovement>();
//        playerUI = GetComponent<PlayerUI>();
//        animController = GetComponentInChildren<Animator>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        GetInput();
//    }
//    void GetInput()
//    {
//        if (!casting)
//        {
//            if (Input.GetKeyDown(KeyCode.Alpha1))
//            {
//                ValidateButtonPress(1);
//            }
//            else if (Input.GetKeyDown(KeyCode.Alpha2))
//            {
//                ValidateButtonPress(2);
//            }
//            else if (Input.GetKeyDown(KeyCode.Alpha3))
//            {
//                ValidateButtonPress(3);
//            }
//        }
//        else
//        {
//            if (!playerMovement.IsPlayerMoving())
//            {
//                StartCasting();
//            }
//            else
//            { 
//                StopCasting();
//                errorMessage.SetMessageToScreen("Interrupted", 3f);
//            }
//        }
//    }

//    private void ValidateButtonPress(int buttonPress)
//    {
//        enemyTarget = playerTarget.GetPlayerTarget().GetComponent<EnemyStats>();
//        if (playerUI.GetAbilityCost(buttonPress) < playerStats.GetCurrentPlayerResource())
//        {
//            if (playerTarget.CheckTargetNotDead())
//            {
//                if (playerUI.GetAbilityInRange(buttonPress))
//                {
//                    if(playerUI.IsAbilityDoT(buttonPress))
//                    {
//                        CastDoTSpell(playerUI.GetAbility(buttonPress).GetComponent<DoT>(), playerUI.GetAbilityCastTime(buttonPress));
//                    }
//                    else
//                    {        
//                        casting = true;
//                        CastSpell(playerUI.GetAbility(buttonPress), playerUI.GetAbilityCastTime(buttonPress));
//                        playerUI.StartCast(buttonPress);
//                    }
//                }
//                else
//                {
//                    errorMessage.SetMessageToScreen("Out of range", 3f);
//                }
//            }
//            else
//            {
//                errorMessage.SetMessageToScreen("Invalid target", 3f);
//            }
//        }
//        else
//        {
//            errorMessage.SetMessageToScreen("Not enough mana", 3f);
//        }
//    }

//    void CastSpell(PlayerProjectile spellToCast, float castTime)
//    {
//        foreach (ParticleSystem glowy in glowyHands)
//        {
//            glowy.gameObject.SetActive(true);
//        }
//        animController.SetBool("castComplete", false);
//        animController.SetBool("castStopped", false);
//        animController.SetBool("casting", true);
//        this.castTime = castTime;
//        this.spellToCast = spellToCast;
//    }
//    void CastDoTSpell(DoT spellToCast, float castTime)
//    {
//        RaycastHit hit;
//        if (Physics.Linecast(transform.position, playerTarget.GetPlayerTarget().transform.position))
//        {
//            Physics.Linecast(transform.position, playerTarget.GetPlayerTarget().transform.position, out hit);
//            if (hit.transform.gameObject.layer != 6 && (Vector3.Angle(transform.forward, playerTarget.GetPlayerTarget().transform.position - transform.position) < 90))
//            {
//                DoT newDot = Instantiate(spellToCast);
//                newDot.SetEnemyForDoT(playerTarget.GetPlayerTarget().GetComponent<EnemyStats>());
//                if (playerTarget.GetPlayerTarget().GetComponent<EnemyStats>().AddDoTToList(newDot))
//                {
//                    newDot.transform.SetParent(playerTarget.GetPlayerTarget().transform);
//                    animController.SetBool("castComplete", true);
//                    StartCoroutine(DelaySetFalse("castComplete"));
//                }
//                else
//                    Destroy(newDot.gameObject);
//            }
//            else
//            {
//                errorMessage.SetMessageToScreen("Not in Line of Sight", 3f);
//            }
//        }
//    }
//    void StartCasting ()
//    {

//        timeElapsed += Time.deltaTime;
//        if(timeElapsed > castTime)
//        {
//            RaycastHit hit;
//            if(Physics.Linecast(transform.position, playerTarget.GetPlayerTarget().transform.position))
//            {
//                Physics.Linecast(transform.position, playerTarget.GetPlayerTarget().transform.position, out hit);
//                if(hit.transform.gameObject.layer != 6 && (Vector3.Angle(transform.forward , playerTarget.GetPlayerTarget().transform.position - transform.position) < 90))
//                {
//                    PlayerProjectile playerAbility = Instantiate(spellToCast, spellCastLocation.position, Quaternion.identity);
//                    playerAbility.SetProjectileTarget(enemyTarget);
//                    playerStats.SetCurrentPlayerResource(playerStats.GetCurrentPlayerResource() - playerAbility.GetAbilityCost());
//                    animController.SetBool("castComplete", true);
//                    StartCoroutine(DelaySetFalse("castComplete"));
//                    succesfulCast = true;
//                }
//                else
//                {
//                    errorMessage.SetMessageToScreen("Not in Line of Sight", 3f);
//                }
//            }           
//            StopCasting();
//        }
//    }
//    void StopCasting()
//    {
//        foreach (ParticleSystem glowy in glowyHands)
//        {
//            glowy.gameObject.SetActive(false);
//        }
//        if (!succesfulCast)
//            animController.SetBool("castStopped", true);
//        animController.SetBool("casting", false);
//        casting = false;
//        castTime = 0;
//        timeElapsed = 0;
//        spellToCast = null;
//        succesfulCast = false;
//    }
//    public bool GetCasting()
//    {
//        return casting;
//    }
//    IEnumerator DelaySetFalse(string animNameToDelay)
//    {
//        yield return new WaitForSeconds(.05f);
//        animController.SetBool(animNameToDelay, false);
//    }
//}
