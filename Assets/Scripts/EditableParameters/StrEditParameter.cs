﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StrEditParameter : EditableParameter 
{
    public StrEditParameter()
    {

    }


    public override void Init(GateOptions optsWindow)
    {
        base.Init(optsWindow);

        GameObject edit = UnityEngine.Object.Instantiate(optsWindow.strEditFab);
        RectTransform tr = edit.GetComponent<RectTransform>();
        tr.SetParent(optsWindow.transform);
        tr.anchoredPosition = optsWindow.rightOffset + new Vector2(0, optsWindow.rightOffset.y * optsWindow.optionsNum);

        edit.SetActive(true);
        optsWindow.tempObjects.Add(edit);
        optsWindow.optionsNum++;

        edit.GetComponent<InputField>().placeholder.GetComponent<Text>().text = (string) GetValue<string>();

        edit.GetComponent<InputField>().onValueChange.AddListener(ChangeValue);

    }

    public void ChangeValue(string text)
    {
       SetValue(text);
    }

}
