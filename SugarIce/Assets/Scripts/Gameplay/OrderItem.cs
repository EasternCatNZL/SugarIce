using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderItem : MonoBehaviour {

    [HideInInspector]
    public enum Actions
    {
        PourBlue,
        PourOrange,
        Heat,
        Mix
    }

    [Header("Order")]
    public Actions[] order; //set of actions that make up an order



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
