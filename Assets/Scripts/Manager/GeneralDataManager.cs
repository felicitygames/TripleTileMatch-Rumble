using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class GeneralDataManager : SingletonComponent<GeneralDataManager>
{
    internal string AndroidShareLink = "https://play.google.com/store/apps/details?id=";

    public static Game_Data GameData = new Game_Data();

    public static GeneralDataManager instance;

    private const string GameDataSaveKey = "game_Data";

    private const string PaidAdsRemoveKey = "PaidAdsRemove";
    private const string ShopDataKey = "ShopSavedDatas";

    public static string ShopSavedData
    {
        get => PlayerPrefs.GetString(ShopDataKey);
        set => PlayerPrefs.SetString(ShopDataKey, value);
    }

    public bool testMode;

    internal readonly List<int> LevelCountForNewElementOpen = new List<int>()
        {  2,5, 10, 15,20,25, 30, 40, 50,60 , 70 , 80 , 90 , 100 , 120,140,160,180,200,220,240,260,280,300,330,360,390};

    internal Dictionary<int, List<string>> WillOpenElement = new Dictionary<int, List<string>>();

    public static bool PurchaseAdsRemove
    {
        get
        {
            if (PlayerPrefs.GetString(PaidAdsRemoveKey) == "True")
            {

            }
            //AdsManager.inst.RemoveAdsApply();

            return PlayerPrefs.GetString(PaidAdsRemoveKey) == "True" ? true : false;
        }
        set
        {
            PlayerPrefs.SetString(PaidAdsRemoveKey, value.ToString());
            if (value == true)
            {

            }
            //AdsManager.inst.RemoveAdsApply();
        }
    }


    protected override void Awake()
    {   Debug.Log("dsd");
        WillOpenElement =
          JsonConvert.DeserializeObject<Dictionary<int, List<string>>>(Resources.Load("Data/OpenElementData")
              .ToString());

        AndroidShareLink = "https://play.google.com/store/apps/details?id=" + Application.identifier;

        // if (PlayerPrefs.HasKey("game_Data"))
        // {
        //     //Load_Data();
        // }
        // else
        // {
        //     for (int i = 0; i < 18; i++)
        //     {
        //         GameData.OpenElementsIndex.Add(i);
        //     }
        //     GameData.NewElementOpenCount++;

        // }

        if (testMode)
        {
            GameData.Coins += 1000;
            GameData.HintCount += 10;
            GameData.UndoCount += 10;
            GameData.SwapCount += 10;
            GameData.FreezeCount += 10;
        }
    }

    private void OnApplicationQuit()
    {
        Save_Data();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Save_Data();
        }
    }

    public static void Save_Data()
    {
        PlayerPrefs.SetString("game_Data", JsonConvert.SerializeObject(GameData));
        PlayerPrefs.Save();
    }

    // private static void Load_Data()
    // {
    //     GameData = JsonConvert.DeserializeObject<Game_Data>(PlayerPrefs.GetString(GameDataSaveKey));
    // }

    public class Game_Data
    {
        public int Diamonds = 0;
        public int Coins = 50;
        public int LevelNo = 1;
        public int HintCount = 1;
        public int UndoCount = 1;
        public int SwapCount = 1;
        public int FreezeCount = 1;
        public int DailyRewardClaimedDayCount = 2;
        public int NewElementOpenCount = 0;
        public bool IsHowToPlayPopUpShow = false;
        public bool IsTutorialShow = false;
        public bool IsLevelChestClaim = false;
        public int StarChestOpenCount = 1;
        public int StarChestDiamond = 0;

        public string DailyRewardOpenDate;
        public bool UserGiveRating = false;
        public readonly List<int> OpenElementsIndex = new List<int>();
        public bool isSplashShow = false;
    }
}