using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UPlayerTargetting : MonoBehaviour
{
    [SerializeField] GameObject playerTarget = null;
    Camera mainCam;
    UPlayerUI myUI;
    Animator playerAnim;
    ErrorMessage errorMessage;
    CurrentCursorSprite cursorSprite;

    // Start is called before the first frame update
    void Start()
    {
        cursorSprite = FindObjectOfType<CurrentCursorSprite>();
        mainCam = FindObjectOfType<Camera>();
        myUI = FindObjectOfType<UPlayerUI>();
        playerAnim = GetComponentInChildren<Animator>();
        errorMessage = GetComponent<UseAbility>().GetCurrentErrorMessagePrompt();
    }

    // Update is called once per frame
    void Update()
    {
        
        HoverCheck();
        // Send out Raycast when you left click
        if(Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                StartLeftClickSequence();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                StartRightClickSequence();
        }

        // More or less if you are targetting something that gets destroyed, clear it from the current target
        if (playerTarget == null)
        {
            if (myUI.DoesPlayerHaveTarget())
                myUI.UI_ClearTargetFrame();
        }
    }

    // =============================================================
    // =============== LEFT CLICK LOGIC BEGINS HERE ================
    // =============================================================
    private void StartLeftClickSequence()
    {
        // Send out a ray to the current mouse position
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // If the ray succesfully hits something, store it in 'hit' above
        if (Physics.Raycast(ray, out hit))
        {
            StartSuccesfulLeftClick(hit);
        }
        else if (playerTarget)
            LoseTarget();
    }
    void StartSuccesfulLeftClick(RaycastHit hit)
    {
        // Check to see if our target has a parent AND that its parent is targettable
        if (hit.transform.parent && hit.transform.parent.GetComponent<IsTargettable>())
        {
            CheckIfLeftClickTargetIsCurrentTarget(hit);
        }
        // The raycast hit a non targettable object
        else
        {
            LoseTarget();
        }
    }
    void CheckIfLeftClickTargetIsCurrentTarget(RaycastHit hit)
    {
        // This check will see if the target the user clicks is the same or a new target, if its a new target, set the current target to not being targetted
        if (playerTarget != null && hit.transform != playerTarget.transform)
        {
            if (playerTarget.GetComponent<BaseEnemyStats>())
                playerTarget.GetComponent<BaseEnemyStats>().SetCurrentlyTargetted(false);
            else
                playerTarget.GetComponent<BaseFriendlyStats>().SetCurrentlyTargetted(false);
        }

        // Set our player target to the newly hit gameObject
        playerTarget = hit.transform.parent.gameObject;

        // Set our friendly target to being currently targetting and update our UI
        if (playerTarget.GetComponent<BaseFriendlyStats>())
        {
            playerTarget.GetComponent<BaseFriendlyStats>().SetCurrentlyTargetted(true);
            myUI.UI_SetPlayerTarget(playerTarget);
        }
        // Set our enemy target to being currently targetting and update our UI
        else if (playerTarget.GetComponent<BaseEnemyStats>())
        {
            playerTarget.GetComponent<BaseEnemyStats>().SetCurrentlyTargetted(true);
            myUI.UI_SetPlayerTarget(playerTarget);
        }
    }

    // =============================================================
    // =============== RIGHT CLICK LOGIC BEGINS HERE ===============
    // =============================================================
   
    private void StartRightClickSequence()
    {
        // Send out a ray to the current mouse position
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // If the ray succesfully hits something, store it in 'hit' above
        if (Physics.Raycast(ray, out hit))
        {
            StartSuccesfulRightClick(hit);
        }
    }

    private void StartSuccesfulRightClick(RaycastHit hit)
    {
        if (hit.transform.parent && hit.transform.parent.GetComponent<IsInteractable>())
        {
            CheckIfRightClickTargetIsCurrentTarget(hit);
        }
    }

    void CheckIfRightClickTargetIsCurrentTarget(RaycastHit hit)
    {
        // This check will see if the target the user clicks is the same or a new target, if its a new target, set the current target to not being targetted
        if (playerTarget != null && hit.transform != playerTarget.transform)
        {
            if (playerTarget.GetComponent<BaseEnemyStats>())
                playerTarget.GetComponent<BaseEnemyStats>().SetCurrentlyTargetted(false);
            else
                playerTarget.GetComponent<BaseFriendlyStats>().SetCurrentlyTargetted(false);
        }

        // Set our player target to the newly hit gameObject
        playerTarget = hit.transform.parent.gameObject;

        // Set our friendly target to being currently targetting and update our UI
        if (playerTarget.GetComponent<BaseFriendlyStats>())
        {
            playerTarget.GetComponent<BaseFriendlyStats>().SetCurrentlyTargetted(true);
            myUI.UI_SetPlayerTarget(playerTarget);
            RightClickLogicFlowFriendly();
        }
        // Set our enemy target to being currently targetting and update our UI
        else if (playerTarget.GetComponent<BaseEnemyStats>())
        {
            playerTarget.GetComponent<BaseEnemyStats>().SetCurrentlyTargetted(true);
            myUI.UI_SetPlayerTarget(playerTarget);
            RightClickLogicFlowEnemy();
        }
    }

    void RightClickLogicFlowEnemy()
    {
        if (playerTarget.GetComponent<BaseEnemyStats>().GetCurrentEnemyHealth() > 0)
            playerAnim.SetBool("rightClickIdle", true);
        else
            playerTarget.GetComponent<EnemyLootTable>().OpenLootWindow();
            
    }

    void RightClickLogicFlowFriendly()
    {
        if (playerTarget.GetComponent<NPCDialogueManager>() && !playerTarget.GetComponent<NPCDialogueManager>().CheckInRangeToTalk(transform))
            errorMessage.SetMessageToScreen("Not close enough to interact", 3f);
        playerAnim.SetBool("rightClickIdle", false);
    }
    void HoverCheck()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        
        RaycastHit hit;

        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        if (Physics.Raycast(ray, out hit))
        {
            if(hit.collider != null && hit.transform.root.GetComponent<UI_HoverInformation>() || (hit.transform.parent != null && hit.transform.parent.GetComponent<UI_HoverInformation>()))
            {
                myUI.ActivateHover(hit.transform.parent.GetComponent<UI_HoverInformation>());
                DetermineCursorStatus(hit.transform.parent);
                return;
            }
            else if (raycastResults.Count > 0)
            {
                foreach (var go in raycastResults)
                {
                    if (go.gameObject.transform.GetComponent<UI_HoverInformation>())
                    {
                        myUI.ActivateHover(go.gameObject.transform.GetComponent<UI_HoverInformation>());
                        return;
                    }
                }   
            }
            myUI.ClearHover();
            cursorSprite.SetCursorToDefault();
        }
    }
    void DetermineCursorStatus(Transform target)
    {
        
        if (target.GetComponent<BaseEnemyStats>())
        {
            if (target.GetComponent<BaseEnemyStats>().GetCurrentEnemyHealth() > 0)
                cursorSprite.SetCursorToAttackableEnemy();
            else
                cursorSprite.SetCursorToLootable();
        }
        else if (target.GetComponent<BaseFriendlyStats>())
        {
            if (target.GetComponent<NPCQuestManager>() && target.GetComponent<NPCQuestManager>().GetQuestsFromNPC().Count > 0)
                cursorSprite.SetCursorToQuestGiver();
            else if (target.GetComponent<NPCDialogueManager>() && target.GetComponent<NPCDialogueManager>().GetDialogues().Length > 0)
                cursorSprite.SetCursorToFriendly();
        }
        else
            cursorSprite.SetCursorToDefault();
    }
    private void LoseTarget()
    {
        // Lose the target of anything you were targetting
        if (playerTarget != null)
        {
            if (playerTarget.GetComponent<BaseEnemyStats>())
                playerTarget.GetComponent<BaseEnemyStats>().SetCurrentlyTargetted(false);
            else
                playerTarget.GetComponent<BaseFriendlyStats>().SetCurrentlyTargetted(false);
        }
        playerTarget = null;
        myUI.UI_ClearTargetFrame();
        playerAnim.SetBool("rightClickIdle", false);
    }

    public GameObject PT_GetCurrentTarget() { return playerTarget; }

    
}
