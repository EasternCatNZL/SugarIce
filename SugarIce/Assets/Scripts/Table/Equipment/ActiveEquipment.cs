using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveEquipment : Equipment
{
    [Header("Positioning vars")]
    public Transform workPos;

    //The current user that is using this workstation
    private PlayerControl currentWorker = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttachWorker(PlayerControl player)
    {
        currentWorker = player;
        PositionWorker(player.gameObject);
    }

    //Set the posiition of the palyer character when working at this station
    protected virtual void PositionWorker(GameObject player)
    {
        player.transform.position = workPos.position;
        player.transform.rotation = workPos.rotation;
    }

    public void DetachWorker()
    {
        currentWorker = null;
    }

    //Checks to see if table is occupied by another worker
    public bool IsInUse()
    {
        bool occupied = false;

        if (currentWorker)
        {
            occupied = true;
        }

        return occupied;
    }
}
