using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.UI; // Include UI namespace for Slider

public class DayNightCycle : MonoBehaviour
{
    public Light2D globalLight; // The global light for the environment
    public Light2D playerSpotlight; // The player's spotlight
    [SerializeField] private Gradient dayNightGradient; // Gradient to transition between day and night
    public float dayDuration = 40f; // Duration of daytime in seconds
    public float nightDuration = 20f; // Duration of nighttime in seconds
    public float spotlightMaxIntensity = 1f; // Maximum intensity of the player's spotlight
    public TextMeshProUGUI clockText; // TextMeshPro component to display the time
    public Slider timeSlider; // UI Slider to synchronize with time
    public float timeScale = 1f; // Factor to adjust the speed of time

    private float timer; // Timer to keep track of the time of day
    private float totalCycleDuration; // Total duration of a complete day-night cycle
    private bool isCycleStopped; // Flag to indicate when the cycle should stop

    void Start()
    {
        if (globalLight == null)
        {
            globalLight = GetComponent<Light2D>(); // Assign global light if not set
        }

        if (playerSpotlight == null)
        {
            Debug.LogWarning("Player Spotlight not assigned!"); // Warning if spotlight is missing
        }

        if (clockText == null)
        {
            Debug.LogWarning("Clock TextMeshPro component not assigned!"); // Warning if clock text is missing
        }

        if (timeSlider == null)
        {
            Debug.LogWarning("Time Slider not assigned!"); // Warning if slider is missing
        }

        timer = 0f;
        totalCycleDuration = dayDuration + nightDuration; // Calculate total duration of the cycle
        isCycleStopped = false; // Start with the cycle running
        playerSpotlight.intensity = 0f; // Initialize spotlight intensity
        playerSpotlight.enabled = false; // Ensure spotlight is off initially

        // Initialize slider
        if (timeSlider != null)
        {
            timeSlider.minValue = 0f;
            timeSlider.maxValue = 1f;
            timeSlider.value = 0f; // Start slider at 0
            timeSlider.onValueChanged.AddListener(OnSliderValueChanged); // Add listener to handle slider changes
        }
    }

    void Update()
    {
        if (isCycleStopped)
        {
            return; // Skip updating if the cycle has stopped
        }

        timer += Time.deltaTime * timeScale; // Update timer based on time scale

        float timeOfDay = Mathf.PingPong(timer / totalCycleDuration, 1f); // Calculate time of day

        globalLight.color = dayNightGradient.Evaluate(timeOfDay); // Update global light color based on gradient

        UpdateLightAndSpotlight(timeOfDay); // Adjust light and spotlight based on time of day

        UpdateClock(timeOfDay); // Update the clock display

        // Update the slider value
        if (timeSlider != null)
        {
            timeSlider.value = timeOfDay; // Sync slider with time of day
        }

        if (timer >= totalCycleDuration)
        {
            isCycleStopped = true; // Stop the cycle when reaching the end of the duration
            timer = totalCycleDuration; // Ensure timer is set to the end value
        }
    }

    private void UpdateLightAndSpotlight(float timeOfDay)
    {
        float dayProgress = Mathf.Clamp01(timeOfDay / 0.5f); // Progress within the day phase
        float nightProgress = Mathf.Clamp01((timeOfDay - 0.5f) / 0.5f); // Progress within the night phase

        if (timeOfDay < 0.5f)
        {
            globalLight.intensity = Mathf.Lerp(0.7f, 0.8f, dayProgress); // Adjust global light intensity for daytime
            playerSpotlight.enabled = false; // Turn off the spotlight during the day
        }
        else
        {
            globalLight.intensity = Mathf.Lerp(0.8f, 0.1f, nightProgress); // Adjust global light intensity for nighttime
            if (!playerSpotlight.enabled)
            {
                playerSpotlight.enabled = true; // Enable the spotlight if not already enabled
            }
            playerSpotlight.intensity = Mathf.Lerp(playerSpotlight.intensity, spotlightMaxIntensity, Time.deltaTime * 1.5f); // Smoothly increase spotlight intensity
        }
    }

    private void UpdateClock(float timeOfDay)
    {
        float startHour = 7f; // Start time of the day phase
        float endHour = 19f; // End time of the day phase
        float nightStartHour = 19.01f; // Start time of the night phase
        float nightEndHour = 24f; // End time of the night phase

        float currentHour;
        if (timeOfDay < 0.5f)
        {
            currentHour = Mathf.Lerp(startHour, endHour, timeOfDay * 2f); // Calculate current hour during the day phase
        }
        else
        {
            currentHour = Mathf.Lerp(nightStartHour, nightEndHour, (timeOfDay - 0.5f) * 2f); // Calculate current hour during the night phase
        }

        int hours = Mathf.FloorToInt(currentHour); // Extract hours
        int minutes = Mathf.FloorToInt((currentHour - hours) * 60f); // Extract minutes

        string timeString = $"{hours:D2}:{minutes:D2}"; // Format time as HH:MM

        if (clockText != null)
        {
            clockText.text = timeString; // Update the clock text
        }
    }

    // Method to handle changes to the slider value
    private void OnSliderValueChanged(float value)
    {
        timer = value * totalCycleDuration; // Adjust the timer based on the slider value
    }
}
