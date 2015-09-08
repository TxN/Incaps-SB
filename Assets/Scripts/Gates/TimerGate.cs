using UnityEngine;
using System.Collections;

public class TimerGate : Gate 
{

    bool enable = false;
    float time;

    public override void Init()
    {

        updateDt = 0f;

        if (!initialized)
        {

            RegisterLink("Time", Link.ValType.Numeric, Link.Type.Out);

            RegisterLink("Enable", Link.ValType.Numeric, Link.Type.In);
            RegisterLink("Reset", Link.ValType.Numeric, Link.Type.In);
            passive = false;
            initialized = true;
        }

    }

    public override void Think()
    {
        float enabler = (float)Inputs["Enable"].GetValue<float>();
        float resetter = (float)Inputs["Reset"].GetValue<float>();

        if (Mathf.Abs(enabler) > 0.5f)
        {
            enable = true;
        }
        else
        {
            enable = false;
        }

        if (Mathf.Abs(resetter) > 0.5f)
        {
            time = 0f;
        }

        Outputs["Time"].SetValue(time);
    }

    void Update()
    {
        if (enable)
        {
            time += Time.deltaTime;
        }
    }
}
