using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Newtonsoft.Json;

public class LevelChestPopUpController : MonoBehaviour
{
    public static LevelChestPopUpController instance;
    [SerializeField] Transform chestIcon;
    [SerializeField] GameObject adBtn, closeBtn;
    private int amount = 40;

    public void Update()
    {
        if(PlayerPrefs.HasKey("LevelChestReward")){
            PlayerPrefs.DeleteKey("LevelChestReward");
            Give_Reward();    
        }
    }

    public void On_Claim_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        GameManager.Increase_Coin(amount);
        adBtn.SetActive(true);
        closeBtn.SetActive(true);
        HomeScreenSontroller.Inst.Set_Level_Chest_Text();
        GetComponent<Animator>().SetTrigger("Play");
        GeneralDataManager.GameData.IsLevelChestClaim = true;
         StartCoroutine(RumbleSDK.instance.SaveDataCoroutine("PROGRESS",JsonConvert.SerializeObject(GeneralDataManager.GameData),PlayerPrefs.GetInt("LevelsUnlocked",1),PlayerPrefs.GetInt("UnlockedAllLevels",1)));
    }

    public void Give_Coin_Anim()
    {
        StartCoroutine(GameManager.Give_Coin_With_Anim(0, chestIcon, 0, HomeScreenSontroller.Inst.Set_Text, HomeScreenSontroller.Inst.coinRef));
    }

    public void On_GetX2_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        if(PlayerPrefs.GetFloat("RumbleBalance") >= 200){
            StartCoroutine(RumbleSDK.instance.UpdateBalanceAsync(200,"LevelChestRewardAd"));
        }
        else{
            RumbleSDK.instance.OnIAPButton();
        }
        //AdsManager.inst.LoadAndShow_RewardVideo("LevelChest");
    }

    public void Give_Reward()
    {
        GameManager.Increase_Coin(amount);
        GeneralRefrencesManager.Inst.No_Click_Panel_On_Off(true);
        StartCoroutine(GameManager.Give_Coin_With_Anim(0, chestIcon, 0, CloseThisPopup, HomeScreenSontroller.Inst.coinRef));
        StartCoroutine(RumbleSDK.instance.SaveDataCoroutine("PROGRESS",JsonConvert.SerializeObject(GeneralDataManager.GameData),PlayerPrefs.GetInt("LevelsUnlocked",1),PlayerPrefs.GetInt("UnlockedAllLevels",1)));
    }

    public void On_Close_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        CloseThisPopup();
    }

    public void CloseThisPopup()
    {
        HomeScreenSontroller.Inst.Set_Text();
        HomeScreenSontroller.Inst.Set_Level_Chest_Text();
        GeneralRefrencesManager.Inst.No_Click_Panel_On_Off(false);
        GameManager.activePopup = GameManager.Popups.Null;
        Destroy(gameObject);
    }
}
