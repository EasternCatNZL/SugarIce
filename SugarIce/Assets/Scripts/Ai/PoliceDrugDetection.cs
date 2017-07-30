using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceDrugDetection : MonoBehaviour {

    //ref to police ai script
    private PoliceAi policeAi;

	// Use this for initialization
	void Start () {
        policeAi = GetComponentInParent<PoliceAi>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        //if interactable comes within trigger
        if (other.gameObject.CompareTag("Interactable"))
        {
            //check if the interactable is a drug
            if (other.gameObject.GetComponent<ItemStateControl>().GetItemType() != ItemStateControl.ItemTypes.DONUTBURNT ||
                other.gameObject.GetComponent<ItemStateControl>().GetItemType() != ItemStateControl.ItemTypes.DONUTCOOKED ||
                other.gameObject.GetComponent<ItemStateControl>().GetItemType() != ItemStateControl.ItemTypes.DONUTDOUGH)
            {
                //game over message to game manager
                policeAi.SetArresting();
            }
        }
    }
}
