using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public bool isDamagable
    {
        get;
        set;
    }

    public void TakeDamage(elements element, int damage);
}
