using UnityEngine;
using System.Collections;

public class OutputPlug : Gate
{



    public override void Init()
    {
        
        if (!initialized)
        {
            passive = true;
            BaseInit();
            initialized = true;
        }
    }

    public override void Think()
    {
        foreach (string inp in Inputs.Keys)
        {
            Outputs[inp].SetValue((float)Inputs[inp].GetValue<float>());
            Outputs[inp].SetValue((string)Inputs[inp].GetValue<string>());
        }
    }

    public override void UpdateParameters()
    {
       
        foreach (string par in Parameters.Keys)
        {
                LinkInput val;
                if (!Inputs.TryGetValue(par, out val))
                {
                    ClearLinks();
                    CreateLinks();
                    break;
                }
        }

        base.UpdateParameters();
    }

    void Update()
    {
    
    }

    void BaseInit()
    {
        StrEditParameter o1Name = (StrEditParameter)RegisterParameter("Output 1 Name", EditableParameter.ParameterType.StrEdit);
        o1Name.SetValue("Out1");

        StrEditParameter o2Name = (StrEditParameter)RegisterParameter("Output 2 Name", EditableParameter.ParameterType.StrEdit);
        o2Name.SetValue("Out2");
    }

    void CreateLinks()
    {
        foreach (EditableParameter par in Parameters.Values)
        {
            string n = (string) par.GetValue<string>();
            RegisterLink(n, Link.ValType.Numeric, Link.Type.Out);
            RegisterLink(n, Link.ValType.Numeric, Link.Type.In);
        }
    }

    void ClearLinks()
    {

        foreach (LinkInput item in Inputs.Values)
        {
            Destroy(item);
        }

        foreach (Output item in Outputs.Values)
        {
            Destroy(item);
        }

        Inputs.Clear();
        Outputs.Clear();
    }
}
