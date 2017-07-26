using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{

    public enum ToolTypes
    {
        NONE,
        OVEN,
        BURNER,
        TUBES,
        FRIDGE,
        DONUTS,
        DRUGS,
        BAGS

    };

    public enum DrugTypes
    {
        BLUE,
        RED
    };

    public ToolTypes Tool = ToolTypes.NONE;
    public GameObject ProgressBar = null;
    [Header("Supply Tool Settings")]
    public DrugTypes DrugType = DrugTypes.BLUE;
    [Header("Cooking Tool Settings")]
    public float DoneTime = 5.0f;
    public float BurntTime = 7.0f;
    private float CookingStartTime = 0.0f;
    [Header("Mixing Tool Settings")]
    public ItemStateControl.ItemTypes[] Recipe1;
    public ItemStateControl.ItemTypes[] Recipe2;
    [Header("Produce Prefabs")]
    public GameObject[] Items;

    private int MixtureStage = 0;
    private ItemStateControl.ItemTypes[] Mixture;

    private GameObject ItemRef = null;
    private bool ToolActive = false;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (ToolActive)
        {
            switch (Tool)
            {
                case ToolTypes.OVEN:
                    OvenUpdate();
                    break;
                case ToolTypes.BURNER:
                    BurnerUpdate();
                    break;
                case ToolTypes.TUBES:
                    TubeUpdate();
                    break;
            }
        }
    }

    void OvenUpdate()
    {
        if (Time.time - CookingStartTime < DoneTime)
        {         
            ProgressBar.transform.localScale = new Vector3(ProgressBar.transform.localScale.x, 2 * ( (Time.time - CookingStartTime) / DoneTime ), ProgressBar.transform.localScale.z);
            print(ProgressBar.transform.localScale.y);
        }
    }

    void BurnerUpdate()
    {
        if (Time.time - CookingStartTime < DoneTime)
        {
            ProgressBar.transform.localScale = new Vector3(ProgressBar.transform.localScale.x, 2 * ((Time.time - CookingStartTime) / DoneTime), ProgressBar.transform.localScale.z);
            print(ProgressBar.transform.localScale.y);
        }
    }

    void TubeUpdate()
    {

    }

    public GameObject GetItemInTool()
    {
        switch (Tool)
        {
            case ToolTypes.OVEN:
                if (Time.time - CookingStartTime < DoneTime)
                {
                    ToolActive = false;
                    return ItemRef;
                }
                if (Time.time - CookingStartTime > BurntTime)
                {
                    Destroy(ItemRef);
                    ItemRef = null;
                    ToolActive = false;
                    return Instantiate(Items[2], new Vector3(0.0f, -5.0f, 0.0f), Quaternion.identity);
                }
                else if (Time.time - CookingStartTime > DoneTime)
                {
                    Destroy(ItemRef);
                    ItemRef = null;
                    ToolActive = false;
                    return Instantiate(Items[1], new Vector3(0.0f, -5.0f, 0.0f), Quaternion.identity);
                }
                break;
            case ToolTypes.DRUGS:
                if(DrugType == DrugTypes.BLUE)
                {
                    return Instantiate(Items[3], new Vector3(0.0f, -5.0f, 0.0f), Quaternion.identity);
                }
                else if(DrugType == DrugTypes.RED)
                {
                    return Instantiate(Items[4], new Vector3(0.0f, -5.0f, 0.0f), Quaternion.identity);
                }
                break;
        }
        ToolActive = false;
        return ItemRef;
    }

    public bool ValidItem(GameObject _Item)
    {
        bool result = false;
        switch (Tool)
        {
            //Return true as there is no tool
            case ToolTypes.NONE:
                return true;
            //Check item is valid for a Oven
            case ToolTypes.OVEN:
                if (_Item.GetComponent<ItemStateControl>().GetItemType() == ItemStateControl.ItemTypes.DONUTDOUGH)
                {
                    ItemRef = _Item;
                    result = true;
                }
                //Start Tool Progress
                ToolActive = true;
                CookingStartTime = Time.time;
                return result;
            //Check item is valid for a Bunsen Burner
            case ToolTypes.BURNER:
                switch (_Item.GetComponent<ItemStateControl>().GetItemType())
                {
                    case ItemStateControl.ItemTypes.MIX1:
                        ItemRef = _Item;
                        result = true;
                        break;
                    case ItemStateControl.ItemTypes.MIX2:
                        ItemRef = _Item;
                        result = true;
                        break;
                    default: break;
                }
                //Start Tool Progress
                ToolActive = true;
                CookingStartTime = Time.time;
                return result;
            //Check item is valid for a Test Tube
            case ToolTypes.TUBES:
                switch (_Item.GetComponent<ItemStateControl>().GetItemType())
                {
                    case ItemStateControl.ItemTypes.POWDER1:
                        ItemRef = _Item;
                        result = true;
                        break;
                    case ItemStateControl.ItemTypes.POWDER2:
                        ItemRef = _Item;
                        result = true;
                        break;
                    default: break;
                }
                return result;
        }
        return result;
    }
}
