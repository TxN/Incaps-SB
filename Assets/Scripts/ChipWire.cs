using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChipWire : MonoBehaviour 
{
    public string ownerChipGUID;
    public string connectedInput;

    LineRenderer line;
    int numPoints = 0;

    public float wireWidth = 0.015f;

    public List<Vector3> Points = new List<Vector3>();

    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    public void AddPoint(Vector3 point)
    {
        Points.Add(point);
        numPoints++;
        SetPoints();
    }

    public void ClearPoints()
    {
        Points.Clear();
        numPoints = 0;
        if (line != null)
        {
            line.SetVertexCount(0);
        }
    }

    public void SetPoints()
    {
        if (line != null)
        {
            int i = 0;
            line.SetVertexCount(numPoints);
            foreach (Vector3 point in Points)
            {
                line.SetPosition(i, point);
                i++;
            }

            line.SetWidth(wireWidth, wireWidth);
        }
    }

}

public class WirePoints
{
    public List<Vector3> Points = new List<Vector3>();
    int numPoints = 0;
    public string ownerChipGUID;
    public string connectedInput;

    public WirePoints()
    {
        numPoints = 0;
        
    }
}