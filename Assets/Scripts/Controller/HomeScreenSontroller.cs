using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using static GameManager;
using static GeneralDataManager;

public class HomeScreenSontroller : SingletonComponent<HomeScreenSontroller>
{
    public static HomeScreenSontroller instance;
    [SerializeField] private Text coinText, diamondText, levelChestText, starChestText;
    [SerializeField] private TextMeshProUGUI levelNo;
    [SerializeField] Slider levelSlider, starSlider;
    [SerializeField] Animation starChestIconAnim, levelChestIconAnim;
    public Transform coinRef;
    public Transform starChestIconRef;
    public Transform levelChestIconRef;
    public GameObject removeAdsButton;
    public GameObject levelChest;

    private void Update()
    {
        if (PlayerPrefs.HasKey("IncreaseCoinReward")){
            PlayerPrefs.DeleteKey("IncreaseCoinReward");
            if(PlayerPrefs.GetInt("InitialMusic") == 1){
            SettingPopUpController.instance.Unmute_Music();
            }
            if(PlayerPrefs.GetInt("InitialSound") == 1){
            SettingPopUpController.instance.Unmute_Sound();
            }
            AfterIncreaseAds(); 
        }
        if(PlayerPrefs.HasKey("CancelIncreaseCoinReward")){
            PlayerPrefs.DeleteKey("CancelIncreaseCoinReward");
            if(PlayerPrefs.GetInt("InitialMusic") == 1){
            SettingPopUpController.instance.Unmute_Music();
            }
            if(PlayerPrefs.GetInt("InitialSound") == 1){
            SettingPopUpController.instance.Unmute_Sound();
            }
        }
    }

    private void OnEnable()
    {  
         GeneralDataManager.Save_Data();
        if(!PlayerPrefs.HasKey("Music")){
            PlayerPrefs.SetInt("Music",1);
            PlayerPrefs.SetInt("Sound",1);
            SoundManager.Inst.Play("bg_sound", true);
        }
        Set_Text();
        Set_Star_Chest();
        Set_Level_Chest_Text();
        ShowCollectMessage();
        Set_Remove_Ad_Btn();

        ScreenCapture.CaptureScreenshot("1.png");
        // // if (GameData.LevelNo <= 3)
        // // {
        // //     GameManager.Inst.Show_Screen(Screens.GameScreen);
        // // }
    }

    internal void Set_Remove_Ad_Btn()
    {
        removeAdsButton.SetActive(!PurchaseAdsRemove);
    }

    internal void Set_Text()
    {
        Set_Coin_Diamond_Level_Text();
    }

    public void On_Setting_Btn_Click()
    {
        Play_Button_Click_Sound();
        GameManager.Inst.Show_Popup(Popups.Setting);
    }

    public void On_Play_Btn_Click()
    {
        if(PlayerPrefs.GetInt("firstGlance") == 1){
            GlanceAds.ReplayAnalytics(GeneralDataManager.GameData.LevelNo);
            GlanceAds.LevelAnalytics(GeneralDataManager.GameData.LevelNo);
        }
        else{
            PlayerPrefs.SetInt("firstGlance",1);
            GlanceAds.StartAnalytics();
            GlanceAds.LevelAnalytics(GeneralDataManager.GameData.LevelNo);
        }
        Play_Button_Click_Sound();
        GameManager.Inst.Show_Screen(Screens.GameScreen);
    }

    public void On_Remove_Ad_Btn_Click()
    {
        Play_Button_Click_Sound();
        GameManager.Inst.Show_Popup(Popups.RemoveAds);
    }

    public void On_Shop_Btn_Click()
    {
        Play_Button_Click_Sound();
        //We will show ad later
        GlanceAds.RewardedAdsAnalytics("IncreaseCoinReward","CancelIncreaseCoinReward");
        GlanceAds.RewardedAd("IncreaseCoin");
        //GameManager.Inst.Show_Popup(Popups.Shop);
        
    }

    public void AfterIncreaseAds(){
        Increase_Coin(150);
        GeneralDataManager.Save_Data();
        Set_Coin_Diamond_Level_Text();
    }

    public void CloseThisScreen()
    {
        gameObject.SetActive(false);
    }

    public void On_Star_Chest_Btn_Click()
    {
        Play_Button_Click_Sound();
        if (starChestText.text.Split('/')[0] != starChestText.text.Split('/')[1]) return;
        GameManager.Inst.Show_Popup(Popups.StarChestPopUp);
    }

