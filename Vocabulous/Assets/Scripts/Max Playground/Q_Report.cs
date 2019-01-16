
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_report : MonoBehaviour, IisOverlayTile
{
    public Color myColor;
    public Color invisibleColor;

    public int ID = 9999;
    public bool visible = true;
    private Renderer rend;

    public int getID()
    {
        return ID;
    }

    public void setVisible(bool value)
    {
        if (value)
        {
            visible = true;
            GetComponent<Renderer>().material.color = myColor;
        }
        else
        {
            visible = false;
            GetComponent<Renderer>().material.color = invisibleColor;
        }
    }

    void Start()
    {
        setVisible(visible);
    }
}

