using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderBehaviour : MonoBehaviour {

    [Header("Orders for level")]
    List<OrderItem> currentOrders = new List<OrderItem>();
    //public OrderItem[] possibleOrders = new OrderItem[0];
    public ItemStateControl.ItemTypes[] possibleProducts = new ItemStateControl.ItemTypes[0];
    public Sprite[] productSprites = new Sprite[0];

    //list of current npc game objects
    List<GameObject> currentCustomers = new List<GameObject>();

    //[Header("Order functionality control")]
    //public float orderIntervalMinimum = 20.0f; //minimum time that has to pass before next order arrives
    //public float orderIntervalMaximum = 25.0f; //maximum time in which next order must arrive when met

    private float timeLastOrder = 0.0f; //time of last order

    // Use this for initialization

    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		//when interval passes
	}

    /*
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
    */

    //recieve an order and add to list
    public void RecieveOrder(OrderItem itemToAdd)
    {
        currentOrders.Add(itemToAdd);
    }

    //remove an object from the queue
    private void RemoveOrder(OrderItem itemToRemove)
    {

        //if item exists in list
        if (currentOrders.Contains(itemToRemove))
        {
            //remove from the list
            currentOrders.Remove(itemToRemove);

            //destroy the game object once it has been removed
            Destroy(itemToRemove.gameObject);
        }
    }

    ////call when order is completed
    //public void CompleteOrder(OrderItem orderToRemove)
    //{
    //    //remove the order from the current orders list
    //    //if item exists in list
    //    if (currentOrders.Contains(orderToRemove))
    //    {
    //        //remove from the list
    //        currentOrders.Remove(orderToRemove);
    //    }

    //    //get the customer object this order belongs to, and tell it to go to cashier
    //    orderToRemove.gameObject.GetComponent<CustomerAi>().SetPaying();
    //}

    //complete a order, taking a itemtype of itemstatecontrol
    public void CompleteOrder(ItemStateControl orderToRemove)
    {
        //loop through list
        for (int i = 0; i < currentOrders.Count; i++)
        {
            //if the current orders item type matches the one given
            if (currentOrders[i].order == orderToRemove.Type)
            {
                //have the customer related to this order leave
                currentOrders[i].gameObject.GetComponent<CustomerAi>().SetPaying();
                //remove this order
                currentOrders.RemoveAt(i);
            }
        }
    }

    //check the orders, and remove expired orders
    private void RemoveExpiredOrders()
    {
        //order at front of list always the oldest
        //if order time duration has expired
        if (Time.time >= currentOrders[0].orderDuration + currentOrders[0].timeStart)
        {
            //remove the order
            currentOrders.RemoveAt(0);
            //check if the object is attached to customer or ai
            if (currentCustomers[0].GetComponent<CustomerAi>())
            {
                //have customer leave
                currentCustomers[0].GetComponent<CustomerAi>().SetLeave();
            }
            else if (currentCustomers[0].GetComponent<PoliceAi>())
            {
                currentCustomers[0].GetComponent<PoliceAi>().SetArresting();
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        //if a customer has entered
        if (other.CompareTag("NPC"))
        {
            //add the customer to the customer list
            currentCustomers.Add(other.gameObject);
            //add the customers order to the order list
            currentOrders.Add(other.gameObject.GetComponent<CustomerAi>().myOrder);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if customer has left
        if (other.CompareTag("NPC"))
        {
            //if the customer exists in the list of customers
            if (currentCustomers.Contains(other.gameObject))
            {
                //remove from the list
                currentCustomers.Remove(other.gameObject);
            }
        }
    }
}
