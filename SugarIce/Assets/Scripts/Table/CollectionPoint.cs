using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionPoint : Table
{
    [Header("Tags")]
    public string handlerTag = "Handler";

    [Header("Script refs")]
    public GameLevelSceneHandler handler;

    // Start is called before the first frame update
    void Start()
    {
        if (!handler)
        {
            handler = GameObject.FindGameObjectWithTag(handlerTag).GetComponent<GameLevelSceneHandler>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void AttachToTable(GameObject item)
    {
        base.AttachToTable(item);
        if (item.GetComponent<Product>())
        {
            CompleteOrder(item.GetComponent<Product>());
        }
        
    }

    //Consumes item, and if order for item existed, complete the order
    private void CompleteOrder(Product item)
    {
        //Have handler attempt to complete the order
        handler.CompleteOrder(item);
        //remove the object from play regardless of whether an order was completed
        Destroy(item.gameObject);
    }
}
