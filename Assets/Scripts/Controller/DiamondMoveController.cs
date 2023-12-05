using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DiamondMoveController : MonoBehaviour
{
    [FormerlySerializedAs("StartMovingTime")]
    [SerializeField]
    internal float startMovingTime = 0f;

    private float count = 0.0f;
    private const float Speed = 1.4f;
    private Vector3 startPoint, middlePoint, targetPos;
    private bool startMoving = false;
    internal bool isTimerDimond = false;

    private void Start()
    {
        startMoving = true;
        startPoint = transform.position;
        HeaderController.Inst.DiamondCountPerLevel++;
        middlePoint = isTimerDimond
            ? new Vector3(startPoint.x - 150f, startPoint.y - 100f, startPoint.z)
            : GamePlayUIController.Inst.Get_Anchor_Pos(5) + new Vector3(400, 500, 0);
        targetPos = HeaderController.Inst.diamondPosRef.position;
    }


    private void Update()
    {
        if (count < 1.0f && startMoving)
        {
            count += Speed * Time.deltaTime;
            var m1 = Vector3.Lerp(startPoint, middlePoint, count);
            var m2 = Vector3.Lerp(middlePoint, targetPos, count);
            transform.position = Vector3.Lerp(m1, m2, count);
        }

        if (!(count >= 1)) return;
        HeaderController.Inst.Set_Text();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GeneralDataManager.GameData.Diamonds++;
        GeneralDataManager.GameData.StarChestDiamond++;
        GamePlayUIController.Inst.Play_Diamond_Anim();
    }
}