using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GlanceAds : MonoBehaviour
{
    [SerializeField] Transform popUpParent;
    public static GlanceAds Instance;
    [DllImport("__Internal")]
    public static extern void LoadGlanceAds();
    [DllImport("__Internal")]
    public static extern void RewardedAd(string type);
    [DllImport("__Internal")]
    public static extern void ReplayAd(string type);
    [DllImport("__Internal")]
    public static extern void LoadAnalytics();
    [DllImport("__Internal")]
    public static extern void StartAnalytics();
    [DllImport("__Internal")]
    public static extern void ReplayAnalytics(int level);
    [DllImport("__Internal")]
    public static extern void EndAnalytics(int level);
    [DllImport("__Internal")]
    public static extern void LevelAnalytics(int level);
    [DllImport("__Internal")]
    public static extern void LevelCompletedAnalytics(int level);
    [DllImport("__Internal")]
    public static extern void RewardedAdsAnalytics(string successCBf, string failureCBf);
    [DllImport("__Internal")]
    public static extern void MilestoneAnalytics(int CollectedStars,int level);
    [DllImport("__Internal")]
    public static extern void GameLifeEndAnalytics(int RemainingLife, int level);
    [DllImport("__Internal")]
    public static extern void IngameAnalytics(string items, int amount, int level);
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void muteAudio(){
        PlayerPrefs.SetInt("InitialMusic",PlayerPrefs.GetInt("Music"));
        PlayerPrefs.SetInt("InitialSound",PlayerPrefs.GetInt("Sound"));
        SettingPopUpController.instance.Mute_All();
    }
    public void deleteGlanceKey(){
        PlayerPrefs.DeleteKey("firstGlance");
    }
    public void setLanguage(string LanguageChar){
        PlayerPrefs.DeleteKey("LanguageChar");
        PlayerPrefs.SetString("LanguageChar",LanguageChar);  
        Debug.Log("CurrentLang:"+PlayerPrefs.GetString("LanguageChar"));
    }
    public void setAd(string Adtype){
        PlayerPrefs.SetInt(Adtype,1);  
        Debug.Log(Adtype);
    }
    public void pauseEvent(){
        HeaderController.instance.On_Pause_Btn_Click();
    }
    public void resumeEvent(){
        PlayerPrefs.SetInt("resumeEvent",1);
    }
    public void replayGameEvent(){
        if(GameManager.activePopup != GameManager.Popups.Null){
        popUpParent.GetChild(popUpParent.childCount - 1).SendMessage("CloseThisPopup");
        }
        GeneralRefrencesManager.Inst.Clear_Level();
        GridManager.Inst.Generate_Grid(GeneralDataManager.GameData.LevelNo);
    }
    public void nextLevelEvent(){
        if(GameManager.activePopup != GameManager.Popups.Null){
        popUpParent.GetChild(popUpParent.childCount - 1).SendMessage("CloseThisPopup");
        }
        int level = GeneralDataManager.GameData.LevelNo % 7;
        if(GeneralDataManager.GameData.LevelNo % 7 == 0)
        {
            level = 7;
        }
        GeneralDataManager.GameData.LevelNo++;
        GeneralRefrencesManager.Inst.Clear_Level();
        GridManager.Inst.Generate_Grid(GeneralDataManager.GameData.LevelNo);
    }
    public void gotoHomeEvent(){
        if(GameManager.activePopup != GameManager.Popups.Null){
        popUpParent.GetChild(popUpParent.childCount - 1).SendMessage("CloseThisPopup");
        }
        GeneralRefrencesManager.Inst.Clear_Level();
        GameManager.Inst.Show_Screen(GameManager.Screens.HomeScreen);
    }
    public void gotoLevel(int levelNo){
        if(GameManager.activePopup != GameManager.Popups.Null){
        popUpParent.GetChild(popUpParent.childCount - 1).SendMessage("CloseThisPopup");
        }
        GeneralDataManager.GameData.LevelNo = levelNo;
        GeneralRefrencesManager.Inst.Clear_Level();
        GridManager.Inst.Generate_Grid(GeneralDataManager.GameData.LevelNo);
    }
    public void enableSound(string boolean){
        if(boolean == "true"){
            SettingPopUpController.instance.Mute_All();
        }
        else{
            SettingPopUpController.instance.Unmute_All();
        }
    }
}
