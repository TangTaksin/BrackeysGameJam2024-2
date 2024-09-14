using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class StormResultUI : MonoBehaviour
{
    public PlantGroup _plantgroup;
    public GameObject _panel;
    public Image _overall_fill;
    public TextMeshProUGUI _overall_tmpro, _neededPerc_tmpro;
    public TextMeshProUGUI _prevented_tmpro, _damaged_tmpro, _died_tmpro, add_tmpro;
    public TextMeshProUGUI _button_tmpro;
    public CanvasGroup _button_canvasG;

    bool suceeded ,failed, lastDay;
    bool canPress;

    public float canPressTime;
    float timer;

    private void OnEnable()
    {
        StormSystem.OnStormEnd += BringUpResult;
        StageSystem.OnLastDay += OnLastDay;
        
    }

    private void OnDisable()
    {
        StormSystem.OnStormEnd -= BringUpResult;
        StageSystem.OnLastDay -= OnLastDay;
    }

    private void Update()
    {
        if (!canPress)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                canPress = true;
            }
            
            _button_canvasG.alpha = 1 - (timer / canPressTime);
        }

        
    }

    public void OnConfirm()
    {
        if (_panel.activeSelf && canPress)
        {
            if (suceeded)
                GenericSceneLoader.TriggerLoadScene("ProtoEndScene");
            else
                Transition.CalledFadeIn?.Invoke();

            Transition.FadeInOver += HideResult;
        }
    }

    void BringUpResult()
    {
        timer = canPressTime;
        canPress = false;

        failed = _plantgroup.CheckFailStatus();
        suceeded = (lastDay && !failed);

        _panel.SetActive(true);
        UpdateResult();
    }

    void HideResult()
    {
        _panel.SetActive(false);
        Transition.FadeInOver -= HideResult;

        if (!suceeded)
        {
            DaySystem.OnDayEnd?.Invoke();
        }
    }

    void OnLastDay(bool _bool)
    {
        lastDay = _bool;
    }

    void UpdateResult()
    {
        _neededPerc_tmpro.text = string.Format("needed {0}%", _plantgroup.FailThreshold);

        var overall_value = _plantgroup.GetOveallStatus();
        _overall_fill.fillAmount = overall_value;
        _overall_tmpro.text = string.Format("{0}%", overall_value * 100);

        _prevented_tmpro.text = string.Format("{0} prevented damages", _plantgroup._PreventedCount);
        _damaged_tmpro.text = string.Format("{0} damaged flowers", _plantgroup._damagedCount);
        _died_tmpro.text = string.Format("{0} dead flowers", _plantgroup._deadCount);

        add_tmpro.text = string.Empty;
        _button_tmpro.text = "continue";

        if (suceeded)
        {
            add_tmpro.text = string.Format("You managed to made it\nThrough the last day\nof the storm!");
        }

        if (failed)
        {
            add_tmpro.text = string.Format("The flowers' status\nwent lower than {0}%.\n\nyou didn't get paid...", _plantgroup.FailThreshold);
            _button_tmpro.text = "retry from day 1";
        }

        
    }
}
