using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour
{
    [Header("Order Details")]
    public Product orderProduct;

    private float orderDuration = 0.0f;
    private float orderStartTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //set timer values for order
    public void InitOrder(float orderDur)
    {
        orderDuration = orderDur;
        orderStartTime = Time.time;
    }

    //score this order
    public float ScoreThisOrder(float multiplier)
    {
        float score = ((Time.time + orderDuration) - orderStartTime);
        score += score * multiplier;
        return score;
    }
}
