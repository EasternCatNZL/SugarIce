﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerPickUpBehaviour : MonoBehaviour {

    //script refs
    PlayerStateControl playerState; //ref to player script refs
    InteractionZoneBehaviour interactionZone; //ref to game object that handles detection of objects for interaction

    //object ref
    GameObject heldObject = null;

    //hold object transform
    public Transform heldObjectPos;

    //xinput stuff
    private PlayerIndex playerID;
    private GamePadState state;
    private GamePadState prevState;

    // Use this for initialization
    void Start () {

        if (GetComponent<PlayerStateControl>())
        {
            playerState = GetComponent<PlayerStateControl>();
        }
        else
        {
            Debug.LogError("Player state does not exist");
        }

        if (GetComponentInChildren<InteractionZoneBehaviour>())
        {
            interactionZone = GetComponentInChildren<InteractionZoneBehaviour>();
        }
        else
        {
            Debug.LogError("Player does not have interaction behaviour attached");
        }

        playerID = GetComponent<PlayerMovement>().PlayerID;
	}
	
	// Update is called once per frame
	void Update () {
		//if holding object, update it's transform based on held object pos
        if (heldObject)
        {
            heldObject.transform.position = heldObjectPos.position;
            heldObject.transform.rotation = heldObjectPos.rotation;
        }

        PickUpInteract();
	}

    //when pickup button pressed, do things based on player state
    void PickUpInteract()
    {
        //if input recieved
        prevState = state;
        state = GamePad.GetState(playerID);

        if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
        {
            //DEBUG
            print("Recieved pickup input from " + playerID.ToString());
            //check if currently holding something
            if (playerState.isHolding)
            {
                
                //if table exists, interact with table
                if (interactionZone.GetClosestTable())
                {
                    //give item to the table
                    interactionZone.GetClosestTable().GetComponent<TableBehaviour>().SetItemOnTable(heldObject);
                    interactionZone.GetClosestTable().GetComponent<TableStateControl>().hasItem = true;
                    //player no longer holding anything, set to null
                    heldObject = null;
                    playerState.isHolding = false;
                }
                //else, drop the item
                else
                {
                    //set player held to null
                    heldObject = null;
                    playerState.isHolding = false;
                }
            }
            //else try to pick up an object
            else
            {
                //if table exists, check to see if item to pickup from table exists first
                if (interactionZone.GetClosestTable())
                {
                    //check if table has object
                    if (interactionZone.GetClosestTable().GetComponent<TableStateControl>().hasItem)
                    {
                        //set held object to this object
                        heldObject = interactionZone.GetClosestTable().GetComponent<TableBehaviour>().GetItemOnTable();
                        playerState.isHolding = true;
                        //set table object to null
                        interactionZone.GetClosestTable().GetComponent<TableBehaviour>().RemoveItemOnTable();
                        interactionZone.GetClosestTable().GetComponent<TableStateControl>().hasItem = false;
                    }
                    //if table does not have item, try to pick up off floor
                    else if (interactionZone.GetClosestInteractable())
                    {
                        heldObject = interactionZone.GetClosestInteractable();
                        playerState.isHolding = true;
                    }
                }
                //check for an object to pick up, if null, do nothing
                else if (interactionZone.GetClosestInteractable())
                {
                    heldObject = interactionZone.GetClosestInteractable();
                    playerState.isHolding = true;
                }
            }
        }
    }
}
