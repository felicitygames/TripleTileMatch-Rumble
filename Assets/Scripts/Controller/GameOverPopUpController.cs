using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverPopUpController : SingletonComponent<GameOverPopUpController>
{
    public static GameOverPopUpController instance;
    [SerializeField] Text titalText, coinText;

    internal void SetText(string gameOverTital)
    {
        titalText.text = gameOverTital;
    }

    public void Update()
    {
        if(PlayerPrefs.HasKey("GameOverReward")){
            PlayerPrefs.DeleteKey("GameOverReward");
            if(PlayerPrefs.GetInt("InitialMusic") == 1){
            SettingPopUpController.instance.Unmute_Music();
            }
            if(PlayerPrefs.GetInt("InitialSound") == 1){
            SettingPopUpController.instance.Unmute_Sound();
            }
            Give_Reward();    
        }
        if(PlayerPrefs.HasKey("CancelGameOverReward")){
            PlayerPrefs.DeleteKey("CancelGameOverReward");
            if(PlayerPrefs.GetInt("InitialMusic") == 1){
            SettingPopUpController.instance.Unmute_Music();
            }
            if(PlayerPrefs.GetInt("InitialSound") == 1){
            SettingPopUpController.instance.Unmute_Sound();
            }
        }
    }

    private void Start()
    {
        if (GeneralDataManager.GameData.LevelNo % 2 == 0)
            ////AdsManager.inst.RequestAndLoadInterstitialAd();
        SoundManager.Inst.Play("game_over");
        GeneralRefrencesManager.Inst.World_No_Click_Panel_On_Off(true);
        TimerController.Inst.Cansel_Timer_Invoke();
        Set_text();
    }

    private void Set_text()
    {
        coinText.text = GeneralDataManager.GameData.Coins.ToString();
        var parent = coinText.transform.parent.GetComponent<RectTransform>();
        parent.sizeDelta = new Vector2(coinText.preferredWidth + 100, parent.sizeDelta.y);
    }

    public void On_Home_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();

        //AdsManager.inst.ShowInterstitial("LevelComplete");
        Home_Btn_Click();
    }

    private void Home_Btn_Click()
    {
        GlanceAds.EndAnalytics(GeneralDataManager.GameData.LevelNo);
        GeneralRefrencesManager.Inst.Clear_Level();
        GameManager.Inst.Show_Screen(GameManager.Screens.HomeScreen);
        CloseThisPopup();
    }

    public void On_Restart_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        //AdsManager.inst.ShowInterstitial("LevelComplete");
        Restart_Btn_Click();
    }

    private void Restart_Btn_Click()
    {
        GlanceAds.EndAnalytics(GeneralDataManager.GameData.LevelNo);
        GlanceAds.ReplayAnalytics(GeneralDataManager.GameData.LevelNo);
        GlanceAds.LevelAnalytics(GeneralDataManager.GameData.LevelNo);
        GeneralRefrencesManager.Inst.Clear_Level();
        GridManager.Inst.Generate_Grid(GeneralDataManager.GameData.LevelNo);
        CloseThisPopup();
    }

    public void On_Watch_Ad_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        GlanceAds.RewardedAdsAnalytics("GameOverReward","CancelGameOverReward");
        GlanceAds.RewardedAd("GameOverReplay");
        //AdsManager.inst.LoadAndShow_RewardVideo("GameOver");
    }

    internal void Give_Reward()
    {
        Resat_Game_Play_For_Game_Over();
    }

    public void On_Coin_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        if (GeneralDataManager.GameData.Coins >= 120)
        {
            GameManager.Decrease_Coin(120);
            Resat_Game_Play_For_Game_Over();
        }
        else
        {
            GameManager.Inst.Make_Toast("You don't have enough coins. ");
        }
    }

    void Resat_Game_Play_For_Game_Over()
    {
        TimerController.Inst.Set_Timer(TimerController.Inst.Second);
        if (titalText.text == "Time up !")
        {
            TimerController.Inst.Second = 30;
            TimerController.Inst.Set_Timer(TimerController.Inst.Second);
        }
        else
        {
            GamePlayUIController.Inst.StartCoroutine(GamePlayUIController.Inst.Undo_Element_For_Game_Over_Watch_Ad());
        }

        CloseThisPopup();
    }

    public void CloseThisPopup()
    {
        GeneralRefrencesManager.Inst.World_No_Click_Panel_On_Off(false);
        GameManager.activePopup = GameManager.Popups.Null;
        Destroy(gameObject);
    }
}