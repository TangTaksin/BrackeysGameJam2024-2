using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interactable
{
    public override void Interact(Player _player)
    {
        DaySystem.OnTimeOver?.Invoke();
    }
}
