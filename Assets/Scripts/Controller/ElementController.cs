using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class ElementController : MonoBehaviour
{
    internal bool IsMoving = false;
    internal bool IsSideMove = true;
    internal bool IsUndo = false;
    private bool isGameOver = false;

    internal Vector3 MyDefaultPos;
    internal Vector3 MyDefaultRotation;

    [SerializeField] private DOTweenAnimation rotateLoopAnim;
    [SerializeField] private Outline outline;
    [SerializeField] private Rigidbody myBody;

    private List<MeshCollider> myMeshColliders = new List<MeshCollider>();

    private int mySibilingIndex = 0;
    private Vector3 pos;
    private float time;

    private GeneralRefrencesManager GRMI;

    private void Start()
    {
        GRMI = GeneralRefrencesManager.Inst;

        if (rotateLoopAnim == null)
        {
            rotateLoopAnim = GetComponent<DOTweenAnimation>();
        }

        if (outline == null)
        {
            outline = GetComponent<Outline>();
        }

        if (myBody == null)
        {
            myBody = GetComponent<Rigidbody>();
        }

        if (GetComponent<Rigidbody>() == null)
        {
            return;
        }

        for (var i = 1; i < 7; i++)
        {
            myBody.velocity += new Vector3(0, 0, i);
        }

        myMeshColliders = GetComponentsInChildren<MeshCollider>().ToList();
    }

    private void Set_Colliders_In_Trigger_Mode_On_Off(bool isOn)
    {
        foreach (var item in myMeshColliders)
        {
            item.isTrigger = isOn;
        }
    }

    public void OnMouseUp()
    {
        On_Mouse_Up_Function();
    }

    public void OnMouseDown()
    {
        On_Mouse_Down_Function();
    }

    private void OnMouseEnter()
    {
        if (!GamePlayUIController.Inst.IsMouseDown) return;
        GameManager.Inst.lastSelectedElement = this;
        On_Mouse_Down_Function();
    }

    private void OnMouseExit()
    {
        if (!GamePlayUIController.Inst.IsMouseDown || transform.parent != GRMI.elementParent) return;
        transform.localScale = new Vector3(GeneralRefrencesManager.NormalScale, GeneralRefrencesManager.NormalScale,
            GeneralRefrencesManager.NormalScale);
        Out_Line_On_Off(false);
    }

    internal void On_Mouse_Up_Function()
    {
        GamePlayUIController.Inst.IsMouseDown = false;

        if (isGameOver || !outline.enabled || transform.parent != GeneralRefrencesManager.Inst.elementParent) return;

        Out_Line_On_Off(false);

        var sameTwo = GRMI.Is_Same_Two_Tile_Found(this.name);
        var sameOne = GRMI.Is_Same_One_Tile_Found(this.name);

        if (sameTwo.Item1)
        {
            pos = sameTwo.Item2;
            mySibilingIndex = sameTwo.Item3;
        }
        else if (sameOne.Item1)
        {
            pos = sameOne.Item2;
            mySibilingIndex = sameOne.Item3;
        }
        else
        {
            Debug.LogWarning("AllMatched");
            pos = GamePlayScreen3DController.Inst.Get_Position(GRMI.Get_Element_Collector_Child_Count());
            mySibilingIndex = GRMI.Get_Element_Collector_Child_Count();
        }

        transform.SetParent(GRMI.elementCollector);
        transform.SetSiblingIndex(mySibilingIndex);
        StartCoroutine(GRMI.Is_Same_Three_Tile_Found(this.name));

        time = Vector3.Distance(pos, transform.position) * GeneralRefrencesManager.MoveSpeed;
        IsSideMove = false;
        Move_Function(pos);
        Scale_Function(
            new Vector3(GeneralRefrencesManager.SmallScale, GeneralRefrencesManager.SmallScale,
                GeneralRefrencesManager.SmallScale), time);
    }

    private void On_Mouse_Down_Function()
    {
        if (transform.parent != GRMI.elementParent)
            return;

        if (GRMI.Get_Element_Collector_Child_Count() > 6)
        {
            isGameOver = true;
            return;
        }

        Increase_Scale();

        MyDefaultPos = transform.position;
        MyDefaultRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        Out_Line_On_Off(true);
    }

    internal void Increase_Scale()
    {
        transform.localScale = new Vector3(GeneralRefrencesManager.BigScale, GeneralRefrencesManager.BigScale,
            GeneralRefrencesManager.BigScale);
    }

    private Tween tweenMOve;

    internal void Move_Function(Vector3 pos, Ease ease = Ease.Linear)
    {
        tweenMOve.Kill();
        myBody.Sleep();
        myBody.isKinematic = false;
        IsMoving = true;
        float duration = 0;

        if (!IsSideMove)
        {
            SoundManager.Inst.Play("tile_click");
            duration = Vector3.Distance(pos, transform.position) * (GeneralRefrencesManager.MoveSpeed - 0.02f);
        }
        else
            duration = Vector3.Distance(pos, transform.position) * (GeneralRefrencesManager.MoveSpeed);


        Set_Colliders_In_Trigger_Mode_On_Off(true);
        tweenMOve = transform.DOMove(pos, duration).SetEase(ease).OnStepComplete(Complete_Move);

        if (!GeneralDataManager.GameData.IsTutorialShow && GeneralDataManager.GameData.LevelNo == 1 && GridManager.Inst.tempForTutorial == 1)
        {
            GamePlayUIController.instance.Highlight.SetActive(true);
            GridManager.Inst.tempObjList[GridManager.Inst.tempForTutorial].AddComponent<ElementController>();
            GridManager.Inst.tempHandRef.transform.localPosition = new Vector3(0, GridManager.Inst.tempHandRef.transform.localPosition.y, GridManager.Inst.tempHandRef.transform.localPosition.z);
            GridManager.Inst.tempForTutorial++;
            return;
        }

        if (!GeneralDataManager.GameData.IsTutorialShow && GeneralDataManager.GameData.LevelNo == 1 && GridManager.Inst.tempForTutorial == 2)
        {
            GamePlayUIController.instance.Highlight.SetActive(true);
            GridManager.Inst.tempObjList[GridManager.Inst.tempForTutorial].AddComponent<ElementController>();
            GridManager.Inst.tempHandRef.transform.localPosition = new Vector3(1.2f, GridManager.Inst.tempHandRef.transform.localPosition.y, GridManager.Inst.tempHandRef.transform.localPosition.z);
            GridManager.Inst.tempForTutorial++;
            return;
        }

        if (!GeneralDataManager.GameData.IsTutorialShow && GeneralDataManager.GameData.LevelNo == 1 && GridManager.Inst.tempForTutorial == 3)
        {
            GamePlayUIController.instance.Highlight.SetActive(true);
            for (int i = 0; i < 6; i++)
            {
                if (GridManager.Inst.tempObjList[GridManager.Inst.tempForTutorial + i].GetComponent<ElementController>() == null)
                    GridManager.Inst.tempObjList[GridManager.Inst.tempForTutorial + i].AddComponent<ElementController>();
            }
            GridManager.Inst.tempForTutorial++;
            Destroy(GameObject.Find("Hand(Clone)"));
            return;
        }
        else
        {
            GamePlayUIController.instance.Highlight.SetActive(false);
        }


    }

    internal void Scale_Function(Vector3 scale, float duration)
    {
        transform.DOScale(scale, duration).SetEase(Ease.Linear);
    }

    internal void Rotate_Function(Vector3 rotation, float duration)
    {
        transform.DORotate(rotation, duration, RotateMode.FastBeyond360).SetEase(Ease.Linear);
    }

    private void Complete_Move()
    {
        if (!IsSideMove)
        {
            if (GRMI.Get_Element_Collector_Child_Count() == 0 && GRMI.destroyElementCollector.childCount == 0)
            {
                transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (GRMI.Get_Element_Collector_Child_Count() > 0)
            {
                transform.rotation = GRMI.Get_Element_Collector_Child(0).rotation;
            }
            else if (GRMI.destroyElementCollector.childCount > 0)
            {
                transform.rotation = GRMI.destroyElementCollector.GetChild(0).rotation;
            }

            IsSideMove = true;
        }

        myBody.WakeUp();
        myBody.isKinematic = transform.parent != GRMI.elementParent;
        Set_Colliders_In_Trigger_Mode_On_Off(false);
        StartCoroutine(Continue_Rotate_Element());
        IsMoving = false;
    }

    internal void Add_Force_UpWard()
    {
        transform.DORotate(new Vector3(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360)), 2f,
            RotateMode.WorldAxisAdd);
        myBody.AddForce(new Vector3(Random.Range(Random.Range(-30, -25), Random.Range(30, 25)) * 250,
            Random.Range(Random.Range(-30, -25), Random.Range(30, 25)) * 250, -14000));
    }

    private void Loop_Rotate_Play()
    {
        rotateLoopAnim.DOPlay();
    }

    internal void Loop_Rotate_Pause()
    {
        rotateLoopAnim.DOPause();
    }

    private IEnumerator Continue_Rotate_Element()
    {
        yield return new WaitForSeconds(0.15f);

        if (!IsUndo)
        {
            Loop_Rotate_Play();
        }
        else
        {
            IsUndo = false;
        }
    }

    internal void Out_Line_On_Off(bool isOn)
    {
        outline.enabled = isOn;
    }

    internal void DesTroy_My_Self()
    {
        Destroy(gameObject);
    }

    internal void Y_Rotate()
    {
        DOTween.To(() => -180f, x => transform.localRotation = Quaternion.Euler(90, x, 0f), 180f, 5f).SetLoops(-1)
            .SetEase(Ease.Linear);
    }
}