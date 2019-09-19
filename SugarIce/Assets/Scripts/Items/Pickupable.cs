using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : Interactable
{
    public enum MaterialType
    {
        Liquid,
        Dough,
        Stone
    }

    [Header("Handling Needs")]
    [Tooltip("True if item will explode when improperly handled")]
    public bool isExplosive = false;
    [Tooltip("True if item will break when improperly handled")]
    public bool isFragile = false;

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
        player.GetItem(this.gameObject);
    }
}
