using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractionZoneBehaviour))]

public class PlayerControl : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Speed of character movement multiplied against axis")]
    public float movementSpeed = 5.0f;
    [Tooltip("Speed turn rate of character")]
    [Range(0, 1)]
    public float turnRate = 0.2f;
    [Tooltip("Amount of axis input required to ")]
    [Range(0, 1)]
    public float deadZone = 0.1f;

    [Header("Pickupable handling")]
    public float throwForce = 2.0f;

    [Header("Input")]
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public string interactButton = "Interact";
    public string processButton = "Process";

    [Header("Animation")]
    public string throwAnimation = "Throw";
    private Animator animator;

    [Header("Script refs")]
    [Tooltip("Game object that handles detection of objects for interaction")]
    public InteractionZoneBehaviour interactionZone;

    private GameObject heldObject;

    public enum PlayerState
    {
        Normal,
        Holding,
        Working,
        Down
    }
    [Header("Player state")]
    public PlayerState playerState = PlayerState.Normal;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Animator>())
        {
            animator = GetComponent<Animator>();
        }
        else
        {
            Debug.LogError(gameObject.name + " does not have an animator attached");
        }
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        RotatePlayer();
    }

    //Move the player using horizontal and vertical axis input from all sources
    void MovePlayer()
    {
        Vector3 newPos = Vector3.zero;
        //get input of thw two axis given that the input is larger than deadzone
        if (Luminosity.IO.InputManager.GetAxisRaw(horizontalAxis) > deadZone)
        {
            newPos += Vector3.right * movementSpeed * Luminosity.IO.InputManager.GetAxisRaw(horizontalAxis);
        }
        if (Luminosity.IO.InputManager.GetAxisRaw(verticalAxis) > deadZone)
        {
            newPos += Vector3.forward * movementSpeed * Luminosity.IO.InputManager.GetAxisRaw(verticalAxis);
        }

        transform.position += newPos * movementSpeed * Time.deltaTime;
    }

    //Rotate the player in the direction they are moving
    void RotatePlayer()
    {
        //Get direction based on input
        Vector3 direction = new Vector3(Luminosity.IO.InputManager.GetAxisRaw(horizontalAxis), 0.0f, Luminosity.IO.InputManager.GetAxisRaw(verticalAxis));
        float angle = Mathf.Atan2(Luminosity.IO.InputManager.GetAxisRaw(horizontalAxis), Luminosity.IO.InputManager.GetAxisRaw(verticalAxis));
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    //Logic for when the interact button is pressed in different states
    void Interact()
    {
        if (Luminosity.IO.InputManager.GetButtonDown(interactButton))
        {
            switch (playerState)
            {
                case PlayerState.Normal:
                    break;
                case PlayerState.Holding:
                    break;
                case PlayerState.Working:
                    break;
                default:
                    break;
            }
        }
    }

    //Logic for when process button is pressed in different states
    void Process()
    {
        if (Luminosity.IO.InputManager.GetButtonDown(processButton))
        {
            switch (playerState)
            {
                case PlayerState.Normal:
                    break;
                case PlayerState.Holding:
                    break;
                case PlayerState.Working:
                    break;
                default:
                    break;
            }
        }
    }

    //called when player uses interact button 
    void PickUpInteraction()
    {
        //Calls for interaction zone to attempt to find an object to attach itself to player character
        GameObject closestObject = interactionZone.GetClosestInteractable();
        //Pick up from closest interactable -> table or loose object
        closestObject.GetComponent<Interactable>().PickUpFrom(this);
    }

    //called when player uses interact button while holding onto an object
    void PutDown()
    {
        //play the throw animation
        animator.SetTrigger(throwAnimation);
        //launch the object with given force, and set state to thrown
        Rigidbody heldObjectRigid = heldObject.GetComponent<Rigidbody>();
        heldObjectRigid.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        //change state of object to thrown

        heldObject = null;
    }

    //Get Item <- action when recieving an item from any source
    public void GetItem(GameObject item)
    {
        heldObject = item;
        //play animation of picking up object
    }

    //Attach player to a active equipment
    void WorkInteraction()
    {
        //get the closest active equipment table

    }
}
