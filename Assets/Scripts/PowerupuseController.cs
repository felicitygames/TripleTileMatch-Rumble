using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class PowerupuseController : SingletonComponent<ShopPopUpController>
{
    public static PowerupuseController instance;
    public int PowerIndex;
    public GameObject CoinsButton;
    public Text Coins;
    private void Update()
    {
        if(PlayerPrefs.HasKey("PowerUpReward")){
            PlayerPrefs.DeleteKey("PowerUpReward");
            if(PlayerPrefs.GetInt("InitialMusic") == 1){
            SettingPopUpController.instance.Unmute_Music();
            }
            if(PlayerPrefs.GetInt("InitialSound") == 1){
            SettingPopUpController.instance.Unmute_Sound();
            }
            UsePower(GamePlayUIController.instance.PowerIndex);
        }
        if(PlayerPrefs.HasKey("PowerUpReward")){
            PlayerPrefs.DeleteKey("PowerUpReward");
            if(PlayerPrefs.GetInt("InitialMusic") == 1){
            SettingPopUpController.instance.Unmute_Music();
            }
            if(PlayerPrefs.GetInt("InitialSound") == 1){
            SettingPopUpController.instance.Unmute_Sound();
            }
            UsePower(GamePlayUIController.instance.PowerIndex);
        }
        if(PlayerPrefs.HasKey("CancelPowerUpReward")){
            PlayerPrefs.DeleteKey("CancelPowerUpReward");
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
        GeneralDataManager.Save_Data();
        CoinsButton.GetComponent<Button>().interactable = true;
        
        Coins.text = PlayerPrefs.GetInt("Coins").ToString();
    }
    public void ContinueWithAd()
    {
        GlanceAds.RewardedAdsAnalytics("PowerUpReward","CancelPowerUpReward");
        GlanceAds.RewardedAd("PowerUpAds");
    }
    public void ContinueWithCoins()
    {
        //Call the method directly after coins deductions
        if(GeneralDataManager.GameData.Coins >= 100)
        {
            if(GamePlayUIController.instance.PowerIndex == 0){
            GlanceAds.IngameAnalytics("Hint PowerUp",100,GeneralDataManager.GameData.LevelNo);
            }
            else if(GamePlayUIController.instance.PowerIndex == 1){
            GlanceAds.IngameAnalytics("Undo PowerUp",100,GeneralDataManager.GameData.LevelNo);
            }
            else if(GamePlayUIController.instance.PowerIndex == 2){
            GlanceAds.IngameAnalytics("Swap PowerUp",100,GeneralDataManager.GameData.LevelNo);
            }
            else if(GamePlayUIController.instance.PowerIndex == 3){
            GlanceAds.IngameAnalytics("Freeze PowerUp",100,GeneralDataManager.GameData.LevelNo);
            }
            UsePower(GamePlayUIController.instance.PowerIndex);
            Decrease_Coin(100);
        }
        else
        {
            //Show message
            GameManager.Inst.Make_Toast("You don't have enough coins. ");
        }
    }
    public void UsePower(int index)
    {
        if(index == 0)
        {
            GamePlayUIController.instance.UseHint();
        }
        else if(index == 1)
        {
            GamePlayUIController.instance.UseUndo();
        }
        else if (index == 2)
        {
            GamePlayUIController.instance.UseSwap();
        }
        else if (index == 3)
        {
            GamePlayUIController.instance.UseFreeze();
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
