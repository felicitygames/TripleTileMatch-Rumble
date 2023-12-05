using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance;
    public GameObject _languagePopup;
    public GameObject _ConfirmationPopup;
    private void Awake()
    {
        instance = this;
        ShowPreviouslySelected();
    }
    void ShowPreviouslySelected()
    {
        LanguageSelecter[] langugages = FindObjectsOfType<LanguageSelecter>();
        string activeLanguage = PlayerPrefs.GetString("SelectedLanguage");
        for(int i =0; i < langugages.Length; i++)
        {
            if (langugages[i].language == activeLanguage)
            {
                langugages[i].SetActiveLanguage();
            }
        }
    }
    public string language = "English";
    public void OnOkayClick()
    {
        PlayerPrefs.SetString("SelectedLanguage", language);
        _languagePopup.SetActive(false);
        //_ConfirmationPopup.SetActive(true);
        SceneManager.LoadScene(2);
        //EventHandler.instance.InvokeLanguageUpdate();
    }
}
