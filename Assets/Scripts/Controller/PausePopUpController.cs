using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePopUpController : MonoBehaviour
{
    [SerializeField] private GameObject musicOn, musicOff, soundOn, soundOff;
    public static PausePopUpController instance;

    private void Start()
    {
        HomeButtonManager.instance.CheckHome();
        GeneralRefrencesManager.Inst.World_No_Click_Panel_On_Off(true);
        TimerController.Inst.Cansel_Timer_Invoke();
        ComboSliderController.Inst.Set_Tween(ComboSliderController.TweenState.Pause);
        GamePlayUIController.Inst.Pause_Play_Freeze_Tween(true);

        musicOn.SetActive(SoundManager.Inst.IsMusicOn);
        musicOff.SetActive(!SoundManager.Inst.IsMusicOn);
        soundOn.SetActive(SoundManager.Inst.IsSoundEffectsOn);
        soundOff.SetActive(!SoundManager.Inst.IsSoundEffectsOn);

    }


    public void On_Sound_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        SoundManager.Inst.IsSoundEffectsOn = !SoundManager.Inst.IsSoundEffectsOn;
        soundOn.SetActive(SoundManager.Inst.IsSoundEffectsOn);
        soundOff.SetActive(!SoundManager.Inst.IsSoundEffectsOn);
    }

    public void On_Music_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        SoundManager.Inst.IsMusicOn = !SoundManager.Inst.IsMusicOn;
        if (SoundManager.Inst.IsMusicOn)
            SoundManager.Inst.Play("bg_sound", true);
        else
            SoundManager.Inst.Stop("bg_sound");
        musicOn.SetActive(SoundManager.Inst.IsMusicOn);
        musicOff.SetActive(!SoundManager.Inst.IsMusicOn);
    }

    public void On_Close_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        CloseThisPopup();
    }


    public void On_Tutorial_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        GameManager.Inst.Show_Popup(GameManager.Popups.HowToPlay);
    }

    public void On_Home_Btn_Click()
    {   
        GameManager.Play_Button_Click_Sound();
        GeneralRefrencesManager.Inst.Clear_Level();
        GameManager.Inst.Show_Screen(GameManager.Screens.HomeScreen);
        CloseThisPopup();
    }

    public void On_Retry_Btn_Click()
    {  
        GameManager.Play_Button_Click_Sound();
        CloseThisPopup();
        GeneralRefrencesManager.Inst.Clear_Level();
        GridManager.Inst.Generate_Grid(GeneralDataManager.GameData.LevelNo);
    }

     public void AfterRetryAd()
    {   
        
    }

    public void CloseThisPopup()
    {
        if(GeneralDataManager.GameData.LevelNo <= 1){
            if(GamePlayUIController.instance != null)
            GamePlayUIController.instance.Highlight.SetActive(false);
            GeneralRefrencesManager.Inst.World_No_Click_Panel_On_Off(false);
            GameManager.activePopup = GameManager.Popups.Null;
            Destroy(gameObject);
        }
        else{
            HomeButtonManager.instance.CheckHome();
            TimerController.Inst.Set_Timer(TimerController.Inst.Second);
            GeneralRefrencesManager.Inst.World_No_Click_Panel_On_Off(false);
            ComboSliderController.Inst.Set_Tween(ComboSliderController.TweenState.Play);
            GamePlayUIController.Inst.Pause_Play_Freeze_Tween(false);
            GameManager.activePopup = GameManager.Popups.Null;
            Destroy(gameObject);
        }
    }
}
