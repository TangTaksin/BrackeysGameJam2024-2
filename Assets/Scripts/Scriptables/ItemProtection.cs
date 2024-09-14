using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data/Plant Protection")]
public class ItemPlantProtection : ItemData
{
    public override bool UseItem(Interactable interactable)
    {
        var plant = interactable as Plant;

        if (plant is Plant && !plant.isProtected && !plant.isDead)
        {
            plant.SetProtected(true);
            isUsed = true;
        }
        else 
            isUsed = false;

        return isUsed;
    }
}