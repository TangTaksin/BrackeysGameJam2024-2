using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StageSystem : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private PlantGroup plantGroup;
    [SerializeField] private int dayLimit = 3;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI dayTMP;

    private int currentDay;

    public delegate void StageEvent(bool isLastDay);
    public static event StageEvent OnReset;
    public static event StageEvent OnLastDay;

    private void OnEnable()
    {
        DaySystem.OnDayEnd += HandleDayEnd;
        InitializeDay();
    }

    private void OnDisable()
    {
        DaySystem.OnDayEnd -= HandleDayEnd;
    }

    private void InitializeDay()
    {
        currentDay = 1;
        UpdateDayText();
    }

    private void HandleDayEnd()
    {
        bool failed = plantGroup.CheckFailStatus();

        if (failed)
        {
            ResetProgress(); // Reset to day 1 if failed
        }
        else
        {
            bool isLastDay = TryAdvanceDay();

            if (isLastDay)
            {
                LoadEndScene(); 
            }
            else
            {
                OnLastDay?.Invoke(isLastDay);
            }
        }
    }

    private void ResetProgress()
    {
        currentDay = 1; // Reset to day 1
        UpdateDayText();
        OnReset?.Invoke(true);
    }

    private bool TryAdvanceDay()
    {
        if (currentDay < dayLimit)
        {
            currentDay++;
            UpdateDayText();
            return false; // Not the last day
        }
        return true; // It's the last day
    }

    private void UpdateDayText()
    {
        if (dayTMP != null)
        {
            if (currentDay >= dayLimit)
            {
                dayTMP.text = "LAST DAY";
            }
            else
            {
                dayTMP.text = $"DAY {currentDay}";
            }
        }
    }

    private void LoadEndScene()
    {
        SceneManager.LoadScene("EndScene");
    }
}
