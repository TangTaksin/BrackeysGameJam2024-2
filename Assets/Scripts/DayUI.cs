using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayUI : MonoBehaviour
{
    public Image clockImage;
    public DaySystem daySystem;

    private void Update()
    {
        UpdateClock();
    }

    void UpdateClock()
    {
        clockImage.fillAmount = daySystem.GetRemainingTime() / daySystem.prepareTime;
    }
}
