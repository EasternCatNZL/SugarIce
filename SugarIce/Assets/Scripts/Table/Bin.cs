using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin : Table
{
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
        BinItem(itemOnTable);
    }

    void BinItem(GameObject item)
    {
        //Remove all physics acting on the object
        itemOnTable.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //Set to null
        itemOnTable = null;
        //Any effects or animations that need to play as item drops
    }
}
