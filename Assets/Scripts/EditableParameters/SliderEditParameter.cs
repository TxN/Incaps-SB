using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderEditParameter : EditableParameter 
{
    public SliderEditParameter()
    {

    }

    float maxValue = 10;
    float minValue = 0;

    public void SetSliderParameters(float min, float max)
    {
        maxValue = max;
        minValue = min;
    }

    public override void Init(GateOptions optsWindow)
    {
        base.Init(optsWindow);

        GameObject slider = UnityEngine.Object.Instantiate(optsWindow.sliderFab);
        RectTransform tr = slider.GetComponent<RectTransform>();
        tr.SetParent(optsWindow.transform);
        tr.anchoredPosition = optsWindow.rightOffset + new Vector2(0, optsWindow.rightOffset.y * optsWindow.optionsNum);

        slider.SetActive(true);
        optsWindow.tempObjects.Add(slider);
        optsWindow.optionsNum++;

        slider.GetComponent<Slider>().maxValue = maxValue;
        slider.GetComponent<Slider>().minValue = minValue;
        slider.GetComponent<Slider>().value = (float) GetValue<float>();

        slider.GetComponent<Slider>().onValueChanged.AddListener(ChangeValue);


    }

    public void ChangeValue(float val)
    {
        SetValue(val);
    }
	
}
