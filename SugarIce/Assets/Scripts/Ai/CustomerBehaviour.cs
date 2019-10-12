using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ai : MonoBehaviour
{
    //navmeshagent ref
    private NavMeshAgent navMashAgent;

    //order item script ref
    [HideInInspector]
    public Product myOrder;

    [Header("Transforms for NavMeshAgent")]
    private Transform queuePos; //pos in queue the customer is in
    private Transform wanderDestination; //destination for wandering around shop
    private Transform myExit; //the exit that this agent will take to exit

    [Header("Controls for NavMesh Agent")]
    public float wanderInterval = 3.0f; //time that has to past to get new wander destination
    private float lastWanderTime = 0.0f; //time since last new destination 
    private NavMeshPath navPath; //for checking paths when wandering

    private enum customerState
    {
        Arriving,
        Waiting,
        Paying,
        Leaving
    }

    [Header("Tags")]
    public string levelManagerTag = "LevelManager";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //For initialization of customer after spawning into scene
    void InitCustomer()
    {
        //set up order item for this customer using this levels possible orders
        OrderBehaviour orderBehaviour = GameObject.Find(levelManagerTag).GetComponent<OrderBehaviour>();
        //myOrder.product = orderBehaviour.possibleProducts[Random.Range(0, orderBehaviour.productSprites.Length)];
        
    }
}
