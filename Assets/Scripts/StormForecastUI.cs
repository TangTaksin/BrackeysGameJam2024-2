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

    private void UpdateForecast()
    {
        var elementCount = _stormSys.StormType.Length;
        for (int i = 0; i < elem_icons_array.Length; i++)
        {
            if (i > elementCount - 1) 
            {
                elem_icons_array[i].gameObject.SetActive(false);
                continue; 
            }

            elem_icons_array[i].gameObject.SetActive(true);
            

        }
    }
}
