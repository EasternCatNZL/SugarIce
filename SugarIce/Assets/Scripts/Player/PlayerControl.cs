using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Speed of character movement multiplied against axis")]
    public float movementSpeed = 5.0f;
    [Tooltip("Turning speed of character")]
    public float turningSpeed = 10.0f;
    [Tooltip("Speed turn rate of character")]
    [Range(0, 1)]
    public float turnRate = 0.2f;
    [Tooltip("Amount of axis input required to ")]
    [Range(0, 1)]
    public float deadZone = 0.1f;

    [Header("Pickupable handling")]
    public float throwForce = 15.0f;
    public Transform launchTrajectory;
    public float upwardTrajectoryAngle = 30.0f;
    public float dropForce = 10.0f;
    public GameObject heldObject;
    public Transform heldObjectPos;

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

   
    private ActiveEquipment currentWorkStation = null;

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
        //If player is holding something
        if(playerState == PlayerState.Holding && heldObject)
        {
            heldObject.transform.position = heldObjectPos.position;
            heldObject.transform.rotation = heldObjectPos.rotation;
        }
        //If not in working state
        if(playerState != PlayerState.Working)
        {
            MovePlayer();
            Interact();
        }
        else if (playerState == PlayerState.Working)
        {
            Process();
        }
    }

    //Move the player using horizontal and vertical axis input from all sources
    void MovePlayer()
    {
        Vector3 newPos = Vector3.zero;
        //get input of thw two axis given that the input is larger than deadzone
        if (Luminosity.IO.InputManager.GetAxisRaw(horizontalAxis) > deadZone
            || Luminosity.IO.InputManager.GetAxisRaw(horizontalAxis) < -deadZone)
        {
            newPos += Vector3.right * movementSpeed * Luminosity.IO.InputManager.GetAxisRaw(horizontalAxis);
        }
        if (Luminosity.IO.InputManager.GetAxisRaw(verticalAxis) > deadZone
            || Luminosity.IO.InputManager.GetAxisRaw(verticalAxis) < -deadZone)
        {
            newPos += Vector3.forward * movementSpeed * Luminosity.IO.InputManager.GetAxisRaw(verticalAxis);
        }

        //if player was working at a station, move them off it
        if(playerState == PlayerState.Working && currentWorkStation)
        {
            playerState = PlayerState.Normal;
            currentWorkStation.DetachWorker();
            currentWorkStation = null;
        }

        //Rotate the player using the direction of movement
        Vector3 direction = newPos - transform.position;
        RotatePlayer(direction);

        transform.position += newPos * movementSpeed * Time.deltaTime;
        
    }

    //Rotate the player in the direction they are moving
    void RotatePlayer(Vector3 direction)
    {
        //Get direction based on input, only do if input recieved
        if(Luminosity.IO.InputManager.GetAxisRaw(horizontalAxis) > deadZone
            || Luminosity.IO.InputManager.GetAxisRaw(horizontalAxis) < -deadZone
            || Luminosity.IO.InputManager.GetAxisRaw(verticalAxis) > deadZone
            || Luminosity.IO.InputManager.GetAxisRaw(verticalAxis) < -deadZone)
        {
            float angle = Mathf.Atan2(Luminosity.IO.InputManager.GetAxisRaw(horizontalAxis), Luminosity.IO.InputManager.GetAxisRaw(verticalAxis));
            angle *= Mathf.Rad2Deg;
            Quaternion faceDirection = Quaternion.Euler(0.0f, angle, 0.0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, faceDirection, turningSpeed * Time.deltaTime);
        }
        
        
    }

    //Logic for when the interact button is pressed in different states
    void Interact()
    {
        if (Luminosity.IO.InputManager.GetButtonDown(interactButton))
        {
            switch (playerState)
            {
                case PlayerState.Normal:
                    PickUpInteraction();
                    break;
                case PlayerState.Holding:
                    PutDown();
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
                    AttachToWorkTable();
                    break;
                case PlayerState.Holding:
                    break;
                case PlayerState.Working:
                    WorkInteraction();
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
        //if found an object
        if (closestObject)
        {
            //Pick up from closest interactable -> table or loose object
            closestObject.GetComponent<Interactable>().PickUpFrom(this);
        }
        //If successfully got an item, change state
        if (heldObject)
        {
            playerState = PlayerState.Holding;
        }
    }

    //called when player uses interact button while holding onto an object
    void PutDown()
    {
        //play the throw animation
        //animator.SetTrigger(throwAnimation);
        //launch the object with given force, and set state to thrown
        Rigidbody heldObjectRigid = heldObject.GetComponent<Rigidbody>();
        //Vector3 launchDirection = Quaternion.AngleAxis(upwardTrajectoryAngle, transform.right) * transform.forward;
        //print(launchDirection);
        //heldObjectRigid.AddForce(launchTrajectory.forward * throwForce, ForceMode.Impulse);
        heldObjectRigid.velocity = launchTrajectory.forward * throwForce;
        //change state of object to thrown
        //ensure current holder cannot immediatly catch again
        heldObject.GetComponent<Pickupable>().isThrown = true;
        heldObject.GetComponent<Pickupable>().lastAttachedObject = this.gameObject;
        //Turn collider back on for object
        heldObject.GetComponent<Collider>().enabled = true;
        heldObject = null;
        //Set player back to normal state
        playerState = PlayerState.Normal;
    }

    //Get Item <- action when recieving an item from any source
    public void GetItem(GameObject item)
    {
        heldObject = item;
        //Turn the held objects collider off
        heldObject.GetComponent<Collider>().enabled = false;
        //play animation of picking up object
    }

    //Attach to equipment table
    void AttachToWorkTable()
    {
        //Call for interaction zone to find the closest workstation
        GameObject table = interactionZone.GetClosestWorkstation();
        //if table is found
        if (table)
        {
            table.GetComponent<ActiveEquipment>().AttachWorker(this);
            playerState = PlayerState.Working;
        }
    }

    //Attach player to a active equipment
    void WorkInteraction()
    {
        //if working at a workstation
        if(playerState == PlayerState.Working && currentWorkStation && currentWorkStation.GetComponent<ActiveEquipment>())
        {
            //if work key is held down, perform work
            if (Luminosity.IO.InputManager.GetButton(processButton))
            {
                currentWorkStation.GetComponent<ActiveEquipment>().Work();
            }
        }

    }

    //Detach from table
    private void DetachFromTable()
    {
        currentWorkStation.DetachWorker();
        currentWorkStation = null;
        //Change state off working
        playerState = PlayerState.Normal;
    }

    //Drop item when hit by another
    private void DropItem()
    {
        //launch the object with given force, and set state to thrown
        Rigidbody heldObjectRigid = heldObject.GetComponent<Rigidbody>();
        heldObjectRigid.AddForce(transform.up * dropForce, ForceMode.Impulse);
        //change state of object to thrown
        heldObject.GetComponent<Pickupable>().lastAttachedObject = this.gameObject;
        //Turn collider back on for object
        heldObject.GetComponent<Collider>().enabled = true;

        heldObject = null;

        //Change player state
        playerState = PlayerState.Normal;
    }

    //Reaction call when thrown object makes contact
    public void TryCatchItemInAir(GameObject item)
    {
        //Item made contact with player, switch off thrown
        item.GetComponent<Pickupable>().isThrown = false;
        //Check through states that the player could be to determine reaction
        //If working <- presume not holding anything
        if(playerState == PlayerState.Working)
        {
            DetachFromTable();
            //Item contact logic
            item.GetComponent<Pickupable>().NoCatchImpact();
        }
        else if(playerState == PlayerState.Holding)
        {
            DropItem();
            //Item contact logic
            item.GetComponent<Pickupable>().NoCatchImpact();
        }
        else if(playerState == PlayerState.Normal)
        {
            //Catch the item
            item.GetComponent<Pickupable>().PickUpFrom(this);
        }
    }
}
