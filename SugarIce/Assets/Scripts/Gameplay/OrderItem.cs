using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderItem : MonoBehaviour {



    [Header("Time vars")]
    public float orderDuration = 60.0f; //duration of the order
    [HideInInspector]
    public float timeStart = 0.0f; //time that order was created

    [Header("Order")]
    public ItemStateControl.ItemTypes order;

    [Header("Order Image")]
    public Image orderImage; //graphical representation of order

    //rect transform where image sits
    [HideInInspector]
    public RectTransform currentPos;

    // Use this for initialization
    void Start () {
        timeStart = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
