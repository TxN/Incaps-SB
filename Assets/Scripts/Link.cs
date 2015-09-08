using UnityEngine;
using System.Collections;

[System.Serializable]
public class Link : ScriptableObject
{
    public string guid = System.Guid.NewGuid().ToString();

    public enum Type
    {
        In,
        Out
    }

    public enum ValType
    {
        Numeric,
        String
    }

    public string name;
    public Type linkType;
    public ValType valueType;

    protected float numVal;
    protected string strVal;

    [SerializeField]
    public Link connectedLink;
    public bool isConnected;
    public string connectedLinkGUID = null;

    public GameObject wire;

    public bool isPublic = false;

    public Link()
    {
        name = "null";
        linkType = Type.Out;
        valueType = ValType.Numeric;
    }

    public Link(string n, Type t, ValType vType)
    {
        name = n;
        linkType = t;
        valueType = vType;
    }

    public static bool ConnectLinks(Link output, Link input)
    {
        if (input.valueType == output.valueType)
        {
            if (input.isConnected)
            {
                input.Disconnect();
            }

            input.connectedLink = output;
            input.connectedLinkGUID = output.guid;
            //l2.connectedLink = l1;
            input.isConnected = true;
            output.isConnected = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Connect(Link lnk)
    {
        if (lnk.valueType == valueType)
        {
            if (lnk.linkType == Link.Type.In)
            {
                lnk.Disconnect();
                lnk.connectedLinkGUID = guid;
            }

            if (linkType == Link.Type.In)
            {
               Disconnect();
               connectedLinkGUID = lnk.guid;
            }

            lnk.Disconnect();
            connectedLink = lnk;
            isConnected = true;
            lnk.connectedLink = this;
            lnk.isConnected = true;
            return true;
        }
        else
            return false;
    }

    public bool Disconnect()
    {
        if (connectedLink != null)
        {
            connectedLink.connectedLink = null;
            connectedLink.isConnected = false;
            connectedLink = null;
            connectedLinkGUID = null;
        }

        connectedLink = null;
        if (linkType == Type.In)
        {
            numVal = 0f;
            strVal = "";
        }

        isConnected = false;
        if (wire != null)
        {
            UnityEngine.Object.Destroy(wire);
        }
        return true;
    }

    public object GetValue<T>()
    {
        if (typeof(T) == typeof(string))
        {
            return strVal;
        }

        if (typeof(T) == typeof(float))
        {
            return numVal;
        }
        else return null;
    }

	
}
