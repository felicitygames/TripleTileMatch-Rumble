using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GamePlayUIController : SingletonComponent<GamePlayUIController>, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject freezEffect, swapEffect, hintPlusIcon, freezPlusIcon, undoPlusIcon, swapPlusIcon;
    [SerializeField] private TextMeshProUGUI hintText, freezText, undoText, swapText;
    [SerializeField] private Image freezFillImg;
    public GameObject hintLock, undoLock, swapLock, freezLock;
    public GameObject hintUnLock, undoUnLock, swapUnLock, freezUnLock;
    public GameObject Highlight;
    public Text lvlText;
    public Font lvlfont;
    [SerializeField] private Animator diamondAnim;
    private bool isSwapping = false;
    public static GamePlayUIController instance;
    public int PowerIndex;
    GeneralRefrencesManager GRMI;

    protected override void Awake()
    {
        GRMI = GeneralRefrencesManager.Inst;
        instance = this;
        if (PlayerPrefs.GetString("LanguageChar") == "Japanese")
        {
            lvlText.font = lvlfont;
            lvlText.resizeTextForBestFit = true;
        }
    }

    internal void Set_Text()
    {
        Set_Text_And_Icon(hintText, GeneralDataManager.GameData.HintCount, hintPlusIcon);
        Set_Text_And_Icon(undoText, GeneralDataManager.GameData.UndoCount, undoPlusIcon);
        Set_Text_And_Icon(freezText, GeneralDataManager.GameData.FreezeCount, freezPlusIcon);
        Set_Text_And_Icon(swapText, GeneralDataManager.GameData.SwapCount, swapPlusIcon);
    }


    internal void Play_Diamond_Anim()
    {
        diamondAnim.SetTrigger("Play");
    }

    private static void Set_Text_And_Icon(TMP_Text textMesh, int amount, GameObject plusIcon)
    {
        if (amount > 0)
        {
            textMesh.gameObject.SetActive(true);
            plusIcon.SetActive(false);
            textMesh.text = amount.ToString();
        }
        else
        {
            plusIcon.SetActive(true);
            textMesh.gameObject.SetActive(false);
        }
    }

    internal Vector3 Get_Anchor_Pos(int index)
    {
        return RectTransformUtility.WorldToScreenPoint(GRMI.mainCam,
            GamePlayScreen3DController.Inst.Get_Position(index) + new Vector3(0, 1.5f, 0));
    }

    private void OnEnable()
    {
        GridManager.Inst.Generate_Grid(GeneralDataManager.GameData.LevelNo);
        Set_Text();
        Invoke(nameof(Check_And_Open_How_To_Play_PopUp), 1f);


    }

    private void Check_And_Open_How_To_Play_PopUp()
    {
        if (GeneralDataManager.GameData.IsHowToPlayPopUpShow) return;
        GameManager.Inst.Show_Popup(GameManager.Popups.HowToPlay);
        GeneralDataManager.GameData.IsHowToPlayPopUpShow = true;
    }

    public void On_Hint_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        PowerUnlockManager.instance.animators[0].enabled = false;
        PowerUnlockManager.instance.animators[0].gameObject.transform.localScale = Vector3.one;
        PowerUnlockManager.instance.PowerUnlockPanel.SetActive(false);
        PowerUnlockManager.instance.hands[0].SetActive(false);
        if (GRMI.elementParent.childCount == 0 || !GRMI.IsDestroyComplete)
            return;

       
        if (GeneralDataManager.GameData.HintCount > 0)
        {
            StartCoroutine(On_Hint_Btn_Click_Coroutine());
        }
        else
        {
            PowerIndex = 0;
            GameManager.Inst.Show_Popup(GameManager.Popups.PowerupConfirm);
            //ShowAdWhenHintIsZero();
        }
    }
    public void UseHint()
    {
        StartCoroutine(On_Hint_Btn_Click_Coroutine());
    }

    private IEnumerator On_Hint_Btn_Click_Coroutine()
    {
        Play_Power_Use_Sound();

        var GRMI = GeneralRefrencesManager.Inst;

        if (GRMI.Get_Element_Collector_Child_Count() == 0)
        {
            GRMI.No_Click_Panel_On_Off(true);
            GRMI.World_No_Click_Panel_On_Off(true);
            GeneralDataManager.GameData.HintCount--;

            var a = Random.Range(0, GRMI.elementParent.childCount);
            var randomElement = GRMI.elementParent.GetChild(a);
            var hintElement = GamePlayScreenManager.Find_Same_Element_For_Hint(randomElement, 3);

            foreach (var t in hintElement)
            {
                t.Out_Line_On_Off(true);
            }

            yield return new WaitForSeconds(0.2f);

            foreach (var t in hintElement)
            {
                t.Out_Line_On_Off(false);
            }

            for (var i = 0; i < hintElement.Count; i++)
            {
                var pos = GamePlayScreen3DController.Inst.Get_Position(i);
                hintElement[i].transform.SetParent(GRMI.elementCollector);
                var time = Vector3.Distance(hintElement[i].transform.position, pos) * GeneralRefrencesManager.MoveSpeed;
                hintElement[i].IsSideMove = false;
                hintElement[i].Move_Function(pos);
                hintElement[i]
                    .Scale_Function(
                        new Vector3(GeneralRefrencesManager.SmallScale, GeneralRefrencesManager.SmallScale,
                            GeneralRefrencesManager.SmallScale), time);
            }

            Call_Same_Three_Tile_Find_Coro(randomElement.name);
        }
        else
        {
            var data = GamePlayScreenManager.Is_Two_Same_Element_In_Collector();

            if (data.Item1)
            {
                GRMI.No_Click_Panel_On_Off(true);
                GRMI.World_No_Click_Panel_On_Off(true);
                GeneralDataManager.GameData.HintCount--;
                var hintElement = GamePlayScreenManager.Find_Same_Element_For_Hint(data.Item2, 1);

                foreach (var t in hintElement)
                {
                    t.Out_Line_On_Off(true);
                }

                yield return new WaitForSeconds(0.25f);

                foreach (var t in hintElement)
                {
                    t.Out_Line_On_Off(false);
                }

                foreach (var t in hintElement)
                {
                    var a = data.Item2.GetSiblingIndex() + 2;
                    var pos = GamePlayScreen3DController.Inst.Get_Position(a);
                    t.transform.SetParent(GRMI.elementCollector);
                    t.transform.SetSiblingIndex(a);
                    var time = Vector3.Distance(t.transform.position, pos) * GeneralRefrencesManager.MoveSpeed;
                    t.IsSideMove = false;
                    t.Move_Function(pos);
                    t.Scale_Function(
                        new Vector3(GeneralRefrencesManager.SmallScale, GeneralRefrencesManager.SmallScale,
                            GeneralRefrencesManager.SmallScale), time);
                }

                if (data.Item3 != 0)
                {
                    var a = data.Item4;
                    for (var i = 0; i < data.Item3; i++)
                    {
                        var elementController = GRMI.Get_Element_Collector_Child(data.Item4 + 1 + i)
                            .GetComponent<ElementController>();
                        elementController.Move_Function(
                            GamePlayScreen3DController.Inst.Get_Position(data.Item4 + 1 + i));
                        a++;
                    }
                }

                Call_Same_Three_Tile_Find_Coro(data.Item2.name);
            }
            else
            {
                if (GRMI.Get_Element_Collector_Child_Count() > 5)
                    yield break;

                GRMI.No_Click_Panel_On_Off(true);
                GRMI.World_No_Click_Panel_On_Off(true);
                GeneralDataManager.GameData.HintCount--;

                var hintElement = GamePlayScreenManager.Find_Same_Element_For_Hint(
                    GRMI.Get_Element_Collector_Child(GRMI.Get_Element_Collector_Child_Count() - 1), 2);

                foreach (var t in hintElement)
                {
                    t.Out_Line_On_Off(true);
                    t.Increase_Scale();
                }

                yield return new WaitForSeconds(0.25f);

                foreach (var t in hintElement)
                {
                    t.Out_Line_On_Off(false);
                }

                foreach (var t in hintElement)
                {
                    t.transform.SetParent(GRMI.elementCollector);
                    var pos = GamePlayScreen3DController.Inst.Get_Position(GRMI.Get_Element_Collector_Child_Count() -
                                                                           1);
                    var time = Vector3.Distance(t.transform.position, pos) * GeneralRefrencesManager.MoveSpeed;
                    t.IsSideMove = false;
                    t.Move_Function(pos);
                    t.Scale_Function(
                        new Vector3(GeneralRefrencesManager.SmallScale, GeneralRefrencesManager.SmallScale,
                            GeneralRefrencesManager.SmallScale), time);
                }

                Call_Same_Three_Tile_Find_Coro(GRMI
                    .Get_Element_Collector_Child(GRMI.Get_Element_Collector_Child_Count() - 1).name);
            }
        }

        Set_Text();
    }


    internal void Call_Same_Three_Tile_Find_Coro(string elementName)
    {
        StartCoroutine(GRMI.Is_Same_Three_Tile_Found(elementName));
        Invoke(nameof(Set_World_No_Click_Panel_Off), 1f);
    }

    internal void Set_World_No_Click_Panel_Off()
    {
        GRMI.World_No_Click_Panel_On_Off(false);
    }

    public void On_Undo_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        PowerUnlockManager.instance.animators[1].enabled = false;
        PowerUnlockManager.instance.animators[1].gameObject.transform.localScale = Vector3.one;
        PowerUnlockManager.instance.PowerUnlockPanel.SetActive(false);
        PowerUnlockManager.instance.hands[1].SetActive(false);
        if (GRMI.Get_Element_Collector_Child_Count() == 0)
            return;

        if (GeneralDataManager.GameData.UndoCount > 0)
        {
            GeneralDataManager.GameData.UndoCount--;
            StartCoroutine(Undo());
        }
        else
        {
            PowerIndex = 1;
            GameManager.Inst.Show_Popup(GameManager.Popups.PowerupConfirm);
        }
    }

    public void UseUndo()
    {
        StartCoroutine(Undo());
    }
    private IEnumerator Undo()
    {
        Play_Power_Use_Sound();
        Set_Text();

        GRMI.World_No_Click_Panel_On_Off(true);

        var element = GRMI.Get_Element_Collector_Child(GRMI.Get_Element_Collector_Child_Count() - 1)
            .GetComponent<ElementController>();
        element.Loop_Rotate_Pause();
        element.IsUndo = true;
        element.transform.SetParent(GRMI.elementParent);
        var time = Vector3.Distance(element.MyDefaultPos, element.transform.position) *
                   GeneralRefrencesManager.MoveSpeed;
        element.Move_Function(element.MyDefaultPos, Ease.OutSine);
        element.Scale_Function(
            new Vector3(GeneralRefrencesManager.NormalScale, GeneralRefrencesManager.NormalScale,
                GeneralRefrencesManager.NormalScale), time);
        element.Rotate_Function(element.MyDefaultRotation, time);

        yield return new WaitUntil(() => !element.IsMoving);

        GRMI.World_No_Click_Panel_On_Off(false);
    }

    internal IEnumerator Undo_Element_For_Game_Over_Watch_Ad()
    {
        yield return new WaitForSeconds(0.2f);

        var elements = new List<ElementController>();

        for (var i = 1; i < 4; i++)
        {
            elements.Add(GRMI.Get_Element_Collector_Child(GRMI.Get_Element_Collector_Child_Count() - i)
                .GetComponent<ElementController>());
        }

        foreach (var t in elements)
        {
            t.Loop_Rotate_Pause();
            t.IsUndo = true;
            t.transform.SetParent(GRMI.elementParent);
            var time = Vector3.Distance(t.MyDefaultPos, t.transform.position) * GeneralRefrencesManager.MoveSpeed;
            t.Move_Function(t.MyDefaultPos, Ease.OutSine);
            t.Scale_Function(
                new Vector3(GeneralRefrencesManager.NormalScale, GeneralRefrencesManager.NormalScale,
                    GeneralRefrencesManager.NormalScale), time);
            t.Rotate_Function(t.MyDefaultRotation, time);
        }

        foreach (var t in elements)
        {
            yield return new WaitUntil(() => !t.IsMoving);
        }

        GRMI.World_No_Click_Panel_On_Off(false);
    }

    public void On_Swap_Btn_Click()
    {
        GameManager.Play_Button_Click_Sound();
        PowerUnlockManager.instance.animators[2].enabled = false;
        PowerUnlockManager.instance.animators[2].gameObject.transform.localScale = Vector3.one;
        PowerUnlockManager.instance.PowerUnlockPanel.SetActive(false);
        PowerUnlockManager.instance.hands[2].SetActive(false);
        if (isSwapping || GRMI.elementParent.childCount < 1) return;
        isSwapping = true;
        Invoke(nameof(Is_Swapping_False), 2f);

        if (GeneralDataManager.GameData.SwapCount > 0)
        {
            GeneralDataManager.GameData.SwapCount--;
            Swap_Btn_Click();
        }
        else
        {
            PowerIndex = 2;
            GameManager.Inst.Show_Popup(GameManager.Popups.PowerupConfirm);
        }
    }
    public void UseSwap()
    {
        //Show ad first then call below method
        Swap_Btn_Click();
    }
    private void Swap_Btn_Click()
    {
        SoundManager.Inst.Play("SwapSound");
        swapEffect.SetActive(true);
        Set_Text();

        var elementControllers = FindObjectsOfType<ElementController>();

        foreach (var item in elementControllers)
        {
            if (item.transform.parent != GRMI.elementCollector)
            {
                item.Add_Force_UpWard();
            }
        }
    }

    private void Is_Swapping_False()
    {
        isSwapping = false;
        swapEffect.SetActive(false);
    }

    internal bool isFreezing = false;

    public void On_Freeze_Btn_Click()
    {
        if (isFreezing || GRMI.elementParent.childCount <= 0) return;
        PowerUnlockManager.instance.animators[3].enabled = false;
        PowerUnlockManager.instance.animators[3].gameObject.transform.localScale = Vector3.one;
        PowerUnlockManager.instance.PowerUnlockPanel.SetActive(false);
        PowerUnlockManager.instance.hands[3].SetActive(false);
        GameManager.Play_Button_Click_Sound();
        Freeze_Effect(true);

        if (GeneralDataManager.GameData.FreezeCount > 0)
        {
            GeneralDataManager.GameData.FreezeCount--;
            Freeze_BtnClick();
        }
        else
        {
            PowerIndex = 3;
            GameManager.Inst.Show_Popup(GameManager.Popups.PowerupConfirm);

        }
    }
    public void UseFreeze()
    {
        //Call below method after you finished watching the ad
        Freeze_BtnClick();
    }

    private Tween freezeTween;

    private void Freeze_BtnClick()
    {
        SoundManager.Inst.Play("FreezeEffect");
        const int freeTime = 7;
        Set_Text();
        freezFillImg.fillAmount = 1;
        isFreezing = true;
        freezeTween = freezFillImg.DOFillAmount(0, freeTime).SetEase(Ease.Linear)
            .OnComplete(Set_Freeze_Fill_Img_Amount);
        TimerController.Inst.Cansel_Timer_Invoke();
        TimerController.Inst.InvokeRepeating(nameof(TimerController.Inst.Start_Countdown), freeTime, 1f);
    }

    internal void Pause_Play_Freeze_Tween(bool isPause)
    {
        if (isPause)
            freezeTween.Pause();
        else
            freezeTween.Play();
    }

    private void Freeze_Effect(bool isOn)
    {
        freezEffect.SetActive(isOn);
    }

    internal void Set_Freeze_Fill_Img_Amount()
    {
        isFreezing = false;
        freezFillImg.fillAmount = 0;
        freezeTween.Kill();
        Freeze_Effect(false);
    }

    internal bool IsMouseDown = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        IsMouseDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (GameManager.Inst.lastSelectedElement == null) return;
        GameManager.Inst.lastSelectedElement.On_Mouse_Up_Function();
        IsMouseDown = false;
    }

    private static void Play_Power_Use_Sound()
    {
        SoundManager.Inst.Play("power_use");
    }

    public void CloseThisScreen()
    {
        gameObject.SetActive(false);
    }
}