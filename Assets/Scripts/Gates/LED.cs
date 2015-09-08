using UnityEngine;
using System.Collections;

public class LED : Gate 
{

    Light light;
    Renderer ren;

    public override void Init()
    {
        if (!miniMode)
        {
            light = GetComponentInChildren<Light>();
            ren = GetComponent<Renderer>();
        }

        if (!initialized)
        {
            SliderEditParameter par = (SliderEditParameter)RegisterParameter("Red", EditableParameter.ParameterType.Slider);
            SliderEditParameter par2 = (SliderEditParameter)RegisterParameter("Green", EditableParameter.ParameterType.Slider);
            SliderEditParameter par3 = (SliderEditParameter)RegisterParameter("Blue", EditableParameter.ParameterType.Slider);

            par.SetValue(1f);
            par2.SetValue(1f);
            par3.SetValue(1f);

            RegisterLink("Intensity", Link.ValType.Numeric, Link.Type.In);
        }

        SliderEditParameter s = (SliderEditParameter) Parameters["Red"];
        SliderEditParameter s2 = (SliderEditParameter)Parameters["Green"];
        SliderEditParameter s3 = (SliderEditParameter)Parameters["Blue"];

        s.SetSliderParameters(0, 1f);
        s2.SetSliderParameters(0, 1f);
        s3.SetSliderParameters(0, 1f);


    }

    public override void Think()
    {
        if (miniMode)
        {
            return;
        }
        else
        {
            if (light != null)
            {
                Color col = new Color((float)Parameters["Red"].GetValue<float>(), (float)Parameters["Green"].GetValue<float>(), (float)Parameters["Blue"].GetValue<float>(), 1);
                light.color = col;
                light.intensity = (float)Inputs["Intensity"].GetValue<float>();

                if (ren != null)
                {
                    ren.material.color = col;
                }
            }
        }
    }

}
