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
    public Image timeFillImage; // Reference to the Image component for fill amount
    public float timeScale = 1f;

    public DaySystem daySystem; // Reference to the DaySystem component

    private float timer;
    private float totalCycleDuration;
    private bool isCycleStopped;

    void Start()
    {
        if (globalLight == null)
        {
            globalLight = GetComponent<Light2D>();
        }

        if (timeFillImage != null)
        {
            timeFillImage.fillAmount = 0f; // Start fill image at 0
        }

        timer = 0f;
        totalCycleDuration = dayDuration + nightDuration;

        if (totalCycleDuration <= 0)
        {
            Debug.LogError("DayNightCycle: totalCycleDuration is zero or negative.");
            totalCycleDuration = 1f; // Prevent division by zero
        }

        playerSpotlight.intensity = 0f;
        playerSpotlight.enabled = false;
        globalLight.intensity = 0.7f;

        // Start the initial cycle
        StartCycle();
    }

    void OnEnable()
    {
        DaySystem.OnDayStart += StartCycle;
    }

    void OnDisable()
    {
        DaySystem.OnDayStart -= StartCycle;

    }

    void Update()
    {
        if (isCycleStopped || daySystem == null)
        {
            return;
        }

        timer += Time.deltaTime * timeScale;
        float normalizedTime = Mathf.Repeat(timer / totalCycleDuration, 1f);

        globalLight.color = dayNightGradient.Evaluate(normalizedTime);
        UpdateLightAndSpotlight(normalizedTime);
        UpdateClock(normalizedTime);

        if (timeFillImage != null)
        {
            timeFillImage.fillAmount = Mathf.Clamp01(normalizedTime);
        }

        if (timer >= totalCycleDuration)
        {
            EndCycle();
        }
    }

    private void UpdateLightAndSpotlight(float normalizedTime)
    {
        float dayProgress = Mathf.Clamp01(normalizedTime / 0.5f);
        float nightProgress = Mathf.Clamp01((normalizedTime - 0.5f) / 0.5f);

        if (normalizedTime < 0.5f)
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

    private void UpdateClock(float normalizedTime)
    {
        float startHour = 7f;
        float endHour = 19f;
        float nightStartHour = 19.01f;
        float nightEndHour = 24f;

        float currentHour = normalizedTime < 0.5f
            ? Mathf.Lerp(startHour, endHour, normalizedTime * 2f)
            : Mathf.Lerp(nightStartHour, nightEndHour, (normalizedTime - 0.5f) * 2f);

        int hours = Mathf.FloorToInt(currentHour);
        int minutes = Mathf.FloorToInt((currentHour - hours) * 60f);
        string timeString = $"{hours:D2}:{minutes:D2}";

        if (clockText != null)
        {
            clockText.text = timeString;
        }
    }

    public void StartCycle()
    {
        Player.ChangePlayerCanActBool?.Invoke(true);
        isCycleStopped = false;
        timer = 0f;
        Debug.Log("Day-Night Cycle Started.");
    }

    public void EndCycle()
    {
        DaySystem.OnTimeOut?.Invoke();
        isCycleStopped = true;
        Debug.Log("Day-Night Cycle Ended.");

    }
}
