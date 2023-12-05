using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : SingletonComponent<TimerController>
{
    [SerializeField] private Text timer;

    private void Start()
    {
        Set_Timer(Second);
    }

    internal int Second = 200;
    internal int startsec =0;

    internal void Set_Timer(int sec)
    {
        startsec = sec;
        Cansel_Timer_Invoke();
        var availableTime = TimeSpan.FromSeconds(sec);
        timer.text = availableTime.Minutes.ToString("d2") + ":" + availableTime.Seconds.ToString("d2");
        InvokeRepeating(nameof(Start_Countdown), 0.4f, 1f);
    }

    internal void Start_Countdown()
    {
        Second--;
        if (Second >= 0)
        {
            var availableTime = TimeSpan.FromSeconds(Second);
            timer.text = availableTime.Minutes.ToString("d2") + ":" + availableTime.Seconds.ToString("d2");
        }
        else
        {
            Cansel_Timer_Invoke();
            GameManager.Inst.Show_Popup(GameManager.Popups.GameOver);
            GameOverPopUpController.Inst.SetText("Time up !");
        }
    }

    internal void Cansel_Timer_Invoke()
    {
        CancelInvoke(nameof(Start_Countdown));
    }
}
