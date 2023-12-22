using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunchScreenController : SingletonComponent<LunchScreenController>
{
    public void Complete()
    {   
        PlayerPrefs.DeleteKey("UnlockedAllLevels");
        PlayerPrefs.DeleteKey("LevelsUnlocked");
        GameManager.Inst.Show_Screen(GameManager.Screens.HomeScreen);
    }
    public void CloseThisScreen()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (SoundManager.Inst.IsMusicOn)
            SoundManager.Inst.Play("bg_sound", true);
    }

}
