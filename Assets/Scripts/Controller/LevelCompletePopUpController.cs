using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static GeneralDataManager;
using Newtonsoft.Json;

public class LevelCompletePopUpController : MonoBehaviour
{
    public static LevelCompletePopUpController instance;
    [SerializeField] private Text diamondText;
    [SerializeField] private Slider starProgressSlider;
    [SerializeField] private Slider levelProgressSlider;
    [SerializeField] private Transform chestRef, levelchestRef, coinrefPos;
    [SerializeField] private Text coinText;
    [SerializeField] private Sprite openChest;
    [SerializeField] private Sprite levelopenChest;
    [SerializeField] private Image ChestImage;
    [SerializeField] private Image LevelChestImage;
    [SerializeField] GameObject doubleRewardBtn;
    [SerializeField] private Animation ChestAnim;
    [SerializeField] private Animation ChestAnimLevel;
    [SerializeField] RectTransform bg;


    private void Awake()
    {
       
    }

    private float endValue = 0f;

    int a = 5;

    public void Update()
    {
        if(PlayerPrefs.HasKey("LevelCompleteReward")){
            PlayerPrefs.DeleteKey("LevelCompleteReward");
            Give_Reward();    
        }
    }

    private void Start()
    {
        ComboSliderController.Inst.Set_Tween(ComboSliderController.TweenState.Pause);
        Set_Text();
        var targetDiamondCount = GeneralRefrencesManager.Inst.Get_Diamond_Count();
        starProgressSlider.maxValue = 5;

        starProgressSlider.value = 0;

        
        endValue = GameData.LevelNo % a == 0 ? a : GameData.LevelNo % a;
        levelProgressSlider.maxValue = 7;
        if(GameData.LevelNo != 7)
        {
            levelProgressSlider.value = endValue - 1;
        }
        else
        {
            levelProgressSlider.value = 0;
        }

        if (GameData.IsLevelChestClaim)
        {
            GameData.IsLevelChestClaim = false;
        }

        

        SoundManager.Inst.Play("level_complete");
        GamePlayUIController.Inst.Set_Freeze_Fill_Img_Amount();
        GeneralRefrencesManager.Inst.World_No_Click_Panel_On_Off(true);

        if (GameData.LevelNo % 2 == 0)
            Invoke(nameof(Show_Intertitial), 1f);

    }

    public void Increase_Score_Anim()
    {
        SoundManager.Inst.Play("score_increment");
        DOTween.To(() => 0, x => diamondText.text = x.ToString(), HeaderController.Inst.DiamondCountPerLevel, 0.15f).SetEase(Ease.Linear);
    }

    public void Show_Intertitial()
    {
        //AdsManager.inst.ShowInterstitial("LevelComplete");
    }

    public void Slider_Anim()
    {
        int level = GameData.LevelNo % 7;
        if(GameData.LevelNo % 7 == 0)
        {
            level = 7;
            LevelUnlocked = true;
        }
        SoundManager.Inst.Play("slider_fill");
        DOTween.To(() => endValue - 1f, x => starProgressSlider.value = x, endValue, 0.5f).SetEase(Ease.Linear).OnComplete(() => StartCoroutine(Coin_Anim(0)));
        DOTween.To(() => 0, x => levelProgressSlider.value = x, level , 0.5f).SetEase(Ease.Linear).OnComplete(() => StartCoroutine(Coin_Anim(1)));
        GameData.LevelNo++;
        GeneralDataManager.Save_Data();
        StartCoroutine(RumbleSDK.instance.SaveDataCoroutine("PROGRESS",JsonConvert.SerializeObject(GeneralDataManager.GameData)));
    }
    bool LevelUnlocked;
    int amount = 0;
    private IEnumerator Coin_Anim(int i)
    {
        if(i==0)
        {
            //Star chest
            if (endValue == a)
            {
                ChestAnim.Play();
                yield return new WaitForSeconds(0.8f);
                ChestImage.sprite = openChest;
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(GameManager.Give_Coin_With_Anim(100, chestRef, 0, Set_Text, coinrefPos));
            AAA:
                amount += (GeneralRefrencesManager.Inst.Get_Diamond_Count() / 10);
                GeneralDataManager.GameData.StarChestDiamond -= GeneralRefrencesManager.Inst.Get_Diamond_Count();
                GeneralDataManager.GameData.StarChestOpenCount++;

                if (GeneralDataManager.GameData.StarChestDiamond >= GeneralRefrencesManager.Inst.Get_Diamond_Count())
                {
                    goto AAA;
                }
            }
        }
        else
        {
            if(LevelUnlocked)
            {
                GeneralDataManager.GameData.IsLevelChestClaim = true;
                LevelUnlocked = false;
                ChestAnimLevel.Play();
                yield return new WaitForSeconds(0.8f);
                LevelChestImage.sprite = levelopenChest;
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(GameManager.Give_Coin_With_Anim(40, levelchestRef, 0, Set_Text, coinrefPos));
            }
            
        }
    }

    internal void Set_Text()
    {
        coinText.text = GameData.Coins.ToString();
        var parent = coinText.transform.parent.GetComponent<RectTransform>();
        parent.sizeDelta = new Vector2(coinText.preferredWidth + 100, parent.sizeDelta.y);
    }


    public void On_Free_Coin_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
         if(PlayerPrefs.GetFloat("RumbleBalance") >= 200){
            StartCoroutine(RumbleSDK.instance.UpdateBalanceAsync(200,"LevelCompleteRewardAd"));
        }
        else{
            RumbleSDK.instance.OnIAPButton();
        }
        //AdsManager.inst.LoadAndShow_RewardVideo("LevelComplete");
    }

    public void Give_Reward()
    {
        doubleRewardBtn.SetActive(false);
        bg.sizeDelta = new Vector2(bg.sizeDelta.x, 1050);
        StartCoroutine(GameManager.Give_Coin_With_Anim(100, transform, 0, Set_Text, coinrefPos));
        GeneralDataManager.Save_Data();
        StartCoroutine(RumbleSDK.instance.SaveDataCoroutine("PROGRESS",JsonConvert.SerializeObject(GeneralDataManager.GameData)));
    }


    public void On_Home_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        Home_Btn_Click();
    }

    private void Home_Btn_Click()
    {   
        GeneralRefrencesManager.Inst.Clear_Level();
        GameManager.Inst.Show_Screen(GameManager.Screens.HomeScreen);
        CloseThisPopup();
    }

    public void On_Next_Btn_Click()
    {   
        GameManager.Play_Button_Click_Sound();
        Next_Btn_Click();
    }

    private void Next_Btn_Click()
    {
        GeneralRefrencesManager.Inst.Clear_Level();
        GridManager.Inst.Generate_Grid(GameData.LevelNo);
        CloseThisPopup();
        
    }

    public void CloseThisPopup()
    {
        GeneralRefrencesManager.Inst.World_No_Click_Panel_On_Off(false);
        GameManager.activePopup = GameManager.Popups.Null;
        Destroy(this.gameObject);
    }
}