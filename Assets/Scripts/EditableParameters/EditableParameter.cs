using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EditableParameter : ScriptableObject
{
    public string parameterName;
    public enum ParameterType
    {
        Switch,
        List,
        Slider,
        NumEdit,
        StrEdit
    }

    float numValue;
    string strValue;
    bool boolValue;
    int intValue;

    public ParameterType type;

    public EditableParameter()
    {
        parameterName = "";
        numValue = 0;
        strValue = "";
        type = ParameterType.NumEdit;

    }

    public EditableParameter(string n,ParameterType t)
    {
        parameterName = n;
        numValue = 0;
        strValue = "";
        type = t;
    }



    public virtual void Init(GateOptions optsWindow) 
    {
        GameObject title = UnityEngine.Object.Instantiate(optsWindow.textFab);
        RectTransform tr = title.GetComponent<RectTransform>();
        tr.SetParent(optsWindow.transform);
        tr.anchoredPosition = optsWindow.leftOffset + new Vector2(0, optsWindow.leftOffset.y * optsWindow.optionsNum);
        title.GetComponent<Text>().text = parameterName;

        title.SetActive(true);
        optsWindow.tempObjects.Add(title);
    }

    public void SetValue(string value) 
    {
        strValue = value;
    }

    public void SetValue(float value) 
    {
        numValue = value;
    }

    public void SetValue(bool value)
    {
        boolValue = value;
    }

    public void SetValue(int value)
    {
        intValue = value;
    }

    public object GetValue<T>()
    {
        if (typeof(T) == typeof(string))
        {
            return strValue;
        }

        if (typeof(T) == typeof(float))
        {
            return numValue;
        }

        if (typeof(T) == typeof(bool))
        {
            return boolValue;
        }
        if (typeof(T) == typeof(int))
        {
            return intValue;
        }
        else return null;
    }
	
}

[System.Serializable]
public class ParameterDictionary : SerializableDictionary<string, EditableParameter> { }