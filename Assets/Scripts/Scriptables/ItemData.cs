using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : ScriptableObject
{
    public float cost;
    public GameObject pickUpObject;

    protected bool isUsed;

    public virtual bool UseItem(Interactable interactable)
    {
        return isUsed;
    }
}

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data/Plant Restorative")]
public class ItemPlantRestorative : ItemData
{
    public override bool UseItem(Interactable interactable)
    {
        return isUsed;
    }
}