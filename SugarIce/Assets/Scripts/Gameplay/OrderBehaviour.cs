using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderBehaviour : MonoBehaviour {

    //mixture array
    //private ChemicalBehaviour[] mixture = new ChemicalBehaviour[0];


    //private ChemicalBehaviour[][] orders = new ChemicalBehaviour[0][0];

    [Header("Orders for level")]
    List<OrderItem> currentOrders = new List<OrderItem>();
    public OrderItem[] possibleOrders = new OrderItem[0];
    //public OrderItem[] currentOrders = new OrderItem[0];


    [Header("Order functionality control")]
    public float orderIntervalMinimum = 20.0f; //minimum time that has to pass before next order arrives
    public float orderIntervalMaximum = 25.0f; //maximum time in which next order must arrive when met

    private float timeLastOrder = 0.0f; //time of last order

    [Header("Order Positioning and Movement")]
    public RectTransform[] orderPositions = new RectTransform[0];
    public float moveSpeed = 25.0f; //speed orders shift positions

    [Header("Canvas ref")]
    public Canvas canvas;

    // Use this for initialization

    void Start () {
        //when level starts, create first order
        CreateOrder();
	}
	
	// Update is called once per frame
	void Update () {
		//when interval passes
	}

    //creates a new order and adds it to the orders queue
    private void CreateOrder()
    {
        //get random order from array of possible orders
        int rand = Random.Range(0, possibleOrders.Length);
        //instantiate new order item, copy the order from possible orders into current orders
        OrderItem newOrder = Instantiate(possibleOrders[rand], transform.position, transform.rotation) as OrderItem;
        newOrder.transform.SetParent(canvas.transform);
        //copy the order from possible orders into current orders
        //newOrder = possibleOrders[rand];

        //set time of order item creation
        newOrder.timeStart = Time.time;
        //add to current orders
        currentOrders.Add(possibleOrders[rand]);
    }

    //remove an object from the queue
    private void RemoveOrder(OrderItem itemToRemove)
    {

        ////search through the current orders that exist
        //foreach(OrderItem item in currentOrders)
        //{
        //    //if it exists
        //    if (item == itemToRemove)
        //    {
        //        //remove the item
        //        currentOrders.Remove(item);
        //        //break out of the loop
        //        break;
        //    }
        //}

        //if item exists in list
        if (currentOrders.Contains(itemToRemove))
        {
            //remove from the list
            currentOrders.Remove(itemToRemove);

            //destroy the game object once it has been removed
            Destroy(itemToRemove.gameObject);
        }
    }

    //check the orders, and remove expired orders
    private void RemoveExpiredOrders()
    {
        //order at front of list always the oldest
        //if order time duration has expired, remove
        if (Time.time >= currentOrders[0].orderDuration + currentOrders[0].timeStart)
        {
            currentOrders.RemoveAt(0);
        }
    }

    //moves orders into correct position in the gui
    private void PositionOrders()
    {
        //for every order currently in order list
        for (int i = 0; i < currentOrders.Count; i++)
        {
            currentOrders[i].GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(
                currentOrders[i].GetComponent<RectTransform>().anchoredPosition,
                orderPositions[i].anchoredPosition,
                moveSpeed);
        }
    }
}
