using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StormSystem : MonoBehaviour
{
    public List<IDamagable> possibleTarget = new List<IDamagable>();
    public List<IDamagable> selectedTargets = new List<IDamagable>();
    public elements[] StormType;
    public int targets;

    public void StormRound()
    {
        FindAllPossibleTarget();

        if (possibleTarget.Count > 0)
        {
            ChooseTarget();
            DealDamage();
            ClearTargetList();
        }
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
            print("iteration : " + i);
            var _index = Random.Range(0, possibleTarget.Count);
            selectedTargets.Add(possibleTarget[_index]);
        }
    }

    void DealDamage()
    {
        foreach(var _dmg in selectedTargets)
        {
            var eleindex = Random.Range(0, StormType.Length);

            _dmg.TakeDamage(StormType[eleindex], 1);
        }
    }
    
    void ClearTargetList()
    {
        possibleTarget.Clear();
        selectedTargets.Clear();
    }
}
