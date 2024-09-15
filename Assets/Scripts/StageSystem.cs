using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageSystem : MonoBehaviour
{
    public PlantGroup plantGroup;

    public TextMeshProUGUI dayTMP;
    
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

    private void InitializeDay()
    {
        day = 1;
    }

    public void OnDayStart()
    {
        if (failed)
            RestartProgress();
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

    private bool TryAdvanceDay()
    {
        if (day < DayLimit)
        {
            day++;
            return false; // Not the last day
        }
        return true; // It's the last day
    }

    void RestartProgress()
    {
        failed = false;
        day = 1; // Reset to day 1
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
