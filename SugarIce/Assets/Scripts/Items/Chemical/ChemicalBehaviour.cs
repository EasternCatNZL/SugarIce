using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalBehaviour : MonoBehaviour {

    public enum DrugType
    {
        Blue,
        Orange
    }

    private DrugType drugType;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public DrugType GetDrugType()
    {
        return drugType;
    }
}
