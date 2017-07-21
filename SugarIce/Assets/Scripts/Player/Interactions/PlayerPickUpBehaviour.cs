using System.Collections;
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
    PlayerIndex playerID;
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
	}

    //when pickup button pressed, do things based on player state
    void PickUpInteract()
    {
        //if input recieved
        state = GamePad.GetState(playerID);
        prevState = state;

        if (state.Buttons.A == ButtonState.Pressed)
        {
            //DEBUG
            print("Recieved pickup input from " + playerID.ToString());
            //check if currently holding something
            if (playerState.isHolding)
            {
                //if table exists, interact with table

                //else, drop the item
            }
            //else try to pick up an object
            else
            {
                //check for an object to pick up, if null, do nothing
                if (interactionZone.GetClosestInteractable())
                {
                    heldObject = interactionZone.GetClosestInteractable();
                }
            }
        }
    }
}
