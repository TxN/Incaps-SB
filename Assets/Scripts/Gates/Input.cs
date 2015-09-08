using UnityEngine;
using System.Collections;

[System.Serializable]
public class LinkInput : Link
{

    string lastStr = "";
    float lastNum = 0;
    public bool changed = true;

    

    public LinkInput(string n, Link.ValType type)
    {
        name = n;
        valueType = type;
        linkType = Type.In;
    }

    public bool Refresh()
    {
       
        if ((isConnected) && (connectedLink != null))
        {
            numVal = (float)connectedLink.GetValue<float>();
            strVal = (string)connectedLink.GetValue<string>();
        }
        else if ((isConnected) && (connectedLink == null))
        {
            Disconnect();
        }
            if (valueType == ValType.Numeric)
            {
                
                if (numVal != lastNum)
                {
                    changed = true;
                    lastNum = numVal;
                }
            }
            else if (valueType == ValType.String)
            {
               
                if (strVal != lastStr)
                {
                    changed = true;
                    lastStr = strVal;
                }
            }


        return changed;
    }



}

[System.Serializable]
public class InputDictionary : SerializableDictionary<string, LinkInput> { }