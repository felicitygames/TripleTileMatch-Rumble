using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HowToPlayPopUpController : MonoBehaviour
{
    [SerializeField] private GameObject pagination;
    [SerializeField] private TextMeshProUGUI instructionText;

    private void Start()
    {
        GeneralRefrencesManager.Inst.World_No_Click_Panel_On_Off(true);
        if (GameManager.activeScreen == GameManager.Screens.GameScreen)
            TimerController.Inst.Cansel_Timer_Invoke();
    }

    public void On_I_Know_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        CloseThisPopup();
    }

    public void On_Close_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        CloseThisPopup();
    }

    public void CloseThisPopup()
    {
        GeneralRefrencesManager.Inst.World_No_Click_Panel_On_Off(false);
        if (GameManager.activeScreen == GameManager.Screens.GameScreen)
            TimerController.Inst.Set_Timer(TimerController.Inst.Second);
        GameManager.activePopup = GameManager.Popups.Null;
        Destroy(gameObject);
    }
}