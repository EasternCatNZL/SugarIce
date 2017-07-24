using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableBehaviour : MonoBehaviour {

    [Header("Item On Table Transform")]
    public Transform itemOnTablePos; //position on table where objects would sit

    private GameObject itemOnTable; //the chemical object that is currently on the table

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //return the item that table is currently holding
    public GameObject GetItemOnTable()
    {
        return itemOnTable;
    }

    //Check the item can be placed on the table
    public bool ValidTable(GameObject _Item)
    {
        //Check there is no table and that the item is valid with the tool
        if(!itemOnTable && GetComponent<Tools>().ValidItem(_Item))
        {
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
