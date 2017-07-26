using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableStateControl : MonoBehaviour {

    //determines what interactions are possible with table
    enum TableType
    {
        Normal,
        Mix,
        Heat
    }

    //item storage and state control of table
    //[HideInInspector]
    public bool hasItem = false; //checks whether any item is on table

    
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    
}
