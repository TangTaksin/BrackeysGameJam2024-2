using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaySystem : MonoBehaviour
{
    public float prepareTime;
    float remainTime;

    bool isTicking;

    public delegate void DayEvent();
    public static DayEvent OnTimeOver;

    private void OnEnable()
    {
        OnTimeOver += TriggerTimeOver;
    }

    private void OnDisable()
    {
        OnTimeOver -= TriggerTimeOver;
    }

    private void Update()
    {
        UpdateTimer();
    }

    public float GetRemainingTime()
    {
        return remainTime;
    }

    void UpdateTimer()
    {
        if (isTicking)
        {
            remainTime -= Time.deltaTime;
            CheckTimeOver();
        }
    }

    public void RestartTimer()
    {
        remainTime = prepareTime;
        isTicking = true;
    }

    public void SetIsTicking(bool _value)
    {
        isTicking = _value;
    }

    void CheckTimeOver()
    {
        if (remainTime <= 0)
        {
            OnTimeOver?.Invoke();
        }
    }

    public void TriggerTimeOver()
    {
        remainTime = 0;
        isTicking = false;
    }
}
