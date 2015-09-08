using UnityEngine;
using System.Collections;

public class ConstantValue : Gate
{
    public string text;
    public float number;


    TextMesh txt;

    public override void Init()
    {
        

        if (!miniMode)
        {
            txt = GetComponentInChildren<TextMesh>();
        }

        if (!initialized)
        {
            Debug.Log("Initializing");
            passive = true;
            RegisterLink("Number", Link.ValType.Numeric, Link.Type.Out);
            RegisterLink("String", Link.ValType.String, Link.Type.Out);

            NumEditParameter numPar = (NumEditParameter)RegisterParameter("Number", EditableParameter.ParameterType.NumEdit);
            StrEditParameter strPar = (StrEditParameter)RegisterParameter("String", EditableParameter.ParameterType.StrEdit);
            numPar.SetValue(number);
            strPar.SetValue(text);
            initialized = true;
        }

        Refresh();

    }

    public override void Think()
    {

    }

    void Update()
    {
        number = (float)Parameters["Number"].GetValue<float>();
        text = (string)Parameters["String"].GetValue<string>();

        if ((float) Outputs["Number"].GetValue<float>() != number) 
        {
            Refresh();
        }
        if ((string)Outputs["String"].GetValue<string>() != text)
        {
            Refresh();
        }
    }

    void Refresh()
    {
        Outputs["Number"].SetValue( number);
        Outputs["String"].SetValue(text);

        if (!miniMode)
        {
            if (txt != null)
            {
                txt.text = "TXT: " + text + "\nNUM: " + ((float)Parameters["Number"].GetValue<float>()).ToString();
            }
        }
    }

}
