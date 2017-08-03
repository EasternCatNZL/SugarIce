using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceCarBehaviour : MonoBehaviour {

    //script refs
    private LevelManager levelManager; //level manager ref
    //private LevelLayoutManager layoutManager; //ref to layout manager

    [HideInInspector]
    public Transform exitPos; //spot where police exits

    public GameObject policePrefab; //police man object ref

    public float moveSpeed; //speed at which car moves

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, exitPos.position, moveSpeed * Time.deltaTime);

        if (transform.position == exitPos.position)
        {
            //spawn new policeman
            GameObject policeClone = policePrefab;
            Instantiate(policeClone, exitPos.position, exitPos.rotation);
            //destroy this object
            Destroy(gameObject);
        }
	}
}
