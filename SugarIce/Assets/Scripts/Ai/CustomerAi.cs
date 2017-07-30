using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerAi : MonoBehaviour {

    //navmeshagent ref
    private NavMeshAgent navMashAgent;

    //order item script ref
    [HideInInspector]
    public OrderItem myOrder;
    [Header("Order Image")]
    public SpriteRenderer orderSprite; //sprite that represents the order

    //order manager ref
    private OrderBehaviour orderManager;

    //level layout manager ref
    private LevelLayoutManager layoutManager;

    [Header("Transforms for NavMeshAgent")]
    private Transform wanderDestination; //destination for wandering around shop
    private Transform myExit; //the exit that this agent will take to exit

    [Header("Controls for NavMesh Agent")]
    public float lastWanderTime = 0.0f; //time since last new destination 
    public float wanderInterval = 3.0f; //time that has to past to get new wander destination

    private NavMeshPath navPath; //for checking paths when wandering
    //set of bools controlling current action
    private bool isArriving = false;
    private bool isWaiting = false;
    private bool isPaying = false;
    private bool isLeaving = false;

	// Use this for initialization
	void Start () {
        orderManager = GameObject.Find("LevelManager").GetComponent<OrderBehaviour>();

        int rand = Random.Range(0, orderManager.possibleOrders.Length);
        myOrder = orderManager.possibleOrders[rand];

        layoutManager = GameObject.Find("LevelManager").GetComponent<LevelLayoutManager>();

        orderSprite = GetComponentInChildren<SpriteRenderer>();

        navMashAgent = GetComponent<NavMeshAgent>();
        //move to shop center after spawning in
        navMashAgent.SetDestination(layoutManager.shopFrontCenter.position);
        //set the exit that this agent will leave from
        rand = Random.Range(0, layoutManager.exitPos.Length);
        myExit = layoutManager.exitPos[rand];
	}
	
	// Update is called once per frame
	void Update () {
        //change behaviour states if paths have completed

        UpdateAgentState();

        //update destination of agent based on current state
        UpdateAgentDestination();

    }

    //update the state of agent is in
    private void UpdateAgentState()
    {
        //check if agent has reached end of path when leaving
        if (isLeaving && transform.position == myExit.position)
        {
            //destroy this gameobject
            Destroy(gameObject);
        }
        //check if agent has reached cashier and is receiving order
        else if (isPaying && transform.position == layoutManager.cashierPos.position)
        {
            //send score stuff to gameplay manager

            //set agent to leave
            isLeaving = true;
        }
    }

    //update destination
    private void UpdateAgentDestination()
    {
        //if leaving
        if (isLeaving)
        {
            navMashAgent.SetDestination(myExit.position);
        }
        //if going to cashier
        else if (isPaying)
        {
            navMashAgent.SetDestination(layoutManager.cashierPos.position);
        }
        //if waiting
        else if (isWaiting)
        {
            //check if time has passed to pick new wander destination
            if (Time.time > lastWanderTime + wanderInterval)
            {
                //get random destination
                Vector3 newDestination = new Vector3(Random.Range(layoutManager.shopFrontCenter.transform.position.x - layoutManager.storeFrontHalfWidth, layoutManager.shopFrontCenter.transform.position.x + layoutManager.storeFrontHalfWidth), transform.position.y, Random.Range(layoutManager.shopFrontCenter.transform.position.z - layoutManager.storeFrontHalfLength, layoutManager.shopFrontCenter.transform.position.z + layoutManager.storeFrontHalfLength));
                //calculate the path
                navMashAgent.CalculatePath(newDestination, navPath);
                //if path is complete, go to it
                if (navPath.status == NavMeshPathStatus.PathComplete)
                {
                    wanderDestination.position = newDestination;
                    navMashAgent.SetDestination(wanderDestination.position);
                    //set last location change time to now
                    lastWanderTime = Time.time;
                }
            }
            //if not yet time to get new location
            else
            {
                navMashAgent.SetDestination(wanderDestination.position);
            }
        }
        //if yet to enter shop
        else if (isArriving)
        {
            navMashAgent.SetDestination(layoutManager.shopFrontCenter.position);
        }
    }

    //set state to leaving
    public void SetLeave()
    {
        isLeaving = true;
        //hide the sprite
        orderSprite.color = new Color(1, 1, 1, 0);
    }

    //set state to paying
    public void SetPaying()
    {
        isPaying = true;
        //hide the sprite
        orderSprite.color = new Color(1, 1, 1, 0);
    }

    //actions after entering shop
    public void ArriveAtShop()
    {
        //set waiting to true
        isWaiting = true;
        //show the sprite
        orderSprite.color = new Color(1, 1, 1, 1);
        //set my order start time to now
        myOrder.timeStart = Time.time;
        //send this customers order to order manager
        orderManager.RecieveOrder(myOrder);
    }
}
