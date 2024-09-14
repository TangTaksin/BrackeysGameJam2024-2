using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour
{
    public Light2D globalLight;
    public Light2D playerSpotlight;
    [SerializeField] private Gradient dayNightGradient;
    public float dayDuration = 40f;
    public float nightDuration = 20f;
    public float spotlightMaxIntensity = 1f;
    public TextMeshProUGUI clockText;
    public Image timeFillImage;
    public float timeScale = 1f;

    private float timer;
    private float totalCycleDuration;
    private bool isCycleStopped;

    public DaySystem daySystem;

    void Start()
    {
        if (globalLight == null)
        {
            globalLight = GetComponent<Light2D>();
        }

        if (daySystem == null)
        {
            Debug.LogError("DaySystem is not assigned!");
        }
        else
        {
            DaySystem.OnDayStart += StartCycle;
            DaySystem.OnDayEnd += StopCycle;
            DaySystem.OnTimeOut += TriggerTimeOver;
            DaySystem.OnPrepareEnd += TriggerNextDay;
        }

        timer = 0f;
        totalCycleDuration = dayDuration + nightDuration;
        playerSpotlight.intensity = 0f;
        playerSpotlight.enabled = false;
        globalLight.intensity = 0.7f;
    }

    void OnDestroy()
    {
        if (daySystem != null)
        {
            DaySystem.OnDayStart -= StartCycle;
            DaySystem.OnDayEnd -= StopCycle;
            DaySystem.OnTimeOut -= TriggerTimeOver;
            DaySystem.OnPrepareEnd -= TriggerNextDay;
        }
    }

    void Update()
    {
        if (isCycleStopped)
        {
            return;
        }

        // Increment timer based on the time scale
        timer += Time.deltaTime * timeScale;

        // Calculate time of day as a PingPong between 0 and 1
        float timeOfDay = Mathf.PingPong(timer / totalCycleDuration, 1f);

        // Update global light and spotlight based on time of day
        globalLight.color = dayNightGradient.Evaluate(timeOfDay);
        UpdateLightAndSpotlight(timeOfDay);
        UpdateClock(timeOfDay);

        // Update the fill amount (progress) from 0 to 1
        if (timeFillImage != null)
        {
            timeFillImage.fillAmount = Mathf.Clamp01(timer / totalCycleDuration);
        }

        // Check if the cycle has completed
        if (timer >= totalCycleDuration)
        {
            isCycleStopped = true;
            timer = totalCycleDuration;

            // Trigger events for the end of the day
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
        Debug.Log("RestartCycle: Restarting the day-night cycle.");
        Player.ChangePlayerCanActBool?.Invoke(true);
        isCycleStopped = false;
        timer = 0f;
    }

    public void StartCycle()
    {
        Debug.Log("StartCycle: Starting the day-night cycle.");
        isCycleStopped = false;
        timer = 0f;
    }

    public void StopCycle()
    {
        Debug.Log("StopCycle: Stopping the day-night cycle.");
        isCycleStopped = true;
    }

    public void TriggerTimeOver()
    {
        Debug.Log("TriggerTimeOver: Time over. Stopping the day-night cycle.");
        isCycleStopped = false;
    }

    public void TriggerNextDay()
    {
        Debug.Log("TriggerNextDay: Preparing for the next day.");
        RestartCycle();
    }
}
