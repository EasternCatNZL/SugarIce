using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveEquipment : Equipment
{
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

    public override void ObjectPlacedReaction()
    {
        base.ObjectPlacedReaction();
        timeItemWasPlaced = Time.time;
    }
}
