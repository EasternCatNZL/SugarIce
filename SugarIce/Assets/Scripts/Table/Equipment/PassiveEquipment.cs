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
        
    }

    public override void AttachToTable(GameObject item)
    {
        base.AttachToTable(item);
        timeItemWasPlaced = Time.time;
    }
}
