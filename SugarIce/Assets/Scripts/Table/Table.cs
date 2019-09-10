using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [Header("Object refs")]
    public GameObject itemOnTable;
    public Vector3 posOnTable = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Logic when item is placed onto this table
    public virtual void ObjectPlacedReaction()
    {
        PositionOnTalbe();
    }

    //Position the object on the table
    protected void PositionOnTalbe()
    {
        itemOnTable.transform.position = posOnTable;
        //remove physics from it
        itemOnTable.GetComponent<Rigidbody>().velocity = Vector3.zero;
        itemOnTable.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
    }

    //When table is interacted with
    public virtual void InteractWith()
    {

    }

    //Logic when trying to pick up from
    public virtual void PickUpFrom()
    {
        //Set physics on for object
        itemOnTable.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        //remove item from table
        itemOnTable = null;
    }
}
