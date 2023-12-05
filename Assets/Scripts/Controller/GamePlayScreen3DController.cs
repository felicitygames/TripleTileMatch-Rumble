using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GamePlayScreen3DController : SingletonComponent<GamePlayScreen3DController>
{
    [SerializeField] private List<Transform> refForElementCollector;
    [SerializeField] private List<ParticleSystem> particlesList;
    [SerializeField] private Transform bottomCollectorRef;
    GeneralRefrencesManager GRMI;

    protected override void Awake()
    {
        GRMI = GeneralRefrencesManager.Inst;
    }

    private void Start()
    {
        var widthAndHeight = GRMI.Get_Canvas_Width_Height();
        var scale = -7.68f + 1.2818f * Mathf.Log(widthAndHeight.Item1);
        scale -= 0.4f;
        bottomCollectorRef.position = new Vector3(0, -21.7584f + 2.5637f * Mathf.Log(widthAndHeight.Item1),
            bottomCollectorRef.position.z);
        bottomCollectorRef.localScale = new Vector3(scale, scale, scale);
    }

    internal Vector3 Get_Position(int index)
    {
        if (index > 6) return refForElementCollector[6].position + new Vector3(0, -0.13f, -0.2f);
        return refForElementCollector[index].position + new Vector3(0, -0.13f, -0.2f);
    }

    internal ParticleSystem Get_Particle_System(int index)
    {
        return particlesList[index];
    }
}