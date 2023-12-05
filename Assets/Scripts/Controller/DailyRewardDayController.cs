using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardDayController : MonoBehaviour
{
    [SerializeField] private Text dayText;
    [SerializeField] private Image myImage;

    internal void Set_Day_Text(string day)
    {
        dayText.text = day;
    }

    internal void Set_My_Image(Sprite sprite)
    {
        myImage.sprite = sprite;
    }

}
