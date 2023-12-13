using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GeneralDataManager;

public class GeneralRefrencesManager : SingletonComponent<GeneralRefrencesManager>
{
    public static GeneralRefrencesManager instance;
    public Transform elementCollector, elementParent, destroyElementCollector, coinRefPos;
    [SerializeField] RectTransform canvas;
    public Camera mainCam;
    public GameObject coinPrefab;

    public List<Mesh> objMeshes;
    public List<Sprite> popUpMessages;
    internal const float MoveSpeed = 0.07f;
    internal const float NormalScale = 0.6f;
    internal const float SmallScale = 0.4f;
    internal const float BigScale = 0.7f;
    internal float AppliedSafeAreaOnGameScreen = 0f;

    [SerializeField]
    private GameObject worldNoClickPanal,
        diamond,
        noClickPanal,
        GamePlayUI,
        newItemUnlockTransphrantBg,
        newItemUnlockUI,
        gamePlay3D;

    [SerializeField] Transform popUpCanvas;

    internal int Get_Element_Collector_Child_Count()
    {
        return elementCollector.childCount;
    }

    internal (float, float) Get_Canvas_Width_Height()
    {
        return (canvas.rect.width, canvas.rect.height);
    }

    internal void Set_Game_Screen_UI(bool isActive)
    {
        GamePlayUI.SetActive(isActive);
        gamePlay3D.SetActive(isActive);
    }

    internal void Set_newItemUnlockTransparentBg(bool isActive, Vector3 vector)
    {
        newItemUnlockTransphrantBg.SetActive(isActive);
        newItemUnlockUI.SetActive(isActive);
        newItemUnlockTransphrantBg.transform.localScale = vector;
    }

    internal Transform Get_Element_Collector_Child(int a)
    {
        return a < Get_Element_Collector_Child_Count() ? elementCollector.GetChild(a) : null;
    }

    internal void World_No_Click_Panel_On_Off(bool isOn)
    {
        worldNoClickPanal.SetActive(isOn);
    }

    internal void No_Click_Panel_On_Off(bool isOn)
    {
        noClickPanal.SetActive(isOn);
    }

    internal void Clear_Level()
    {
        HeaderController.Inst.DiamondCountPerLevel = 0;
        GamePlayUIController.Inst.Set_Text();
        HeaderController.Inst.Set_Text();
        ComboSliderController.Inst.Reset_Combo_Slider();

        for (var i = 0; i < destroyElementCollector.childCount; i++)
        {
            Destroy(destroyElementCollector.GetChild(i).gameObject);
        }

        for (var i = 0; i < Get_Element_Collector_Child_Count(); i++)
        {
            Destroy(Get_Element_Collector_Child(i).gameObject);
        }

        for (var i = 0; i < elementParent.childCount; i++)
        {
            Destroy(elementParent.GetChild(i).gameObject);
        }
    }


    internal (bool, Vector3, int) Is_Same_Two_Tile_Found(string nameOfElement)
    {
        var isFound = false;
        var pos = new Vector3();
        var count = 0;
        var startingNumber = 0;
        var sibilingIndex = 0;

        for (var i = 0; i < Get_Element_Collector_Child_Count() - 1; i++)
        {
            if (Get_Element_Collector_Child(i).name != nameOfElement ||
                Get_Element_Collector_Child(i + 1).name != nameOfElement) continue;
            isFound = true;
            pos = GamePlayScreen3DController.Inst.Get_Position(i + 2);
            count = Get_Element_Collector_Child_Count() - (i + 2);
            startingNumber = i + 2;
            sibilingIndex = i + 2;
        }

        if (!isFound || count <= 0) return (isFound, pos, sibilingIndex);
        {
            for (var i = 0; i < count; i++)
            {
                var elementController =
                    Get_Element_Collector_Child(startingNumber + i).GetComponent<ElementController>();
                elementController.Move_Function(GamePlayScreen3DController.Inst.Get_Position(startingNumber + i + 1));
            }
        }

        return (true, pos, sibilingIndex);
    }

    internal (bool, Vector3, int) Is_Same_One_Tile_Found(string nameOfElement)
    {
        var isFound = false;
        var pos = new Vector3();
        var count = 0;
        var startingNumber = 0;
        var sibilingIndex = 0;

        for (var i = 0; i < Get_Element_Collector_Child_Count(); i++)
        {
            if (Get_Element_Collector_Child(i).name != nameOfElement) continue;
            isFound = true;
            pos = GamePlayScreen3DController.Inst.Get_Position(i + 1);
            count = Get_Element_Collector_Child_Count() - (i + 1);
            startingNumber = i + 1;
            sibilingIndex = i + 1;
        }

        if (!isFound || count <= 0) return (isFound, pos, sibilingIndex);
        {
            for (var i = 0; i < count; i++)
            {
                var elementController =
                    Get_Element_Collector_Child(startingNumber + i).GetComponent<ElementController>();
                elementController.Move_Function(GamePlayScreen3DController.Inst.Get_Position(startingNumber + i + 1));
            }
        }

        return (true, pos, sibilingIndex);
    }

