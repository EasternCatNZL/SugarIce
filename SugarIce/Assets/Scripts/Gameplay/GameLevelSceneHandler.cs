using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLevelSceneHandler : MonoBehaviour
{
    private bool isLevelActive = false; //game control bool

    [Header("Order parameters")]
    [Tooltip("Maximum concurrent orders")]
    public int maxNumOrders = 5; 
    [Tooltip("Minimum time between orders")]
    public float minTimeBetweenOrder = 20.0f; 
    [Tooltip("Maximum time between orders")]
    public float maxTimeBetweenOrder = 25.0f;
    [Tooltip("Order duration")]
    public float orderDuration = 30.0f;
    [Tooltip("Slight delay when orders already full and trying to create new order")]
    public float ordersFullDelay = 1.5f;
    [Tooltip("List of possible products for this level")]
    public List<Order> possibleOrdersList = new List<Order>();

    private List<Order> currentActiveOrders = new List<Order>(); //orders currently active in level

    //private int currentNumOrders = 0; //the current number of orders
    private float timeToNextOrder = 0.0f; //the time until next order
    private float timeLastOrder = 0.0f; //time last order occurred

    [Header("Timer")]
    [Tooltip("Starting time limit of level")]
    public float levelTimeLimit = 300.0f;

    private float timeLevelStarted = 0.0f;
    private float levelCurrentTimeLeft = 0.0f;

    [Header("Scoring")]
    public float levelScoreMultiplier = 0.5f;

    private float score = 0.0f;

    [Header("Tags")]
    public string UiTag = "UI";

    [Header("UI Components")]
    public LevelSceneUI levelUi;


    // Start is called before the first frame update
    void Start()
    {
        //ensure ui exists and is connected
        if (!levelUi)
        {
            levelUi = GameObject.FindGameObjectWithTag(UiTag).GetComponent<LevelSceneUI>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Do game things while level is active
        if (isLevelActive)
        {
            //update timed vars
            TickTimer();
            if(Time.time >= timeLastOrder + timeToNextOrder)
            {
                CreateOrder();
            }

        }
    }

    //Logic ran when level begins <-prep before giving players control
    private void StartLevel()
    {
        //set level to active
        isLevelActive = true;
        //set timers
        InitClock();

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
            GameObject orderClone = Instantiate(possibleOrdersList[randChoice].gameObject, transform.position, Quaternion.identity);
            //init order with this levels time limits
            orderClone.GetComponent<Order>().InitOrder(orderDuration);

            //add this order to active order
            currentActiveOrders.Add(orderClone.GetComponent<Order>());

            //set time of last order to now
            timeLastOrder = Time.time;
            //set time till next order
            timeToNextOrder = SetNextOrderTime();

            //Update the ui
            levelUi.CreateOrderInUI(currentActiveOrders.Count - 1, orderClone.GetComponent<Product>());
        }
        //else, alter timer such that there is a slight gap between full order opening gap and filling again
        else
        {
            timeToNextOrder += ordersFullDelay;
        }
    }

    //Set next order time <- less active orders = sooner next order
    private float SetNextOrderTime()
    {

        //get random number between min and max
        float time = Random.Range(minTimeBetweenOrder, maxTimeBetweenOrder);
        //alter by number of orders currently active against the max that can be actinve
        time /= (maxNumOrders - currentActiveOrders.Count);

        return time;
    }

    //Complete order by removing it from the list
    private void CompleteOrder(Order completedOrder)
    {
        //check if this product exists within the requested orders
        Order foundOrder = null;
        foreach (Order i in currentActiveOrders){
            //if found, ref the order and then break out of loop
            if(completedOrder.orderProduct.productName == i.orderProduct.productName)
            {
                foundOrder = i;
                break;
            }
        }
        //if order was found, remove it from play and add score
        if (foundOrder)
        {
            currentActiveOrders.Remove(foundOrder);
            score = foundOrder.ScoreThisOrder(levelScoreMultiplier);
            Destroy(foundOrder.gameObject);
        }
    }

    //Initialize the clock
    private void InitClock()
    {
        //set timers
        timeLevelStarted = Time.time;
        levelCurrentTimeLeft = levelTimeLimit;
        PresentTimer();
    }

    //Timer ticking logic
    private void TickTimer()
    {
        levelCurrentTimeLeft = levelTimeLimit - (Time.time - timeLevelStarted);
        if (levelCurrentTimeLeft <= 0)
        {
            isLevelActive = false;
            //timerText.text = levelFinishMessage;
        }
        else
        {
            PresentTimer();
        }
    }

    //Timer ui logic
    private void PresentTimer()
    {
        int secondsLeft = (int)(levelCurrentTimeLeft % 60);
        float timeMinusSeconds = levelCurrentTimeLeft - secondsLeft;
        int minutesLeft = (int)(timeMinusSeconds / 60);
        levelUi.TimerTextUpdate(minutesLeft, secondsLeft);
    }
}
