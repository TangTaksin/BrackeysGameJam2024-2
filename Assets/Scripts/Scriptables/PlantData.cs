using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlantData : ScriptableObject
{
    [Header("Sprite")]
    public Sprite normal;
    public Sprite hurt;
    public Sprite dead;
    public Sprite reenforce;

    [Header("Stats")]
    public int baseHitPoint;
    public int daysToHeal;
    public elements resistance;
}
