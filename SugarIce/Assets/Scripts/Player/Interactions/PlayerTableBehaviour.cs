using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerTableBehaviour : MonoBehaviour {

    //script refs
    PlayerStateControl playerState; //ref to player script refs
    InteractionZoneBehaviour interactionZone; //ref to game object that handles detection of objects for interaction

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
		
	}
}
