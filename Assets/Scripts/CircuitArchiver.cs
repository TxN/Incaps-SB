using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CircuitArchiver : MonoBehaviour 
{
    public GameObject circ;
    public GameObject bl;


    public static void PackCircuit(GameObject block,GameObject circuit)
    {
        CircuitData data;
        if (block.GetComponent<CircuitData>() == null)
        {
            data = block.AddComponent<CircuitData>();

        }
        else
        {
            data = block.GetComponent<CircuitData>();
        }

        Gate[] gates = circuit.transform.GetComponentsInChildren<Gate>();
        foreach (Gate gate in gates) 
        {
            gate.SavePositionInfo();

            Type t = gate.GetType();
            Gate g = (Gate) block.AddComponent(t);
            data.Gates.Add(g);
            g.initialized = true;
            g.miniMode = true;
            g.position = gate.position;
            g.rotation = gate.rotation;
            g.fabName = gate.fabName;
            g.displayName = gate.displayName;


            //Копируем параметры
            CopyParameters(gate, g);

            //Копируем инпуты
            foreach (string n in gate.Inputs.Keys)
            {
                LinkInput lnk = gate.Inputs[n];
                g.RegisterLink(n, lnk.valueType, lnk.linkType);
                LinkInput inp = g.Inputs[n];
                inp.connectedLinkGUID = lnk.connectedLinkGUID;
                inp.isConnected = lnk.isConnected;
                inp.guid = lnk.guid;
                inp.isPublic = lnk.isPublic;

                if(lnk.wire != null) 
                {
                    ChipWire wire = lnk.wire.GetComponent<ChipWire>();
                    WirePoints pnt = new WirePoints();
                    foreach (Vector3 p in wire.Points)
                    {
                        pnt.Points.Add(p);
                    }
                    
                    pnt.ownerChipGUID = wire.ownerChipGUID;
                    pnt.connectedInput = wire.connectedInput;
                    data.Wires.Add(pnt);
                }


                if (lnk.isPublic)
                {
                    data.Inputs.Add(inp);
                }

                data.InternalInputs.Add(inp);
            }

            //Копируем аутпуты
            foreach (string n in gate.Outputs.Keys)
            {
                Output lnk = gate.Outputs[n];
                g.RegisterLink(n, lnk.valueType, lnk.linkType);
                Output outp = g.Outputs[n];
                outp.connectedLinkGUID = lnk.connectedLinkGUID;
                outp.isConnected = lnk.isConnected;
                outp.guid = lnk.guid;
                outp.isPublic = lnk.isPublic;

                if (outp.isPublic)
                {
                    data.Outputs.Add(outp);
                }

                data.InternalOutputs.Add(outp);
            }

            //Соединяем линки
            foreach (LinkInput inp in data.InternalInputs)
            {
                if (inp.isConnected)
                {
                    string guid = inp.connectedLinkGUID;

                    foreach (Output outp in data.InternalOutputs)
                    {
                        if (outp.guid == guid)
                        {
                            Link.ConnectLinks(outp, inp);
                        }
                    }
                }
            }


            g.UpdateParameters();
        }



    }


    public static void UnpackCircuit(CircuitData data, ElectronicsBoard pcb)
    {
        GameObject parent = pcb.gameObject;

        List<LinkInput> inputs = new List<LinkInput>();
        List<Output> outputs = new List<Output>();

        List<Gate> gates = new List<Gate>();

        //Спавним гейты
        foreach (Gate gate in data.Gates)
        {
            GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("Gates/" + gate.fabName)) as GameObject;
            obj.transform.position = gate.position;
            obj.transform.rotation = gate.rotation;
            obj.transform.parent = parent.transform;

            Gate g = obj.GetComponent<Gate>();
            CopyParameters(gate, g);
            gates.Add(g);

            g.initialized = true;
            g.miniMode = false;

            foreach (string n in gate.Inputs.Keys)
            {
                LinkInput lnk = gate.Inputs[n];
                g.RegisterLink(n, lnk.valueType, lnk.linkType);
                LinkInput inp = g.Inputs[n];
                inp.connectedLinkGUID = lnk.connectedLinkGUID;
                inp.isConnected = lnk.isConnected;
                inp.guid = lnk.guid;
                inp.isPublic = lnk.isPublic;

                inputs.Add(inp);

            }

            foreach (string n in gate.Outputs.Keys)
            {
                Output lnk = gate.Outputs[n];
                g.RegisterLink(n, lnk.valueType, lnk.linkType);
                Output outp = g.Outputs[n];
                outp.connectedLinkGUID = lnk.connectedLinkGUID;
                outp.isConnected = lnk.isConnected;
                outp.guid = lnk.guid;
                outp.isPublic = lnk.isPublic;

                outputs.Add(outp);
            }

            foreach (LinkInput inp in inputs)
            {
                if (inp.isConnected)
                {
                    string guid = inp.connectedLinkGUID;

                    foreach (Output outp in outputs)
                    {
                        if (outp.guid == guid)
                        {
                            Link.ConnectLinks(outp, inp);
                        }
                    }
                }
            }




        }


        GameObject wireFab = Resources.Load("wire") as GameObject;

        foreach (WirePoints wire in data.Wires)
        {
            GameObject w = Instantiate(wireFab) as GameObject;
            ChipWire comp = w.GetComponent<ChipWire>();
            comp.ownerChipGUID = wire.ownerChipGUID;
            comp.connectedInput = wire.connectedInput;


            foreach (Gate gt in gates)
            {
                if (gt.guid == wire.ownerChipGUID)
                {
                    w.transform.parent = gt.transform;

                    gt.Inputs[wire.connectedInput].wire = w;

                    int i = 0;
                    foreach (WirePoints point in data.Wires)
                    {
                        comp.AddPoint(point.Points[i]);
                        i++;
                    }

                   // break;
                }
            }
        }

    }

    public static void CopyParameters(Gate gate, Gate g)
    {
        foreach (string par in gate.Parameters.Keys)
        {
            EditableParameter p = gate.Parameters[par];

            g.RegisterParameter(par, p.type);
            g.Parameters[par].SetValue((float)p.GetValue<float>());
            g.Parameters[par].SetValue((int)p.GetValue<int>());
            g.Parameters[par].SetValue((bool)p.GetValue<bool>());
            g.Parameters[par].SetValue((string)p.GetValue<string>());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("l"))
        {
            PackCircuit(bl,circ);
        }



        if (Input.GetKeyDown("o"))
        {
            UnpackCircuit(bl.GetComponent<CircuitData>(), circ.GetComponent<ElectronicsBoard>());
        }
    }
}