    internal IEnumerator Is_Same_Three_Tile_Found(string nameOfElement)
    {
        var isAnyDestroy = false;
        var movableElement = new List<ElementController>();

        if (Get_Element_Collector_Child_Count() > 2)
        {
            for (var i = 0; i < Get_Element_Collector_Child_Count() - 2; i++)
            {
                if (Get_Element_Collector_Child(i).name != nameOfElement ||
                    Get_Element_Collector_Child(i + 1).name != nameOfElement ||
                    Get_Element_Collector_Child(i + 2).name != nameOfElement) continue;
                isAnyDestroy = true;
                var count = Get_Element_Collector_Child_Count() - (i + 3);

                if (destroyElementCollector.childCount > 0)
                {
                    World_No_Click_Panel_On_Off(true);
                    yield return new WaitUntil(() => IsDestroyComplete);
                    IsDestroyComplete = false;
                    World_No_Click_Panel_On_Off(false);
                }

                var destroyElements = new List<ElementController>();

                for (var j = 0; j < 3; j++)
                {
                    destroyElements.Insert(j, Get_Element_Collector_Child(i + j).GetComponent<ElementController>());
                }

                for (var k = i + 3; k < Get_Element_Collector_Child_Count(); k++)
                {
                    movableElement.Add(Get_Element_Collector_Child(k).GetComponent<ElementController>());
                }

                for (var j = 0; j < 3; j++)
                {
                    destroyElements[j].transform.SetParent(destroyElementCollector);
                }

                StartCoroutine(Destroy_After_Move_Complete(destroyElements, count, i, movableElement,
                    i + 1));
            }
        }


        if (Get_Element_Collector_Child_Count() > 6 && !isAnyDestroy)
        {
            No_Click_Panel_On_Off(true);
            World_No_Click_Panel_On_Off(true);
            StartCoroutine(Show_Game_Over_PopUp());
        }

        if (elementParent.childCount != 0) yield break;
        No_Click_Panel_On_Off(true);
        StartCoroutine(Show_Level_Complete_PopUp());
    }

    internal bool IsDestroyComplete = true;

    private IEnumerator Destroy_After_Move_Complete(List<ElementController> elements, int count, int startIndex,
        IReadOnlyList<ElementController> elementControllers, int particleIndex)
    {
        ShowTextOnMatch.instance.showText();
        IsDestroyComplete = false;
        foreach (var t in elements)
        {
            yield return new WaitUntil(() => !t.IsMoving);
        }

        yield return new WaitForSeconds(0.1f);

        elements[0].Move_Function(elements[0].transform.position - new Vector3(0.1f, 0, 0));
        elements[elements.Count - 1].Move_Function(elements[elements.Count - 1].transform.position + new Vector3(0.1f, 0, 0));

        yield return new WaitUntil(() => !elements[0].IsMoving);
        yield return new WaitUntil(() => !elements[elements.Count - 1].IsMoving);

        elements[0].Move_Function(elements[1].transform.position);
        elements[elements.Count - 1].Move_Function(elements[1].transform.position);

        yield return new WaitUntil(() => !elements[0].IsMoving);
        yield return new WaitUntil(() => !elements[elements.Count - 1].IsMoving);

        ComboSliderController.Inst.ComboCount++;
        ComboSliderController.Inst.Set_Combo_Slider();

        GamePlayScreen3DController.Inst.Get_Particle_System(particleIndex).Play();

        for (var i = 0; i < ComboSliderController.Inst.ComboCount; i++)
        {
            var d = Instantiate(diamond, GamePlayUIController.Inst.Get_Anchor_Pos(particleIndex),
                Quaternion.identity, popUpCanvas);
            yield return new WaitForSeconds(0.035f);
        }


        for (var i = 0; i < 3; i++)
        {
            elements[i].DesTroy_My_Self();
        }

        SoundManager.Inst.Play("tile_matched");

        if (count > 0)
        {
            for (var i = 0; i < elementControllers.Count; i++)
            {
                if (Get_Element_Collector_Child(startIndex + i).GetComponent<ElementController>() ==
                    elementControllers[i])
                {
                    elementControllers[i].Move_Function(GamePlayScreen3DController.Inst.Get_Position(startIndex + i));
                }
            }
        }

        IsDestroyComplete = true;
        No_Click_Panel_On_Off(false);
    }

    private IEnumerator Show_Game_Over_PopUp()
    {
        yield return new WaitForSeconds(1f);
        No_Click_Panel_On_Off(false);
        World_No_Click_Panel_On_Off(false);
        GameManager.Inst.Show_Popup(GameManager.Popups.GameOver);
        GameOverPopUpController.Inst.SetText(TranslateManager.instance.ActiveTranslation_Dict["OUT_OF_SPACE"] + " !");
    }


    internal int Get_Diamond_Count()
    {
        return (GameData.StarChestOpenCount * 400 > 2000 ? 2000 : GameData.StarChestOpenCount * 400);
    }

    public IEnumerator Show_Level_Complete_PopUp()
    {
        yield return new WaitForSeconds(0.5f);
        SoundManager.Inst.Play("DiamondMove");

        var counts = TimerController.Inst.Second / 18;

        for (var i = 0; i < counts; i++)
        {
            var d = Instantiate(diamond, TimerController.Inst.transform.position, Quaternion.identity,
                popUpCanvas);
            d.GetComponent<DiamondMoveController>().isTimerDimond = true;
            yield return new WaitForSeconds(0.08f);
        }

        yield return new WaitForSeconds(0.7f);
        TimerController.Inst.Cansel_Timer_Invoke();
        No_Click_Panel_On_Off(false);

        if (GameData.LevelNo <= 390)
        {
            GameManager.Inst.Show_Popup(
          GameData.LevelNo == GeneralDataManager.Inst.LevelCountForNewElementOpen[GameData.NewElementOpenCount - 1]
              ? GameManager.Popups.NewElementUnLock
              : GameManager.Popups.LevelComplete);
        }
        else
        {
            GameManager.Inst.Show_Popup(GameManager.Popups.LevelComplete);
        }



    }
}