using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerAi : MonoBehaviour {

    //navmeshagent ref
    private NavMeshAgent navMashAgent;

    [Header("Transforms for NavMeshAgent")]
    public Transform shopFrontCenter; //transform at center of shop, where ai first moves when entering
    public Transform cashierPos; //transform pos in front of cashier
    public Transform[] exitPos = new Transform[0]; //set of exits that the agent could take to leave

    private Transform wanderDestination; //destination for wandering around shop
    private Transform myExit; //the exit that this agent will take to exit

    [Header("Controls for NavMesh Agent")]
    public float storeFrontHalfWidth = 3.0f; //half of the width of store front
    public float storeFrontHalfLength = 1.0f; //half of the length of store front
    public float lastWanderTime = 0.0f; //time since last new destination 
    public float wanderInterval = 3.0f; //time that has to past to get new wander destination

    private NavMeshPath navPath; //for checking paths when wandering
    private bool isArriving = false;
    private bool isWaiting = false;
    private bool isPaying = false;
    private bool isLeaving = false;

	// Use this for initialization
	void Start () {
        navMashAgent = GetComponent<NavMeshAgent>();
        //move to shop center after spawning in
        navMashAgent.SetDestination(shopFrontCenter.position);
        //set the exit that this agent will leave from
        int rand = Random.Range(0, exitPos.Length);
        myExit = exitPos[rand];
	}
	
	// Update is called once per frame
	void Update () {
        //change behaviour states if paths have completed

        UpdateAgentState();

        //update destination of agent based on current state
        //if leaving
        if (isLeaving)
        {
            navMashAgent.SetDestination(myExit.position);
        }
        //if going to cashier
        else if (isPaying)
        {
            navMashAgent.SetDestination(cashierPos.position);
        }
        //if waiting
        else if (isWaiting)
        {
            //check if time has passed to pick new wander destination
            if (Time.time > lastWanderTime + wanderInterval)
            {
                //get random destination
                Vector3 newDestination = new Vector3(Random.Range(shopFrontCenter.transform.position.x - storeFrontHalfWidth, shopFrontCenter.transform.position.x + storeFrontHalfWidth), transform.position.y, Random.Range(shopFrontCenter.transform.position.z  - storeFrontHalfLength, shopFrontCenter.transform.position.z + storeFrontHalfLength));
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
            navMashAgent.SetDestination(shopFrontCenter.position);
        }
	}

    //update the state of agent is in
    private void UpdateAgentState()
    {
        //check if agent has reached end of path when leaving
        if (isLeaving && navMashAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            //destroy this gameobject
            Destroy(gameObject);
        }
        //check if agent has reached cashier and is receiving order
        else if (isPaying && navMashAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            //set agent to leave
            isLeaving = true;
        }
        //check if agent has arrived at the shop
        else if (isArriving && navMashAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            //set agent to begin wandering inside the shop
            isWaiting = true;
        }
    }
}
