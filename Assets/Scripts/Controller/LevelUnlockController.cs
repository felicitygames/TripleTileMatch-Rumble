using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnlockController : MonoBehaviour
{
    [SerializeField] private Text RumbleAmt;
    [SerializeField] private GameObject ErrorMsg;
    private int RumbleAmount = 500;
    private string LevelRange;
    
    void Start(){
        if(GeneralDataManager.GameData.LevelNo > 10){
            if((GeneralDataManager.GameData.LevelNo > 10 && GeneralDataManager.GameData.LevelNo <= 20)){
                RumbleAmount = 500;
            }
            else if((GeneralDataManager.GameData.LevelNo > 20 && GeneralDataManager.GameData.LevelNo <= 30)){
                RumbleAmount = 1000;
            }
            else if((GeneralDataManager.GameData.LevelNo > 30 && GeneralDataManager.GameData.LevelNo <= 40)){
                RumbleAmount = 1500;
            }
            else if((GeneralDataManager.GameData.LevelNo > 40 && GeneralDataManager.GameData.LevelNo <= 50)){
                RumbleAmount = 2000;
            }
            else{
                RumbleAmount = 2000;
            }
        }
        RumbleAmt.text = RumbleAmount.ToString();
    }

    public void ContinueWithAd()
    {
        PlayerPrefs.SetString("RewardAdType","LevelsUnlockedAd");
        PlayerPrefs.Save();
        RumbleSDK.instance.OnRewardLvlAdButton();
    }

    public void ContinueWithRumble()
    {
        if(PlayerPrefs.GetFloat("RumbleBalance") >= RumbleAmount){
            StartCoroutine(RumbleSDK.instance.UpdateBalanceAsync(RumbleAmount,("LevelsUnlockedAd")));
        }
        else{
            ErrorMsg.SetActive(true);
            Invoke("CloseErrorMsg",2.5f);
        }
    }
    public void CloseErrorMsg(){
        ErrorMsg.SetActive(false);
    }
}
