using UnityEngine;
using System.Collections;

public class KeyInput : Gate
{

    enum KeyIDs
    {
        None = 0,
        RightArrow = 1,
        LeftArrow = 2,
        UpArrow = 3,
        DownArrow = 4,
        Enter = 5,
        RShift = 6,
        LShift = 7,
        Space = 8,
        Ctrl = 9


    }

    bool toggle = false;
    float onValue = 1;
    float offValue = 0f;
    KeyIDs keyID = KeyIDs.None;
    bool pressed = false;
    bool lastState = false;


    public override void Init()
    {
        if (!initialized)
        {
            passive = false;

            NumEditParameter par = (NumEditParameter)RegisterParameter("On Value", EditableParameter.ParameterType.NumEdit);
            NumEditParameter par2 = (NumEditParameter)RegisterParameter("Off Value", EditableParameter.ParameterType.NumEdit);
            SwitchEditParameter par3 = (SwitchEditParameter)RegisterParameter("Toggle", EditableParameter.ParameterType.Switch);
            ListEditParameter par4 = (ListEditParameter)RegisterParameter("Key", EditableParameter.ParameterType.List);

            par.SetValue(1f);
            par2.SetValue(0f);
            par3.SetValue(false);
            par4.SetValue((int)0);

            RegisterLink("Out", Link.ValType.Numeric, Link.Type.Out);
            initialized = true;
        }

        ListEditParameter key = (ListEditParameter)Parameters["Key"];
        key.items.Add("None");
        key.items.Add("Right Arrow");
        key.items.Add("Left Arrow");
        key.items.Add("Up Arrow");
        key.items.Add("Down Arrow");
        key.items.Add("Enter");
        key.items.Add("Right Shift");
        key.items.Add("Left Shift");
        key.items.Add("Space");
        key.items.Add("Right Ctrl");

    }

    void Update()
    {
        bool ev = false;

        if ((int)keyID == 1)
        {
            if(Input.GetKey(KeyCode.RightArrow))
            {
                ev = true;
            }
        }
        else if ((int)keyID == 2)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                ev = true;
            }
        }
        else if ((int)keyID == 3)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                ev = true;
            }
        }
        else if ((int)keyID == 4)
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                ev = true;
            }
        }
        else if ((int)keyID == 5)
        {
            if (Input.GetKey(KeyCode.Return))
            {
                ev = true;
            }
        }
        else if ((int)keyID == 6)
        {
            if (Input.GetKey(KeyCode.RightShift))
            {
                ev = true;
            }
        }
        else if ((int)keyID == 7)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                ev = true;
            }
        }
        else if ((int)keyID == 8)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                ev = true;
            }
        }
        else if ((int)keyID == 9)
        {
            if (Input.GetKey(KeyCode.RightControl))
            {
                ev = true;
            }
        }

        

        if (ev)
        {
            if (!toggle)
            {
                pressed = true;
            }
            else
            {
                if (ev != lastState)
                {
                    pressed = !pressed;
                }
            }
        }

        lastState = ev;
    }

    public override void Think()
    {
        onValue = (float)Parameters["On Value"].GetValue<float>();
        offValue = (float)Parameters["Off Value"].GetValue<float>();
        toggle = (bool)Parameters["Toggle"].GetValue<bool>();
        keyID = (KeyIDs)Parameters["Key"].GetValue<int>();

        if (!toggle)
        {
            if (pressed)
            {
                pressed = false;
                Outputs["Out"].SetValue(onValue);
            }
            else
            {
                Outputs["Out"].SetValue(offValue);
            }
        }
        else
        {
            if (pressed)
            {
                Outputs["Out"].SetValue(onValue);
            }
            else
            {
                Outputs["Out"].SetValue(offValue);
            }
        }
        
    }
	
}
