using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoSelector : MonoBehaviour
{
    [Header("Logo for different languages")]
    public Image LogoImage;
    public List<Sprite> textures;
    private void Awake()
    {
        //EventHandler.instance.OnLanguageChange += CheckLogoChange;
    }
    void Start()
    {
        CheckLogoChange();
    }
    void CheckLogoChange()
    {
        string lang = PlayerPrefs.GetString("LanguageChar");
        if (lang == "English")
        {
            LogoImage.sprite = textures[0];
        }
        else if (lang == "Japanese")
        {
            LogoImage.sprite = textures[1];
        }
        else if (lang == "Indonesia")
        {
            LogoImage.sprite = textures[5];
        }
        else if (lang == "German")
        {
            LogoImage.sprite = textures[6];
        }
    }
}
