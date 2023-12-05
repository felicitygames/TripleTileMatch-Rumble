using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TranslateManager : MonoBehaviour
{
    public TMP_FontAsset Japanese;
    public Font Japanese1;
    public int totalGOs;
    [Header("All gameobjects with TMP text components")]
    private List<GameObject> AllTextObjects = new List<GameObject>();

    [Header("Dictionary of words to translate with languages")]
    [SerializeField] private Dictionary<string, string> TranslatedDict_English = new Dictionary<string, string>();
    [SerializeField] private Dictionary<string, string> TranslatedDict_Indonesia = new Dictionary<string, string>();
    [SerializeField] private Dictionary<string, string> TranslatedDict_Japanese = new Dictionary<string, string>();
    public Dictionary<string, string> ActiveTranslation_Dict = new Dictionary<string, string>();



    public static TranslateManager instance;
    public bool initialised;
    private void Awake()
    { 
        instance = this;
    }

    void Start()
    {   string activeLanguage = PlayerPrefs.GetString("LanguageChar");
        switch (activeLanguage)
        {
            case "English":
                ActiveTranslation_Dict.Clear();
                ActiveTranslation_Dict.Add("COMPLETE", "COMPLETE");
                ActiveTranslation_Dict.Add("REPLAY", "REPLAY");
                ActiveTranslation_Dict.Add("DOUBLE_COINS", "DOUBLE COINS");
                ActiveTranslation_Dict.Add("HINT", "HINT");
                ActiveTranslation_Dict.Add("SKIP_LEVEL", "SKIP LEVEL");
                ActiveTranslation_Dict.Add("LEVEL", "Level");
                ActiveTranslation_Dict.Add("LEVELS", "Levels");
                ActiveTranslation_Dict.Add("SHARE", "Share");
                ActiveTranslation_Dict.Add("LANG", "Language");
                ActiveTranslation_Dict.Add("OFF", "OFF");
                ActiveTranslation_Dict.Add("ON", "ON");
                ActiveTranslation_Dict.Add("PLAY", "PLAY");
                ActiveTranslation_Dict.Add("NEXT", "NEXT");
                ActiveTranslation_Dict.Add("COIN", "COIN");
                ActiveTranslation_Dict.Add("STAR_CHEST", "STAR CHEST");
                ActiveTranslation_Dict.Add("LEVEL_CHEST", "LEVEL CHEST");
                ActiveTranslation_Dict.Add("RATE_US", "Rate us");
                ActiveTranslation_Dict.Add("OKAY", "Okay");
                ActiveTranslation_Dict.Add("SHOP", "SHOP");
                ActiveTranslation_Dict.Add("SHOP_IAP", "SWAG SHOP");
                ActiveTranslation_Dict.Add("SHOP_POPUP", "CHECK IN TO THE SWAG SHOP TO PERSONALIZE YOUR DOGSTER!");
                ActiveTranslation_Dict.Add("WORDS", "WORDS");
                ActiveTranslation_Dict.Add("STRING", "STRING");
                ActiveTranslation_Dict.Add("ARRAY", "ARRAY");
                ActiveTranslation_Dict.Add("SIZE", "SIZE");
                ActiveTranslation_Dict.Add("COINS", "COINS");
                ActiveTranslation_Dict.Add("OPTION", "OPTION");
                ActiveTranslation_Dict.Add("MUSIC", "MUSIC");
                ActiveTranslation_Dict.Add("SOUND", "SOUND");
                ActiveTranslation_Dict.Add("CLAIM", "CLAIM");
                ActiveTranslation_Dict.Add("POWER_UP", "Increase Strength");
                ActiveTranslation_Dict.Add("CHOOSE_ONE", "Choose One");
                ActiveTranslation_Dict.Add("WATCH", "WATCH");
                ActiveTranslation_Dict.Add("HOW_TO_PLAY", "How to Play");
                ActiveTranslation_Dict.Add("INSTRUCT", "Match 3 same tiles\r\n\r\n");
                ActiveTranslation_Dict.Add("PAUSE", "PAUSE");
                ActiveTranslation_Dict.Add("HOME", "HOME");
                ActiveTranslation_Dict.Add("RETRY", "RETRY");
                ActiveTranslation_Dict.Add("BUTTON", "BUTTON");
                ActiveTranslation_Dict.Add("TAP_ON_THE_BALL", "TAP ON THE BALL");
                ActiveTranslation_Dict.Add("COLLECT", "COLLECT");
                ActiveTranslation_Dict.Add("NEW_OBJECTS", "NEW_OBJECTS!");
                ActiveTranslation_Dict.Add("POWER_UNLOCKED", "POWER_UNLOCKED");
                ActiveTranslation_Dict.Add("LOADING_AD", "LOADING AD...");
                ActiveTranslation_Dict.Add("COLLECT_LEVEL_CHEST", "COLLECT LEVEL CHEST");
                ActiveTranslation_Dict.Add("COLLECT_STAR_CHEST", "COLLECT STAR CHEST");
                ActiveTranslation_Dict.Add("CONTINUE", "CONTINUE");
                ActiveTranslation_Dict.Add("OUT_OF_SPACE", "OUT OF SPACE");
                ActiveTranslation_Dict.Add("CONFIRM_EXIT", "Are you sure you want to exit the game?");
                ActiveTranslation_Dict.Add("YES", "YES");
                ActiveTranslation_Dict.Add("NO", "NO");
                ActiveTranslation_Dict.Add("EXIT", "EXIT");
                ActiveTranslation_Dict.Add("DAILY_BONUS", "DAILY_BONUS");
                break;
            case "Indonesia":
                ActiveTranslation_Dict.Clear();
                ActiveTranslation_Dict.Add("COMPLETE", "MENYELESAIKAN");
                ActiveTranslation_Dict.Add("REPLAY", "MEMUTAR ULANG");
                ActiveTranslation_Dict.Add("PLAY", "BERMAIN");
                ActiveTranslation_Dict.Add("DOUBLE_COINS", "KOIN GANDA");
                ActiveTranslation_Dict.Add("COIN", "KOIN");
                ActiveTranslation_Dict.Add("STAR_CHEST", "Peti Bintang");
                ActiveTranslation_Dict.Add("LEVEL_CHEST", "Dada Tingkat");
                ActiveTranslation_Dict.Add("HINT", "PETUNJUK");
                ActiveTranslation_Dict.Add("SKIP_LEVEL", "LEWATI LEVEL");
                ActiveTranslation_Dict.Add("LEVEL", "TINGKAT");
                ActiveTranslation_Dict.Add("LEVELS", "TINGKATAN");
                ActiveTranslation_Dict.Add("SHARE", "MEMBAGIKAN");
                ActiveTranslation_Dict.Add("NEXT", "BERIKUTNYA");
                ActiveTranslation_Dict.Add("LANG", "Bahasa");
                ActiveTranslation_Dict.Add("OFF", "MATI");
                ActiveTranslation_Dict.Add("ON", "PADA");
                ActiveTranslation_Dict.Add("RATE_US", "NILAI KAMI");
                ActiveTranslation_Dict.Add("OKAY", "OKE");
                ActiveTranslation_Dict.Add("SHOP", "BELANJA");
                ActiveTranslation_Dict.Add("SHOP_IAP", "TOKO SWAG");
                ActiveTranslation_Dict.Add("SHOP_POPUP", "PERIKSA KE TOKO SWAG UNTUK MEMPERSONALISASI DOGSTER ANDA!");
                ActiveTranslation_Dict.Add("WORDS", "KATA-KATA");
                ActiveTranslation_Dict.Add("STRING", "STRING");
                ActiveTranslation_Dict.Add("ARRAY", "ARRAY");
                ActiveTranslation_Dict.Add("SIZE", "UKURAN");
                ActiveTranslation_Dict.Add("COINS", "KOIN");
                ActiveTranslation_Dict.Add("OPTION", "OPSI");
                ActiveTranslation_Dict.Add("MUSIC", "MUSIK");
                ActiveTranslation_Dict.Add("SOUND", "SUARA");
                ActiveTranslation_Dict.Add("CLAIM", "KLAIM");
                ActiveTranslation_Dict.Add("POWER_UP", "TINGKATKAN KEKUATAN");
                ActiveTranslation_Dict.Add("CHOOSE_ONE", "PILIH SATU");
                ActiveTranslation_Dict.Add("WATCH", "TONTON");
                ActiveTranslation_Dict.Add("HOW_TO_PLAY", "CARA BERMAIN");
                ActiveTranslation_Dict.Add("INSTRUCT", "Cocokkan 3 ubin yang sama\r\n\r\n");
                ActiveTranslation_Dict.Add("PAUSE", "JEDA");
                ActiveTranslation_Dict.Add("HOME", "BERANDA");
                ActiveTranslation_Dict.Add("RETRY", "COBA LAGI");
                ActiveTranslation_Dict.Add("BUTTON", "TOMBOL");
                ActiveTranslation_Dict.Add("TAP_ON_THE_BALL", "TAP PADA BOLA");
                ActiveTranslation_Dict.Add("COLLECT", "KUMPULKAN");
                ActiveTranslation_Dict.Add("NEW_OBJECTS", "OBJEK BARU !");
                ActiveTranslation_Dict.Add("POWER_UNLOCKED", "KEKUATAN TERBUKA");
                ActiveTranslation_Dict.Add("LOADING_AD", "MEMUAT IKLAN...");
                ActiveTranslation_Dict.Add("COLLECT_LEVEL_CHEST", "KUMPULKAN PETI LEVEL");
                ActiveTranslation_Dict.Add("COLLECT_STAR_CHEST", "KUMPULKAN PETI BINTANG");
                ActiveTranslation_Dict.Add("CONTINUE", "LANJUTKAN");
                ActiveTranslation_Dict.Add("OUT_OF_SPACE", "KEHABISAN RUANG");
                ActiveTranslation_Dict.Add("CONFIRM_EXIT", "Apakah Anda yakin ingin keluar dari permainan");
                ActiveTranslation_Dict.Add("YES", "YA");
                ActiveTranslation_Dict.Add("NO", "TIDAK");
                ActiveTranslation_Dict.Add("EXIT", "KELUAR");
                ActiveTranslation_Dict.Add("DAILY_BONUS", "BONUS HARIAN");
                break;
            case "Japanese":
                ActiveTranslation_Dict.Clear();
                InitialiseTexts();
                ActiveTranslation_Dict.Add("COMPLETE", "完了");
                ActiveTranslation_Dict.Add("DAILY_BONUS", "デイリーボーナス");
                ActiveTranslation_Dict.Add("CONFIRM_EXIT", "ゲームを終了してもよろしいですか");
                ActiveTranslation_Dict.Add("YES", "はい ");
                ActiveTranslation_Dict.Add("NO", "いいえ");
                ActiveTranslation_Dict.Add("EXIT", "終了する");
                ActiveTranslation_Dict.Add("CONTINUE", "続ける");
                ActiveTranslation_Dict.Add("OUT_OF_SPACE", "スペース不足 ");
                ActiveTranslation_Dict.Add("REPLAY", "リプレイ");
                ActiveTranslation_Dict.Add("DOUBLE_COINS", "ダブルコイン");
                ActiveTranslation_Dict.Add("HINT", "ヒント");
                ActiveTranslation_Dict.Add("SKIP_LEVEL", "スキップレベル");
                ActiveTranslation_Dict.Add("LEVEL", "レベル");
                ActiveTranslation_Dict.Add("LEVELS", "レベル");
                ActiveTranslation_Dict.Add("SHARE", "共有");
                ActiveTranslation_Dict.Add("LANG", "言語");
                ActiveTranslation_Dict.Add("CLAIM", "請求する");
                ActiveTranslation_Dict.Add("NEXT", "次へ");
                ActiveTranslation_Dict.Add("POWER_UP", "強化する");
                ActiveTranslation_Dict.Add("CHOOSE_ONE", "一つ選ぶ");
                ActiveTranslation_Dict.Add("HOW_TO_PLAY", "遊び方");
                ActiveTranslation_Dict.Add("INSTRUCT", "同じ種類のタイルを3つ揃える");
                ActiveTranslation_Dict.Add("WATCH", "見る");
                ActiveTranslation_Dict.Add("OFF", "オフ");
                ActiveTranslation_Dict.Add("ON", "の上");
                ActiveTranslation_Dict.Add("PAUSE", "一時停止");
                ActiveTranslation_Dict.Add("HOME", "ホーム");
                ActiveTranslation_Dict.Add("RETRY", "やり直す ");
                ActiveTranslation_Dict.Add("RATE_US", "評価してください");
                ActiveTranslation_Dict.Add("OKAY", "わかった");
                ActiveTranslation_Dict.Add("SHOP", "買い物");
                ActiveTranslation_Dict.Add("SHOP_IAP", "スワッグショップ");
                ActiveTranslation_Dict.Add("SHOP_POPUP", "あなたの犬をパーソナライズするには、グッズショップにチェックインしてください!");
                ActiveTranslation_Dict.Add("WORDS", "ワーズ");
                ActiveTranslation_Dict.Add("STRING", "ストリング");
                ActiveTranslation_Dict.Add("ARRAY", "アレイ");
                ActiveTranslation_Dict.Add("SIZE", "サイズ");
                ActiveTranslation_Dict.Add("LEVEL_CHEST", "レベルのチェスト");
                ActiveTranslation_Dict.Add("STAR_CHEST", "スターチェスト");
                ActiveTranslation_Dict.Add("COIN", "コイン");
                ActiveTranslation_Dict.Add("OPTION", "オプション");
                ActiveTranslation_Dict.Add("MUSIC", "音楽 ");
                ActiveTranslation_Dict.Add("SOUND", "サウンド");
                ActiveTranslation_Dict.Add("PLAY", "プレー");
                ActiveTranslation_Dict.Add("BUTTON", "ボタン");
                ActiveTranslation_Dict.Add("TAP_ON_THE_BALL", "ボールをタップ");
                ActiveTranslation_Dict.Add("COLLECT", "収集");
                ActiveTranslation_Dict.Add("NEW_OBJECTS", "新しいオブジェクト !");
                ActiveTranslation_Dict.Add("POWER_UNLOCKED", "パワーが解放された");
                ActiveTranslation_Dict.Add("LOADING_AD", "広告の読み込み中...");
                ActiveTranslation_Dict.Add("COLLECT_LEVEL_CHEST", "レベルのチェストを収集");
                ActiveTranslation_Dict.Add("COLLECT_STAR_CHEST", "スターチェストを収集");
                break;
            case "German":
                ActiveTranslation_Dict.Clear();
                ActiveTranslation_Dict.Add("COMPLETE", "VOLLSTANDIG");
                ActiveTranslation_Dict.Add("REPLAY", "WIEDERHOLUNG");
                ActiveTranslation_Dict.Add("DOUBLE_COINS", "DOPPELMUNZEN");
                ActiveTranslation_Dict.Add("HINT", "HINT");
                ActiveTranslation_Dict.Add("SKIP_LEVEL", "EBENE UBERSPRINGEN");
                ActiveTranslation_Dict.Add("LEVEL", "EBENE");
                ActiveTranslation_Dict.Add("LEVELS", "EBENEN");
                ActiveTranslation_Dict.Add("SHARE", "AKTIE");
                ActiveTranslation_Dict.Add("LANG", "Sprache");
                ActiveTranslation_Dict.Add("OFF", "AUS");
                ActiveTranslation_Dict.Add("ON", "AN");
                ActiveTranslation_Dict.Add("PLAY", "SPIELEN");
                ActiveTranslation_Dict.Add("NEXT", "NACHSTE");
                ActiveTranslation_Dict.Add("COIN", "MUNZE");
                ActiveTranslation_Dict.Add("STAR_CHEST", "STERN TRUHE");
                ActiveTranslation_Dict.Add("LEVEL_CHEST", "LEVEL TRUHE");
                ActiveTranslation_Dict.Add("RATE_US", "Bewerte uns");
                ActiveTranslation_Dict.Add("OKAY", "Okay");
                ActiveTranslation_Dict.Add("SHOP", "LADEN");
                ActiveTranslation_Dict.Add("SHOP_IAP", "SWAG LADEN");
                ActiveTranslation_Dict.Add("SHOP_POPUP", "CHECK IN BEIM SWAG LADEN, UM DEINEN DOGSTER ZU PERSONALISIEREN!");
                ActiveTranslation_Dict.Add("WORDS", "WORTER");
                ActiveTranslation_Dict.Add("STRING", "ZEICHENFOLGE");
                ActiveTranslation_Dict.Add("ARRAY", "ARRAY");
                ActiveTranslation_Dict.Add("SIZE", "GROSSE");
                ActiveTranslation_Dict.Add("COINS", "MUNZEN");
                ActiveTranslation_Dict.Add("OPTION", "OPTION");
                ActiveTranslation_Dict.Add("MUSIC", "MUSIK");
                ActiveTranslation_Dict.Add("SOUND", "TON");
                ActiveTranslation_Dict.Add("CLAIM", "BEANSPRUCHEN");
                ActiveTranslation_Dict.Add("POWER_UP", "Stärke erhohen");
                ActiveTranslation_Dict.Add("CHOOSE_ONE", "Wahle eins");
                ActiveTranslation_Dict.Add("WATCH", "ANSEHEN");
                ActiveTranslation_Dict.Add("HOW_TO_PLAY", "Anleitung");
                ActiveTranslation_Dict.Add("INSTRUCT", "Drehe 3 gleiche Kacheln");
                ActiveTranslation_Dict.Add("PAUSE", "PAUSE");
                ActiveTranslation_Dict.Add("HOME", "HEIM");
                ActiveTranslation_Dict.Add("RETRY", "WIEDERHOLEN");
                ActiveTranslation_Dict.Add("BUTTON", "SCHALTFLACHE");
                ActiveTranslation_Dict.Add("TAP_ON_THE_BALL", "AUF DEN BALL TIPPEN");
                ActiveTranslation_Dict.Add("COLLECT", "SAMMELN");
                ActiveTranslation_Dict.Add("NEW_OBJECTS", "NEUE OBJEKTE!");
                ActiveTranslation_Dict.Add("POWER_UNLOCKED", "POWER FREIGESCHALTET");
                ActiveTranslation_Dict.Add("LOADING_AD", "WERBUNG WIRD GELADEN...");
                ActiveTranslation_Dict.Add("COLLECT_LEVEL_CHEST", "LEVEL TRUHE SAMMELN");
                ActiveTranslation_Dict.Add("COLLECT_STAR_CHEST", "STERN TRUHE SAMMELN");
                ActiveTranslation_Dict.Add("CONTINUE", "WEITER");
                ActiveTranslation_Dict.Add("OUT_OF_SPACE", "KEIN PLATZ MEHR");
                ActiveTranslation_Dict.Add("CONFIRM_EXIT", "Mochtest du das Spiel wirklich verlassen?");
                ActiveTranslation_Dict.Add("YES", "JA");
                ActiveTranslation_Dict.Add("NO", "NEIN");
                ActiveTranslation_Dict.Add("EXIT", "BEENDEN");
                ActiveTranslation_Dict.Add("DAILY_BONUS", "TAGLICHER BONUS");
                break;
            case "":
                ActiveTranslation_Dict.Clear();
                ActiveTranslation_Dict.Add("COMPLETE", "COMPLETE");
                ActiveTranslation_Dict.Add("REPLAY", "REPLAY");
                ActiveTranslation_Dict.Add("DOUBLE_COINS", "DOUBLE COINS");
                ActiveTranslation_Dict.Add("HINT", "HINT");
                ActiveTranslation_Dict.Add("SKIP_LEVEL", "SKIP LEVEL");
                ActiveTranslation_Dict.Add("LEVEL", "Level");
                ActiveTranslation_Dict.Add("LEVELS", "Levels");
                ActiveTranslation_Dict.Add("SHARE", "Share");
                ActiveTranslation_Dict.Add("LANG", "Language");
                ActiveTranslation_Dict.Add("OFF", "OFF");
                ActiveTranslation_Dict.Add("ON", "ON");
                ActiveTranslation_Dict.Add("PLAY", "PLAY");
                ActiveTranslation_Dict.Add("NEXT", "NEXT");
                ActiveTranslation_Dict.Add("COIN", "COIN");
                ActiveTranslation_Dict.Add("STAR_CHEST", "STAR CHEST");
                ActiveTranslation_Dict.Add("LEVEL_CHEST", "LEVEL CHEST");
                ActiveTranslation_Dict.Add("RATE_US", "Rate us");
                ActiveTranslation_Dict.Add("OKAY", "Okay");
                ActiveTranslation_Dict.Add("SHOP", "SHOP");
                ActiveTranslation_Dict.Add("SHOP_IAP", "SWAG SHOP");
                ActiveTranslation_Dict.Add("SHOP_POPUP", "CHECK IN TO THE SWAG SHOP TO PERSONALIZE YOUR DOGSTER!");
                ActiveTranslation_Dict.Add("WORDS", "WORDS");
                ActiveTranslation_Dict.Add("STRING", "STRING");
                ActiveTranslation_Dict.Add("ARRAY", "ARRAY");
                ActiveTranslation_Dict.Add("SIZE", "SIZE");
                ActiveTranslation_Dict.Add("COINS", "COINS");
                ActiveTranslation_Dict.Add("OPTION", "OPTION");
                ActiveTranslation_Dict.Add("MUSIC", "MUSIC");
                ActiveTranslation_Dict.Add("SOUND", "SOUND");
                ActiveTranslation_Dict.Add("CLAIM", "CLAIM");
                ActiveTranslation_Dict.Add("POWER_UP", "Increase Strength");
                ActiveTranslation_Dict.Add("CHOOSE_ONE", "Choose One");
                ActiveTranslation_Dict.Add("WATCH", "WATCH");
                ActiveTranslation_Dict.Add("HOW_TO_PLAY", "How to Play");
                ActiveTranslation_Dict.Add("INSTRUCT", "Match 3 same tiles\r\n\r\n");
                ActiveTranslation_Dict.Add("PAUSE", "PAUSE");
                ActiveTranslation_Dict.Add("HOME", "HOME");
                ActiveTranslation_Dict.Add("RETRY", "RETRY");
                ActiveTranslation_Dict.Add("BUTTON", "BUTTON");
                ActiveTranslation_Dict.Add("TAP_ON_THE_BALL", "TAP ON THE BALL");
                ActiveTranslation_Dict.Add("COLLECT", "COLLECT");
                ActiveTranslation_Dict.Add("NEW_OBJECTS", "NEW_OBJECTS!");
                ActiveTranslation_Dict.Add("POWER_UNLOCKED", "POWER_UNLOCKED");
                ActiveTranslation_Dict.Add("LOADING_AD", "LOADING AD...");
                ActiveTranslation_Dict.Add("COLLECT_LEVEL_CHEST", "COLLECT LEVEL CHEST");
                ActiveTranslation_Dict.Add("COLLECT_STAR_CHEST", "COLLECT STAR CHEST");
                ActiveTranslation_Dict.Add("CONTINUE", "CONTINUE");
                ActiveTranslation_Dict.Add("OUT_OF_SPACE", "OUT OF SPACE");
                ActiveTranslation_Dict.Add("CONFIRM_EXIT", "Are you sure you want to exit the game?");
                ActiveTranslation_Dict.Add("YES", "YES");
                ActiveTranslation_Dict.Add("NO", "NO");
                ActiveTranslation_Dict.Add("EXIT", "EXIT");
                ActiveTranslation_Dict.Add("DAILY_BONUS", "DAILY_BONUS");
                break;
            default:
                ActiveTranslation_Dict.Clear();
                ActiveTranslation_Dict.Add("COMPLETE", "COMPLETE");
                ActiveTranslation_Dict.Add("REPLAY", "REPLAY");
                ActiveTranslation_Dict.Add("DOUBLE_COINS", "DOUBLE COINS");
                ActiveTranslation_Dict.Add("HINT", "HINT");
                ActiveTranslation_Dict.Add("SKIP_LEVEL", "SKIP LEVEL");
                ActiveTranslation_Dict.Add("LEVEL", "Level");
                ActiveTranslation_Dict.Add("LEVELS", "Levels");
                ActiveTranslation_Dict.Add("SHARE", "Share");
                ActiveTranslation_Dict.Add("LANG", "Language");
                ActiveTranslation_Dict.Add("OFF", "OFF");
                ActiveTranslation_Dict.Add("ON", "ON");
                ActiveTranslation_Dict.Add("PLAY", "PLAY");
                ActiveTranslation_Dict.Add("NEXT", "NEXT");
                ActiveTranslation_Dict.Add("COIN", "COIN");
                ActiveTranslation_Dict.Add("STAR_CHEST", "STAR CHEST");
                ActiveTranslation_Dict.Add("LEVEL_CHEST", "LEVEL CHEST");
                ActiveTranslation_Dict.Add("RATE_US", "Rate us");
                ActiveTranslation_Dict.Add("OKAY", "Okay");
                ActiveTranslation_Dict.Add("SHOP", "SHOP");
                ActiveTranslation_Dict.Add("SHOP_IAP", "SWAG SHOP");
                ActiveTranslation_Dict.Add("SHOP_POPUP", "CHECK IN TO THE SWAG SHOP TO PERSONALIZE YOUR DOGSTER!");
                ActiveTranslation_Dict.Add("WORDS", "WORDS");
                ActiveTranslation_Dict.Add("STRING", "STRING");
                ActiveTranslation_Dict.Add("ARRAY", "ARRAY");
                ActiveTranslation_Dict.Add("SIZE", "SIZE");
                ActiveTranslation_Dict.Add("COINS", "COINS");
                ActiveTranslation_Dict.Add("OPTION", "OPTION");
                ActiveTranslation_Dict.Add("MUSIC", "MUSIC");
                ActiveTranslation_Dict.Add("SOUND", "SOUND");
                ActiveTranslation_Dict.Add("CLAIM", "CLAIM");
                ActiveTranslation_Dict.Add("POWER_UP", "Increase Strength");
                ActiveTranslation_Dict.Add("CHOOSE_ONE", "Choose One");
                ActiveTranslation_Dict.Add("WATCH", "WATCH");
                ActiveTranslation_Dict.Add("HOW_TO_PLAY", "How to Play");
                ActiveTranslation_Dict.Add("INSTRUCT", "Match 3 same tiles\r\n\r\n");
                ActiveTranslation_Dict.Add("PAUSE", "PAUSE");
                ActiveTranslation_Dict.Add("HOME", "HOME");
                ActiveTranslation_Dict.Add("RETRY", "RETRY");
                ActiveTranslation_Dict.Add("BUTTON", "BUTTON");
                ActiveTranslation_Dict.Add("TAP_ON_THE_BALL", "TAP ON THE BALL");
                ActiveTranslation_Dict.Add("COLLECT", "COLLECT");
                ActiveTranslation_Dict.Add("NEW_OBJECTS", "NEW_OBJECTS!");
                ActiveTranslation_Dict.Add("POWER_UNLOCKED", "POWER_UNLOCKED");
                ActiveTranslation_Dict.Add("LOADING_AD", "LOADING AD...");
                ActiveTranslation_Dict.Add("COLLECT_LEVEL_CHEST", "COLLECT LEVEL CHEST");
                ActiveTranslation_Dict.Add("COLLECT_STAR_CHEST", "COLLECT STAR CHEST");
                ActiveTranslation_Dict.Add("CONTINUE", "CONTINUE");
                ActiveTranslation_Dict.Add("OUT_OF_SPACE", "OUT OF SPACE");
                ActiveTranslation_Dict.Add("CONFIRM_EXIT", "Are you sure you want to exit the game?");
                ActiveTranslation_Dict.Add("YES", "YES");
                ActiveTranslation_Dict.Add("NO", "NO");
                ActiveTranslation_Dict.Add("EXIT", "EXIT");
                ActiveTranslation_Dict.Add("DAILY_BONUS", "DAILY_BONUS");
                break;
        }
        initialised = true;
    }

    private List<string> Words = new List<string>();
    public static List<GameObject> GetAllObjectsInScene(bool includeInactive = false)
    {
        List<GameObject> objects = new List<GameObject>();

        Scene currentScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = currentScene.GetRootGameObjects();

        foreach (GameObject rootObject in rootObjects)
        {
            GetAllChildObjects(rootObject, objects, includeInactive);
        }

        return objects;
    }

    private static void GetAllChildObjects(GameObject parent, List<GameObject> objects, bool includeInactive)
    {
        if (includeInactive || parent.activeInHierarchy)
        {
            objects.Add(parent);
        }

        foreach (Transform child in parent.transform)
        {
            GetAllChildObjects(child.gameObject, objects, includeInactive);
        }
    }
    private void InitialiseTexts()
    {
        // Get all GameObjects in the scene
        GameObject[] gameObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject go in gameObjects)
        {
            totalGOs++;
            TextMeshProUGUI text = go.GetComponent<TextMeshProUGUI>();
            if (text != null)
            {
                text.font = Japanese;
                text.enableAutoSizing = true;
                text.UpdateFontAsset();
                AllTextObjects.Add(text.gameObject);
            }
            Text text1 = go.GetComponent<Text>();
            if (text1 != null)
            {
                text1.font = Japanese1;
                text1.resizeTextForBestFit = true;
                AllTextObjects.Add(text1.gameObject);
            }
        }
    }
}
