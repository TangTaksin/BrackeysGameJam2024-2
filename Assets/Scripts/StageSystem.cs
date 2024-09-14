using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageSystem : MonoBehaviour
{
    public PlantGroup plantGroup;

    public int DayLimit = 3;
    static int _day = 1;
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

    public TextMeshProUGUI dayTMP;

    public delegate void StageEvent();
    public static StageEvent OnReset;
    public static StageEvent OnLastDay;

    private void OnEnable()
    {
        DaySystem.OnDayEnd += OnDayEnd;
        day = 1;
    }

    private void OnDisable()
    {
        DaySystem.OnDayEnd -= OnDayEnd;
    }

    void OnDayEnd()
    {
        //fail check
        var failed = plantGroup.CheckFailStatus();

        if (failed)
        {
            RestartProgress();
            return;
        }

        var lastDay = DayCheck();
        if (lastDay)
        {
            OnLastDay?.Invoke();
        }
    }

    void RestartProgress()
    {
        day = 1;
        OnReset?.Invoke();
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
