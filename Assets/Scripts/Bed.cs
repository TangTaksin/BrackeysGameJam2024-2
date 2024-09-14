using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interactable
{
    bool waitingComfirm;

    public GameObject confirmPrompt;
    public float confirmTime = 1f;
    float countdown;

    private void Update()
    {
        confirmPrompt.SetActive(waitingComfirm);

        if (waitingComfirm)
        {
            countdown -=Time.deltaTime;

            if (countdown <= 0)
                waitingComfirm = false;
        }
    }

    public override void Interact(Player _player)
    {
        if (!waitingComfirm)
        {
            waitingComfirm = true;
            countdown = confirmTime;
        }
        else
        {
            DaySystem.OnTimeOut?.Invoke();
            _player.stamina_penely = false;
        }
    }
}
