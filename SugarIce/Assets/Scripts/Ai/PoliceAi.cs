using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceAi : MonoBehaviour {

    //navmeshagent ref
    private NavMeshAgent navMashAgent;

    //order item script ref
    [HideInInspector]
    public OrderItem myOrder;
    [Header("Order Image")]
    public SpriteRenderer orderSprite; //sprite that represents the order

    //order manager ref
    private OrderBehaviour orderManager;

    //level manager ref
    private LevelManager levelManager;

    //level layout manager ref
    private LevelLayoutManager layoutManager;

    [Header("Transforms for NavMeshAgent")]
    private Transform queuePos; //pos in queue the customer is in
    private Transform wanderDestination; //destination for wandering around shop
    private Transform myWaitPos; //the pos that police will stand in in shop while waiting
    private Transform myExit; //the exit that this agent will take to exit
    private Transform arrestPos; //pos police moves to to arrest

    [Header("Controls for NavMesh Agent")]
    public float lastWanderTime = 0.0f; //time since last new destination 
    public float wanderInterval = 3.0f; //time that has to past to get new wander destination

    private NavMeshPath navPath; //for checking paths when wandering
    //set of bools controlling current action
    private bool isArriving = false;
    private bool isWaiting = false;
    private bool isPaying = false;
    private bool isLeaving = false;
    private bool isArresting = false;

    // Use this for initialization
    void Start()
    {
        orderManager = GameObject.Find("LevelManager").GetComponent<OrderBehaviour>();
        myOrder = GetComponent<OrderItem>();
        layoutManager = GameObject.Find("LevelManager").GetComponent<LevelLayoutManager>();

        orderSprite = GetComponentInChildren<SpriteRenderer>();
        orderSprite.color = new Color(1, 1, 1, 0);


        navMashAgent = GetComponent<NavMeshAgent>();
        //move to shop center after spawning in
        navMashAgent.SetDestination(layoutManager.shopFrontCenter.position);
        //set the exit that this agent will leave from
        int rand = Random.Range(0, layoutManager.exitPos.Length);
        myExit = layoutManager.exitPos[rand];
        //set wait pos
        rand = Random.Range(0, layoutManager.policeWaitPos.Length);
        myWaitPos = layoutManager.policeWaitPos[rand];

        arrestPos = GameObject.Find("LevelManager").GetComponent<LevelLayoutManager>().arrestPos;

        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //change behaviour states if paths have completed
        UpdateAgentState();

        //update destination of agent based on current state
        UpdateAgentDestination();

        //have the order image face the camera
        orderSprite.gameObject.transform.LookAt(Camera.main.transform);
    }

    //update the state of agent is in
    private void UpdateAgentState()
    {
        //check if police has reached arrest pos
        if (isArresting && transform.position == arrestPos.position)
        {
            //do arrest stuff
        }
        //check if agent has reached end of path when leaving
        else if (isLeaving && transform.position == myExit.position)
        {
            //reduce the number of customers curretnly in world
            levelManager.RemoveLeavingCustomer();
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
        //if arresting <- always top priority
        if (isArresting)
        {
            navMashAgent.SetDestination(arrestPos.position);
        }
        //if leaving
        else if (isLeaving)
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
            //move to queue pos if not already there
            navMashAgent.SetDestination(queuePos.position);
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

    //set state to arresting
    public void SetArresting()
    {
        isArresting = true;
    }

    //set the queue position for this customer
    public void SetQueuePos(Transform pos)
    {
        queuePos = pos;
    }
}
