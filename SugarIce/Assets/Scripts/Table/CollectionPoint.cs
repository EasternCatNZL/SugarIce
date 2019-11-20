using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionPoint : Table
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void AttachToTable()
    {
        base.AttachToTable();
        CompleteOrder();
    }

    //Consumes item, and if order for item existed, complete the order
    private void CompleteOrder()
    {

    }
}
