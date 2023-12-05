using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPopUpController : MonoBehaviour
{
    private void Start()
    {
        ////AdsManager.inst.ShowRewardVideo();
    }
    public void On_Yes_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        Application.Quit();
    }

    public void On_No_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        CloseThisPopup();
    }

    public void CloseThisPopup()
    {
        GameManager.activePopup = GameManager.Popups.Null;
        Destroy(gameObject);
    }
}
