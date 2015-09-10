using UnityEngine;
using System.Collections;

public class ArithmeticGate : Gate 
{

    TextMesh txt;

    public enum OpType
    {
        Add = 0,
        Subtract = 1,
        Multiply = 2,
        Divide = 3,
        Pow = 4,
        Sqrt = 5

    }

    public OpType op;

    public override void Init()
    {

        if (!initialized)
        {
            passive = true;
            RegisterLink("A", Link.ValType.Numeric, Link.Type.In);
            RegisterLink("B", Link.ValType.Numeric, Link.Type.In);
            RegisterLink("Out", Link.ValType.Numeric, Link.Type.Out);

            RegisterParameter("Operation", EditableParameter.ParameterType.List);
            Parameters["Operation"].SetValue((int)op);
            initialized = true;
        }


        ListEditParameter par = (ListEditParameter)Parameters["Operation"];
        
        par.items.Add("Add");
        par.items.Add("Subtract");
        par.items.Add("Multiply");
        par.items.Add("Divide");
        par.items.Add("Power");
        par.items.Add("Root");

        if (!miniMode)
        {
            txt = GetComponentInChildren<TextMesh>();
            if (txt != null)
            {
                txt.text = ((OpType)Parameters["Operation"].GetValue<int>()).ToString();
            }
        }

    }

    public override void Think()
    {
        if (!miniMode)
        {
            if (txt != null)
            {
                txt.text = ((OpType)Parameters["Operation"].GetValue<int>()).ToString();
            }
        }

        switch ((OpType) Parameters["Operation"].GetValue<int>())
        {
            case OpType.Add:
                Outputs["Out"].SetValue((float)Inputs["A"].GetValue<float>() + (float)Inputs["B"].GetValue<float>());
                break;

            case OpType.Subtract:
                Outputs["Out"].SetValue((float)Inputs["A"].GetValue<float>() - (float)Inputs["B"].GetValue<float>());
                break;

            case OpType.Multiply:
                Outputs["Out"].SetValue((float)Inputs["A"].GetValue<float>() * (float)Inputs["B"].GetValue<float>());
                break;

            case OpType.Divide:
                if ((float)Inputs["B"].GetValue<float>() != 0f)
                {
                    Outputs["Out"].SetValue((float)Inputs["A"].GetValue<float>() / (float)Inputs["B"].GetValue<float>());
                }
                else { Outputs["Out"].SetValue(0f); }
                break;

            case OpType.Pow:
                Outputs["Out"].SetValue(Mathf.Pow( (float)Inputs["A"].GetValue<float>(),(float)Inputs["B"].GetValue<float>()));
                break;

            case OpType.Sqrt:
                float p = (float)Inputs["B"].GetValue<float>();
                if (p != 0f)
                {
                    p = 1 / p;
                }
                else
                {
                    p = 0;
                }
                
                Outputs["Out"].SetValue(Mathf.Pow((float)Inputs["A"].GetValue<float>(), p ));
                break;

        }
    }


}