    public void On_Level_Chest_Btn_Click()
    {
        if (levelChestText.text.Split('/')[0] != levelChestText.text.Split('/')[1] || GameData.IsLevelChestClaim) return;
        Play_Button_Click_Sound();
        GameManager.Inst.Show_Popup(Popups.LevelChestPopUp);
    }
    public GameObject collectLevelChest;
    public GameObject collectStarChest;
    public void ShowCollectMessage()
    {
        if(levelChestText.text.Split('/')[0] != levelChestText.text.Split('/')[1] || GameData.IsLevelChestClaim)
        {
            collectLevelChest.SetActive(false);
        }
        else
        {
            collectLevelChest.SetActive(true);
            StartCoroutine(CloseThisGameObject(collectLevelChest));
        }
        if(starChestText.text.Split('/')[0] != starChestText.text.Split('/')[1])
        {
            collectStarChest.SetActive(false);
        }
        else
        {
            collectStarChest.SetActive(true);
            StartCoroutine(CloseThisGameObject(collectStarChest));
        }
    }
    IEnumerator CloseThisGameObject(GameObject go)
    {
        yield return new WaitForSeconds(2);
        go.SetActive(false);
    }

    int q = 7;

    internal void Set_Level_Chest_Text()
    {
        if (GameData.LevelNo > 1)
        {
            if (GameData.IsLevelChestClaim)
            {
                levelChestText.text = (((GameData.LevelNo - 1) % q) == 0) ? 0 + "/" + q : ((GameData.LevelNo - 1) % q) + "/" + q;
                levelSlider.value = (GameData.LevelNo - 1) % q == 0 ? 0 : (GameData.LevelNo - 1) % q;
            }
            else
            {

                levelChestText.text = (((GameData.LevelNo - 1) % q) == 0) ? q + "/" + q : ((GameData.LevelNo - 1) % q) + "/" + q;
                levelSlider.value = (GameData.LevelNo - 1) % q == 0 ? q : (GameData.LevelNo - 1) % q;
            }
        }
        else
        {
            levelChestText.text = 0 + "/" + q;
            levelSlider.value = 0;
        }

        if (levelChestText.text.Split('/')[0] == levelChestText.text.Split('/')[1] && !GameData.IsLevelChestClaim)
        {
            levelChestIconAnim.enabled = true;
        }
        else
        {
            levelChestIconAnim.enabled = false;
            levelChestIconAnim.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(levelChestIconAnim.transform.GetComponent<RectTransform>().anchoredPosition.x, 10);
        }
    }


    internal void Set_Star_Chest()
    {
        var targetDiamondCount = GeneralRefrencesManager.Inst.Get_Diamond_Count();
        starSlider.maxValue = targetDiamondCount;

        starSlider.value = GameData.StarChestDiamond;

        if (GameData.StarChestDiamond <= targetDiamondCount)
        {
            if(GameData.StarChestDiamond > 0)
            {
                starChestText.text = GameData.StarChestDiamond + "/" + targetDiamondCount;
            }
            else
            {
                starChestText.text = "232" + "/" + targetDiamondCount;
            }
            starChestIconAnim.enabled = false;
            starChestIconAnim.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(starChestIconAnim.transform.GetComponent<RectTransform>().anchoredPosition.x, 10);
        }
        else
        {
            starChestText.text = targetDiamondCount + "/" + targetDiamondCount;
            starChestIconAnim.enabled = true;
        }
    }

    private void Set_Coin_Diamond_Level_Text()
    {
        PlayerPrefs.SetInt("Coins", GameData.Coins);
        coinText.text = GameData.Coins.ToString();
        levelNo.text = GameData.LevelNo.ToString();

        diamondText.text = GameData.Diamonds > 10000
            ? NumberConverter.numberFormat(GameData.Diamonds)
            : GameData.Diamonds.ToString();

        var parent = coinText.transform.parent.GetComponent<RectTransform>();
        parent.sizeDelta = new Vector2(coinText.preferredWidth + 130, parent.sizeDelta.y);

        var parent1 = diamondText.transform.parent.GetComponent<RectTransform>();
        parent1.sizeDelta = new Vector2(diamondText.preferredWidth + 110, parent1.sizeDelta.y);
        parent1.anchoredPosition = new Vector2(-parent.sizeDelta.x - 100, parent1.anchoredPosition.y);
    }
}