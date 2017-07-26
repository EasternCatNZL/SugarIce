using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderBehaviour : MonoBehaviour {

    //mixture array
    //private ChemicalBehaviour[] mixture = new ChemicalBehaviour[0];

<<<<<<< HEAD
    //private ChemicalBehaviour[][] orders = new ChemicalBehaviour[0][0];
=======
    [Header("Orders for level")]
    List<OrderItem> currentOrders = new List<OrderItem>();
    public OrderItem[] possibleOrders = new OrderItem[0];
    //public OrderItem[] currentOrders = new OrderItem[0];
>>>>>>> 850d803b1fa4d1492b5bc98e6896d992f45a74ee

    [Header("Order functionality control")]
    public float orderIntervalMinimum = 20.0f; //minimum time that has to pass before next order arrives
    public float orderIntervalMaximum = 25.0f; //maximum time in which next order must arrive when met

    private float timeLastOrder = 0.0f; //time of last order

    // Use this for initialization

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //creates a new order and adds it to the orders queue
    private void CreateOrder()
    {
        //get random order from array of possible orders
        int rand = Random.Range(0, possibleOrders.Length);
        //copy the order from possible orders into current orders
        currentOrders.Add(possibleOrders[rand]);
    }

    //remove an object from the queue
    private void RemoveOrder(OrderItem itemToRemove)
    {
        if (currentOrders.Find(itemToRemove)){

        }
    }
}
