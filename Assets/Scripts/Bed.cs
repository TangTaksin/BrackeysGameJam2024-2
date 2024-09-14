using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interactable
{
    bool waitingComfirm;
    bool confirmed;

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
        if (confirmed)
        {
            DaySystem.OnTimeOut?.Invoke();
        }

        if (!waitingComfirm)
        {
            waitingComfirm = true;
            countdown = confirmTime;
        }
        else
        {
            confirmed = true;
        }
    }
}
