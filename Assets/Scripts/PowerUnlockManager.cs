using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PowerUnlockManager : MonoBehaviour
{
    public static PowerUnlockManager instance;
    public GameObject PowerUnlockPanel;
    public GameObject IconUnlockPanel;
    public List<Animator> animators;
    public List<GameObject> hands;
    public List<Sprite> icons;
    public List<string> names;
    public Image icon;
    public TMP_Text name;
    private void Awake()
    {
        foreach (var animator in animators)
        {
            animator.enabled = false;
        }
        instance = this;
    }
    public void showMessage(int index)
    {

        SoundManager.Inst.Play("powerUnlocked");
        animators[index].enabled = true;
        if(index >= 0){
        int indexCount = index;
        while(indexCount>=0){
        hands[indexCount].SetActive(false);
        indexCount--;
        }
        }
        StartCoroutine(On_OffMessage(index));
    }
    private IEnumerator On_OffMessage(int index)
    {
        icon.sprite = icons[index];
        name.text = names[index];
        PowerUnlockPanel.SetActive(true);
        IconUnlockPanel.SetActive(true);
        for(int i = 0; i < index;i++){
            hands[i].SetActive(false);
        }
        hands[index].SetActive(true);
        yield return new WaitForSeconds(5);
        animators[index].enabled = false;
        animators[index].gameObject.transform.localScale = Vector3.one;
        PowerUnlockPanel.SetActive(false);
        IconUnlockPanel.SetActive(false);
        hands[index].SetActive(false);
    }
}
