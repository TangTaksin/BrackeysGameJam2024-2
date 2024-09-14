using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGroup : MonoBehaviour
{
    public Plant[] plantGroup;

    [Range(0,100)] public float FailThreshold;
    bool _failed;


    float _baseIntegrity;
    float _integrity;

    public int _damagedCount;
    public int _deadCount;
    public int _PreventedCount;

    private void OnEnable()
    {
        StormSystem.OnStormEnd += UpdateOverall;
        Plant.OnDamage += DamageCheck;
        DaySystem.OnDayStart += ResetStatistic;
    }

    private void OnDisable()
    {
        StormSystem.OnStormEnd -= UpdateOverall;
        Plant.OnDamage -= DamageCheck;
        DaySystem.OnDayStart -= ResetStatistic;
    }

    public void GetDefaultIntegrity()
    {
        _baseIntegrity = 0;
        foreach (var plant in plantGroup)
        {
            _baseIntegrity += plant.plantdata.baseHitPoint;
        }
    }


    public void CheckOverallIntegrity()
    {
        _integrity = 0;
        foreach (var plant in plantGroup)
        {
            _integrity += plant.hitPoints;
        }
    }

    public void UpdateOverall()
    {
        GetDefaultIntegrity();
        CheckOverallIntegrity();
    }

    public float GetOveallStatus()
    {
        UpdateOverall();
        return _integrity / _baseIntegrity;
    }

    public bool CheckFailStatus()
    {
        var status = GetOveallStatus();
        return _failed = (status * 100 < FailThreshold);
    }

    void DamageCheck(Plant _plant,int _takenDamage, bool _resisted, bool _proteted, bool _died, bool _attacked)
    {
        if (_takenDamage > 0)
            _damagedCount++;

        if (_attacked)
            _damagedCount--;

        if (_proteted)
            _PreventedCount++;

        if (_died)
        {
            _deadCount++;
        }
    }

    void ResetStatistic()
    {
        _damagedCount = 0;
        _PreventedCount = 0;
        _deadCount = 0;
    }

}
