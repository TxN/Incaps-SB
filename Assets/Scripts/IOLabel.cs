using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IOLabel : MonoBehaviour 
{
    public Text text;
    bool selected = false;

    public Color selectedColor = Color.green;
    Color initColor;

    void Awake()
    {
        initColor = GetComponent<Image>().color;
    }

    void Update()
    {

    }

    public void SetLabel(string n)
    {
        text.text = n;
    }

    public string GetLabel()
    {
        return text.text;
    }

    public void Select()
    {
        GetComponent<Image>().color = selectedColor;
        selected = true;
    }

    public void Deselect()
    {
        selected = false;
        GetComponent<Image>().color = initColor;
    }

}
