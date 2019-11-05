using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionZoneBehaviour : MonoBehaviour {

    //list of colliders, needed for interaction
    public List<GameObject> collidedObjects = new List<GameObject>(); //for interactables
    //public List<Collider> tableObjects = new List<Collider>(); //for bench/tables

    //parent ref
    [Header("Parent object")]
    public Transform playerObject;

    //tags
    [Header("Tags")]
    public string interactableTag;

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
        foreach (GameObject interactable in collidedObjects)
        {
            //if distance is shorter, set closest object to this object
            //set shortest distance to this distance
            if (Vector3.Distance(interactable.transform.position, frontOfPlayer) < shortestDistance)
            {
                //Check if the object is interactable
                if (interactable.GetComponent<Interactable>())
                {
                    closestObject = interactable.gameObject;
                    shortestDistance = Vector3.Distance(interactable.transform.position, frontOfPlayer);
                }
            }
        }

        return closestObject;
    }

    //Get a workbench
    public GameObject GetClosestWorkstation()
    {
        //gameobject ref to return
        GameObject closestWorkstation = null;
        //initialize a base distance to compare against
        float shortestDistance = 50.0f;

        GetFrontOfPlayer();

        //compare distance between all interactable objects in list to current shortest distance
        foreach (GameObject workstation in collidedObjects)
        {
            //Check that the object is of tupe equipment
            if (workstation.GetComponent<ActiveEquipment>())
            {
                //if distance is shorter, set closest object to this object
                //set shortest distance to this distance
                if (Vector3.Distance(workstation.transform.position, frontOfPlayer) < shortestDistance)
                {
                    //Check if the object is workstation, and there is noone already working there
                    if (workstation.GetComponent<ActiveEquipment>() && !workstation.GetComponent<ActiveEquipment>().IsInUse())
                    {
                        closestWorkstation = workstation.gameObject;
                        shortestDistance = Vector3.Distance(workstation.transform.position, frontOfPlayer);
                    }
                }
            }
        }

        return closestWorkstation;
    }

    //get the transform of position between player and interaction zone and label as front of player
    void GetFrontOfPlayer()
    {
        //frontOfPlayer = (transform.position - playerObject.position) * 0.5f;
        frontOfPlayer = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        //check only if parent
        if (!other.gameObject.transform.parent)
        {
            //if object is interactable
            if (other.CompareTag(interactableTag))
            {
                //if object not already in list
                if (!collidedObjects.Contains(other.gameObject))
                {
                    collidedObjects.Add(other.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if object is in list
        if (collidedObjects.Contains(other.gameObject))
        {
            //remove from list
            collidedObjects.Remove(other.gameObject);
        }
    }
}
