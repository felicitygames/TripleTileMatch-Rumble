using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GamePlayScreenManager : SingletonComponent<GamePlayScreenManager>
{
    public static void On_Hint_Btn_Click()
    {

        GamePlayUIController.Inst.Set_Text();

        var grmi = GeneralRefrencesManager.Inst;
        
        if (grmi.Get_Element_Collector_Child_Count() == 0)
        {
            var a = Random.Range(0, grmi.elementParent.childCount);
            var randomElement = grmi.elementParent.GetChild(a);
            var hintElement = Find_Same_Element_For_Hint(randomElement, 3);

            foreach (var t in hintElement)
            {
                t.Out_Line_On_Off(true);
            }

            for (var i = 0; i < hintElement.Count; i++)
            {
                var pos = GamePlayScreen3DController.Inst.Get_Position(i);
                hintElement[i].transform.SetParent(grmi.elementCollector);
                var time = Vector3.Distance(hintElement[i].transform.position, pos) * GeneralRefrencesManager.MoveSpeed;
                hintElement[i].IsSideMove = false;
                hintElement[i].Move_Function(pos);
                hintElement[i].Scale_Function(new Vector3(GeneralRefrencesManager.SmallScale, GeneralRefrencesManager.SmallScale, GeneralRefrencesManager.SmallScale), time);
            }
            GamePlayUIController.Inst.Call_Same_Three_Tile_Find_Coro(randomElement.name);
        }
        else
        {
            var data = Is_Two_Same_Element_In_Collector();

            if (data.Item1)
            {
                var hintElement = Find_Same_Element_For_Hint(data.Item2, 1);
                foreach (var item in hintElement)
                {
                    var a = data.Item2.GetSiblingIndex() + 2;
                    var pos = GamePlayScreen3DController.Inst.Get_Position(a);
                    item.transform.SetParent(grmi.elementCollector);
                    Transform transform1;
                    (transform1 = item.transform).SetSiblingIndex(a);
                    var time = Vector3.Distance(transform1.position, pos) * GeneralRefrencesManager.MoveSpeed;
                    item.IsSideMove = false;
                    item.Move_Function(pos);
                    item.Scale_Function(new Vector3(GeneralRefrencesManager.SmallScale, GeneralRefrencesManager.SmallScale, GeneralRefrencesManager.SmallScale), time);
                }


                if (data.Item3 != 0)
                {
                    var a = data.Item4;
                    for (var i = 0; i < data.Item3; i++)
                    {
                        var elementController = grmi.Get_Element_Collector_Child(data.Item4 + 1 + i).GetComponent<ElementController>();
                        elementController.Move_Function(GamePlayScreen3DController.Inst.Get_Position(data.Item4 + 1 + i));
                        a++;
                    }
                }

                GamePlayUIController.Inst.Call_Same_Three_Tile_Find_Coro(data.Item2.name);
            }
            else
            {
                if (grmi.Get_Element_Collector_Child_Count() > 5)
                    return;

                var hintElement = Find_Same_Element_For_Hint(grmi.Get_Element_Collector_Child(grmi.Get_Element_Collector_Child_Count() - 1), 2);
                foreach (var item in hintElement)
                {
                    item.transform.SetParent(grmi.elementCollector);
                    var pos = GamePlayScreen3DController.Inst.Get_Position(grmi.Get_Element_Collector_Child_Count() - 1);
                    var time = Vector3.Distance(item.transform.position, pos) * GeneralRefrencesManager.MoveSpeed;
                    item.IsSideMove = false;
                    item.Move_Function(pos);
                    item.Scale_Function(new Vector3(GeneralRefrencesManager.SmallScale, GeneralRefrencesManager.SmallScale, GeneralRefrencesManager.SmallScale), time);
                }
                GamePlayUIController.Inst.Call_Same_Three_Tile_Find_Coro(grmi.Get_Element_Collector_Child(grmi.Get_Element_Collector_Child_Count() - 1).name);
            }
        }
    }

    internal static List<ElementController> Find_Same_Element_For_Hint(Transform element, int count)
    {
        var grmi = GeneralRefrencesManager.Inst;
        var sameElement = new List<ElementController>();
        var elementControllers = FindObjectsOfType<ElementController>();

        var a = 0;

        foreach (var item in elementControllers)
        {
            if (item.transform.parent == grmi.elementCollector) continue;
            if (item.transform.name != element.name) continue;
            sameElement.Add(item);
            a++;
            if (a == count) break;
        }

        return sameElement;
    }

    internal static (bool, Transform, int, int) Is_Two_Same_Element_In_Collector()
    {
        var grmi = GeneralRefrencesManager.Inst;
        var isSame = false;
        var count = 0;
        var startingPoint = 0;
        Transform element = null;

        for (var i = 0; i < grmi.Get_Element_Collector_Child_Count() - 1; i++)
        {
            if (grmi.Get_Element_Collector_Child(i).name != grmi.Get_Element_Collector_Child(i + 1).name) continue;
            isSame = true;
            element = grmi.Get_Element_Collector_Child(i);
            count = grmi.Get_Element_Collector_Child_Count() - (i + 2);
            startingPoint = (i + 2);
            break;
        }
        return (isSame, element, count, startingPoint);
    }
}
