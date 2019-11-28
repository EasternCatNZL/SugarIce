using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : Interactable
{
    public enum MaterialType
    {
        Liquid,
        Dough,
        Stone,
        Powder
    }

    [Header("Handling Needs")]
    public float amountToProcess = 2.5f;
    public float processedAmount = 0.0f;
    [Tooltip("True if item will explode when improperly handled")]
    public bool isExplosive = false;
    [Tooltip("True if item will break when improperly handled")]
    public bool isFragile = false;
    [Tooltip("Is being thrown and in catchable state")]
    public bool isThrown = false;
    [HideInInspector]
    public bool canBeProcessed = true;

    [Header("Material Type")]
    public MaterialType matType = MaterialType.Dough;
    [Header("Can be processed via")]
    public List<Equipment.ProcessMethod> myProcessMethods = new List<Equipment.ProcessMethod>();
    [Header("Can change into")]
    public List<GameObject> processedFormList = new List<GameObject>();

    [Header("Physics vars")]
    public float bounceOffReduction = 4.0f;

    [HideInInspector]
    public Table attachedTable = null;
    [HideInInspector]
    public GameObject lastAttachedObject = null; //For not reattaching to something after throwing

    

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PickUpFrom(PlayerControl player)
    {
        player.GetItem(this.gameObject);
        lastAttachedObject = player.gameObject;
    }

    public void WorkItem(float workAmount, Equipment.ProcessMethod processMethod)
    {
        //add work amount to items processed amount
        processedAmount += workAmount;
        //if processed amount has reached required threshold, complete it
        if (processedAmount >= amountToProcess)
        {
            ChangedToProcessedForm(processMethod);
        }
    }

    //change the object after equipment work has occured that would cause it to change form
    protected virtual void ChangedToProcessedForm(Equipment.ProcessMethod processMethod)
    {

    }

    //Impact logic when bouncing off player when player cant catch the item thrown
    public virtual void NoCatchImpact()
    {
        //Set the item to bounce off hit target with reverse horizontal directino + upward force
        Vector3 currentForce = GetComponent<Rigidbody>().velocity;
        //zero out force and apply new one
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        Vector3 bounceOffForce = new Vector3(-1 * (currentForce.x / bounceOffReduction), 1.0f, -1 *(currentForce.z / bounceOffReduction));
        GetComponent<Rigidbody>().AddForce(bounceOffForce, ForceMode.Impulse);
    }

    //Do collision detection when thrown from player
    protected void OnCollisionEnter(Collision collision)
    {
        //Only do this check when item launched by player
        if (isThrown)
        {
            //When making impact with an other thing, only try do things if other is player or table
            if (collision.gameObject.GetComponent<PlayerControl>())
            {
                collision.gameObject.GetComponent<PlayerControl>().TryCatchItemInAir(this.gameObject);
            }
            else if (collision.gameObject.GetComponent<Table>())
            {
                collision.gameObject.GetComponent<Table>().AttachToTable(this.gameObject);
            }
            //else If item reaches floor

        }
        
    }
}
