using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStateControl : MonoBehaviour {

    public enum ItemTypes
    {
        DONUTDOUGH,
        DONUTCOOKED,
        POWDER1,
        POWDER2,
        DRUG1,
        DRUG2,
        DRUG3
    };

    public ItemTypes Type = ItemTypes.DONUTDOUGH;
    public bool Bagged = false;

    public void SetItemType(ItemTypes _Type)
    {
        Type = _Type;
    }

    public ItemTypes GetItemType()
    {
        return Type;
    }
}
