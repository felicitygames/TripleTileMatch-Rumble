using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowMessageInActiveLanguage : MonoBehaviour
{
    public List<string> Messages = new List<string>();
    public List<string> Buttons = new List<string>();

    public TMP_Text Message;
    public TMP_Text Button;

    public TMP_FontAsset FontAsset;

    private void Start()
    {
        ShowMessageInCurrentLangugage();
    }

    public void On_okay_click()
    {
        Application.Quit();
    }
    void ShowMessageInCurrentLangugage()
    {
        string activeLanguage = PlayerPrefs.GetString("SelectedLanguage");
        switch (activeLanguage)
        {
            case "English":
                Message.text = Messages[0];
                Message.enableAutoSizing = false;
                Button.text = Buttons[0];
                break;
            case "Indonesia":
                Message.text = Messages[1];
                Message.enableAutoSizing = false;
                Button.text = Buttons[1];
                break;
            case "Japanese":
                Message.font = FontAsset;
                Button.font = FontAsset;
                Message.enableAutoSizing = false;
                Message.text = Messages[2];
                Button.text = Buttons[2];
                break;
            case "Spanish":
                Message.text = Messages[3];
                Message.enableAutoSizing = false;
                Button.text = Buttons[3];
                break;
            case "Russian":
                Message.font = FontAsset;
                Button.font = FontAsset;
                Message.text = Messages[4];
                Message.enableAutoSizing = false;
                Button.text = Buttons[4];
                break;
            case "Portuguese":
                Message.text = Messages[5];
                Message.enableAutoSizing = false;
                Button.text = Buttons[5];
                break;
            case "":
                Message.text = Messages[0];
                Message.enableAutoSizing = false;
                Button.text = Buttons[0];
                break;
        }
    }
}
