using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(TableStateControl))]
[RequireComponent(typeof(Tools))]

public class TableBehaviour : MonoBehaviour {

    //[Header("Item On Table Transform")]
    public Transform itemOnTablePos; //position on table where objects would sit

    public GameObject itemOnTable; //the chemical object that is currently on the table

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //return the item that table is currently holding
    public GameObject GetItemOnTable()
    {      
        //Its just a table take what you want
        if (GetComponent<Tools>().Tool == Tools.ToolTypes.NONE)
        {
            itemOnTable.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GameObject temp = itemOnTable;
            RemoveItemOnTable();
            GetComponent<TableStateControl>().hasItem = false;
            return temp;
        }
        //Remove item from Table
        if (GetComponent<Tools>().Tool != Tools.ToolTypes.DRUGS && GetComponent<Tools>().Tool != Tools.ToolTypes.DONUTS)
        {
            RemoveItemOnTable();
            GetComponent<TableStateControl>().hasItem = false;
        }
        //Get output item from the tool
        if(GetComponent<Tools>().Tool != Tools.ToolTypes.NONE)
        {
            return GetComponent<Tools>().GetItemInTool();
        }
        return null;
    }

    //Check the item can be placed on the table
    public bool ValidTable(GameObject _Item)
    {
        //Check there is no item on the table and that the item is valid with the tool
        if(!itemOnTable && GetComponent<Tools>().ValidItem(_Item))
        {
            return true;
        }
        return false;
    }

    //gives item to table
    public void SetItemOnTable(GameObject item)
    {
        if (GetComponent<Tools>().Tool != Tools.ToolTypes.TUBES && GetComponent<Tools>().Tool != Tools.ToolTypes.BIN && GetComponent<Tools>().Tool != Tools.ToolTypes.DROPOFF)
        {
            GetComponent<TableStateControl>().hasItem = true;
            itemOnTable = item;
            itemOnTable.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //move item to set transform on table
            item.transform.position = itemOnTablePos.position;
            item.transform.rotation = itemOnTablePos.rotation;
        }
        else
        {
            Destroy(item);         
            RemoveItemOnTable();        
        }
    }

    //sets item on table to null
    public void RemoveItemOnTable()
    {
        itemOnTable = null;
    }
}
