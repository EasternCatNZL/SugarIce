using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : Interactable
{
    public enum MaterialType
    {
        Liquid,
        Dough,
        Stone
    }

    [Header("Handling Needs")]
    public float amountToProcess = 2.5f;
    [Tooltip("True if item will explode when improperly handled")]
    public bool isExplosive = false;
    [Tooltip("True if item will break when improperly handled")]
    public bool isFragile = false;
    [HideInInspector]
    public bool canBeProcessed = true;

    [Header("Can change into")]
    public List<GameObject> processedFormList = new List<GameObject>();

    [HideInInspector]
    public Table attachedTable = null;

    private float processedAmount = 0.0f;

    

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
    }

    public void WorkItem(float workAmount, Equipment.ProcessMethod processMethod)
    {
        //add work amount to items processed amount
        processedAmount += workAmount;
        //if processed amount has reached required threshold, complete it
        if (processedAmount >= amountToProcess)
        {
            ChangedToProcessedForm();
        }
    }

    protected void ChangedToProcessedForm()
    {

    }
}
