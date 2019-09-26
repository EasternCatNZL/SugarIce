using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : ActiveEquipment
{
    [Header("Processing vars")]
    public float processRate = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Work()
    {
        //base.Work();

        //if the object exists, and can be worked, work it
        if (itemOnTable)
        {
            if (itemOnTable.GetComponent<Pickupable>() && itemOnTable.GetComponent<Pickupable>().canBeProcessed)
            {
                itemOnTable.GetComponent<Pickupable>().WorkItem(processRate, myProcessMethod);
            }
        }
    }
}
