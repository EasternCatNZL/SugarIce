using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderBehaviour : MonoBehaviour {

    [Header("Orders for level")]
    public List<OrderItem> currentOrders = new List<OrderItem>();
    public ItemStateControl.ItemTypes[] possibleProducts = new ItemStateControl.ItemTypes[0];
    public Sprite[] productSprites = new Sprite[0];

    //list of current npc game objects
    List<GameObject> currentCustomers = new List<GameObject>();

    private float timeLastOrder = 0.0f; //time of last order

    private LevelLayoutManager layoutManager;

    // Use this for initialization

    void Start () {
        layoutManager = GameObject.Find("LevelManager").GetComponent<LevelLayoutManager>();
	}
	
	// Update is called once per frame
	void Update () {
        RemoveExpiredOrders();
        ArrangeQueue();
	}

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

    //complete a order, taking a itemtype of itemstatecontrol
    public void CompleteOrder(ItemStateControl orderToRemove)
    {
        ItemStateControl.ItemTypes temp = orderToRemove.GetComponent<ItemStateControl>().Type;
        //loop through list
        for (int i = 0; i < currentOrders.Count; i++)
        {
            //if the current orders item type matches the one given
            if (currentOrders[i].order == temp)
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
        if (currentOrders.Count > 0)//Don't check the first position if i doesn't exist
        {
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
    }


    private void OnTriggerEnter(Collider other)
    {
        //if a customer has entered
        if (other.CompareTag("NPC"))
        {
            //add the customer to the customer list
            currentCustomers.Add(other.gameObject);
            //add the customers order to the order list
            //currentOrders.Add(other.gameObject.GetComponent<CustomerAi>().myOrder);
            if (other.gameObject.GetComponent<CustomerAi>())
            {
                other.gameObject.GetComponent<CustomerAi>().ArriveAtShop();
                print("Added customer");
            }
            if (other.gameObject.GetComponent<PoliceAi>())
            {
                other.gameObject.GetComponent<PoliceAi>().ArriveAtShop();
                print ("Added police");
            }
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

    //sends the transforms of which customers should form a queue to current customers
    public void ArrangeQueue()
    {
        //loop through list
        for (int i = 0; i < currentCustomers.Count; i++)
        {
            //based on if customer or police
            if (currentCustomers[i].GetComponent<CustomerAi>())
            {
                currentCustomers[i].GetComponent<CustomerAi>().SetQueuePos(layoutManager.queuePos[i]);
            }
            if (currentCustomers[i].GetComponent<PoliceAi>())
            {
                currentCustomers[i].GetComponent<PoliceAi>().SetQueuePos(layoutManager.queuePos[i]);
            }
        }
    }
}
