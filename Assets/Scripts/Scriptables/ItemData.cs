using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : ScriptableObject
{
    public Sprite item_icon;
    public float cost;
    public GameObject pickUpObject;

    protected bool isUsed;

    public virtual bool UseItem(Interactable interactable)
    {
        return isUsed;
    }
}