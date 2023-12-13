using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StaticTextObjectTranslater : MonoBehaviour
{
    [SerializeField] string Key;
    [SerializeField] string ValueToAppend;
    
    // private void OnEnable()
    // {
    //     SetLangs();
    // }
    // private void Awake()
    // {
    //     //EventHandler.instance.OnLanguageChange += SetLangs;
    // }
    // void SetLangs()
    // {
    //     if(this.gameObject.activeInHierarchy)
    //     {
    //         StartCoroutine(SetTranslatedValue(Key));
    //     }
    // }
    // IEnumerator SetTranslatedValue(string key)
    // {
    //     while(!TranslateManager.instance.initialised)
    //     {
    //         yield return null;
    //     }
    //     if(this.gameObject.GetComponent<TMP_Text>() != null)
    //     {
    //         if (PlayerPrefs.GetString("LanguageChar") == "Japanese")
    //         {
    //             this.gameObject.GetComponent<TMP_Text>().font = TranslateManager.instance.Japanese;
    //             this.gameObject.GetComponent<TMP_Text>().enableAutoSizing = true;
    //         }
    //         this.gameObject.GetComponent<TMP_Text>().text = TranslateManager.instance.ActiveTranslation_Dict[key] + " " +ValueToAppend;
    //     }
    //     else
    //     {
    //         //if (TranslateManager.instance.activeLanguage == "Japanese" || TranslateManager.instance.activeLanguage == "Russian")
    //         //{
    //         //    this.gameObject.GetComponent<TMP_Text>().font = TranslateManager.instance.Japanese;
    //         //    this.gameObject.GetComponent<TMP_Text>().enableAutoSizing = true;
    //         //}
    //         if (PlayerPrefs.GetString("LanguageChar") == "Japanese")
    //         {
                
    //             this.gameObject.GetComponent<Text>().font = TranslateManager.instance.Japanese1;
    //             this.gameObject.GetComponent<Text>().resizeTextForBestFit = true;
    //         }
    //         this.gameObject.GetComponent<Text>().text = TranslateManager.instance.ActiveTranslation_Dict[key] + " " + ValueToAppend;
    //     }
        
    // }
    // IEnumerator VerifyTranslation()
    // {
    //     yield return null;
    //     if(this.gameObject.GetComponent<TMP_Text>().text != TranslateManager.instance.ActiveTranslation_Dict[Key])
    //     {
    //         this.gameObject.GetComponent<TMP_Text>().text = TranslateManager.instance.ActiveTranslation_Dict[Key];
    //     }
    //     StartCoroutine(VerifyTranslation());
    // }
}
