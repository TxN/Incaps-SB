using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Linker : MonoBehaviour 
{
    public GateOptions optsWindow;

    public GameObject wireFab;
    public Canvas canvas;

    Vector3 lastClickPos;
    Gate selectedGate;

    public GameObject window;
    public GameObject IOLabelFab;

    List<GameObject> inputs = new List<GameObject>();
    List<GameObject> outputs = new List<GameObject>();
    int selectedInput = 0;
    int selectedOutput = 0;

    bool showInputs = true;
    bool connectOutput = false;

    LinkInput inp;
    Output output;

    GameObject currentWire;
    int pointNum = 0;

    void Start()
    {
       // window = new GameObject();
       // window.GetComponent<Transform>().SetParent(canvas.transform, false);
       // window.AddComponent<Ima

    }

    void Update()
    {
        Gate gate = ClickGate();

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
                        currentWire.GetComponent<LineRenderer>().SetPosition(0, lastClickPos);
                        currentWire.GetComponent<LineRenderer>().SetPosition(1, lastClickPos);
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
                        currentWire.GetComponent<LineRenderer>().SetVertexCount(pointNum + 1);
                        lastClickPos.y += 0.015f;
                        currentWire.GetComponent<LineRenderer>().SetPosition(pointNum, lastClickPos);
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
                optsWindow.gameObject.SetActive(true);
                optsWindow.LoadParameters(gate);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 pos = ClickPos();

            if ((connectOutput)&&(pos!= Vector3.zero))
            {
                pos.y += 0.015f;
                currentWire.GetComponent<LineRenderer>().SetVertexCount(pointNum + 1);
                currentWire.GetComponent<LineRenderer>().SetPosition(pointNum, pos);
                pointNum++;
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
        window.SetActive(true);
        if (!showInputs)
        {
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
        tr.localPosition = new Vector3(0,-i * offset - 5,0);

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
