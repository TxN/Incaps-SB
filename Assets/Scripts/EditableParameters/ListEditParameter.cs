using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ListEditParameter : EditableParameter
{

    public List<string> items = new List<string>();

    public ListEditParameter()
    {
        //items.Add("Null");
    }

    public override void Init(GateOptions optsWindow)
    {
        base.Init(optsWindow);

        GameObject list = UnityEngine.Object.Instantiate(optsWindow.listFab);
        RectTransform tr = list.GetComponent<RectTransform>();
        tr.SetParent(optsWindow.transform);
        tr.anchoredPosition = optsWindow.rightOffset + new Vector2(0, optsWindow.rightOffset.y * optsWindow.optionsNum);
        foreach (string item in items)
	    {
            list.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(item));
	    }

        list.SetActive(true);
        optsWindow.tempObjects.Add(list);
        optsWindow.optionsNum++;

        list.GetComponent<Dropdown>().value = (int)GetValue<int>();
             
        list.GetComponent<Dropdown>().onValueChanged.AddListener(ElementClick);
    }

    public void ElementClick(int item)
    {
        SetValue(item);
    }

}
