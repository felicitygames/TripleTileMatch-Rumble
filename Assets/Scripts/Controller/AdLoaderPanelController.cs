using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AdLoaderPanelController : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke(nameof(Disable_Panel), 10f);
    }

    private void Disable_Panel()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        GeneralRefrencesManager.Inst.No_Click_Panel_On_Off(false);
        CancelInvoke(nameof(Disable_Panel));
    }
}
