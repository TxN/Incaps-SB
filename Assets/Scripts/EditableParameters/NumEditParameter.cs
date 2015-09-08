using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NumEditParameter : EditableParameter
{

    public NumEditParameter()
    {

    }

    public override void Init(GateOptions optsWindow)
    {
        base.Init(optsWindow);

        GameObject edit = UnityEngine.Object.Instantiate(optsWindow.numEditFab);
        RectTransform tr = edit.GetComponent<RectTransform>();
        tr.SetParent(optsWindow.transform);
        tr.anchoredPosition = optsWindow.rightOffset + new Vector2(0, optsWindow.rightOffset.y * optsWindow.optionsNum);

        edit.SetActive(true);
        optsWindow.tempObjects.Add(edit);
        optsWindow.optionsNum++;

        edit.GetComponent<InputField>().placeholder.GetComponent<Text>().text = ((float) GetValue<float>()).ToString();

        edit.GetComponent<InputField>().onValueChange.AddListener(ChangeValue);

    }

    public void ChangeValue(string text)
    {
        float val = 0;
        float.TryParse(text, out val);
        SetValue(val);
    }
}
