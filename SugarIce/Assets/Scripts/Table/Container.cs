using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : Table
{
    public GameObject containerItem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PickUpFrom(PlayerControl player)
    {
        //if there is something ontop of the container
        if (itemOnTable)
        {
            base.PickUpFrom(player);
        }
        //else, take something from container
        else
        {
            //spawn a new object for this ocntainer in
            GameObject newItem = Instantiate(containerItem, transform.position, transform.rotation);
            //give object to player
            player.GetItem(newItem);
        }        
    }
}
