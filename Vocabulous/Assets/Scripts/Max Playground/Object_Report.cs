//////////////////////////////////////////
// Kingston University: Module CI6530   //
// Games Creation Processes             //
// Coursework 2: PC/MAC Game            //
// Team Chumbawumba                     //
// Vocabulous                           //
//////////////////////////////////////////


using UnityEngine;

// KEY COMPONENT
// Used by Overlaytiles (i.e. 3D GUI proxies, tiles and dice) to report back ID
// VITAL ... do not mess this (or you will break lots of things you did not know existed)
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
