using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowTextOnMatch : MonoBehaviour
{
    public static ShowTextOnMatch instance;
    public GameObject TextPanel;
    public Image popUpMessage;

    private void Awake()
    {
        instance = this;
    }
    public void showText()
    {
        SoundManager.Inst.Play("tile3match");
        List<Sprite> messages = GeneralRefrencesManager.Inst.popUpMessages;
        StartCoroutine(ShowTextsRandomly(messages[Random.Range(0,messages.Count)]));
    }
    private IEnumerator ShowTextsRandomly(Sprite currentSprite)
    {
        TextPanel.SetActive(true);
        popUpMessage.sprite = currentSprite;
        yield return new WaitForSeconds(1.25f);
        TextPanel.SetActive(false);
    }
}
