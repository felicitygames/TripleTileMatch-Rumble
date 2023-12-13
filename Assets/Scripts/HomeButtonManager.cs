using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GeneralDataManager;

public class HomeButtonManager : MonoBehaviour
{
    public static HomeButtonManager instance;
    
    public GameObject HomeButton;
    public GameObject ReplayButton;
    [SerializeField] RectTransform bg;
    private void Awake()
    {
        instance = this;
        CheckHome();
    }
    public void CheckHome()
    {
        if (GameData.LevelNo <= 1)
        {
            Debug.Log("Turning off : " + GameData.LevelNo);
            // // HomeButton.SetActive(false);
            if(bg != null)
            bg.sizeDelta = new Vector2(bg.sizeDelta.x, 500);
            if(HomeButton != null)
            HomeButton.SetActive(false);
            if(ReplayButton != null)
            ReplayButton.SetActive(false);
        }
        else
        {
            if(ReplayButton != null)
            ReplayButton.SetActive(true);
            if(HomeButton != null)
            HomeButton.SetActive(true);
        }
    }
}
