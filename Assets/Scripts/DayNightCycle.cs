using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.UI;
using System;

public class DayNightCycle : MonoBehaviour
{
    public Light2D globalLight; // Global light for the environment
    public Light2D playerSpotlight; // Player's spotlight
    [SerializeField] private Gradient dayNightGradient; // Gradient for day-night transition
    public float dayDuration = 40f; // Duration of daytime in seconds
    public float nightDuration = 20f; // Duration of nighttime in seconds
    public float spotlightMaxIntensity = 1f; // Maximum intensity of the player's spotlight
    public TextMeshProUGUI clockText; // TextMeshPro component for the time display
    public Image timeFillImage; // UI Image for time progress
    public float timeScale = 1f; // Speed of time progression

    private float timer; // Timer for tracking the time of day
    private float totalCycleDuration; // Total duration of the day-night cycle
    private bool isCycleStopped; // Flag to indicate if the cycle is stopped

    // Reference to DaySystem
    public DaySystem daySystem;

    void Start()
    {
        if (globalLight == null)
        {
            globalLight = GetComponent<Light2D>();
        }

        if (playerSpotlight == null)
        {
            Debug.LogWarning("Player Spotlight not assigned!");
        }

        if (clockText == null)
        {
            Debug.LogWarning("Clock TextMeshPro component not assigned!");
        }

        if (timeFillImage == null)
        {
            Debug.LogWarning("Time Fill Image not assigned!");
        }

        timer = 0f;
        totalCycleDuration = dayDuration + nightDuration;
        isCycleStopped = true; // Start with the cycle stopped
        playerSpotlight.intensity = 0f;
        playerSpotlight.enabled = false;
        globalLight.intensity = .7f;

        // Subscribe to DaySystem events
        DaySystem.OnDayStart += StartCycle;
        DaySystem.OnDayEnd += StopCycle;
        DaySystem.OnTimeOut += TriggerTimeOver;
        DaySystem.OnPrepareEnd += TriggerNextDay;
    }

    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        DaySystem.OnDayStart -= StartCycle;
        DaySystem.OnDayEnd -= StopCycle;
        DaySystem.OnTimeOut -= TriggerTimeOver;
        DaySystem.OnPrepareEnd -= TriggerNextDay;
    }

    void Update()
    {
        if (isCycleStopped)
        {
            return;
        }

        timer += Time.deltaTime * timeScale;
        float timeOfDay = Mathf.PingPong(timer / totalCycleDuration, 1f);

        globalLight.color = dayNightGradient.Evaluate(timeOfDay);
        UpdateLightAndSpotlight(timeOfDay);
        UpdateClock(timeOfDay);

        if (timeFillImage != null)
        {
            timeFillImage.fillAmount = timeOfDay;
        }

        if (timer >= totalCycleDuration)
        {
            isCycleStopped = true;
            timer = totalCycleDuration;

            // Trigger DaySystem events when the cycle ends
            DaySystem.OnTimeOut?.Invoke();
            DaySystem.OnDayEnd?.Invoke();
        }
    }

    private void UpdateLightAndSpotlight(float timeOfDay)
    {
        float dayProgress = Mathf.Clamp01(timeOfDay / 0.5f);
        float nightProgress = Mathf.Clamp01((timeOfDay - 0.5f) / 0.5f);

        if (timeOfDay < 0.5f)
        {
            globalLight.intensity = Mathf.Lerp(0.7f, 0.8f, dayProgress);
            playerSpotlight.enabled = false;
        }
        else
        {
            globalLight.intensity = Mathf.Lerp(0.8f, 0.1f, nightProgress);
            if (!playerSpotlight.enabled)
            {
                playerSpotlight.enabled = true;
            }
            playerSpotlight.intensity = Mathf.Lerp(playerSpotlight.intensity, spotlightMaxIntensity, Time.deltaTime * 1.5f);
        }
    }

    private void UpdateClock(float timeOfDay)
    {
        float startHour = 7f;
        float endHour = 19f;
        float nightStartHour = 19.01f;
        float nightEndHour = 24f;

        float currentHour = timeOfDay < 0.5f
            ? Mathf.Lerp(startHour, endHour, timeOfDay * 2f)
            : Mathf.Lerp(nightStartHour, nightEndHour, (timeOfDay - 0.5f) * 2f);

        int hours = Mathf.FloorToInt(currentHour);
        int minutes = Mathf.FloorToInt((currentHour - hours) * 60f);

        string timeString = $"{hours:D2}:{minutes:D2}";

        if (clockText != null)
        {
            clockText.text = timeString;
        }
    }

    public void RestartCycle()
    {
        Player.ChangePlayerCanActBool?.Invoke(true);
        isCycleStopped = false;
        timer = 0f;
    }

    public void StartCycle()
    {
        // Start the day-night cycle
        isCycleStopped = false;
        timer = 0f;
        Debug.Log("Day Started: Restarting Day-Night Cycle");
    }

    public void StopCycle()
    {
        // Stop the day-night cycle
        isCycleStopped = true;
        Debug.Log("Day Ended: Stopping Day-Night Cycle");
    }

    public void TriggerTimeOver()
    {
        // Stop the cycle immediately when time runs out
        isCycleStopped = true;
        Debug.Log("Time Over: Stopping Day-Night Cycle");
    }

    public void TriggerNextDay()
    {
        // Restart the cycle when the next day starts
        RestartCycle();
        Debug.Log("Preparing for the Next Day: Restarting Cycle");
    }
}
