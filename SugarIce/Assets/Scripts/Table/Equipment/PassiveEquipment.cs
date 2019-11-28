using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveEquipment : Equipment
{
    [Header("Usage vars")]
    public bool inUse = false;

    //control vars
    protected float timeItemWasPlaced = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Work as long as item is on table
        Work();
    }

    public override void AttachToTable(GameObject item)
    {
        base.AttachToTable(item);
        timeItemWasPlaced = Time.time;
    }

    public override void Work()
    {
        base.Work();

        //if the object exists, and can be worked, work it
        if (itemOnTable)
        {
            inUse = true;
            if (itemOnTable.GetComponent<Pickupable>() && itemOnTable.GetComponent<Pickupable>().canBeProcessed)
            {
                itemOnTable.GetComponent<Pickupable>().WorkItem(processRate, myProcessMethod);
            }
        }
        else
        {
            inUse = false;
        }
    }
}
