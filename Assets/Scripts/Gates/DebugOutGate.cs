using UnityEngine;
using System.Collections;

public class DebugOutGate : Gate
{
    TextMesh txt;

    public override void Init()
    {
        passive = false;

        if (!initialized)
        {
            RegisterLink("Number", Link.ValType.Numeric, Link.Type.In);
            RegisterLink("String", Link.ValType.String, Link.Type.In);
            initialized = true;
        }


        if (!miniMode)
        {
            txt = GetComponentInChildren<TextMesh>();
        }

        Refresh();

    }

    public override void Think()
    {
        Refresh();
        
    }


    void Refresh()
    {
        if (!miniMode)
        {
            if (txt != null)
            {
                txt.text = "TXT: " + ((string)Inputs["String"].GetValue<string>()) + "\nNUM: " + ((float)Inputs["Number"].GetValue<float>()).ToString();
            }
        }
        else
        {
            Debug.Log("TXT: " + ((string)Inputs["String"].GetValue<string>()) + "\nNUM: " + ((float)Inputs["Number"].GetValue<float>()).ToString());
        }
    }
}
