using DG.Tweening;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : SingletonComponent<GridManager>
{
    public GameObject obj;
    [SerializeField] List<GameObject> tutorialObj;
    public GameObject hand3D;

    private float xMax = 0, xMin = 0, yMax = 0, yMin = 0;

    private int difficulty = 0;
    private int totalElementCount = 0;
    private int time = 0;

    internal int tempForTutorial;
    internal GameObject tempHandRef;
    internal List<GameObject> tempObjList = new List<GameObject>();

    protected override void Awake()
    {
        var widthAndHeight = GeneralRefrencesManager.Inst.Get_Canvas_Width_Height();
        xMax = (-17.105f + 2.7676f * Mathf.Log(widthAndHeight.Item1)) + 100;
        yMax = (15.57f - 1.797f * Mathf.Log(widthAndHeight.Item1)) + 150;
        xMin = -xMax;
        yMin = -yMax;
    }

    internal void Generate_Grid(int levelNo = 1)
    {
        GeneralRefrencesManager.Inst.World_No_Click_Panel_On_Off(true);

        if (GeneralDataManager.GameData.LevelNo > 14)
        {
            GamePlayUIController.Inst.hintLock.SetActive(false);
            GamePlayUIController.Inst.undoLock.SetActive(false);
            GamePlayUIController.Inst.swapLock.SetActive(false);
            GamePlayUIController.Inst.freezLock.SetActive(false);
            if (!PlayerPrefs.HasKey("Freeze"))
            {
                PowerUnlockManager.instance.showMessage(3);
                PlayerPrefs.SetInt("Freeze", 1);
            }
            GamePlayUIController.Inst.hintUnLock.SetActive(true);
            GamePlayUIController.Inst.undoUnLock.SetActive(true);
            GamePlayUIController.Inst.swapUnLock.SetActive(true);
            GamePlayUIController.Inst.freezUnLock.SetActive(true);

        }
        else if (GeneralDataManager.GameData.LevelNo > 9)
        {
            GamePlayUIController.Inst.hintLock.SetActive(false);
            GamePlayUIController.Inst.undoLock.SetActive(false);
            GamePlayUIController.Inst.swapLock.SetActive(false);
            GamePlayUIController.Inst.freezLock.SetActive(true);
            if (!PlayerPrefs.HasKey("Swap"))
            {
                PowerUnlockManager.instance.showMessage(2);
                PlayerPrefs.SetInt("Swap", 1);
            }

            GamePlayUIController.Inst.hintUnLock.SetActive(true);
            GamePlayUIController.Inst.undoUnLock.SetActive(true);
            GamePlayUIController.Inst.swapUnLock.SetActive(true);
            GamePlayUIController.Inst.freezUnLock.SetActive(false);
        }
        else if (GeneralDataManager.GameData.LevelNo > 4)
        {
            GamePlayUIController.Inst.hintLock.SetActive(false);
            GamePlayUIController.Inst.undoLock.SetActive(false);
            GamePlayUIController.Inst.swapLock.SetActive(true);
            GamePlayUIController.Inst.freezLock.SetActive(true);
            if (!PlayerPrefs.HasKey("Undo"))
            {
                PowerUnlockManager.instance.showMessage(1);
                PlayerPrefs.SetInt("Undo", 1);
            }

            GamePlayUIController.Inst.hintUnLock.SetActive(true);
            GamePlayUIController.Inst.undoUnLock.SetActive(true);
            GamePlayUIController.Inst.swapUnLock.SetActive(false);
            GamePlayUIController.Inst.freezUnLock.SetActive(false);
        }
        else if (GeneralDataManager.GameData.LevelNo > 1)
        {
            GamePlayUIController.Inst.hintLock.SetActive(false);//Show message here
            if(!PlayerPrefs.HasKey("Hint"))
            {
                PowerUnlockManager.instance.showMessage(0);
                PlayerPrefs.SetInt("Hint", 1);
            }
            GamePlayUIController.Inst.undoLock.SetActive(true);
            GamePlayUIController.Inst.swapLock.SetActive(true);
            GamePlayUIController.Inst.freezLock.SetActive(true);

            GamePlayUIController.Inst.hintUnLock.SetActive(true);
            GamePlayUIController.Inst.undoUnLock.SetActive(false);
            GamePlayUIController.Inst.swapUnLock.SetActive(false);
            GamePlayUIController.Inst.freezUnLock.SetActive(false);
        }
        else
        {
            GamePlayUIController.Inst.hintLock.SetActive(true);
            GamePlayUIController.Inst.undoLock.SetActive(true);
            GamePlayUIController.Inst.swapLock.SetActive(true);
            GamePlayUIController.Inst.freezLock.SetActive(true);

            GamePlayUIController.Inst.hintUnLock.SetActive(false);
            GamePlayUIController.Inst.undoUnLock.SetActive(false);
            GamePlayUIController.Inst.swapUnLock.SetActive(false);
            GamePlayUIController.Inst.freezUnLock.SetActive(false);
        }


        Invoke(nameof(Invoke_World_No_Click_Panel), 2f);

        if (!GeneralDataManager.GameData.IsTutorialShow && levelNo == 1)
        {
            var q = new List<Vector3>();
            var qw = new List<GameObject>();

            var s = 83;

            q.Add(new Vector3(-1.2f, 1.5f, s));
            q.Add(new Vector3(0, 1.5f, s));
            q.Add(new Vector3(1.2f, 1.5f, s));
            q.Add(new Vector3(-1.2f, 0, s));
            q.Add(new Vector3(0, 0, s));
            q.Add(new Vector3(1.2f, 0, s));
            q.Add(new Vector3(-1.2f, -1.5f, s));
            q.Add(new Vector3(0f, -1.5f, s));
            q.Add(new Vector3(1.2f, -1.5f, s));

            qw.Add(tutorialObj[1]);
            qw.Add(tutorialObj[1]);
            qw.Add(tutorialObj[1]);
            qw.Add(tutorialObj[0]);
            qw.Add(tutorialObj[0]);
            qw.Add(tutorialObj[2]);
            qw.Add(tutorialObj[0]);
            qw.Add(tutorialObj[2]);
            qw.Add(tutorialObj[2]);

            for (int i = 0; i < 9; i++)
            {
                var aa = Instantiate(qw[i], GeneralRefrencesManager.Inst.elementParent);
                tempObjList.Add(aa);
                aa.transform.localPosition = q[i];
                if (i != 0)
                {
                    Destroy(aa.GetComponent<ElementController>());
                }
                else
                {
                    tempHandRef = Instantiate(hand3D, GeneralRefrencesManager.Inst.elementParent);
                    tempHandRef.transform.localPosition = q[i] + new Vector3(0, 0.2f, 0);
                    tempForTutorial++;
                }
            }
            GamePlayUIController.instance.Highlight.SetActive(true);
            return;

        }



        var openMeshes = new List<Mesh>();

        for (int i = 0; i < GeneralDataManager.GameData.OpenElementsIndex.Count; i++)
        {
            openMeshes.Add(GeneralRefrencesManager.Inst.objMeshes[GeneralDataManager.GameData.OpenElementsIndex[i]]);
        }


        difficulty = Get_Difficulty(levelNo);
    reset:
        var timeCount = Get_Time_And_Count_For_Level(difficulty, levelNo);
        totalElementCount = timeCount.Item2;
        time = timeCount.Item1;
        if (levelNo < 9)
        {
            time = 60;
        }
        if (levelNo < 4)
        {
            time = 75;
        }
        

        if (difficulty > GeneralDataManager.GameData.OpenElementsIndex.Count)
        {
            difficulty--;
            goto reset;
        }

        var elementsInst = new List<Mesh>();

        var pairsCount = totalElementCount / 3;

        var a = 0;

        while (a != difficulty)
        {
            var el = openMeshes[Random.Range(0, openMeshes.Count)];
            if (elementsInst.Contains(el)) continue;
            elementsInst.Add(el);
            a++;
        }


        for (var i = 0; i < pairsCount - difficulty; i++)
        {
            var el = openMeshes[Random.Range(0, openMeshes.Count)];
            elementsInst.Add(el);
        }

        var temp = new List<GameObject>();
        var temp1 = new List<string>();


        foreach (var item in elementsInst)
        {
            for (var j = 0; j < 3; j++)
            {
                var element = Instantiate(obj, GeneralRefrencesManager.Inst.elementParent);
                element.name = item.name;
                element.transform.GetChild(0).GetComponent<MeshFilter>().mesh = item;
                element.transform.GetChild(0).GetComponent<MeshCollider>().sharedMesh = item;

                var x = Random.Range(xMin, xMax) / 100f;
                var y = Random.Range(yMin, yMax) / 100f;
                var z = transform.position.z + Random.Range(50, 100) / 100f;

                element.transform.DORotate(
                    new Vector3(Random.Range(-300, 300), Random.Range(-300, 300), Random.Range(-300, 300)), 1.5f,
                    RotateMode.WorldAxisAdd);
                element.GetComponent<Transform>().SetPositionAndRotation(new Vector3(x, y, z),
                    Quaternion.Euler(new Vector3(Random.Range(-180, 180), Random.Range(-180, 180),
                        Random.Range(-180, 180))));
            }
        }

        Physics.gravity = new Vector3(0, 0, 35);

        if (GameManager.activeScreen == GameManager.Screens.GameScreen)
        {
            TimerController.Inst.Second = time;
            TimerController.Inst.Set_Timer(TimerController.Inst.Second);
            GamePlayUIController.Inst.Set_Freeze_Fill_Img_Amount();
        }
    }

    public void Invoke_World_No_Click_Panel()
    {
        GeneralRefrencesManager.Inst.World_No_Click_Panel_On_Off(false);
    }


    private static int Get_Difficulty(int levelNumber)
    {

        if (levelNumber == 1 )
        {
            return 3;
        }
        if (levelNumber == 2)
        {
            return 4;
        }
        if (levelNumber == 3)
        {
            return 10;
        }
        if (levelNumber <= 13)
        {
            return levelNumber + 7;
        }

        const int minDifficulty = 10;
        const int totalDifficulty = 43;
        var closestInterval = ((levelNumber - 1) / 10 + 1) * 10;

        var closestHundred = closestInterval;
        while (closestHundred % 100 != 0)
        {
            closestHundred += 10;
        }

        const int minRange = 15;
        var maxRange = Mathf.Clamp(14 + ((closestHundred / 100) * 2), minDifficulty, totalDifficulty);

        var difficultyNumber = minRange;
        var numb = minRange;
        for (var i = closestHundred - 100; i < closestHundred; i++)
        {
            if (i >= levelNumber)
            {
                difficultyNumber = numb;
                break;
            }

            numb++;
            if (numb > maxRange) numb = minRange;
        }

        return Mathf.Clamp(difficultyNumber, minDifficulty, totalDifficulty);
    }


    private static (int, int) Get_Time_And_Count_For_Level(int difficulty, int levelNo)
    {
        int difficultyIncrease = 3;
        if (levelNo == 1) return (200 / difficultyIncrease, 9);
        if (levelNo == 2) return (210 / difficultyIncrease, 12);
        if (levelNo == 3) return (230 / difficultyIncrease, 18);

        if (levelNo % 7 == 0)
        {
            var a = (int)Mathf.Log(levelNo, 7) * (difficulty) * 3;
            a = a > 119 ? 119 : a;
            var b = a > 81 ? 120 : 60;
            return (b, a);
        }

        var time = Mathf.Clamp(
            (int)((1f - 0.102216f * Mathf.Log(levelNo)) * (1 - 0.2198 * Mathf.Log(difficulty)) * 0.1f * levelNo *
                  difficulty), 180, 800);
        if (time == 180)
            time = Mathf.Clamp(
                levelNo * 13 * difficulty * (int)(1.4 - 0.13 * Mathf.Log(levelNo)) *
                (int)(1.5f - 0.2442 * Mathf.Log(difficulty)), 140, 400);
        if (time == 140)
            time = Mathf.Clamp(
                time + (int)(time * (1.4 - 0.1 * Mathf.Log(levelNo))) + (int)((1.5f - 0.2442 * Mathf.Log(difficulty))),
                160, 340);
        var count = Mathf.Clamp((int)(difficulty * levelNo * time * 0.000005), 36, 120);
        if (count % 3 != 0) count += 3 - (count % 3);
        if (count < 70)
        {
            count = Mathf.Clamp((int)(difficulty * levelNo * time * 0.00005), 36, 120);
            if (count % 3 != 0) count += 3 - (count % 3);
        }

        if (count == 120)
        {
            count = Mathf.Clamp((int)(difficulty * levelNo * time * 0.000015), 36, 120);
            if (count % 3 != 0) count += 3 - (count % 3);
        }

        if (count < difficulty * 3) count = difficulty * 3;
        if (count <= 120) return ((time - 30) / difficultyIncrease, count + 6);
        count = 120;
        time -= time / 4;
        return ((time - 30)/ difficultyIncrease, count + 6);
    }
}