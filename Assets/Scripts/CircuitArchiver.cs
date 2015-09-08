using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CircuitArchiver : MonoBehaviour 
{
    public GameObject circuit;
    public GameObject block;


    public void PackCircuit()
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


           // g.UpdateParameters

            //Копируем параметры
            foreach (string par in gate.Parameters.Keys)
            {
                EditableParameter p = gate.Parameters[par];
                Debug.Log(p.type);
                g.RegisterParameter(par, p.type);
                g.Parameters[par].SetValue((float)p.GetValue<float>());
                g.Parameters[par].SetValue((int)p.GetValue<int>());
                g.Parameters[par].SetValue((bool)p.GetValue<bool>());
                g.Parameters[par].SetValue((string)p.GetValue<string>());
            }

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

    void Update()
    {
        if (Input.GetKeyDown("l"))
        {
            PackCircuit();
        }
    }
}
