using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public abstract class Gate : LogicObject
{
    public string displayName = "Fill Me";
    public string fabName = "nullfab";

    [SerializeField]
    public InputDictionary Inputs = new InputDictionary();
    [SerializeField]
    public OutputDictionary Outputs = new OutputDictionary();
    [SerializeField]
    public ParameterDictionary Parameters = new ParameterDictionary();

    [System.NonSerialized]
    public bool miniMode = false;

    [SerializeField]
    public bool passive = false;
    [SerializeField]
    public float updateDt = 0.1f;
    float lastUpdateT = 0f;

    bool first = true;

    [System.NonSerialized]
    public bool initialized = false;

    public Vector3 position;
    public Quaternion rotation;



    public abstract void Init();

    void Start()
    {    
        Init();
    }

    void FixedUpdate()
    {
        if (first)
        {
            Think();
            first = false;
            lastUpdateT = Time.time;
            return;
        }

        if (passive)
        {
            bool think = false;
            foreach (LinkInput input in Inputs.Values)
            {
                if (input.Refresh() == true)
                {
                    think = true;
                    input.changed = false;
                }
            }

            if (think)
            {
                Think();
                think = false;
                return;
            }
        }
        else
        {
            foreach (LinkInput input in Inputs.Values)
            {
               input.Refresh();
            }

            if (Time.time >= lastUpdateT + updateDt)
            {
                Think();
                lastUpdateT = Time.time;
                return;
            }
            
        }
    }

    public abstract void Think();

    public virtual void UpdateParameters()
    {
        if (passive)
        {
            Think();
        }
    }

    public void RegisterLink(string name, Link.ValType vType, Link.Type type)
    {
        if (type == Link.Type.Out)
        {
            Outputs.Add(name, new Output(name, vType));
        }
        else
        {
            Inputs.Add(name, new LinkInput(name, vType));
        }
    }

    public EditableParameter RegisterParameter(string name, EditableParameter.ParameterType type)
    {
        EditableParameter par = null;

        switch (type)
        {
            case EditableParameter.ParameterType.List:
                par = new ListEditParameter();
                
                break;

            case EditableParameter.ParameterType.NumEdit:
                par = new NumEditParameter();

                break;

            case EditableParameter.ParameterType.StrEdit:
                par = new StrEditParameter();

                break;

            case EditableParameter.ParameterType.Slider:
                par = new SliderEditParameter();

                break;

            case EditableParameter.ParameterType.Switch:
                par = new SwitchEditParameter();

                break;

            default:
                par = new EditableParameter();
                break;
        }

        par.type = type;
        par.parameterName = name;
        Parameters.Add(name, par);
        return par;
    }

    void OnDestroy()
    {
        foreach (LinkInput item in Inputs.Values)
        {
            Destroy(item);
        }

        foreach (Output item in Outputs.Values)
        {
            Destroy(item);
        }

        foreach (EditableParameter item in Parameters.Values)
        {
            Destroy(item);
        }

    }

    public void SavePositionInfo()
    {
        position = transform.position;
        rotation = transform.rotation;
    }



}
