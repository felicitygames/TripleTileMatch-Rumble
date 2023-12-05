using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ComboSliderController : SingletonComponent<ComboSliderController>
{
    [SerializeField] private Slider comboSlider;
    [SerializeField] private Text comboText;

    internal int ComboCount = 0;
    private float comboMaxValue = 11.2f;

    private void Start()
    {
        Reset_Combo_Slider();
    }

    internal void Reset_Combo_Slider()
    {
        ComboCount = 0;
        comboMaxValue = 11.2f;
        comboSlider.value = 0;
        Set_Tween(TweenState.Kill);
        comboText.gameObject.SetActive(false);
    }

    private Tween tween;

    internal void Set_Combo_Slider()
    {
        if (ComboCount <= 1) return;
        SoundManager.Inst.Play((ComboCount < 10 ? (ComboCount - 1) : 8).ToString());

        Set_Tween(TweenState.Kill);

        var maxvalue = comboMaxValue / 1.12f;

        comboMaxValue = maxvalue;
        comboSlider.maxValue = maxvalue;

        comboText.gameObject.SetActive(true);
        comboText.text = (ComboCount - 1) + "x";
        tween = DOTween.To(() => maxvalue, x => comboSlider.value = x, 0f, maxvalue).SetEase(Ease.Linear).OnStepComplete(Reset_Combo_Slider);
    }

    public enum TweenState
    {
        Pause,
        Play,
        Kill
    }

    internal void Set_Tween(TweenState tweenState)
    {
        switch (tweenState)
        {
            case TweenState.Pause:
                tween.Pause();
                break;
            case TweenState.Play:
                tween.Play();
                break;
            case TweenState.Kill:
                tween.Kill();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(tweenState), tweenState, null);
        }
    }
}
