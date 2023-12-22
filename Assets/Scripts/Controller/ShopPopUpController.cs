using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;
using Newtonsoft.Json;

public class ShopPopUpController : SingletonComponent<ShopPopUpController>
{
    [SerializeField] private Text coinText;
    [SerializeField] private GameObject loader, productContent, purchaseSuccessFulPopUp;
    public GameObject removeAdsButton;

    private void OnEnable()
    {
        removeAdsButton.SetActive(!GeneralDataManager.PurchaseAdsRemove);
        ////IAPManager.inst.InitilizitionIAP();
        Set_Text();
        Show_Loader(true);
        //StartCoroutine(SetupIAPList());
    }

    internal void Purchase_Successful_PopUp_Set(MyState state, int coins = 0, int hint = 0, int undo = 0,
        int freeze = 0, int swap = 0)
    {
        purchaseSuccessFulPopUp.SetActive(true);
        PurchaseSuccesfulPopUpController.Inst.Set_Me(state, coins, hint, undo, freeze, swap);
    }

    internal void Set_Cantant()
    {
        var y = removeAdsButton.activeSelf ? 2200 : 1950f;
        productContent.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(productContent.GetComponent<RectTransform>().anchoredPosition.x, y);
    }

    public void Get50CoinsAfterAd()
    {
        Increase_Coin(50);//Increase the coins when you get callback of successfully watched ad on glance integration
        StartCoroutine(RumbleSDK.instance.SaveDataCoroutine("PROGRESS",JsonConvert.SerializeObject(GeneralDataManager.GameData),PlayerPrefs.GetInt("LevelsUnlocked",1),PlayerPrefs.GetInt("UnlockedAllLevels",1)));
    }

    internal void Set_Text()
    {
        coinText.text = GeneralDataManager.GameData.Coins.ToString();
        var parent = coinText.transform.parent.GetComponent<RectTransform>();
        parent.sizeDelta = new Vector2(coinText.preferredWidth + 100, parent.sizeDelta.y);
    }

    public void On_Back_Btn_Click()
    {
        Play_Button_Click_Sound();
        CloseThisPopup();
    }

    public void CloseThisPopup()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        activePopup = Popups.Null;
        if (activeScreen == GameManager.Screens.HomeScreen)
            HomeScreenSontroller.Inst.Set_Text();
        else
            GamePlayUIController.Inst.Set_Text();
    }

    internal void Show_Loader(bool isOn)
    {
        //loader.SetActive(isOn);
    }

    //public List<IAPProductButton> iAPProductButtonsList = new List<IAPProductButton>();

    //public IEnumerator SetupIAPList()
    //{
    //    //foreach (var item in iAPProductButtonsList)
    //    //{
    //    //    item.Update_IAP_Button();
    //    //    yield return new WaitForEndOfFrame();
    //    //}

    //    Show_Loader(false);

    //    productContent.SetActive(true);
    //}

    public void On_Hint_Convert_Btn_Click()
    {
        Check_And_Purchase(Powers.Hint, 200);
    }

    public void On_Undo_Convert_Btn_Click()
    {
        Check_And_Purchase(Powers.Undo, 140);
    }

    public void On_Freeze_Convert_Btn_Click()
    {
        Check_And_Purchase(Powers.Freeze, 250);
    }

    public void On_Swap_Convert_Btn_Click()
    {
        Check_And_Purchase(Powers.Swap, 180);
    }

    private void Check_And_Purchase(Powers powers, int amount)
    {
        Play_Button_Click_Sound();
        if (GeneralDataManager.GameData.Coins >= amount)
        {
            Decrease_Coin(amount);
            StartCoroutine(RumbleSDK.instance.SaveDataCoroutine("PROGRESS",JsonConvert.SerializeObject(GeneralDataManager.GameData),PlayerPrefs.GetInt("LevelsUnlocked",1),PlayerPrefs.GetInt("UnlockedAllLevels",1)));
            Increase_Powers(1, powers);
            Set_Text();

            GameManager.Inst.Make_Toast(" 1 " + powers.ToString() + " Added.");
        }
        else
        {
            GameManager.Inst.Make_Toast("You don't have enough coins.");
        }
    }
}