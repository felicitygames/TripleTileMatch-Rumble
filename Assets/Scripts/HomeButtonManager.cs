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
            bg.sizeDelta = new Vector2(bg.sizeDelta.x, 500);
            HomeButton.SetActive(false);
            ReplayButton.SetActive(false);
        }
        else
        {
            ReplayButton.SetActive(true);
            HomeButton.SetActive(true);
        }
    }
}
