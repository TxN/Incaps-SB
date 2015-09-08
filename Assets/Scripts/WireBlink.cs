using UnityEngine;
using System.Collections;

public class WireBlink : MonoBehaviour 
{
    bool blink = false;

    Renderer ren;
    void Awake()
    {
        ren = GetComponent<Renderer>();
    }

    public void StartBlinking()
    {
        blink = true;
        ren.material.color = Color.blue;
    }

    public void StopBlinking()
    {
        ren.material.color = Color.green;
        blink = false;
    }

    void Update()
    {
        if (blink == true)
        {
            blink = false;
        }
        else
        {
            ren.material.color = Color.green;
        }
    }

}
