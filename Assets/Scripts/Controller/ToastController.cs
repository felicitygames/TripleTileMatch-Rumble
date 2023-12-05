using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToastController : MonoBehaviour
{
    public Text messageText;

    public void ToastDisable()
    {
        GameManager.Inst.ToastRef = null;
        Destroy(gameObject);
    }
}
