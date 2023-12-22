using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;
using Newtonsoft.Json;

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
            UsePower(GamePlayUIController.instance.PowerIndex);
        }
    }
    private void Start()
    {
        CoinsButton.GetComponent<Button>().interactable = true;
        
        Coins.text = PlayerPrefs.GetInt("Coins").ToString();
    }
    public void ContinueWithAd()
    {
        if(PlayerPrefs.GetFloat("RumbleBalance") >= 200){
            StartCoroutine(RumbleSDK.instance.UpdateBalanceAsync(200,"PowerUpRewardAd"));
        }
        else{
            RumbleSDK.instance.OnIAPButton();
        }
    }
    public void ContinueWithCoins()
    {
        //Call the method directly after coins deductions
        if(GeneralDataManager.GameData.Coins >= 100)
        {
            UsePower(GamePlayUIController.instance.PowerIndex);
            Decrease_Coin(100);
            StartCoroutine(RumbleSDK.instance.SaveDataCoroutine("PROGRESS",JsonConvert.SerializeObject(GeneralDataManager.GameData),PlayerPrefs.GetInt("LevelsUnlocked",1),PlayerPrefs.GetInt("UnlockedAllLevels",1)));
        }
        else
        {
            //Show message
            GameManager.Inst.Make_Toast("You don't have enough coins. ");
        }
    }
    public void UsePower(int index)
    {
        GeneralDataManager.Save_Data();
        StartCoroutine(RumbleSDK.instance.SaveDataCoroutine("PROGRESS",JsonConvert.SerializeObject(GeneralDataManager.GameData),PlayerPrefs.GetInt("LevelsUnlocked",1),PlayerPrefs.GetInt("UnlockedAllLevels",1)));
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
