using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGroup : MonoBehaviour
{
    public Plant[] plantGroup;

    int _baseIntegrity;
    int _integrity;

    void GetDefaultIntegrity()
    {
        foreach (var plant in plantGroup)
        {
            _baseIntegrity += plant.plantdata.baseHitPoint;
        }
    }

    void CheckOverallIntegrity()
    {
        foreach (var plant in plantGroup)
        {
            _integrity += plant.hitPoints;
        }
    }
}
