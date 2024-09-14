using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StormForecastUI : MonoBehaviour
{
    public StormSystem _stormSys;

    public Image[] elem_icons_array;
    public TextMeshProUGUI[] elem_perce_array;

    List<elements> elementPool = new List<elements>();
    List<float> ele_percent = new List<float>();

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
        ReadElement();
        FindElementPercentage();
        DisplayElementInfo();
    }

    void ReadElement()
    {
        elementPool.Clear();
        ele_percent.Clear();

        foreach (var ele in _stormSys.StormType)
        {
            var ele_index = 0;
            if (!(elementPool.Contains(ele)))
            {
                elementPool.Add(ele);
                ele_percent.Add(1);
            }
            else
            {
                ele_index = elementPool.IndexOf(ele);
                ele_percent[ele_index]++;
            }
        }
    }

    void FindElementPercentage()
    {
        for (int i = 0; i < ele_percent.Count; i++)
        {
            ele_percent[i] = ele_percent[i] / _stormSys.StormType.Length;
        }
    }

    void DisplayElementInfo()
    {
        for (int i = 0; i < elem_icons_array.Length; i++)
        {
            var _in = (i < elementPool.Count);
            elem_icons_array[i].gameObject.SetActive(_in);

            if (!_in)
                continue;

            elem_icons_array[i].sprite = SetElementSprite(elementPool[i]);
            elem_perce_array[i].text = string.Format("{0}%", (int)(ele_percent[i] * 100));
        }


    }

    Sprite SetElementSprite(elements _ele)
    {
        var sprite = Wind;

        switch (_ele)
        {
            case elements.Wind: sprite = Wind; break;
            case elements.Rain: sprite = Rain; break;
            case elements.Hail: sprite = Hail; break;
            case elements.Thunder: sprite = Thunder; break;
        }

        return sprite;
    }
}
