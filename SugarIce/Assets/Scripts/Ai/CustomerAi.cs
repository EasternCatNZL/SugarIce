using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerAi : MonoBehaviour {

    //navmeshagent ref
    private NavMeshAgent navMashAgent;

    [Header("Transforms for NavMeshAgent")]
    public Transform shopFrontCenter; //transform at center of shop, where ai first moves when entering
    public Transform[] exitPos = new Transform[0]; //

    [Header("Controls for NavMesh Agent")]
    public Transform storeFloorPointA; //top left most point for store floor space
    public Transform storeFloorPointB; //bottom right most point for store floor space

    private bool enteredShop = false;
    private bool leavingShop = false;

	// Use this for initialization
	void Start () {
        navMashAgent = GetComponent<NavMeshAgent>();
        //move to shop center after spawning in
        navMashAgent.SetDestination(shopFrontCenter.position);
	}
	
	// Update is called once per frame
	void Update () {
        //change behaviour states if paths have completed



		//change behaviour state based on what is happening
        //if yet to enter shop
        if (!enteredShop)
        {
            navMashAgent.SetDestination(shopFrontCenter.position);
        }
	}
}
