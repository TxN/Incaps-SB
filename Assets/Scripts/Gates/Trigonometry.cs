using UnityEngine;
using System.Collections;

public class Trigonometry : Gate 
{
    TextMesh txt;

    public enum OpType
    {
        Sin = 0,
        Cos = 1,
        Tan = 2,
        Acos = 3,
        Asin = 4,
        Atan = 5

    }

    public OpType op = OpType.Sin;

    public override void Init()
    {
        if (!miniMode)
        {
            txt = GetComponentInChildren<TextMesh>();
        }

        if (!initialized)
        {
            passive = true;

            RegisterLink("A", Link.ValType.Numeric, Link.Type.In);
            RegisterLink("Out", Link.ValType.Numeric, Link.Type.Out);

            ListEditParameter par = (ListEditParameter)RegisterParameter("Operation", EditableParameter.ParameterType.List);

            Parameters["Operation"].SetValue((int)op);
            initialized = true;
        }

        ListEditParameter p = (ListEditParameter) Parameters["Operation"];
        p.items.Add("Sinus");
        p.items.Add("Cosine");
        p.items.Add("Tangent");
        p.items.Add("ArcCos");
        p.items.Add("ArcSin");
        p.items.Add("ArcTg");


        

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

        switch ((OpType)Parameters["Operation"].GetValue<int>())
        {
            case OpType.Sin:
                Outputs["Out"].SetValue( Mathf.Sin((float)Inputs["A"].GetValue<float>()) );
                break;

            case OpType.Cos:
                Outputs["Out"].SetValue(Mathf.Cos((float)Inputs["A"].GetValue<float>()) );
                break;

            case OpType.Tan:
                Outputs["Out"].SetValue( Mathf.Tan((float)Inputs["A"].GetValue<float>()) );
                break;

            case OpType.Acos:
                Outputs["Out"].SetValue(Mathf.Acos((float)Inputs["A"].GetValue<float>()));
                break;

            case OpType.Asin:
                Outputs["Out"].SetValue(Mathf.Asin((float)Inputs["A"].GetValue<float>()));
                break;

            case OpType.Atan:
                Outputs["Out"].SetValue(Mathf.Atan((float)Inputs["A"].GetValue<float>()));
                break;

        }
    }
}
