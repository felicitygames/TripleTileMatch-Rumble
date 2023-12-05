using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewItemUnlockUIController : SingletonComponent<NewItemUnlockUIController>
{
    [SerializeField] RectTransform bg;

    protected override void Awake()
    {
        var scale = 7.6249f - 0.978f * Mathf.Log(GeneralRefrencesManager.Inst.Get_Canvas_Width_Height().Item1);
        bg.localScale = new Vector3(scale, scale, scale);
    }

    private void OnEnable()
    {
        var a = GeneralDataManager.GameData.OpenElementsIndex.Max();
        for (var i = a + 1; i < a + 7; i++)
        {
            GeneralDataManager.GameData.OpenElementsIndex.Add(i);
        }
        NewItemUnlock3DController.Inst.Instantiate_Elements();
    }

    public void On_Claim_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        GameManager.Inst.Show_Popup(GameManager.Popups.LevelComplete);
    }
}