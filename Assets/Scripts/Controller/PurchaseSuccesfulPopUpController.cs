using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum MyState
{
    combo,
    onlyCoin,
    removeAds
}


public class PurchaseSuccesfulPopUpController : SingletonComponent<PurchaseSuccesfulPopUpController>
{
    [SerializeField] private RectTransform   combo, onlyCoin, removeAd;
    [SerializeField] private Text coinText, hintText, undoText, freezeText, swapText, onlyCoinText;


    internal void Set_Me(MyState myState, int coins = 0, int hint = 0, int undo = 0, int freeze = 0, int swap = 0)
    {
        switch (myState)
        {
            case MyState.combo:
                combo.gameObject.SetActive(true);
                onlyCoin.gameObject.SetActive(false);
                removeAd.gameObject.SetActive(false);
                coinText.text = coins.ToString();
                hintText.text = hint.ToString();
                undoText.text = undo.ToString();
                freezeText.text = freeze.ToString();
                swapText.text = swap.ToString();
                break;
            case MyState.onlyCoin:
                onlyCoin.gameObject.SetActive(true);
                combo.gameObject.SetActive(false);
                removeAd.gameObject.SetActive(false);
                onlyCoinText.text = coins.ToString();
                break;
            case MyState.removeAds:
                removeAd.gameObject.SetActive(true);
                onlyCoin.gameObject.SetActive(false);
                combo.gameObject.SetActive(false);
                break;
            default:
                return;
        }
        
        ShopPopUpController.Inst.Set_Text();
    }

    public void On_Ok_Btn_Click()
    {
        ShopPopUpController.Inst.Show_Loader(false);
        GameManager.Play_Button_Click_Sound();
        CloseThisPopup();
    }

    public void CloseThisPopup()
    {
        GameManager.activePopup = GameManager.Popups.Null;
        gameObject.SetActive(false);
    }
}