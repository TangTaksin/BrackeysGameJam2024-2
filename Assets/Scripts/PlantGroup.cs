using UnityEngine;

public class PlantGroup : MonoBehaviour
{
    public Plant[] plantGroup;

    [Range(0, 100)] public float FailThreshold;
    private bool _failed;
    private bool _hasCheckedFailStatus = false; // Track if fail status has been checked

    private float _baseIntegrity;
    private float _integrity;

    public int _damagedCount;
    public int _deadCount;
    public int _PreventedCount;

    private void OnEnable()
    {
        StormSystem.OnStormEnd += UpdateOverall;
        Plant.OnDamage += DamageCheck;
        DaySystem.OnDayStart += ResetStatistic;
        DaySystem.OnDayStart += ResetFailStatusCheck; // Reset fail status check at the start of each day
    }

    private void OnDisable()
    {
        StormSystem.OnStormEnd -= UpdateOverall;
        Plant.OnDamage -= DamageCheck;
        DaySystem.OnDayStart -= ResetStatistic;
        DaySystem.OnDayStart -= ResetFailStatusCheck;
    }

    public void GetDefaultIntegrity()
    {
        _baseIntegrity = 0;
        foreach (var plant in plantGroup)
        {
            _baseIntegrity += plant.plantdata.baseHitPoint;
        }
    }

    public void CheckOverallIntegrity()
    {
        _integrity = 0;
        foreach (var plant in plantGroup)
        {
            _integrity += plant.hitPoints;
        }
    }

    public void UpdateOverall()
    {
        GetDefaultIntegrity();
        CheckOverallIntegrity();
    }

    public float GetOverallStatus()
    {
        UpdateOverall();
        // Prevent division by zero
        if (_baseIntegrity == 0)
            return 0;

        return _integrity / _baseIntegrity;
    }

    public bool CheckFailStatus()
    {
        if (_hasCheckedFailStatus)
        {
            Debug.Log("Fail status already checked for today.");
            return _failed;
        }

        var status = GetOverallStatus();
        Debug.Log($"CheckFailStatus - Status: {status * 100}, Fail Threshold: {FailThreshold}, Failed: {status * 100 < FailThreshold}");

        // Compare the status directly with the FailThreshold
        _failed = (status * 100 < FailThreshold);
        _hasCheckedFailStatus = true; // Mark as checked
        return _failed;
    }

    private void ResetFailStatusCheck()
    {
        _hasCheckedFailStatus = false; // Reset for the new day
    }

    void DamageCheck(Plant _plant, int _takenDamage, bool _resisted, bool _proteted, bool _died, bool _attacked)
    {
        if (_takenDamage > 0)
            _damagedCount++;

        if (_attacked)
            _damagedCount--;

        if (_proteted)
            _PreventedCount++;

        if (_died)
        {
            _deadCount++;
        }
    }

    void ResetStatistic()
    {
        _damagedCount = 0;
        _PreventedCount = 0;
        _deadCount = 0;
    }
}
