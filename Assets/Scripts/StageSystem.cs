using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageSystem : MonoBehaviour
{
    public PlantGroup plantGroup;

    public int DayLimit = 3;
    int _day = 1;
    public int day
    {
        get { return _day; }
        set 
        { 
            _day = value;
            if (dayTMP)
            {
                dayTMP.text = string.Format("DAY {0}", _day);

                if (_day >= DayLimit)
                    dayTMP.text = string.Format("LAST DAY");
            }
        }
    }

    bool failed;

    public delegate void StageEvent(bool isLastDay);
    public static event StageEvent OnReset;

    private void OnEnable()
    {
        DaySystem.OnDayStart += OnDayStart;
        InitializeDay();
    }

    private void OnDisable()
    {
        DaySystem.OnDayStart -= OnDayStart;
    }

    void OnDayEnd()
    {
        currentDay = 1;
        UpdateDayText();
    }

    public void OnDayStart()
    {
        if (failed)
            ResetProgress();
    }

    public (bool, bool) HandleDayEnd()
    {
        failed = plantGroup.CheckFailStatus();
        bool isLastDay = false;

        if (!failed)
        {
            isLastDay = TryAdvanceDay();
        }

        return (failed, isLastDay);
    }

    void RestartProgress()
    {
        failed = false;
        currentDay = 1; // Reset to day 1
        UpdateDayText();
        OnReset?.Invoke(true);
    }

    bool DayCheck()
    {
        bool lastDay = false;

        day++;
        if (day >= DayLimit)
        {
            
            lastDay = true;
        }

        return lastDay;
    }
}
