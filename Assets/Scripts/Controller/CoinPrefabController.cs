using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPrefabController : SingletonComponent<CoinPrefabController>
{

    internal Vector3 reff;

    internal void Move_Coin()
    {
        transform.DOMove(reff, 0.5f).SetEase(Ease.Linear)
            .OnComplete(Complete_Move);
    }

    private void Complete_Move()
    {
        Destroy(gameObject, 0.5f);
    }
}