using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguageSelecter : MonoBehaviour
{
    public string language;
    public Color SelectedColor;
    public void SetActiveLanguage()
    {
        for(int i = 1; i < this.gameObject.transform.parent.childCount;i++)
        {
            this.gameObject.transform.parent.transform.GetChild(i).GetComponent<Image>().color = Color.white;
            this.gameObject.transform.parent.transform.GetChild(i).GetChild(0).GetComponent<TMP_Text>().color = Color.black;
        }
        this.gameObject.GetComponent<Image>().color = SelectedColor;
        this.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().color = Color.white;
        SelectionManager.instance.language = language;
        PlayerPrefs.SetString("SelectedLanguage", language);
    }
}
