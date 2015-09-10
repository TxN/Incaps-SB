using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircuitData : MonoBehaviour 
{
    public List<Gate> Gates = new List<Gate>();
    public List<LinkInput> InternalInputs = new List<LinkInput>();
    public List<Output> InternalOutputs = new List<Output>();
    public List<WirePoints> Wires = new List<WirePoints>();
    //Todo: wire object list

    public List<LinkInput> Inputs = new List<LinkInput>();
    public List<Output> Outputs = new List<Output>();
    



}
