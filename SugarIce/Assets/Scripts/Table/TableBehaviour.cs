using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            RemoveItemOnTable();
            return true;
        }
        return false;
    }

    //gives item to table
    public void SetItemOnTable(GameObject item)
    {
        itemOnTable = item;
        //move item to set transform on table
        item.transform.position = itemOnTablePos.position;
        item.transform.rotation = itemOnTablePos.rotation;
    }

    //sets item on table to null
    public void RemoveItemOnTable()
    {
        itemOnTable = null;
    }
}
