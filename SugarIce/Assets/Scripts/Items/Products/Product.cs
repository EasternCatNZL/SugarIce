﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : Pickupable
{
    public enum ProductName
    {
        Donut,
        Unknown
    }

    public ProductName productName = ProductName.Unknown;

    [Header("Order Image")]
    public Sprite productImage = null; //graphical representation of order

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
