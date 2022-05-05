using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UPlayerInput : MonoBehaviour
{
    MageClass player;
    UPlayerUI playerUI;
    PlayerMovement movement;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<MageClass>();
        playerUI = FindObjectOfType<UPlayerUI>();
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerInputHotBar();
        GetPlayerInputUtility();
    }
    private void FixedUpdate()
    {
        HandlePhysicsInput();
    }

    private void GetPlayerInputUtility()
    {
        if (Input.GetKeyDown(KeyCode.P))
            playerUI.ToggleSpellBookDisplay();
        else if (Input.GetKeyDown(KeyCode.I))
            playerUI.ToggleInventoryDisplay();
    }

    private void GetPlayerInputHotBar()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.GetAbilityFromSpellBook(playerUI.GetAbilityIDFromKeyPress(1));
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            player.GetAbilityFromSpellBook(playerUI.GetAbilityIDFromKeyPress(2));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            player.GetAbilityFromSpellBook(playerUI.GetAbilityIDFromKeyPress(3));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            player.GetAbilityFromSpellBook(playerUI.GetAbilityIDFromKeyPress(4));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            player.GetAbilityFromSpellBook(playerUI.GetAbilityIDFromKeyPress(5));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            player.GetAbilityFromSpellBook(playerUI.GetAbilityIDFromKeyPress(6));
        }
    }

    void HandlePhysicsInput()
    {
        Vector3 finalMovement = Vector3.zero;
        // Movement
        if(Input.GetKey(KeyCode.W))
        {
            finalMovement += transform.forward;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            finalMovement = transform.forward * 0.5f;
        }
        if (Input.GetMouseButton(1))
        {
            if (Input.GetKey(KeyCode.D))
            {
                finalMovement += transform.right;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                finalMovement -= transform.right;
            }
        }
        finalMovement.Normalize();
        //movement.HandlePlayerInput(finalMovement);
    }
}
