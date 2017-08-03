using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    private bool isLevelActive = false; //game control bool

    private LevelLayoutManager layoutManager; //ref to layout manager

    [Header("Reference Storage")]
    public GameObject[] customerArray = new GameObject[0]; //array of customers that can spawn
    public GameObject policeCar; //police car and plane object
    //public GameObject policeman; //police object

    private GameObject[] playerArray = new GameObject[0]; //array of players, to send commands to

    [Header("Spawn Controls")]
    public int maxCustomers = 5; //maximum number of customers allowed in the world at a time
    public float customerWeight = 7.0f; //chance of customer over police, out of 10

    private int currentNumCustomers = 0; //the current number of customers

    [Header("Time Vars")]
    public float levelTimeLimit = 300.0f; //the total time in seconds that the level lasts for
    public float newCustomerInterval = 25.0f; //interval in which new customers arrive

    private float levelTimeStart = 0.0f; //the time that this round started
    private float lastCustomerSpawnTime = 0.0f; //time that last customer was spawned in

    private float scoreValue = 0.0f; //float for holding score, if needed

	// Use this for initialization
	void Start () {
        playerArray = GameObject.FindGameObjectsWithTag("Player");
        layoutManager = GetComponent<LevelLayoutManager>();

        levelTimeStart = Time.time;
        StartLevel();
        //once ready, spawn first customer
        SpawnNewCustomer();
	}
	
	// Update is called once per frame
	void Update () {
        SpawnNewCustomer();
	}

    //spawn a new customer if interval has passed
    private void SpawnNewCustomer()
    {
        //check time has passed
        if (Time.time > lastCustomerSpawnTime + newCustomerInterval)
        {
            //check if current customers does not exceed max number
            if (currentNumCustomers < maxCustomers)
            {
                float customerChance = Random.Range(0, 10);
                //if weighted choice is customer
                if (customerChance <= customerWeight)
                {
                    int randCustomer = Random.Range(0, customerArray.Length);
                    //get random seed of customer and spawn location
                    int randSpawn = Random.Range(0, layoutManager.exitPos.Length);
                    //spawn the customer
                    Instantiate(customerArray[randCustomer], layoutManager.exitPos[randSpawn].transform.position, layoutManager.exitPos[randSpawn].rotation);
                }
                //else weighted towards police
                else
                {
                    int randPoliceSpawn = Random.Range(0, layoutManager.policeCarEntryPos.Length);
                    GameObject policeCarClone = policeCar;
                    policeCarClone.GetComponent<PoliceCarBehaviour>().exitPos = layoutManager.policeCarExitPos[randPoliceSpawn];
                    //spawn the car
                    Instantiate(policeCarClone, layoutManager.policeCarEntryPos[randPoliceSpawn].position, layoutManager.policeCarEntryPos[randPoliceSpawn].rotation);
                }
                //set last spawn time to now
                lastCustomerSpawnTime = Time.time;
                //increase the current customer count
                currentNumCustomers++;
            }
        }
    }

    //adds to the score value
    public void AddScore(float addScore)
    {
        scoreValue += addScore;
    }

    //returns the score value
    public float GetScore()
    {
        return scoreValue;
    }

    //initialise functions for when level starts
    public void StartLevel()
    {
        //give players control of characters
        for (int i = 0; i < playerArray.Length; i++)
        {
            playerArray[i].GetComponent<PlayerStateControl>().isPlaying = true;
        }
        //set the start time to now
        levelTimeStart = Time.time;
        //initialise the score value
        scoreValue = 0.0f;

    }

    //ends the level
    public void EndLevel()
    {
        //remove control from the players
        for (int i = 0; i < playerArray.Length; i++)
        {
            playerArray[i].GetComponent<PlayerStateControl>().isPlaying = false;
        }
        //do game end stuff
        SceneManager.LoadScene(0);
    }

    //game over the level <- called from police when arresting
    public void GameOverLevel()
    {
        //remove control from the players

        //set the players to panic
        for (int i = 0; i < playerArray.Length; i++)
        {
            playerArray[i].GetComponent<PlayerPickUpBehaviour>().Panic();
        }

        //do game over stuff
    }

    //decrease the customers by one
    public void RemoveLeavingCustomer()
    {
        currentNumCustomers--;
    }
}
