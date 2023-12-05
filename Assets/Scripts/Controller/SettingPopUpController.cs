using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SettingPopUpController : MonoBehaviour
{
    public static SettingPopUpController instance;
    [SerializeField] private GameObject musicOn, musicOff, soundOn, soundOff;

    public void On_Close_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        CloseThisPopup();
    }

    private void Start()
    {
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

    public void Mute_All()
    {
        SoundManager.Inst.IsMusicOn = false;
        SoundManager.Inst.IsSoundEffectsOn = false;
        if (SoundManager.Inst.IsMusicOn)
            SoundManager.Inst.Play("bg_sound", true);
        else
            SoundManager.Inst.Stop("bg_sound");
    }
    public void Mute_Music()
    {
        SoundManager.Inst.IsMusicOn = false;
        if (SoundManager.Inst.IsMusicOn)
            SoundManager.Inst.Play("bg_sound", true);
        else
            SoundManager.Inst.Stop("bg_sound");
    }
    public void Mute_Sound()
    {
        SoundManager.Inst.IsSoundEffectsOn = false;
    }
    public void Unmute_Music()
    {
        SoundManager.Inst.IsMusicOn = true;
        if (SoundManager.Inst.IsMusicOn)
            SoundManager.Inst.Play("bg_sound", true);
        else
            SoundManager.Inst.Stop("bg_sound");
    }
    public void Unmute_Sound()
    {
        SoundManager.Inst.IsSoundEffectsOn = true;
    }
    public void Unmute_All()
    {
        SoundManager.Inst.IsMusicOn = true;
        SoundManager.Inst.IsSoundEffectsOn = true;
        if (SoundManager.Inst.IsMusicOn)
            SoundManager.Inst.Play("bg_sound", true);
        else
            SoundManager.Inst.Stop("bg_sound");
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

    public void On_Tutorial_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        GameManager.Inst.Show_Popup(GameManager.Popups.HowToPlay);
    }

    public void On_RateUs_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        GameManager.On_Rate_Btn_Click();
    }

    public void On_Share_The_Game_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        GameManager.Inst.Share_Game();
    }

    public void On_Privacy_Policy_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        Application.OpenURL("https://felicitygames.com/privacypolicy.html");
    }

    public void On_Email_Us_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        const string email = "contact@felicitygames.com";
        var subject = MyEscapeURL(Application.productName + " V" + Application.version);
        var body = MyEscapeURL("");
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    private static string MyEscapeURL(string url)
    {
        return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    }

    public void CloseThisPopup()
    {
        //AdsManager.inst.ShowInterstitial("Setting");
        GameManager.activePopup = GameManager.Popups.Null;
        Destroy(gameObject);
    }
}