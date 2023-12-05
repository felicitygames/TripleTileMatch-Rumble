using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HeaderController : SingletonComponent<HeaderController>
{
    public static HeaderController instance;
    [SerializeField] private Text diamondCountText, levelNo;
    internal int DiamondCountPerLevel = 0; 
    public Transform diamondPosRef;

    private void OnEnable()
    {
        diamondCountText.text = DiamondCountPerLevel.ToString();
        Set_Text();
    }

    internal void Set_Text()
    {
        diamondCountText.text = DiamondCountPerLevel.ToString();
        levelNo.text = TranslateManager.instance.ActiveTranslation_Dict["LEVEL"] +" " + GeneralDataManager.GameData.LevelNo.ToString();
        var parent = diamondCountText.transform.parent;
        parent.GetComponent<RectTransform>().sizeDelta = new Vector2(diamondCountText.preferredWidth + 117,
            parent.GetComponent<RectTransform>().sizeDelta.y);
        if (!(levelNo.preferredWidth > 245)) return;
        levelNo.GetComponent<ContentSizeFitter>().enabled = false;
        levelNo.resizeTextForBestFit = true;
    }

    public void On_Pause_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        GameManager.Inst.Show_Popup(GameManager.Popups.Pause);
    }
}