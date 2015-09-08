using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SwitchEditParameter : EditableParameter
{
    public SwitchEditParameter()
    {

    }

    public override void Init(GateOptions optsWindow)
    {
        base.Init(optsWindow);

        GameObject sw = UnityEngine.Object.Instantiate(optsWindow.switchFab);
        RectTransform tr = sw.GetComponent<RectTransform>();
        tr.SetParent(optsWindow.transform);
        tr.anchoredPosition = optsWindow.rightOffset + new Vector2(0, optsWindow.rightOffset.y * optsWindow.optionsNum);

        sw.SetActive(true);
        optsWindow.tempObjects.Add(sw);
        optsWindow.optionsNum++;

        sw.GetComponent<Toggle>().isOn = (bool) GetValue<bool>();

        sw.GetComponent<Toggle>().onValueChanged.AddListener(ChangeValue);

    }

    public void ChangeValue(bool val)
    {
        SetValue(val);
    }
}
