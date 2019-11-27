using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : Interactable
{
    [Header("Object refs")]
    public GameObject itemOnTable;
    //public Vector3 posOnTable = new Vector3(0.0f, 0.5f, 0.0f);
    public Transform onTableTransform;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Logic when item is placed onto this table
    public virtual void AttachToTable(GameObject item)
    {
        //Turn of thrown check
        item.GetComponent<Pickupable>().isThrown = false;
        itemOnTable = item;
        PositionOnTalbe();
        itemOnTable.GetComponent<Pickupable>().attachedTable = this;
        item.GetComponent<Pickupable>().lastAttachedObject = this.gameObject;
    }

    //Position the object on the table
    protected void PositionOnTalbe()
    {
        itemOnTable.transform.position = onTableTransform.position;
        //remove physics from it
        itemOnTable.GetComponent<Rigidbody>().velocity = Vector3.zero;
        itemOnTable.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
    }

    //When table is interacted with
    public virtual void InteractWith()
    {

    }

    //Logic when trying to pick up from
    public override void PickUpFrom(PlayerControl player)
    {
        if (itemOnTable)
        {
            //Set physics on for object
            itemOnTable.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            //give object to player
            player.GetItem(itemOnTable);
            itemOnTable.GetComponent<Pickupable>().attachedTable = null;
            //remove item from table
            itemOnTable = null;
        }
            
    }
}
