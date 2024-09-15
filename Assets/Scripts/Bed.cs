using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bed : Interactable
{
    bool holding;
    bool progressHit;

    public Image confirmPrompt;
    public float progressTime = 1f;
    float progress;

    private void Update()
    {
        confirmPrompt.gameObject.SetActive(progress > 0);
        confirmPrompt.fillAmount = progress / progressTime;

        if (holding)
        {
            progress += Time.deltaTime;
            progressHit = (progress >= progressTime);

            if (progressHit)
            {
                progress = 0;
                holding = false;
                DaySystem.OnTimeOut?.Invoke();
            }

        }
        else
            progress -= Time.deltaTime;

        if (progress <= 0)
            progress = 0;

    }

    public override void Interact(Player _player)
    {
        holding = _player._holdingButton;

    }
}
