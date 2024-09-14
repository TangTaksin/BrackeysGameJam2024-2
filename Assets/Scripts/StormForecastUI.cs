using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StormForecastUI : MonoBehaviour
{
    public StormSystem _stormSys;

    public Image[] elem_icons_array;

    [Header("Element Sprites")]
    public Sprite Wind;
    public Sprite Rain;
    public Sprite Hail;
    public Sprite Thunder;

    private void OnEnable()
    {
        StormSystem.OnRandomized += UpdateForecast;
    }

    private void OnDisable()
    {
        StormSystem.OnRandomized -= UpdateForecast;
    }

    private void UpdateForecast()
    {

    }

    void ReadElement()
    {
        
        foreach (var ele in _stormSys.StormType)
        {
            
        }
    }
}
