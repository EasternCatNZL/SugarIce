using UnityEngine;

public class LevelLayoutManager : MonoBehaviour {

    [Header("Transforms for NavMeshAgent")]
    public Transform shopFrontCenter; //transform at center of shop, where ai first moves when entering
    public Transform cashierPos; //transform pos in front of cashier
    public Transform[] policeWaitPos = new Transform[0]; //set of pos that police will wait at
    public Transform[] queuePos = new Transform[0]; //set of pos which represent queue positions
    public Transform[] exitPos = new Transform[0]; //set of exits that the agent could take to leave
    public Transform[] policeCarEntryPos = new Transform[0]; //set of pos for police car to enter
    public Transform[] policeCarExitPos = new Transform[0]; //set of exits for police to exit
    public Transform arrestPos; //transform police moves to when arresting

    [Header("Controls for NavMesh Agent")]
    public float storeFrontHalfWidth = 3.0f; //half of the width of store front
    public float storeFrontHalfLength = 1.0f; //half of the length of store front

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
