using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    private bool isLevelActive = false; //game control bool

    private LevelLayoutManager layoutManager; //ref to layout manager

    [Header("Reference Storage")]
    public GameObject[] playerArray = new GameObject[0]; //array of players, to send commands to
    public GameObject[] customerArray = new GameObject[0]; //array of customers that can spawn

    [Header("Spawn Controls")]
    public float maxCustomers = 5.0f;

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
        if (Time.time > lastCustomerSpawnTime + newCustomerInterval)
        {
            int randSpawn = Random.Range(0, layoutManager.exitPos.Length);
            int randCustomer = Random.Range(0, customerArray.Length);

            Instantiate(customerArray[randCustomer], layoutManager.exitPos[randSpawn].transform.position, layoutManager.exitPos[randSpawn].rotation);
            lastCustomerSpawnTime = Time.time;
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
}
