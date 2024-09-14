using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StormSystem : MonoBehaviour
{
    public List<IDamagable> possibleTarget = new List<IDamagable>();
    public List<IDamagable> selectedTargets = new List<IDamagable>();
    public int min_storm_types = 1, max_storm_types = 4;
    public elements[] StormType;
    public int targets;
    public int minTargets = 5, maxTargets = 10;

    public delegate void StormEvent();
    public static StormEvent OnStormEnd;
    public static StormEvent OnRandomized;

    private void OnEnable()
    {
        DaySystem.OnPrepareEnd += OnPrepareTimeOver;
        DaySystem.OnDayStart += OnNewDay;
    }

    private void OnDisable()
    {
        DaySystem.OnPrepareEnd -= OnPrepareTimeOver;
        DaySystem.OnDayStart -= OnNewDay;
    }

    void OnNewDay()
    {
        RandomizeStorm();
    }

    public void RandomizeStorm()
    {
        var elementToRand = Random.Range(min_storm_types, max_storm_types +1);
        StormType = new elements[elementToRand];
        for (int i = 0; i < elementToRand; i++)
        {
            var eleindex = Random.Range(1, 5);
            StormType[i] = Elements.IntToElement(eleindex);
        }
        
        targets = Random.Range(minTargets, maxTargets + 1);
        OnRandomized?.Invoke();
    }

    public void StormRound()
    {
        FindAllPossibleTarget();

        if (possibleTarget.Count > 0)
        {
            ChooseTarget();
            DealDamage();
            ClearTargetList();
        }

        OnStormEnd?.Invoke();
    }

    void FindAllPossibleTarget()
    {
        var _object = FindObjectsOfType<MonoBehaviour>().OfType<IDamagable>();
        foreach (IDamagable _dmg in _object)
        {
            if (_dmg.isDamagable)
                possibleTarget.Add(_dmg);
        }
    }

    void ChooseTarget()
    {
        for ( int i = 0; i < targets; i++)
        {
            var _index = Random.Range(0, possibleTarget.Count);
            selectedTargets.Add(possibleTarget[_index]);
        }
    }

    void DealDamage()
    {
        foreach(var _dmg in selectedTargets)
        {
            if (!_dmg.isDamagable)
                continue;

            var eleindex = Random.Range(0, StormType.Length);
            _dmg.TakeDamage(StormType[eleindex], 1);
        }
    }
    
    void ClearTargetList()
    {
        possibleTarget.Clear();
        selectedTargets.Clear();
    }

    void OnPrepareTimeOver()
    {
        Transition.FadeInOver += OnPrepareFadedOut;
    }

    void OnPrepareFadedOut()
    {
        StormRound();
        Transition.FadeInOver -= OnPrepareFadedOut;
    }
}
