using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardPopUpController : MonoBehaviour
{
    public static DailyRewardPopUpController instance;
    [SerializeField] private GameObject doubleRewardBtn;
    [SerializeField] private Transform coinParent;
    [SerializeField] private Text coinAmountText;

    private int rewardedAmount = 0;

    private List<int> rewardAmountList = new List<int>() { 90, 120, 160, 200, 250, 300 };

    private void Update()
    {
        if(PlayerPrefs.HasKey("DailyReward")){
            PlayerPrefs.DeleteKey("DailyReward");
            if(PlayerPrefs.GetInt("InitialMusic") == 1){
            SettingPopUpController.instance.Unmute_Music();
            }
            if(PlayerPrefs.GetInt("InitialSound") == 1){
            SettingPopUpController.instance.Unmute_Sound();
            }
            Give_Reward();    
            CloseThisPopup(); 
        }
        if(PlayerPrefs.HasKey("CancelDailyReward")){
            PlayerPrefs.DeleteKey("CancelDailyReward");
            if(PlayerPrefs.GetInt("InitialMusic") == 1){
            SettingPopUpController.instance.Unmute_Music();
            }
            if(PlayerPrefs.GetInt("InitialSound") == 1){
            SettingPopUpController.instance.Unmute_Sound();
            }
            CloseThisPopup();
        }
    }

    private void Start()
    {
        rewardedAmount = rewardAmountList[Random.Range(0, rewardAmountList.Count)];
        coinAmountText.text = "+ " + rewardedAmount.ToString();
    }


    public void On_Close_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        CloseThisPopup();
    }

    public void On_Clim_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        StartCoroutine(Coin_Anim_Start());
    }

    private IEnumerator Coin_Anim_Start()
    {
        GeneralRefrencesManager.Inst.No_Click_Panel_On_Off(true);
        yield return new WaitForSeconds(0.35f);
        GameManager.Inst.StartCoroutine(GameManager.Give_Coin_With_Anim(rewardedAmount, coinParent, 0,
            Daily_Reward_Btn_Enable , HomeScreenSontroller.Inst.coinRef));
    }

    private void Daily_Reward_Btn_Enable()
    {
        GeneralRefrencesManager.Inst.No_Click_Panel_On_Off(false);
        doubleRewardBtn.SetActive(true);
    }

    public void On_Double_Reward_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        GeneralRefrencesManager.Inst.No_Click_Panel_On_Off(true);
        GlanceAds.RewardedAdsAnalytics("DailyReward","CancelDailyReward");
        GlanceAds.RewardedAd("DailyReward");
        ////AdsManager.inst.LoadAndShow_RewardVideo("DailyReward");
    }
    public void Give_Reward()
    {
        GameManager.Inst.StartCoroutine(GameManager.Give_Coin_With_Anim(rewardedAmount, coinParent, 0, CloseThisPopup, HomeScreenSontroller.Inst.coinRef));
    }
    public void CloseThisPopup()
    {
        GeneralRefrencesManager.Inst.No_Click_Panel_On_Off(false);
        GameManager.activePopup = GameManager.Popups.Null;
        Destroy(this.gameObject);
    }
}