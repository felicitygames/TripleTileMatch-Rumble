using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class StarChestOpenPopUpController : MonoBehaviour
{
    public static StarChestOpenPopUpController instance;
    [SerializeField] Transform chestIcon;
    [SerializeField] GameObject adBtn, closeBtn;

    private int amount = 0;

    public void Update()
    {
        if(PlayerPrefs.HasKey("StarChestReward")){
            PlayerPrefs.DeleteKey("StarChestReward");
            Give_Reward();    
        }
    }

    public void On_Claim_Btn_Click()
    {

    AAA:
        amount += (GeneralRefrencesManager.Inst.Get_Diamond_Count() / 10);
        GeneralDataManager.GameData.StarChestDiamond -= GeneralRefrencesManager.Inst.Get_Diamond_Count();
        GeneralDataManager.GameData.StarChestOpenCount++;

        if (GeneralDataManager.GameData.StarChestDiamond >= GeneralRefrencesManager.Inst.Get_Diamond_Count())
        {
            goto AAA;
        }

        GameManager.Play_Button_Click_Sound();
        GameManager.Increase_Coin(amount);
         StartCoroutine(RumbleSDK.instance.SaveDataCoroutine("PROGRESS",JsonConvert.SerializeObject(GeneralDataManager.GameData)));
        adBtn.SetActive(true);
        closeBtn.SetActive(true);
        GetComponent<Animator>().SetTrigger("Play");
        HomeScreenSontroller.Inst.Set_Star_Chest();
    }
    public void Give_Coin_Anim()
    {
        StartCoroutine(GameManager.Give_Coin_With_Anim(0, chestIcon, 0, HomeScreenSontroller.Inst.Set_Text, HomeScreenSontroller.Inst.coinRef));
    }

    public void On_GetX2_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        if(PlayerPrefs.GetFloat("RumbleBalance") >= 200){
            StartCoroutine(RumbleSDK.instance.UpdateBalanceAsync(200,"StarChestRewardAd"));
        }
        else{
            RumbleSDK.instance.OnIAPButton();
        }
    }
    public void On_Close_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        CloseThisPopup();
    }

    public void Give_Reward()
    {
        GameManager.Increase_Coin(amount);
        StartCoroutine(RumbleSDK.instance.SaveDataCoroutine("PROGRESS",JsonConvert.SerializeObject(GeneralDataManager.GameData)));
        GeneralRefrencesManager.Inst.No_Click_Panel_On_Off(true);
        StartCoroutine(GameManager.Give_Coin_With_Anim(0, chestIcon, 0, CloseThisPopup, HomeScreenSontroller.Inst.coinRef));
    }

    public void CloseThisPopup()
    {
        HomeScreenSontroller.Inst.Set_Text();
        GeneralRefrencesManager.Inst.No_Click_Panel_On_Off(false);
        GameManager.activePopup = GameManager.Popups.Null;
        Destroy(gameObject);
    }
}
