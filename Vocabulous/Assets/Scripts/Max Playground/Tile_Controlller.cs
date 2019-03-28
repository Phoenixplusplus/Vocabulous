//////////////////////////////////////////
// Kingston University: Module CI6530   //
// Games Creation Processes             //
// Coursework 2: PC/MAC Game            //
// Team Chumbawumba                     //
// Vocabulous                           //
//////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Controlller : MonoBehaviour
{
    public Object_Report InnerQuad;
    public Object_Report OuterQuad;
    public GameObject letter;
    public Color BaseColor;
    public Color LegalColor;
    public Color SelectedColor;
    public Color HighlightedColor;
    private TextMesh txt;
    public int ID = -1;
    public GameGrid myGrid = null;

    public void SetLetter (string value)
    {
        txt.text = value;
    }

    public void setID (int value)
    {
        ID = value;
        InnerQuad.ID = value;
    }

    public void SetBothID (int value)
    {
        ID = value;
        InnerQuad.ID = value;
        OuterQuad.ID = value;
    }

    public void SetVisible(bool value)
    {
        InnerQuad.setVisible(value);
        OuterQuad.setVisible(value);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (letter != null) txt = letter.GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myGrid != null)
        {
            SetLetter(myGrid.bins[ID]);
            if (myGrid.legals.Contains(ID))
            {
                txt.color = LegalColor;
            }
            else if (myGrid.path.Contains(ID))
            {
                txt.color = SelectedColor;
            }
            else if (!myGrid.highlights.Contains(ID))
            {
                txt.color = BaseColor;
            }
            else
            {
                txt.color = HighlightedColor;
            }
        }
    }
}
