using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionZoneBehaviour : MonoBehaviour {

    //list of colliders, needed for interaction
    public List<Collider> collidedObjects = new List<Collider>(); //for interactables
    public List<Collider> tableObjects = new List<Collider>(); //for bench/tables

    //parent ref
    [Header("Parent object")]
    public Transform playerObject;

    //tags
    [Header("Tags")]
    public string interactableTag;
    public string tableTag; 

    //transforms
    Vector3 frontOfPlayer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //return object closest to player, only call if collided objects is not empty
    //for safety, when function called, always check back for null object return
    public GameObject GetClosestInteractable()
    {
        //gameobject ref to return
        GameObject closestObject = null;
        //initialize a base distance to compare against
        float shortestDistance = 50.0f;

        GetFrontOfPlayer();

        //compare distance between all interactable objects in list to current shortest distance
        foreach (Collider col in collidedObjects)
        {
            //if distance is shorter, set closest object to this object
            //set shortest distance to this distance
            if (Vector3.Distance(col.gameObject.transform.position, frontOfPlayer) < shortestDistance)
            {
                closestObject = col.gameObject;
                shortestDistance = Vector3.Distance(col.gameObject.transform.position, frontOfPlayer);
            }
        }

        return closestObject;
    }

    //return the table closest to player, only call if collided tables is not empty
    //for safety, when function called, always check back for null object return
    public GameObject GetClosestTable()
    {
        //gameobject ref to return
        GameObject closetsTable = null;
        //initialize a base distance to compare against
        float shortestDistance = 50.0f;

        GetFrontOfPlayer();

        //compare distance between all interactable objects in list to current shortest distance
        foreach (Collider col in tableObjects)
        {
            //if distance is shorter, set closest object to this object
            //set shortest distance to this distance
            if (Vector3.Distance(col.gameObject.transform.position, frontOfPlayer) < shortestDistance)
            {
                closetsTable = col.gameObject;
                shortestDistance = Vector3.Distance(col.gameObject.transform.position, frontOfPlayer);
            }
        }

        return closetsTable;
    }

    //get the transform of position between player and interaction zone and label as front of player
    void GetFrontOfPlayer()
    {
        //frontOfPlayer = (transform.position - playerObject.position) * 0.5f;
        frontOfPlayer = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if object not in list
        if (!collidedObjects.Contains(other))
        {
            //if object is an interactable
            if (other.CompareTag(interactableTag))
            {
                //add to list
                collidedObjects.Add(other);
                //DEBUG
                //print("Found " + other.gameObject.ToString());
            }
            if (other.CompareTag(tableTag))
            {
                //add to table list
                tableObjects.Add(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if object is in list
        if (collidedObjects.Contains(other))
        {
            //remove from list
            collidedObjects.Remove(other);
        }
        //if object is in table list
        if (tableObjects.Contains(other))
        {
            //remove from the list
            tableObjects.Remove(other);
        }
    }
}
