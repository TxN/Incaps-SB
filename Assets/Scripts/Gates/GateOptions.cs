using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class GateOptions : MonoBehaviour 
{
    public Linker linkTool;

    public GameObject textFab;
    public GameObject switchFab;
    public GameObject numEditFab;
    public GameObject strEditFab;
    public GameObject sliderFab;
    public GameObject listFab;

    public Vector2 leftOffset = new Vector2(-80,-25);
    public Vector2 rightOffset = new Vector2(80, -25);

    public int optionsNum;

    public List<GameObject> tempObjects = new List<GameObject>();

    Gate selectedGate;

    public void LoadParameters(Gate g)
    {
        linkTool.enabled = false;
        SetGate(g);
        foreach (EditableParameter item in g.Parameters.Values)
        {

            item.Init(this);
        }
    }

    public void ApplyParameters()
    {

    }

    public void ClearWindow()
    {
        foreach (GameObject obj in tempObjects)
        {
            Destroy(obj);
        }
        tempObjects.Clear();
        optionsNum = 0;
    }

    public void SetGate(Gate gate)
    {
        selectedGate = gate;
    }

    public void CloseWindow()
    {
        selectedGate.UpdateParameters();
        linkTool.enabled = true;
        ClearWindow();
        gameObject.SetActive(false);
    }
}
