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
        BAGS,
        DROPOFF

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
    [Header("VFX")]
    public ParticleSystem VFXInProgress = null;
    public ParticleSystem VFXFinished = null;
    [Header("Produce Prefabs")]
    public GameObject[] Items;

    public Material BagMaterial;
    public Mesh BagMesh;
    //Mixture variables
    public bool MixtureDone = false;
    public int MixtureStage = 0; //What stage is the mixture at
    public ItemStateControl.ItemTypes[] Mixture;

    private GameObject ItemRef = null;
    public bool ToolActive = false;


    // Use this for initialization
    void Start()
    {
        if(Tool == ToolTypes.DRUGS || Tool == ToolTypes.DONUTS)
        {
            GetComponent<TableStateControl>().hasItem = true;
        }
        Mixture = new ItemStateControl.ItemTypes[3];
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
        if (Time.time - CookingStartTime < BurntTime)
        {         
            ProgressBar.transform.localScale = new Vector3(ProgressBar.transform.localScale.x, 2 * ( (Time.time - CookingStartTime) / DoneTime ), ProgressBar.transform.localScale.z);         
        }
        if (Time.time - CookingStartTime > BurntTime)
        {
            if (VFXInProgress.isPlaying)
            {
                VFXInProgress.Stop();
            }
            if (!VFXFinished.isPlaying)
            {
                VFXFinished.Play();
            }
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

    void ValidTestTube()
    {
        if(Mixture[0] == Recipe1[0])
        {
            if(Mixture[MixtureStage] == Recipe1[MixtureStage])
            {
                MixtureStage++;
            }
            else
            {
                print("Explosion");
                if (!VFXFinished.isPlaying)
                {
                    VFXFinished.Play();
                }
                //Explode
            }
        }
        else if(Mixture[0] == Recipe2[0])
        {
            if (Mixture[MixtureStage] == Recipe2[MixtureStage])
            {
                MixtureStage++;
            }
            else
            {
                print("Explosion");
                if (!VFXFinished.isPlaying)
                {
                    VFXFinished.Play();
                }
                //Explode
            }
        }

        if (MixtureStage >= 3)
        {
            MixtureDone = true;
        }
    }


    public GameObject GetItemInTool()
    {
        switch (Tool)
        {
            case ToolTypes.OVEN:
                if (Time.time - CookingStartTime < DoneTime) //Return Donut Dough
                {
                    if (VFXInProgress.isPlaying)
                    {
                        VFXInProgress.Stop();
                    }
                    ToolActive = false;
                    return ItemRef;
                }
                if (Time.time - CookingStartTime > BurntTime) //Return Burnt Donut
                {
                    if (VFXFinished.isPlaying)
                    {
                        VFXFinished.Stop();
                    }
                    Destroy(ItemRef);
                    ItemRef = null;
                    ToolActive = false;
                    return Instantiate(Items[2], new Vector3(0.0f, -5.0f, 0.0f), Quaternion.identity);
                }
                else if (Time.time - CookingStartTime > DoneTime) //Return Cooked Donut
                {
                    if (VFXInProgress.isPlaying)
                    {
                        VFXInProgress.Stop();
                    }
                    Destroy(ItemRef);
                    ItemRef = null;
                    ToolActive = false;
                    return Instantiate(Items[1], new Vector3(0.0f, -5.0f, 0.0f), Quaternion.identity);
               }
               break;
            case ToolTypes.BURNER:
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
            //Supply Tools
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
            case ToolTypes.DONUTS:
                return Instantiate(Items[0], new Vector3(0.0f, -5.0f, 0.0f), Quaternion.identity);
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
                    //Start Tool Progress
                    ToolActive = true;
                    CookingStartTime = Time.time;
                    //Run VFX
                    if(!VFXInProgress.isPlaying)
                    {
                        VFXInProgress.Play();
                    }
                    result = true;
                }
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
                if (result)
                {
                    //Start Tool Progress
                    ToolActive = true;
                    CookingStartTime = Time.time;
                }
                return result;
            //Check item is valid for a Test Tube
            case ToolTypes.TUBES:
                if (!MixtureDone)
                {
                    switch (_Item.GetComponent<ItemStateControl>().GetItemType())
                    {
                        case ItemStateControl.ItemTypes.POWDER1:
                            ItemRef = _Item;
                            Mixture[MixtureStage] = ItemRef.GetComponent<ItemStateControl>().GetItemType();
                            result = true;
                            break;
                        case ItemStateControl.ItemTypes.POWDER2:
                            ItemRef = _Item;
                            Mixture[MixtureStage] = ItemRef.GetComponent<ItemStateControl>().GetItemType();
                            result = true;
                            break;
                        default: break;
                    }
                    if (result)
                    {
                        ToolActive = true;
                        ValidTestTube();
                    }
                }
                return result;
            case ToolTypes.BAGS:
                switch (_Item.GetComponent<ItemStateControl>().GetItemType())
                {
                    case ItemStateControl.ItemTypes.DRUG1:
                        _Item.GetComponent<MeshFilter>().mesh = BagMesh;
                        _Item.GetComponent<MeshRenderer>().material = BagMaterial;
                        result = false;
                        break;
                    case ItemStateControl.ItemTypes.DRUG2:
                        _Item.GetComponent<MeshFilter>().mesh = BagMesh;
                        _Item.GetComponent<MeshRenderer>().material = BagMaterial;
                        result = false;
                        break;
                    case ItemStateControl.ItemTypes.DONUTCOOKED:
                        _Item.GetComponent<MeshFilter>().mesh = BagMesh;
                        _Item.GetComponent<MeshRenderer>().material = BagMaterial;
                        result = false;
                        break;
                    default: break;
                }
                return result;
        }
        return result;
    }
}
