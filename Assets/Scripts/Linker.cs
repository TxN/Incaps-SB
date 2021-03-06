﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Linker : MonoBehaviour 
{
    public GateOptions optsWindow;
        
    public Canvas canvas;

   
    public GameObject window;
    public Text windowHeader;

    GameObject IOLabelFab;

    List<GameObject> inputs = new List<GameObject>();
    List<GameObject> outputs = new List<GameObject>();

    Vector3 lastClickPos;
    Gate selectedGate;

    int selectedInput = 0;
    int selectedOutput = 0;

    bool showInputs = true;
    bool connectOutput = false;

    LinkInput inp;
    Output output;

    private GameObject wireFab;
    GameObject currentWire;
    int pointNum = 0;

    bool enable = true;

    void Start()
    {
        wireFab = Resources.Load("Wire") as GameObject; // Хардкод это плохо
        IOLabelFab = Resources.Load("IOLabel") as GameObject;


    }

    void OnEnable()
    {
        GameState.OnStateChanged += StateChanged;
        GameState.OnCircuitSubstateChanged += SubstateChanged;
    }

    void OnDisable()
    {
        GameState.OnStateChanged -= StateChanged;
        GameState.OnCircuitSubstateChanged -= SubstateChanged;
    }

    void StateChanged()
    {
        if (GameState.Instance.CurrentGameState == GameState.State.CircuitMode)
        {
            enable = true;

            GameState.Instance.SetCircuitEditSubstate(GameState.CircuitEditSubstate.Wiring);
        }
    }

    void SubstateChanged()
    {
        if (GameState.Instance.CurrentCircuitSubstate != GameState.CircuitEditSubstate.Wiring)
        {
            enable = false;
        }
        else
        {
            enable = true;
        }
    }

    void Update()
    {
        Gate gate = ClickGate();

        if (enable)
        {

            if (gate == null)
            {
                window.SetActive(false);
            }
            else
            {
                if (gate != selectedGate)
                {
                    selectedGate = gate;
                    ClearLinks();
                    FillLinks();
                }

                DrawLabels();

                if (Input.GetAxis("Mouse ScrollWheel") != 0)
                {
                    ScrollLabels();
                }


                if (Input.GetMouseButtonDown(0))
                {
                    if (!connectOutput)
                    {
                        if (inputs.Count > 0)
                        {

                            inp = selectedGate.Inputs[inputs[selectedInput].GetComponent<IOLabel>().GetLabel()];
                            inp.Disconnect();
                            connectOutput = true;
                            showInputs = false;
                            currentWire = Instantiate(wireFab) as GameObject;
                            currentWire.transform.position = selectedGate.transform.position;
                            currentWire.transform.parent = selectedGate.transform;
                            ChipWire w = currentWire.GetComponent<ChipWire>();
                            w.ownerChipGUID = selectedGate.guid;
                            w.AddPoint(lastClickPos);
                            Debug.Log(inputs[selectedInput].GetComponent<IOLabel>().GetLabel());
                            w.connectedInput = inputs[selectedInput].GetComponent<IOLabel>().GetLabel();
                            pointNum++;

                            inp.wire = currentWire;
                        }
                    }
                    else
                    {
                        if (outputs.Count > 0)
                        {
                            output = selectedGate.Outputs[outputs[selectedOutput].GetComponent<IOLabel>().GetLabel()];
                            connectOutput = false;
                            showInputs = true;

                            currentWire.GetComponent<ChipWire>().AddPoint(lastClickPos);

                            LinkInput();
                            pointNum = 0;
                        }

                    }
                }


                if (Input.GetKeyDown("r"))
                {
                    if (showInputs)
                    {
                        if (inputs.Count > 0)
                        {
                            LinkInput inpt = selectedGate.Inputs[inputs[selectedInput].GetComponent<IOLabel>().GetLabel()];
                            inpt.Disconnect();
                        }

                    }
                    else
                    {
                        connectOutput = false;
                        showInputs = true;
                        pointNum = 0;
                        Destroy(currentWire);
                        inp = null;
                    }
                }

                if (Input.GetMouseButtonDown(1))
                {
                    GameState.Instance.SetCircuitEditSubstate(GameState.CircuitEditSubstate.EditOptions);
                    optsWindow.gameObject.SetActive(true);
                    optsWindow.LoadParameters(gate);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = ClickPos();

                if ((connectOutput) && (pos != Vector3.zero))
                {
                    // pos.y += 0.015f;
                    currentWire.GetComponent<ChipWire>().AddPoint(pos);
                    pointNum++;
                }
            }

            if (Input.GetKeyDown("r"))
            {
                if (connectOutput)
                {
                    connectOutput = false;
                    showInputs = true;
                    pointNum = 0;
                    Destroy(currentWire);
                    inp = null;
                }
            }

        }

    }

    void LinkInput()
    {
        Link.ConnectLinks(output, inp);
    }

    void ScrollLabels()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if ((showInputs) && (inputs.Count > 0))
            {
                inputs[selectedInput].GetComponent<IOLabel>().Deselect();
                selectedInput++;
                if (selectedInput > inputs.Count - 1)
                {
                    selectedInput = 0;
                }
            }
            else if (outputs.Count > 0)
            {
                outputs[selectedOutput].GetComponent<IOLabel>().Deselect();
                selectedOutput++;
                if (selectedOutput > outputs.Count - 1)
                {
                    selectedOutput = 0;
                }
            }

        }
        else
        {
            if ((showInputs) && (inputs.Count > 0))
            {
                inputs[selectedInput].GetComponent<IOLabel>().Deselect();
                selectedInput--;
                if (selectedInput < 0)
                {
                    selectedInput = inputs.Count - 1;
                }
            }
            else if (outputs.Count > 0)
            {
                outputs[selectedOutput].GetComponent<IOLabel>().Deselect();
                selectedOutput--;
                if (selectedOutput < 0)
                {
                    selectedOutput = outputs.Count - 1;
                }

            }
        }
    }

    void DrawLabels()
    {
        if ((showInputs)&&(inputs.Count == 0))
        {
            return;
        }
        else if ((!showInputs) && (outputs.Count == 0))
        {
            return;
        }

        window.SetActive(true);

        if (!showInputs)
        {
            windowHeader.text = "Outputs";

            foreach (GameObject item in outputs)
            {
                item.SetActive(true);
            }

            foreach (GameObject item in inputs)
            {
                item.SetActive(false);
            }

            if (outputs.Count > 0)
            {
                outputs[selectedOutput].GetComponent<IOLabel>().Select();
            }

           

        }
        else
        {
            windowHeader.text = "Inputs";

            foreach (GameObject item in inputs)
            {
                item.SetActive(true);
            }

            foreach (GameObject item in outputs)
            {
                item.SetActive(false);
            }

            if (inputs.Count > 0)
            {
                inputs[selectedInput].GetComponent<IOLabel>().Select();
            }

            if (inputs.Count > 0)
            {
                string inpName = inputs[selectedInput].GetComponent<IOLabel>().text.text;
                GameObject wire = selectedGate.Inputs[inpName].wire;
                if (wire != null)
                {
                    wire.GetComponent<WireBlink>().StartBlinking();
                }
            }

            
        }

        window.GetComponent<RectTransform>().anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    GameObject CreateLabel(Link item, int offset, int i)
    {
        GameObject label = Instantiate(IOLabelFab) as GameObject;
        RectTransform tr = label.GetComponent<RectTransform>();
        tr.SetParent(window.transform);
        tr.localPosition = new Vector3(0,-i * offset - 25,0);

        IOLabel lab = label.GetComponent<IOLabel>();
        lab.SetLabel(item.name);
        label.SetActive(false);
        return label;
    }

    void FillLinks()
    {
        int offset = 35;
        int i = 0;
        foreach (LinkInput item in selectedGate.Inputs.Values)
        {
            inputs.Add(CreateLabel(item, offset, i));
            i++;
        }

        i = 0;
        foreach (Output item in selectedGate.Outputs.Values)
        {
            outputs.Add(CreateLabel(item, offset, i));
            i++;
        }
    }

    void ClearLinks()
    {
        foreach (GameObject item in inputs)
        {
            Destroy(item);
        }

        foreach (GameObject item in outputs)
        {
            Destroy(item);
        }

        outputs.Clear();
        inputs.Clear();

        selectedInput = 0;
        selectedOutput = 0;

    }

    Vector3 ClickPos()
    {
        Vector3 pos = Vector3.zero;
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, ray.direction, out hit, 200f))
        {
            pos = hit.point;
            
        }
        

        return pos;
    }

    Gate ClickGate()
    {
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, ray.direction, out hit, 200f))
        {

            Gate g = hit.collider.gameObject.GetComponent<Gate>();
            if (g != null)
            {
                lastClickPos = hit.point;
                return g;
            }
            else
                return null;
        }
        else { return null; }
    }

}
