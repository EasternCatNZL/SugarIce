using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour {

    public enum ToolTypes
    {
        NONE,
        OVEN,
        BURNER,
        TUBES,
        FRIDGE,
        DONUTs,
        DRUGS,
        BAGS

    };

    public enum DrugTypes
    {
        BLUE,
        ORANGE
    };

    public ToolTypes Tool;

    private GameObject ItemRef;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool ValidItem(GameObject _Item)
    {
        switch(Tool)
        {
            //Return true as there is no tool
            case ToolTypes.NONE:
                return true;
            //Check item is valid for a Oven
            case ToolTypes.OVEN:
                if(_Item.GetComponent<ItemStateControl>().GetItemType() == ItemStateControl.ItemTypes.DONUTDOUGH)
                {
                    ItemRef = _Item;
                    return true;
                }
                break;
            //Check item is valid for a Bunsen Burner
            case ToolTypes.BURNER:
                switch(_Item.GetComponent<ItemStateControl>().GetItemType())
                {
                    case ItemStateControl.ItemTypes.POWDER1:
                        ItemRef = _Item;
                        return true;
                    case ItemStateControl.ItemTypes.POWDER2:
                        ItemRef = _Item;
                        return true;
                    default: break;
                }
                break;
            //Check item is valid for a Test Tube
            case ToolTypes.TUBES:
                switch (_Item.GetComponent<ItemStateControl>().GetItemType())
                {
                    case ItemStateControl.ItemTypes.POWDER1:
                        ItemRef = _Item;
                        return true;
                    case ItemStateControl.ItemTypes.POWDER2:
                        ItemRef = _Item;
                        return true;
                    default: break;
                }
                break;
        }
        return false;
    }
}
