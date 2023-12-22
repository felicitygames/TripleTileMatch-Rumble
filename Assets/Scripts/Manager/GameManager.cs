using UnityEngine;
using System;
using static GeneralDataManager;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine.Serialization;
using Newtonsoft.Json;

public class GameManager : SingletonComponent<GameManager>
{
    [SerializeField] Transform bottom, right, left, top;
    [SerializeField] Transform popUpParent, newUnlockItemParent;

    internal ElementController lastSelectedElement;

    [SerializeField] GameObject toast;

    public GameObject adLoader;

    [Header("Screen Reference")] public GameObject gamePlay;
    public GameObject homeScreen;
    public GameObject lunchScreen;
    public GameObject levelUnlock;

    [Header("PopUp Reference")] public GameObject gameOver;
    public GameObject levelComplete;
    public GameObject levelComplete2;
    public GameObject setting;
    public GameObject pause;
    public GameObject exit;
    public GameObject shop;
    public GameObject howToPlay;
    public GameObject newElementUnLock;
    public GameObject starChestPopUp;
    public GameObject levelChestPopUp;
    public GameObject removeAds;
    public GameObject powerupConfirm;

    protected override void Awake()
    {   
        var (canvasWidth, canvasHeight) = GeneralRefrencesManager.Inst.Get_Canvas_Width_Height();
        bottom.position = new Vector3(bottom.transform.position.x,
            (float)(-22.13612 + (2.796656 * Mathf.Log(canvasWidth))) - 1f, bottom.transform.position.z);
        top.position = new Vector3(top.transform.position.x,
            (float)(3.4 + (0.7 * Mathf.Log(canvasHeight / canvasWidth))) - 1f, top.transform.position.z);
        var sideX = (float)(32.733 - (5.14785 * Mathf.Log(canvasWidth)));
        left.position = new Vector3(sideX, left.transform.position.y, left.transform.position.z);
        right.position = new Vector3(-sideX, right.transform.position.y, right.transform.position.z);
        //GameData.LevelNo = 7;
        /*#if UNITY_EDITOR
                activeScreen = Screens.LunchScreen;
                Show_Screen(Screens.HomeScreen);
        #else
                homeScreen.SetActive(false);
                Show_Screen(Screens.LunchScreen);
        #endif*/
        homeScreen.SetActive(false);
        Show_Screen(Screens.LunchScreen);
        activeScreen = Screens.LunchScreen;
        //lunchScreen.SetActive(false);
        //Show_Screen(Screens.HomeScreen);
    }


    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        switch (activePopup)
        {
            case Popups.LevelComplete:
                activePopUpObj.SendMessage("On_Home_Btn_Click");
                break;
            case Popups.GameOver:
                activePopUpObj.SendMessage("On_Home_Btn_Click");
                break;
            default:
                {
                    if (activePopup != Popups.Null)
                    {
                        popUpParent.GetChild(popUpParent.childCount - 1).SendMessage("CloseThisPopup");
                    }
                    else if (activeScreenObj != null)
                    {
                        switch (activeScreen)
                        {
                            case Screens.HomeScreen:
                                ////AdsManager.inst.ShowInterstitial("Exit");
                                Show_Popup(Popups.Exit);
                                break;
                            case Screens.GameScreen:
                                Show_Popup(Popups.Pause);
                                break;
                            case Screens.LunchScreen:
                                break;
                            default:
                                return;
                        }
                    }

                    break;
                }
        }
    }

    public enum Screens
    {
        LunchScreen,
        HomeScreen,
        GameScreen
    }

    public static Screens activeScreen = Screens.HomeScreen;
    private static GameObject activeScreenObj;

    public void Show_Screen(Screens screen)
    {
        if (activeScreen == screen) return;
        if (activeScreenObj != null)
        {
            activeScreenObj.SendMessage("CloseThisScreen");
        }

        activeScreen = screen;

        switch (screen)
        {
            case Screens.HomeScreen:
                GeneralRefrencesManager.Inst.Set_Game_Screen_UI(false);
                activeScreenObj = homeScreen;
                homeScreen.SetActive(true);
                break;
            case Screens.LunchScreen:
                GeneralRefrencesManager.Inst.Set_Game_Screen_UI(false);
                activeScreenObj = lunchScreen;
                lunchScreen.SetActive(true);
                break;
            case Screens.GameScreen:
                GeneralRefrencesManager.Inst.Set_Game_Screen_UI(true);
                activeScreenObj = GamePlayUIController.Inst.gameObject;
                break;
            default:
                return;
        }
    }

    public enum Popups
    {
        Null,
        GameOver,
        LevelComplete,
        Setting,
        Pause,
        Exit,
        Shop,
        HowToPlay,
        NewElementUnLock,
        StarChestPopUp,
        LevelChestPopUp,
        RemoveAds,
        PowerupConfirm
    }

    public static Popups activePopup = Popups.Null;
    private static GameObject activePopUpObj;

    public void Show_Popup(Popups popup)
    {
        if (activePopup == popup) return;
        if (activePopUpObj != null)
        {
            activePopUpObj.SendMessage("CloseThisPopup");
        }

        activePopup = popup;

        switch (popup)
        {
            case Popups.GameOver:
                activePopUpObj = Instantiate(gameOver, popUpParent);
                break;
            case Popups.LevelComplete:
                activePopUpObj = Instantiate(levelComplete, popUpParent);
                //if (GameData.LevelNo% 7 !=0)
                //{
                    
                //}
                //else
                //{
                //    activePopUpObj = Instantiate(levelComplete2, popUpParent);
                //}
                break;
            case Popups.Setting:
                activePopUpObj = Instantiate(setting, popUpParent);
                break;
            case Popups.Pause:
                GeneralRefrencesManager.Inst.World_No_Click_Panel_On_Off(true);
                activePopUpObj = Instantiate(pause, popUpParent);
                break;
            case Popups.Exit:
                GeneralRefrencesManager.Inst.World_No_Click_Panel_On_Off(true);
                activePopUpObj = Instantiate(exit, popUpParent);
                break;
            case Popups.NewElementUnLock:
                activePopUpObj = Instantiate(newElementUnLock, newUnlockItemParent);
                break;
            case Popups.Shop:
                activePopUpObj = Instantiate(shop, popUpParent);
                break;
            case Popups.StarChestPopUp:
                activePopUpObj = Instantiate(starChestPopUp, popUpParent);
                break;
            case Popups.LevelChestPopUp:
                activePopUpObj = Instantiate(levelChestPopUp, popUpParent);
                break;
            case Popups.HowToPlay:
                GeneralRefrencesManager.Inst.World_No_Click_Panel_On_Off(true);
                activePopUpObj = Instantiate(howToPlay, popUpParent);
                break;
            case Popups.RemoveAds:
                activePopUpObj = Instantiate(removeAds, popUpParent);
                break;
            case Popups.PowerupConfirm:
                activePopUpObj = Instantiate(powerupConfirm, popUpParent);
                break;
            case Popups.Null:
                break;
            default:
                return;
        }
    }

    public void Share_Game()
    {
        var ShareMessage =
            "Tile Star 3D" +
            "\n\n" +
            "Android :\n" + GeneralDataManager.Inst.AndroidShareLink;
#if UNITY_ANDROID
        //Create intent for action send
        AndroidJavaClass intentClass =
            new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject =
            new AndroidJavaObject("android.content.Intent");
        intentObject.Call<AndroidJavaObject>
            ("setAction", intentClass.GetStatic<string>("ACTION_SEND"));

        //put text and subject extra
        intentObject.Call<AndroidJavaObject>("setType", "text/plain");
        intentObject.Call<AndroidJavaObject>
            ("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), ShareMessage);

        //call createChooser method of activity class
        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity =
            unity.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject chooser =
            intentClass.CallStatic<AndroidJavaObject>
                ("createChooser", intentObject, "Share your high score");
        currentActivity.Call("startActivity", chooser);
#elif UNITY_IOS
        NativeShare.Share(ShareMessage);
#endif
    }

    internal static void On_Rate_Btn_Click()
    {
#if UNITY_ANDROID
        Application.OpenURL(GeneralDataManager.Inst.AndroidShareLink);
#endif
        if (Is_Internet_Available()) GameData.UserGiveRating = true;
    }




    internal static bool Is_Internet_Available()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }


    internal static void Increase_Coin(int amount)
    {
        GameData.Coins += amount;
        PlayerPrefs.SetInt("Coins", GameData.Coins);
        GeneralDataManager.Save_Data();
    }

    internal static void Decrease_Coin(int amount)
    {
        GameData.Coins -= amount;
        PlayerPrefs.SetInt("Coins", GameData.Coins);
        GeneralDataManager.Save_Data();
    }

    internal GameObject ToastRef;

    public void Make_Toast(string text)
    {
        if (ToastRef != null && ToastRef.GetComponent<ToastController>().messageText.text == text) return;
        ToastRef = Instantiate(toast, popUpParent);
        ToastRef.GetComponent<ToastController>().messageText.text = text;
    }

    internal void Set_Ad_Loader_panel(bool isOn)
    {
        adLoader.SetActive(isOn);
    }


    internal static IEnumerator Give_Coin_With_Anim(int amount, Transform parent, float waitTimeBeforeStartAnim = 0,
        UnityAction callBackAction = null, Transform target = null)
    {
        Increase_Coin(amount);
        yield return new WaitForSeconds(waitTimeBeforeStartAnim);

        var coins = new List<CoinPrefabController>();

        for (var i = 0; i < 8; i++)
        {
            var coin = Instantiate(GeneralRefrencesManager.Inst.coinPrefab, parent)
                .GetComponent<CoinPrefabController>();
            coin.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            coin.GetComponent<RectTransform>().anchoredPosition = new Vector2(UnityEngine.Random.Range(-7, 7) * 10,
                UnityEngine.Random.Range(-7, 7) * 10);
            coins.Add(coin);
        }

        SoundManager.Inst.Play("CoinCollect");

        foreach (var item in coins)
        {
            item.reff = target.position;
            item.Move_Coin();
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1f);

        HomeScreenSontroller.Inst?.Set_Text();

        yield return new WaitForSeconds(0.55f);

        callBackAction?.Invoke();
    }

    public enum Powers
    {
        Hint,
        Undo,
        Freeze,
        Swap
    }

    internal static void Increase_Powers(int amount, Powers powers)
    {
        switch (powers)
        {
            case Powers.Hint:
                GameData.HintCount += amount;
                break;
            case Powers.Undo:
                GameData.UndoCount += amount;
                break;
            case Powers.Freeze:
                GameData.FreezeCount += amount;
                break;
            case Powers.Swap:
                GameData.SwapCount += amount;
                break;
            default:
                return;
        }
    }

    public static void Play_Button_Click_Sound()
    {
        SoundManager.Inst.Play("btn_click");
    }
}