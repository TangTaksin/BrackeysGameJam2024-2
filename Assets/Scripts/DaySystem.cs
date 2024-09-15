using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaySystem : MonoBehaviour
{
    public float prepareTime;
    float remainTime;

    bool isTicking;

    public delegate void DayEvent();

    public static DayEvent OnDayStart;
    public static DayEvent OnTimeOut;
    public static DayEvent OnPrepareEnd;
    public static DayEvent OnDayEnd;

    private void OnEnable()
    {
        OnTimeOut += TriggerTimeOver;
        OnDayEnd += TriggerNextDay;
    }

    private void OnDisable()
    {
        OnTimeOut -= TriggerTimeOver;
        OnDayEnd -= TriggerNextDay;
    }

    private void Start()
    {
        TriggerNextDay();
    }

    private void Update()
    {
        //UpdateTimer();
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
        OnDayStart?.Invoke();

        Transition.FadeOutOver += OnAfterFade;

        remainTime = prepareTime;
        isTicking = true;
    }

    void OnAfterFade()
    {
        Player.ChangePlayerCanActBool?.Invoke(true);

        Transition.FadeOutOver -= OnAfterFade;
    }

    public void SetIsTicking(bool _value)
    {
        isTicking = _value;
    }

    void CheckTimeOver()
    {
        if (remainTime <= 0)
        {
            OnTimeOut?.Invoke();
        }
    }

    public void TriggerTimeOver()
    {
        remainTime = 0;
        isTicking = false;
        AudioManager.Instance.StopMusicFadeOut();
        AudioManager.Instance.StopAmbientFadeOut();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.nightTime_sfx);

        Player.ChangePlayerCanActBool?.Invoke(false);
        Transition.CalledFadeIn?.Invoke();
        OnPrepareEnd?.Invoke();
    }

    void TriggerNextDay()
    {
        RestartTimer();
    }
}
