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
    public TextMeshProUGUI _overall_tmpro;
    public TextMeshProUGUI _prevented_tmpro, _damaged_tmpro, _died_tmpro;

    private void OnEnable()
    {
        StormSystem.OnStormEnd += BringUpResult;
    }

    private void OnDisable()
    {
        StormSystem.OnStormEnd -= BringUpResult;
    }

    public void OnConfirm()
    {
        if (_panel.activeSelf)
        {
            Transition.CalledFadeIn?.Invoke();
            Transition.FadeInOver += HideResult;
        }
    }

    void BringUpResult()
    {
        _panel.SetActive(true);
        UpdateResult();
    }

    void HideResult()
    {
        _panel.SetActive(false);
        DaySystem.OnDayEnd?.Invoke();
        Transition.FadeInOver -= HideResult;
    }

    void UpdateResult()
    {
        var overall_value = _plantgroup.GetOveallStatus();
        _overall_fill.fillAmount = overall_value;
        _overall_tmpro.text = string.Format("{0}%", overall_value * 100);

        _prevented_tmpro.text = string.Format("{0} prevented damages", _plantgroup._PreventedCount);
        _damaged_tmpro.text = string.Format("{0} damaged flowers", _plantgroup._damagedCount);
        _died_tmpro.text = string.Format("{0} dead flowers", _plantgroup._deadCount);
    }
}
