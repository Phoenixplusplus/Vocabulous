using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Report : MonoBehaviour, IisOverlayTile
{
    public Color myColor;
    public int ID = 9999;
    public bool visible = true;

    public int getID()
    {
        return ID;
    }

    public void setVisible(bool value)
    {
        if (value)
        {
            visible = true;
            GetComponent<Renderer>().enabled = false;
        }
        else
        {
            visible = false;
            GetComponent<Renderer>().enabled = false;
        }
    }

    void Start()
    {
        setVisible(visible);
    }
}
