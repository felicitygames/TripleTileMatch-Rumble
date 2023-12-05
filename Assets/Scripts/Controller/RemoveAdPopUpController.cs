using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAdPopUpController : SingletonComponent<RemoveAdPopUpController>
{
    // Start is called before the first frame update
    void Start()
    {
        //IAPManager.inst.InitilizitionIAP();
    }
    // Update is called once per frame
    public void On_Buy_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        if (GameManager.Is_Internet_Available())
        {
            //IAPManager.inst.BuyProduct(//IAPManager.inst.productIDs[7]);
        }
        else
        {
            GameManager.Inst.Make_Toast("No Internet Connection!");
        }
    }

    public void On_Close_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        CloseThisPopup();
    }

    public void CloseThisPopup()
    {
        GameManager.activePopup = GameManager.Popups.Null;
        Destroy(gameObject);
    }
}
