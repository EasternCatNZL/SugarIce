using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelSceneHandler : MonoBehaviour
{
    private bool isLevelActive = false; //game control bool

    [Header("Order control")]
    [Tooltip("Maximum concurrent orders")]
    public int maxNumOrders = 5; 
    [Tooltip("Minimum time between orders")]
    public float minTimeBetweenOrder = 20.0f; 
    [Tooltip("Maximum time between orders")]
    public float maxTimeBetweenOrder = 25.0f; 
    [Tooltip("List of possible products for this level")]
    public List<Product> possibleOrdersList = new List<Product>();

    private List<Product> currentActiveOrders = new List<Product>(); //orders currently active in level

    //private int currentNumOrders = 0; //the current number of orders
    private float timeToNextOrder = 0.0f; //the time until next order
    private float timeLastOrder = 0.0f; //time last order occurred

    [Header("Timer")]
    [Tooltip("Starting time limit of level")]
    public float levelTimeLimit = 300.0f;

    private float timeLevelStarted = 0.0f;
    private float levelCurrentTimeLeft = 0.0f;

    private float score = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Logic ran when level begins <-prep before giving players control
    private void StartLevel()
    {
        //set level to active
        isLevelActive = true;
        //set timers
        timeLevelStarted = Time.time;
        levelCurrentTimeLeft = levelTimeLimit;

        //Create first order
        CreateOrder();

        //Give players control
    }

    //Create new order from possible orders for this level given space for new active order
    private void CreateOrder()
    {
        //Check current num of orders is lower than max
        if(currentActiveOrders.Count < maxNumOrders)
        {
            //create random order based on possible products
            int randChoice = Random.Range(0, possibleOrdersList.Count);
            GameObject productClone = Instantiate(possibleOrdersList[randChoice].gameObject, transform.position, Quaternion.identity);

            //add this order to active order
            currentActiveOrders.Add(productClone.GetComponent<Product>());

            //set time of last order to now
            timeLastOrder = Time.time;
            //set time till next order
            timeToNextOrder = SetNextOrderTime();
        }
        //else, alter timer such that there is a slight gap between full order opening gap and filling again
        else
        {

        }
    }

    //Set next order time <- less active orders = sooner next order
    private float SetNextOrderTime()
    {
        float time = 0.0f;

        //get random number between min and max
        time = Random.Range(minTimeBetweenOrder, maxTimeBetweenOrder);
        //alter by number of orders currently active against the max that can be actinve
        time = time / (maxNumOrders - currentActiveOrders.Count);

        return time;
    }
}
