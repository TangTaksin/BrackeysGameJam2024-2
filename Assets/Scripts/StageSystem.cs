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

    bool failed;

    public delegate void StageEvent(bool isLastDay);
    public static event StageEvent OnReset;

    private void OnEnable()
    {
        DaySystem.OnDayStart += OnDayStart;
        InitializeDay();
    }

    private void OnDisable()
    {
        DaySystem.OnDayStart -= OnDayStart;
    }

    private void InitializeDay()
    {
        currentDay = 1;
        UpdateDayText();
    }

    public void OnDayStart()
    {
        if (failed)
            ResetProgress();
    }

    public (bool, bool) HandleDayEnd()
    {
        failed = plantGroup.CheckFailStatus();
        bool isLastDay = false;

        if (!failed)
        {
            isLastDay = TryAdvanceDay();
        }

        return (failed, isLastDay);
    }

    private void ResetProgress()
    {
        failed = false;
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
