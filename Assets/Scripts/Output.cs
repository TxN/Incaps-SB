using UnityEngine;
using System.Collections;

[System.Serializable]
public class Output : Link 
{

    public Output(string n, Link.ValType type)
    {
        name = n;
        valueType = type;
        linkType = Type.Out;
    }

    public void SetValue(float val)
    {
        if (valueType == ValType.Numeric)
        {
            numVal = val;
        }
    }

    public void SetValue(string val)
    {
        if (valueType == ValType.String)
        {
            strVal = val;
        }
    }

    
	
}

[System.Serializable]
public class OutputDictionary : SerializableDictionary<string, Output> { }