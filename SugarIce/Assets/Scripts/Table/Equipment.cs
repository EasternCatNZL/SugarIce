using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Table
{
    public enum ProcessMethod
    {
        Heat,
        Bake,
        Mix,
        Break,
        Cut,
        Chill
    }

    [Header("Processing vars")]
    public float processRate = 0.2f;

    [Header("Processing method")]
    public ProcessMethod myProcessMethod = ProcessMethod.Heat;

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
        //Hide the object
    }

    public override void InteractWith()
    {
        if (itemOnTable)
        {
            Work();
        }        
    }

    public virtual void Work()
    {

    }

    public override void PickUpFrom(PlayerControl player)
    {
        //show  the object

        base.PickUpFrom(player);
    }
}
