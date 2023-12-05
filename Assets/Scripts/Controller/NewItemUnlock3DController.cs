using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewItemUnlock3DController : SingletonComponent<NewItemUnlock3DController>
{
    [SerializeField] private RectTransform bg;
    [SerializeField] private Transform itemParent;
    [SerializeField] private GameObject particals;

    protected override void Awake()
    {
        var scale = 7.6249f - 0.978f * Mathf.Log(GeneralRefrencesManager.Inst.Get_Canvas_Width_Height().Item1);
        bg.localScale = new Vector3(scale, scale, scale);
        GeneralRefrencesManager.Inst.Set_newItemUnlockTransparentBg(true, new Vector3(scale, scale, scale));
    }

    internal void Instantiate_Elements()
    {
        GeneralDataManager.GameData.OpenElementsIndex.Sort();
        var b = GeneralDataManager.GameData.OpenElementsIndex.Max();
        var a = new List<Mesh>();
        for (var i = b-5; i < b+1; i++)
        {
            a.Add(GeneralRefrencesManager.Inst.objMeshes[i]);
        }


        for (var i = 0; i < a.Count; i++)
        {
            var item = Instantiate(GridManager.Inst.obj, itemParent.GetChild(i)).transform;
            item.transform.GetChild(0).GetComponent<MeshFilter>().mesh = a[i];
            item.transform.GetChild(0).GetComponent<MeshCollider>().sharedMesh = a[i];
            item.localScale = new Vector3(150, 150, 150);
            LayerMask layer = LayerMask.NameToLayer("UI");
            item.gameObject.layer = layer;
            item.GetChild(0).gameObject.layer = layer;
            item.rotation = Quaternion.Euler(90, -180, 0);
            item.localPosition = new Vector3(0, 0, -100);
            item.GetComponent<ElementController>().Y_Rotate();
            Destroy(item.GetComponent<Rigidbody>());
            var particle = Instantiate(particals, itemParent.GetChild(i)).transform;
            particle.localScale = item.transform.localScale = new Vector3(150, 150, 150);
            particle.localPosition = new Vector3(0, 0, -100f);
        }


        SoundManager.Inst.Play("NewTileOpen");
        GeneralDataManager.GameData.NewElementOpenCount++;
    }

    public void CloseThisPopup()
    {
        GeneralRefrencesManager.Inst.Set_newItemUnlockTransparentBg(false, Vector3.one);
        GameManager.activePopup = GameManager.Popups.Null;
        Destroy(gameObject);
    }
}